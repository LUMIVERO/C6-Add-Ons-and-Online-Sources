using Infragistics.Win.Misc;
using SwissAcademic.Citavi.Shell;
using System;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Timers;

namespace SwissAcademic.Addons.TomatoTimerAddon
{
    public class TomatoTimer
    {
        #region Constants

        const string TomatoTimer_DesktopAlert_Key = "TomatoTimer.Key";

        #endregion

        #region Fields

        Timer _timer;
        readonly TomatoTimerStates _states;

        #endregion

        #region Events

        public event EventHandler Updated;
        protected void OnUpdated() => Updated?.Invoke(this, EventArgs.Empty);

        #endregion

        #region Constructors

        public TomatoTimer()
        {
            _states = new TomatoTimerStates(new StoppedTimer(0))
            {
                new RunningTimer(25){ MessageResolver=()=>{return Properties.Resources.Message_Break; } },
                new PausingTimer(5){ MessageResolver=()=>{return Properties.Resources.Message_Go; } },
                new RunningTimer(25){ MessageResolver=()=>{return Properties.Resources.Message_Break; } },
                new PausingTimer(5){ MessageResolver=()=>{return Properties.Resources.Message_Go; } },
                new RunningTimer(25){ MessageResolver=()=>{return Properties.Resources.Message_Break; } },
                new PausingTimer(5){ MessageResolver=()=>{return Properties.Resources.Message_Go; } },
                new RunningTimer(25){ MessageResolver=()=>{return Properties.Resources.FinishMessage; } },
                new WalkingTimer(15){ MessageResolver=()=>{return Properties.Resources.Message_Go; } }
            };

            IsRunning = false;
        }

        #endregion

        #region Properties

        public bool IsRunning { get; private set; }

        public TimerState State => _timer != null ? _states.Current.State : _states.Default.State;

        public int Minutes { get; private set; }

        public string Status => _timer != null ? (Minutes > 1 ? _states.Current.Status : Properties.Resources.LastMinute) : Properties.Resources.StartMessage;

        public Image Image => _timer != null ? _states.Current.Image : _states.Default.Image;

        public string ToolTip => _timer != null ? (Minutes > 1 ? Properties.Resources.RemainingMinutes.FormatString(Minutes) : Properties.Resources.LastMinute) : "TomatoTimer";

        #endregion

        #region EventHandlers

        void Timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            Minutes--;
            if (Minutes == 0)
            {
                ShowMessage(_states.Current.Message, _states.Current.State);
                _states.Next();
                Minutes = _states.Current.Minutes;
                OnUpdated();
            }
        }

        #endregion

        #region Methods

        public void Start()
        {
            if (!IsRunning)
            {
                IsRunning = true;

                Minutes = _states.Current.Minutes;
#if DEBUG
                _timer = new Timer(500)
                {
                    AutoReset = true
                };
#else
            _timer = new Timer(60000)
            {
                AutoReset = true
            };
#endif
                _timer.Elapsed += Timer_Elapsed;
                _timer.Start();

                OnUpdated();
            }
        }

        public void Stop()
        {
            if (IsRunning)
            {
                IsRunning = false;

                Minutes = 0;
                _timer.Stop();
                _timer.Elapsed -= Timer_Elapsed;
                _timer.Dispose();
                _timer = null;
                _states.Reset();
                OnUpdated();
            }
        }

        public void ShowMessage(string message, TimerState state)
        {
            var mainForm = GetActiveMainForm();

            mainForm.Invoke((Action)delegate
            {
                mainForm.TryCloseAlert(TomatoTimer_DesktopAlert_Key);

                var info = new UltraDesktopAlertShowWindowInfo
                {
                    Caption = message,
                    Image = (state == TimerState.Pausing || state == TimerState.Walking) ? Properties.Resources.clock : Properties.Resources.pause,
                    ScreenPosition = ScreenPosition.Center,
                    Key = TomatoTimer_DesktopAlert_Key,
                    Screen = System.Windows.Forms.Screen.FromControl(mainForm)
                };

                mainForm.GetType()
                        .GetMethods(BindingFlags.Instance | BindingFlags.NonPublic)
                        .Where(mi => mi.Name.Equals("ShowDesktopAlert", StringComparison.OrdinalIgnoreCase) && mi.GetParameters().Length == 2)
                        .FirstOrDefault(mi => mi.GetParameters()[0].ParameterType.Name.Equals("UltraDesktopAlertShowWindowInfo", StringComparison.OrdinalIgnoreCase) && mi.GetParameters()[1].ParameterType.Name.Equals("Int32", StringComparison.OrdinalIgnoreCase))?
                        .Invoke(mainForm, new object[] { info, 3000 });

            });
        }

        static MainForm GetActiveMainForm() => (Program.ActiveProjectShell.ActiveForm as MainForm) ?? Program.ActiveProjectShell.PrimaryMainForm;

        #endregion
    }
}
