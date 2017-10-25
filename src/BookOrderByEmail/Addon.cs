using SwissAcademic.Addons.BookOrderByEmail.Properties;
using SwissAcademic.Citavi.Shell;
using SwissAcademic.Controls;
using System.Windows.Forms;

namespace SwissAcademic.Addons.BookOrderByEmail
{
    public class Addon : CitaviAddOn
    {
        #region Fields

        Configuration _configuration;

        #endregion

        #region Properties

        public override AddOnHostingForm HostingForm => AddOnHostingForm.MainForm;

        #endregion

        #region Methods

        protected override void OnBeforePerformingCommand(BeforePerformingCommandEventArgs e)
        {
            e.Handled = true;
            if (e.Form is MainForm mainForm)
            {
                switch (e.Key)
                {
                    case (AddonKeys.OrderPerEmail):
                        {
                            try
                            {
                                mainForm.ActiveReference.OrderByEMail(_configuration);
                            }
                            catch (System.Runtime.InteropServices.COMException)
                            {
                                MessageBox.Show(mainForm, BookOrderByEmailResources.OutlookRightsMessage, "Citavi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }
                        }
                        break;
                    case (AddonKeys.OrderPerClipboard):
                        {
                            mainForm.ActiveReference.OrderByClipboard(_configuration);
                        }
                        break;
                    case (AddonKeys.ConfigOrders):
                        {
                            Configurate(mainForm);
                        }
                        break;
                    default:
                        e.Handled = false;
                        break;
                }
            }
            base.OnBeforePerformingCommand(e);
        }

        protected override void OnHostingFormLoaded(Form form)
        {
            if (_configuration == null) _configuration = new Configuration(Settings);

            if (form is MainForm mainForm)
            {
                var menu = mainForm.GetReferenceEditorTasksCommandbarManager()
                                   .GetCommandbar(MainFormReferenceEditorTasksCommandbarId.Toolbar)
                                   .InsertCommandbarMenu(2,AddonKeys.Menu, BookOrderByEmailResources.TasksOrder, CommandbarItemStyle.ImageAndText, image: BookOrderByEmailResources.addon);
                if (menu != null)
                {
                    menu.HasSeparator = true;
                    if (Outlook.IsInstalled) menu.AddCommandbarButton(AddonKeys.OrderPerEmail, BookOrderByEmailResources.OrderByEMail);
                    menu.AddCommandbarButton(AddonKeys.OrderPerClipboard, BookOrderByEmailResources.OrderByClipboard);
                    var button = menu.AddCommandbarButton(AddonKeys.ConfigOrders, BookOrderByEmailResources.ConfigureOrders);
                    button.HasSeparator = true;
                }
            }
            base.OnHostingFormLoaded(form);
        }

        protected override void OnLocalizing(Form form)
        {
            if (_configuration == null) _configuration = new Configuration(Settings);

            if (form is MainForm mainForm)
            {
                var menu = mainForm.GetReferenceEditorTasksCommandbarManager()
                                   .GetCommandbar(MainFormReferenceEditorTasksCommandbarId.Toolbar)
                                   .GetCommandbarMenu(AddonKeys.Menu);
                if (menu != null)
                {
                    menu.Text = BookOrderByEmailResources.TasksOrder;
                    var button = menu.GetCommandbarButton(AddonKeys.OrderPerEmail);
                    if (button != null) button.Text = BookOrderByEmailResources.OrderByEMail;

                    button = menu.GetCommandbarButton(AddonKeys.OrderPerClipboard);
                    if (button != null) button.Text = BookOrderByEmailResources.OrderByClipboard;

                    button = menu.GetCommandbarButton(AddonKeys.ConfigOrders);
                    if (button != null) button.Text = BookOrderByEmailResources.ConfigureOrders;

                }
            }
            base.OnLocalizing(form);
        }

        void Configurate(Form form)
        {
            using (var dialog = new ConfigDialog(_configuration.Receiver, _configuration.Body) { Owner = form })
            {
                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    _configuration.Body = dialog.Body;
                    _configuration.Receiver = dialog.Receiver;
                }
            }
        }

        #endregion
    }
}