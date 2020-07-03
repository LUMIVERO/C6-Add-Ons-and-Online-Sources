using SwissAcademic.Addons.BookOrderByEmailAddon.Properties;
using SwissAcademic.Citavi.Shell;
using SwissAcademic.Controls;
using System.Windows.Forms;

namespace SwissAcademic.Addons.BookOrderByEmailAddon
{
    public partial class Addon : CitaviAddOn<MainForm>
    {
        public override void OnBeforePerformingCommand(MainForm mainForm, BeforePerformingCommandEventArgs e)
        {
            e.Handled = true;

            var body = Settings.GetValueOrDefault(Settings_Key_Body);
            var receiver = Settings.GetValueOrDefault(Settings_Key_Receiver);

            switch (e.Key)
            {
                case Key_Button_OrderPerEmail:
                    {
                        try
                        {
                            mainForm.ActiveReference.OrderByEMail(receiver, body);
                        }
                        catch (System.Runtime.InteropServices.COMException)
                        {
                            MessageBox.Show(mainForm, Resources.OutlookRightsMessage, mainForm.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                    break;
                case Key_Button_OrderPerClipboard:
                    {
                        mainForm.ActiveReference.OrderByClipboard(receiver, body);
                    }
                    break;
                case Key_Button_ConfigOrders:
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
            using (var dialog = new ConfigDialog(Settings) { Owner = form })
            {
                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    Settings.SetValueSafe(Settings_Key_Body, dialog.Body);
                    Settings.SetValueSafe(Settings_Key_Receiver, dialog.Receiver);
                }
            }
        }
    }
}