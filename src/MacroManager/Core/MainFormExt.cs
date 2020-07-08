using SwissAcademic.Citavi.Shell;
using SwissAcademic.Controls;

namespace SwissAcademic.Addons.MacroManagerAddon
{
    internal static class MainFormExt
    {
        public static CommandbarMenu GetMacrosMenu(this MainForm mainForm)
        {
            return
                mainForm
                .GetMainCommandbarManager()
                .GetReferenceEditorCommandbar(MainFormReferenceEditorCommandbarId.Menu)
                .GetCommandbarMenu(Addon.MenuKey_Macros);
        }
    }
}
