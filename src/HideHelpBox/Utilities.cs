using SwissAcademic.Citavi.Shell;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;

namespace SwissAcademic.Addons.HideHelpBox
{
    public static class Utilities
    {
        public static List<Form> OpenForms(this MainForm form)
        {
            var field = typeof(ProjectShell).GetProperties(BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.DeclaredOnly).FirstOrDefault(f => f.Name.Equals("OpenForms"));

            return field?.GetValue(form.ProjectShell) as List<Form>;
        }

        public static IEnumerable<T> FormsOf<T>(this List<Form> openForms)
        {
            var results = new List<T>();
            foreach (var form in openForms)
            {
                try
                {
                    results.Add((T)Convert.ChangeType(form, typeof(T)));
                }
                catch (InvalidCastException)
                {

                }
            }
            return results;
        }
    }
}
