using Infragistics.Win.UltraWinToolbars;
using SwissAcademic.Citavi.Shell;
using SwissAcademic.Controls;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace SwissAcademic.Addons.OpenWith
{
    public class Addon : CitaviAddOn<MainForm>
    {
        #region Constants

        const string Key_Menu_OpenWith = "SwissAcademic.Addons.OpenWith.MenuCommand";
        const string Key_Button_OpenWith = "SwissAcademic.Addons.OpenWith.OpenWithCommand.{0}";

        #endregion

        #region Fields

        PopupMenuTool _menu;
        List<ButtonTool> _tools;
        List<Application> _applications;

        #endregion

        #region Constructors

        public Addon()
        {
            _tools = new List<ButtonTool>();
            _applications = new List<Application>();
        }

        #endregion

        #region Methods

        public override void OnHostingFormLoaded(MainForm mainForm)
        {
            if (mainForm.ReferenceEditorElectronicLocationsToolbarsManager?
                           .Tools.Cast<ToolBase>()
                           .FirstOrDefault(tool => tool.Key.Equals("ReferenceEditorUriLocationsContextMenu")) is PopupMenuTool popupMenu)
            {
                popupMenu.BeforeToolDropdown += (sender, e) => UpdateList(mainForm);
                popupMenu.ToolbarsManager.Tools.Add(new PopupMenuTool(Key_Menu_OpenWith));
                _menu = popupMenu.ToolbarsManager.Tools[Key_Menu_OpenWith] as PopupMenuTool;
                popupMenu.Tools.Insert(4, _menu);
                _menu.SharedProps.Caption = Properties.OpenWithResources.MenuCaption;
                _menu.SharedProps.AppearancesSmall.Appearance.Image = Properties.OpenWithResources.addon;
            }
            base.OnHostingFormLoaded(mainForm);
        }

        public override void OnLocalizing(MainForm form)
        {
            if (_menu != null) _menu.SharedProps.Caption = Properties.OpenWithResources.MenuCaption;
            base.OnLocalizing(form);
        }

        public override void OnBeforePerformingCommand(MainForm mainForm, BeforePerformingCommandEventArgs e)
        {
            var application = _applications.FirstOrDefault(appplication => e.Key.Equals(Key_Button_OpenWith.FormatString(appplication.Id), StringComparison.OrdinalIgnoreCase));
            if (application != null)
            {
                Program.ClosePreview(mainForm.PreviewControl.ActiveUri);

                var locations = mainForm.GetAvailableSelectedLocationsAsFilePath().GroupByExtension().FirstOrDefault();
                application.Run(locations);

                e.Handled = true;
            }
            else
            {
                e.Handled = false;
            }

            base.OnBeforePerformingCommand(mainForm, e);
        }

        void UpdateList(MainForm mainForm)
        {
            if (_menu == null) return;

            foreach (var tool in _tools)
            {
                _menu.Tools.Remove(tool);
                _menu.ToolbarsManager.Tools.Remove(tool);
                tool.Dispose();
            }

            _applications.Clear();
            _tools.Clear();

            var counter = 0;

            var locations = mainForm.GetAvailableSelectedLocationsAsFilePath().GroupByExtension().FirstOrDefault();

            if (locations.Count() != 0)
            {
                var applications = Registry2.GetOpenWithList(Path.GetExtension(locations.FirstOrDefault()));

                if (applications.Count() == 0)
                {
                    _menu.SharedProps.Enabled = false;
                    _menu.SharedProps.Visible = false;
                    return;
                }
                _menu.SharedProps.Enabled = true;
                _menu.SharedProps.Visible = true;
                foreach (var application in applications)
                {
                    var key = Key_Button_OpenWith.FormatString(application.Id);
                    _menu.ToolbarsManager.Tools.Add(new ButtonTool(key));
                    var button = _menu.ToolbarsManager.Tools[key] as ButtonTool;
                    _menu.Tools.Insert(counter, button);
                    button.SharedProps.Caption = application.Name;
                    button.SharedProps.AppearancesSmall.Appearance.Image = System.Drawing.Icon.ExtractAssociatedIcon(application.Path)?.ToBitmap();
                    counter++;
                    _tools.Add(button);
                    _applications.Add(application);
                }
            }
            else
            {
                _menu.SharedProps.Enabled = false;
                _menu.SharedProps.Visible = false;
            }
        }

        #endregion
    }
}