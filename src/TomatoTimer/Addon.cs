using SwissAcademic.Addons.TomatoTimerAddon.Properties;
using SwissAcademic.Citavi.Shell;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace SwissAcademic.Addons.TomatoTimerAddon
{
    public partial class Addon : CitaviAddOn<MainForm>
    {
        // Fields

        readonly TomatoTimer _tomatoTimer;
        readonly Dictionary<Form, Infragistics.Win.UltraWinStatusBar.UltraStatusPanel> _formsAndPanels;

        // Constructors

        public Addon()
        {
            _tomatoTimer = new TomatoTimer();
            _tomatoTimer.Updated += TomatoTimer_Updated;
            _formsAndPanels = new Dictionary<Form, Infragistics.Win.UltraWinStatusBar.UltraStatusPanel>();
        }

        // Methods

        public override void OnApplicationIdle(MainForm mainForm)
        {
            if (_formsAndPanels.ContainsKey(mainForm))
            {
                _formsAndPanels[mainForm].ToolTipText = _tomatoTimer.ToolTip;
            }
        }

        public override void OnHostingFormLoaded(MainForm mainForm)
        {
            if (!_formsAndPanels.ContainsKey(mainForm) && !mainForm.IsPreviewFullScreenForm)
            {
                var statusbar = mainForm.GetStatusBar();
                var appearance = new Infragistics.Win.Appearance
                {
                    Image = Resources.tomato,
                    TextHAlign = Infragistics.Win.HAlign.Left,
                    TextVAlign = Infragistics.Win.VAlign.Middle,
                    Cursor = Cursors.Hand
                };

                var panel = new Infragistics.Win.UltraWinStatusBar.UltraStatusPanel
                {
                    Key = PanelKey,
                    Appearance = appearance,
                    Text = Resources.StartMessage
                };

                statusbar.Panels.Insert(statusbar.Panels.Count - 2, panel);
                statusbar.PanelClick += Statusbar_PanelClick;
                _formsAndPanels.Add(mainForm, panel);
                mainForm.FormClosing += MainForm_FormClosing;
            }
        }

        public override void OnLocalizing(MainForm mainForm)
        {
            if (_formsAndPanels.ContainsKey(mainForm))
            {
                _formsAndPanels[mainForm].Appearance.Image = _tomatoTimer.Image;
                _formsAndPanels[mainForm].Text = _tomatoTimer.Status;
                _formsAndPanels[mainForm].ToolTipText = _tomatoTimer.ToolTip;
            }
        }

        // EventHandlers

        void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (sender is MainForm mainForm)
            {
                mainForm.FormClosing -= MainForm_FormClosing;
                if (_formsAndPanels.ContainsKey(mainForm))
                {
                    _formsAndPanels.Remove(mainForm);
                    if (_formsAndPanels.Count == 0 && _tomatoTimer.IsRunning) _tomatoTimer.Stop();
                }
            }
        }

        void Statusbar_PanelClick(object sender, Infragistics.Win.UltraWinStatusBar.PanelClickEventArgs e)
        {
            if (_formsAndPanels.ContainsValue(e.Panel))
            {
                if (_tomatoTimer.IsRunning)
                {
                    _tomatoTimer.Stop();
                }
                else
                {
                    _tomatoTimer.Start();
                }
            }
        }

        void TomatoTimer_Updated(object sender, System.EventArgs e)
        {
            if (Program.ActiveProjectShell.ActiveForm is MainForm mainForm && _formsAndPanels.ContainsKey(mainForm))
            {
                mainForm.Invoke((Action)delegate
                {
                    _formsAndPanels[mainForm].Appearance.Image = _tomatoTimer.Image;
                    _formsAndPanels[mainForm].Text = _tomatoTimer.Status;

                });
            }
        }
    }
}