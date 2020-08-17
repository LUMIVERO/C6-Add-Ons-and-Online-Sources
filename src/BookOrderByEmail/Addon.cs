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

            var body = Settings.GetValueOrDefault(SettingsKey_Body);
            var receiver = Settings.GetValueOrDefault(SettingsKey_Receiver);

            switch (e.Key)
            {
                case ButtonKey_OrderPerEmail:
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
                case ButtonKey_OrderPerClipboard:
                    {
                        mainForm.ActiveReference.OrderByClipboard(receiver, body);
                    }
                    break;
                case ButtonKey_ConfigOrders:
                    {
                        ShowMailTemplateForm(mainForm);
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
                       .InsertCommandbarMenu(2, MenuKey, Resources.TasksOrder, CommandbarItemStyle.ImageAndText, image: Resources.addon);

            if (menu != null)
            {
                menu.HasSeparator = true;

                if (Outlook.IsInstalled)
                {
                    menu.AddCommandbarButton(ButtonKey_OrderPerEmail, Resources.OrderByEMail);
                }

                menu.AddCommandbarButton(ButtonKey_OrderPerClipboard, Resources.OrderByClipboard);
                var button = menu.AddCommandbarButton(ButtonKey_ConfigOrders, Resources.ConfigureOrders);
                button.HasSeparator = true;
            }

        }

        public override void OnLocalizing(MainForm mainForm)
        {
            var menu = mainForm.GetReferenceEditorTasksCommandbarManager()
                               .GetCommandbar(MainFormReferenceEditorTasksCommandbarId.Toolbar)
                               .GetCommandbarMenu(MenuKey);
            if (menu != null)
            {
                menu.Text = Resources.TasksOrder;
                var button = menu.GetCommandbarButton(ButtonKey_OrderPerEmail);
                if (button != null)
                {
                    button.Text = Resources.OrderByEMail;
                }

                button = menu.GetCommandbarButton(ButtonKey_OrderPerClipboard);
                if (button != null)
                {
                    button.Text = Resources.OrderByClipboard;
                }

                button = menu.GetCommandbarButton(ButtonKey_ConfigOrders);
                if (button != null)
                {
                    button.Text = Resources.ConfigureOrders;
                }
            }

        }

        void ShowMailTemplateForm(Form form)
        {
            using (var dialog = new MailTemplateForm(Settings) { Owner = form })
            {
                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    Settings.SetValueSafe(SettingsKey_Body, dialog.Body);
                    Settings.SetValueSafe(SettingsKey_Receiver, dialog.Receiver);
                }
            }
        }
    }
}