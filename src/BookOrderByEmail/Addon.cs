using SwissAcademic.Addons.BookOrderByEmail.Properties;
using SwissAcademic.Citavi.Shell;
using SwissAcademic.Controls;
using System.Windows.Forms;

namespace SwissAcademic.Addons.BookOrderByEmail
{
    public class Addon : CitaviAddOn
    {
        #region Constants

        const string Key_OrderPerEmail = "SwissAcademic.Addons.BookOrderByEmail.OrderPerEmail";
        const string Key_OrderPerClipboard = "SwissAcademic.Addons.BookOrderByEmail.OrderPerClipboard";
        const string Key_ConfigOrders = "SwissAcademic.Addons.BookOrderByEmail.ConfigOrders";
        const string Key_Menu = "SwissAcademic.Addons.BookOrderByEmail.Menu";

        #endregion

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
                    case (Key_OrderPerEmail):
                        {
                            try
                            {
                                mainForm.ActiveReference.OrderByEMail(_configuration);
                            }
                            catch (System.Runtime.InteropServices.COMException)
                            {
                                MessageBox.Show(mainForm, BookOrderByEmailResources.OutlookRightsMessage, mainForm.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }
                        }
                        break;
                    case (Key_OrderPerClipboard):
                        {
                            mainForm.ActiveReference.OrderByClipboard(_configuration);
                        }
                        break;
                    case (Key_ConfigOrders):
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
                                   .InsertCommandbarMenu(2, Key_Menu, BookOrderByEmailResources.TasksOrder, CommandbarItemStyle.ImageAndText, image: BookOrderByEmailResources.addon);
                if (menu != null)
                {
                    menu.HasSeparator = true;
                    if (Outlook.IsInstalled) menu.AddCommandbarButton(Key_OrderPerEmail, BookOrderByEmailResources.OrderByEMail);
                    menu.AddCommandbarButton(Key_OrderPerClipboard, BookOrderByEmailResources.OrderByClipboard);
                    var button = menu.AddCommandbarButton(Key_ConfigOrders, BookOrderByEmailResources.ConfigureOrders);
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
                                   .GetCommandbarMenu(Key_Menu);
                if (menu != null)
                {
                    menu.Text = BookOrderByEmailResources.TasksOrder;
                    var button = menu.GetCommandbarButton(Key_OrderPerEmail);
                    if (button != null) button.Text = BookOrderByEmailResources.OrderByEMail;

                    button = menu.GetCommandbarButton(Key_OrderPerClipboard);
                    if (button != null) button.Text = BookOrderByEmailResources.OrderByClipboard;

                    button = menu.GetCommandbarButton(Key_ConfigOrders);
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