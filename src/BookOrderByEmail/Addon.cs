using SwissAcademic.Addons.BookOrderByEmailAddon.Properties;
using SwissAcademic.Citavi.Shell;
using SwissAcademic.Controls;
using System.Windows.Forms;

namespace SwissAcademic.Addons.BookOrderByEmailAddon
{
    public class Addon : CitaviAddOn<MainForm>
    {
        #region Constants

        const string Key_Button_OrderPerEmail = "SwissAcademic.Addons.BookOrderByEmail.OrderPerEmail";
        const string Key_Button_OrderPerClipboard = "SwissAcademic.Addons.BookOrderByEmail.OrderPerClipboard";
        const string Key_Button_ConfigOrders = "SwissAcademic.Addons.BookOrderByEmail.ConfigOrders";
        const string Key_Menu = "SwissAcademic.Addons.BookOrderByEmail.Menu";

        #endregion

        #region Fields

        Configuration _configuration;

        #endregion

        #region Methods

        public override void OnBeforePerformingCommand(MainForm mainForm, BeforePerformingCommandEventArgs e)
        {
            e.Handled = true;
            switch (e.Key)
            {
                case (Key_Button_OrderPerEmail):
                    {
                        try
                        {
                            mainForm.ActiveReference.OrderByEMail(_configuration);
                        }
                        catch (System.Runtime.InteropServices.COMException)
                        {
                            MessageBox.Show(mainForm, Resources.OutlookRightsMessage, mainForm.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                    break;
                case (Key_Button_OrderPerClipboard):
                    {
                        mainForm.ActiveReference.OrderByClipboard(_configuration);
                    }
                    break;
                case (Key_Button_ConfigOrders):
                    {
                        Configurate(mainForm);
                    }
                    break;
                default:
                    e.Handled = false;
                    break;
            }
        }

        public override void OnHostingFormLoaded(MainForm mainForm)
        {
            if (_configuration == null) _configuration = new Configuration(Settings);

            var menu = mainForm
                       .GetReferenceEditorTasksCommandbarManager()
                       .GetCommandbar(MainFormReferenceEditorTasksCommandbarId.Toolbar)
                       .InsertCommandbarMenu(2, Key_Menu, Resources.TasksOrder, CommandbarItemStyle.ImageAndText, image: Resources.addon);
            if (menu != null)
            {
                menu.HasSeparator = true;

                if (Outlook.IsInstalled)
                {
                    menu.AddCommandbarButton(Key_Button_OrderPerEmail, Resources.OrderByEMail);
                }

                menu.AddCommandbarButton(Key_Button_OrderPerClipboard, Resources.OrderByClipboard);
                var button = menu.AddCommandbarButton(Key_Button_ConfigOrders, Resources.ConfigureOrders);
                button.HasSeparator = true;
            }

        }

        public override void OnLocalizing(MainForm mainForm)
        {
            if (_configuration == null) _configuration = new Configuration(Settings);

            var menu = mainForm.GetReferenceEditorTasksCommandbarManager()
                               .GetCommandbar(MainFormReferenceEditorTasksCommandbarId.Toolbar)
                               .GetCommandbarMenu(Key_Menu);
            if (menu != null)
            {
                menu.Text = Resources.TasksOrder;
                var button = menu.GetCommandbarButton(Key_Button_OrderPerEmail);
                if (button != null)
                {
                    button.Text = Resources.OrderByEMail;
                }

                button = menu.GetCommandbarButton(Key_Button_OrderPerClipboard);
                if (button != null)
                {
                    button.Text = Resources.OrderByClipboard;
                }

                button = menu.GetCommandbarButton(Key_Button_ConfigOrders);
                if (button != null)
                {
                    button.Text = Resources.ConfigureOrders;
                }
            }

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