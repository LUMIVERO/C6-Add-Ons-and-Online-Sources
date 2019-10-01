using SwissAcademic.Citavi.Shell;
using System.Linq;

namespace SwissAcademic.Addons.TomatoTimer
{
    internal static class Extensions
    {
        #region Infragistics.Win.UltraWinStatusBar.UltraStatusBar

        public static Infragistics.Win.UltraWinStatusBar.UltraStatusBar GetStatusBar(this MainForm mainForm)
        {
            var propertyInfo = mainForm.GetType().GetFields(System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic).FirstOrDefault(p => p.Name.Equals("statusBar"));

            return propertyInfo?.GetValue(mainForm) as Infragistics.Win.UltraWinStatusBar.UltraStatusBar;
        }

        #endregion
    }
}
