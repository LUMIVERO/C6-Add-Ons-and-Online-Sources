using System;
using System.Drawing;
using SwissAcademic.Addons.TomatoTimer.Properties;

namespace SwissAcademic.Addons.TomatoTimer
{
    internal class TomatoTimerState
    {
        #region Constructors

        public TomatoTimerState(int period)
        {
            Minutes = period;
            MessageResolver = () => null;
        }

        #endregion

        #region Properties

        public int Minutes { get; }

        public virtual TimerState State { get; }

        public virtual Image Image { get; }

        public string Message => MessageResolver.Invoke();

        public Func<string> MessageResolver { get; set; }

        public virtual string Status { get; }

        #endregion
    }

    internal class StoppedTimer : TomatoTimerState
    {
        #region Constructors

        public StoppedTimer(int period) : base(period) { }

        #endregion

        #region Properties

        public override TimerState State => TimerState.Stopped;

        public override Image Image => TomatoTimerResources.tomato_stop;

        public override string Status => TomatoTimerResources.StartMessage;

        #endregion
    }

    internal class RunningTimer : TomatoTimerState
    {
        #region Constructors

        public RunningTimer(int period) : base(period) { }

        #endregion

        #region Properties

        public override TimerState State => TimerState.Running;

        public override Image Image => TomatoTimerResources.tomato;

        public override string Status => TomatoTimerResources.Active;

        #endregion
    }

    internal class WalkingTimer : TomatoTimerState
    {
        #region Constructors

        public WalkingTimer(int period) : base(period) { }

        #endregion

        #region Properties

        public override TimerState State => TimerState.Walking;

        public override Image Image => TomatoTimerResources.walk;

        public override string Status => TomatoTimerResources.Rest;

        #endregion
    }

    internal class PausingTimer : TomatoTimerState
    {
        #region Constructors

        public PausingTimer(int period) : base(period) { }

        #endregion

        #region Properties

        public override TimerState State => TimerState.Pausing;

        public override Image Image => TomatoTimerResources.pause;

        public override string Status => TomatoTimerResources.Rest;

        #endregion
    }
}
