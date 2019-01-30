using SwissAcademic.Citavi.Shell;
using System.Collections.Generic;
using System.Windows.Forms;
using SwissAcademic.Addons.TomatoTimer.Properties;

namespace SwissAcademic.Addons.TomatoTimer
{
    public class Addon : CitaviAddOn<MainForm>
    {
        #region Constants

        const string PanelKey = "Tomato.Panel.{0}";

        #endregion

        #region Fields

        TomatoTimer _tomatoTimer;

        Dictionary<Form, Infragistics.Win.UltraWinStatusBar.UltraStatusPanel> _formsAndPanels;

        #endregion

        #region Constructors

        public Addon()
        {
            _tomatoTimer = new TomatoTimer();
            _formsAndPanels = new Dictionary<Form, Infragistics.Win.UltraWinStatusBar.UltraStatusPanel>();
        }

        #endregion

        #region Methods

        public override void OnApplicationIdle(MainForm mainForm)
        {
            if (_formsAndPanels.ContainsKey(mainForm))
            {
                _formsAndPanels[mainForm].Appearance.Image = _tomatoTimer.Image;
                _formsAndPanels[mainForm].Text = _tomatoTimer.Status;
                _formsAndPanels[mainForm].ToolTipText = _tomatoTimer.ToolTip;
            }

            base.OnApplicationIdle(mainForm);
        }

        public override void OnHostingFormLoaded(MainForm mainForm)
        {
            if (!_formsAndPanels.ContainsKey(mainForm) && !mainForm.IsPreviewFullScreenForm)
            {
                var statusbar = mainForm.GetStatusBar();
                var appearance = new Infragistics.Win.Appearance
                {
                    Image = TomatoTimerResources.tomato,
                    TextHAlign = Infragistics.Win.HAlign.Left,
                    TextVAlign = Infragistics.Win.VAlign.Middle,
                    Cursor = Cursors.Hand
                };

                var panel = new Infragistics.Win.UltraWinStatusBar.UltraStatusPanel
                {
                    Key = PanelKey,
                    Appearance = appearance,
                    Text = TomatoTimerResources.StartMessage
                };

                statusbar.Panels.Insert(statusbar.Panels.Count - 2, panel);
                statusbar.PanelClick += Statusbar_PanelClick;
                _formsAndPanels.Add(mainForm, panel);
                mainForm.FormClosing += MainForm_FormClosing;
            }
            base.OnHostingFormLoaded(mainForm);
        }

        public override void OnLocalizing(MainForm mainForm)
        {
            if (_formsAndPanels.ContainsKey(mainForm))
            {
                _formsAndPanels[mainForm].Appearance.Image = _tomatoTimer.Image;
                _formsAndPanels[mainForm].Text = _tomatoTimer.Status;
                _formsAndPanels[mainForm].ToolTipText = _tomatoTimer.ToolTip;
            }

            base.OnLocalizing(mainForm);
        }

        #endregion

        #region EventHandlers

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
                if (_tomatoTimer.IsRunning) _tomatoTimer.Stop();
                else _tomatoTimer.Start();
            }
        }

        #endregion
    }
}