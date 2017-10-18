using Infragistics.Win.UltraWinToolbars;
using SwissAcademic.Citavi.Shell;
using SwissAcademic.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace SwissAcademic.Addons.OpenWith
{
    public class Addon : CitaviAddOn
    {
        #region Fields

        PopupMenuTool _menu;
        ButtonTool _configurationButton;
        Dictionary<ButtonTool, Application> _tools;

        #endregion

        #region Constructors

        public Addon()
        {
            _tools = new Dictionary<ButtonTool, Application>();
        }

        #endregion

        #region Properties
        public override AddOnHostingForm HostingForm => AddOnHostingForm.MainForm;

        public static Configuration Configuration { get; set; }

        #endregion

        #region Methods

        protected override void OnApplicationIdle(Form form)
        {
            if (_menu != null && form is MainForm mainForm)
            {
                var locations = mainForm.GetAvailableSelectedLocations();

                if (locations.Count != 1)
                {
                    _menu.SharedProps.Visible = false;
                    _menu.SharedProps.Enabled = false;
                    foreach (var tool in _tools)
                    {
                        tool.Key.SharedProps.Visible = false;
                        tool.Key.SharedProps.Enabled = false;
                    }
                }
                else
                {
                    _menu.SharedProps.Visible = true;
                    _menu.SharedProps.Enabled = true;

                    var location = locations.FirstOrDefault();

                    var dataType = System.IO.Path.GetExtension(location.Value);

                    foreach (var tool in _tools)
                    {
                        var application = tool.Value;
                        if (!application.Filters.Any())
                        {
                            tool.Key.SharedProps.Visible = true;
                            tool.Key.SharedProps.Enabled = true;
                        }
                        else
                        {
                            var supportDataType = application.Filters.Contains(dataType);
                            tool.Key.SharedProps.Visible = supportDataType;
                            tool.Key.SharedProps.Enabled = supportDataType;
                        }
                    }
                }
            }

            base.OnApplicationIdle(form);
        }

        protected override void OnBeforePerformingCommand(BeforePerformingCommandEventArgs e)
        {
            if (Addon.Configuration == null) Addon.Configuration = Settings.Load();

            if (e.Form is MainForm mainForm)
            {
                e.Handled = true;
                switch (e.Key)
                {
                    case (AddonKeys.ConfigurationCommand):
                        {
                            using (var dialog = new ConfigurationDialog(Addon.Configuration.Clone() as Configuration))
                            {
                                if (dialog.ShowDialog(e.Form) == DialogResult.OK)
                                {
                                    Addon.Configuration = dialog.Configuration.Clone() as Configuration;
                                    Settings.Save(Addon.Configuration);
                                    Refresh();
                                }
                            }
                        }
                        break;

                    default:
                        {
                            var application = Addon.Configuration.Applications.FirstOrDefault(a => e.Key.Equals(AddonKeys.OpenWithCommand.FormatString(a.Id), StringComparison.OrdinalIgnoreCase));
                            if (application != null)
                            {
                                Program.ClosePreview(mainForm.PreviewControl.ActiveUri);

                                var locations = mainForm.GetAvailableSelectedLocations();
                                application.Run(locations);

                                e.Handled = true;
                            }
                            else
                            {
                                e.Handled = false;
                            }
                        }
                        break;
                }
            }

            base.OnBeforePerformingCommand(e);
        }

        protected override void OnHostingFormLoaded(Form form)
        {
            if (Addon.Configuration == null) Addon.Configuration = Settings.Load();

            if (form is MainForm mainForm)
            {
                if (mainForm.ReferenceEditorElectronicLocationsToolbarsManager?
                            .Tools.Cast<ToolBase>()
                            .FirstOrDefault(tool => tool.Key.Equals("ReferenceEditorUriLocationsContextMenu")) is PopupMenuTool popupMenu)
                {
                    popupMenu.ToolbarsManager.Tools.Add(new PopupMenuTool(AddonKeys.MenuCommand));
                    _menu = popupMenu.ToolbarsManager.Tools[AddonKeys.MenuCommand] as PopupMenuTool;
                    popupMenu.Tools.Insert(4, _menu);
                    _menu.SharedProps.Caption = Properties.OpenWithResources.MenuCaption;
                    _menu.SharedProps.AppearancesSmall.Appearance.Image = Properties.OpenWithResources.addon;

                    _menu.ToolbarsManager.Tools.Add(new ButtonTool(AddonKeys.ConfigurationCommand));
                    _configurationButton = _menu.ToolbarsManager.Tools[AddonKeys.ConfigurationCommand] as ButtonTool;
                    _menu.Tools.Insert(0, _configurationButton);
                    _configurationButton.SharedProps.Caption = Properties.OpenWithResources.ConfigurationCaption;
                    Refresh();
                }
            }

            base.OnHostingFormLoaded(form);
        }

        protected override void OnLocalizing(Form form)
        {
            if (_menu != null) _menu.SharedProps.Caption = Properties.OpenWithResources.MenuCaption;
            if (_configurationButton != null) _configurationButton.SharedProps.Caption = Properties.OpenWithResources.ConfigurationCaption;

            base.OnLocalizing(form);
        }

        void Refresh()
        {
            if (_menu == null) return;

            foreach (var tool in _tools)
            {
                _menu.Tools.Remove(tool.Key);
                _menu.ToolbarsManager.Tools.Remove(tool.Key);
                tool.Key.Dispose();
            }

            _tools.Clear();
            var counter = 0;

            foreach (var application in Addon.Configuration.Applications)
            {
                var key = AddonKeys.OpenWithCommand.FormatString(application.Id);
                _menu.ToolbarsManager.Tools.Add(new ButtonTool(key));
                var button = _menu.ToolbarsManager.Tools[key] as ButtonTool;
                _menu.Tools.Insert(counter, button);
                button.SharedProps.Caption = application.Name;
                button.SharedProps.AppearancesSmall.Appearance.Image = System.Drawing.Icon.ExtractAssociatedIcon(application.Path)?.ToBitmap();
                counter++;
                _tools.Add(button, application);
            }
        }

        #endregion
    }
}