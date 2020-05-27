using Infragistics.Win.UltraWinStatusBar;
using SwissAcademic.Citavi.Shell;
using System.Linq;
using System.Reflection;

namespace SwissAcademic.Addons.TomatoTimerAddon
{
    internal static class Extensions
    {
        #region Infragistics.Win.UltraWinStatusBar.UltraStatusBar

        public static UltraStatusBar GetStatusBar(this MainForm mainForm)
        {
            return mainForm.GetType()
                           .GetFields(BindingFlags.Instance | BindingFlags.NonPublic)
                           .FirstOrDefault(p => p.Name.Equals("statusBar"))?
                           .GetValue(mainForm) as UltraStatusBar;
        }

        #endregion

        public static void TryCloseAlert(this MainForm mainForm, string key)
        {
            try
            {
                var alert = mainForm.GetType().GetField("desktopAlert", BindingFlags.NonPublic | BindingFlags.Instance)?.GetValue(mainForm) as Infragistics.Win.Misc.UltraDesktopAlert;

                if ((bool)alert?.IsOpen(key))
                {
                    alert?.Close(key);
                }
            }
            catch (System.Exception) { }
        }
    }
}
