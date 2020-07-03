using SwissAcademic.Citavi;
using SwissAcademic.Citavi.Shell;
using SwissAcademic.Citavi.Shell.Controls.Preview;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;

namespace SwissAcademic.Addons.ExportAttachmentsToCategoryFolderStructureAddon
{
    internal class Preview
    {
        // Fields

        readonly PreviewControl _previewControl;
        readonly Location _location;

        // Constructors

        public Preview(PreviewControl previewControl, Location location) => (_previewControl, _location) = (previewControl, location);

        // Method

        public void Close() => _previewControl.ShowNoPreview();

        public void Open() => _previewControl.ShowLocationPreview(_location, PreviewBehaviour.SkipEntryPage, false);
    }

    internal class Previews : List<Preview>
    {
        Previews() { }

        public void Close() => ForEach(preview => preview.Close());

        public void Open() => ForEach(preview => preview.Open());

        public static Previews Instance
        {
            get
            {
                {

                    var previews = new Previews();

                    var previewLocations = from projectShell in Program.ProjectShells
                                           from mainForm in projectShell.Field<ProjectShell, List<MainForm>>("_previewFullScreenForms")
                                           where
                                                  mainForm.PreviewControl.ActiveLocation != null
                                           select new Preview(mainForm.PreviewControl, mainForm.PreviewControl.ActiveLocation);

                    previewLocations = previewLocations.Concat(from projectShell in Program.ProjectShells
                                                               from mainForm in projectShell.MainForms
                                                               where
                                                                   mainForm.PreviewControl != null &&
                                                                   mainForm.PreviewControl.ActiveLocation != null
                                                               select new Preview(mainForm.PreviewControl, mainForm.PreviewControl.ActiveLocation));

                    previews.AddRange(previewLocations.ToList());

                    return previews;
                }
            }
        }
    }

    internal static class PreviewExtensions
    {
        public static TResult Field<TObject, TResult>(this TObject tObject, string fieldName) where TObject : class
        {
            var obj = tObject
                   .Members(fieldName, MemberTypes.Field, BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public)
                   .Cast<FieldInfo>()
                   .FirstOrDefault(field => field.Name.Equals(fieldName, StringComparison.OrdinalIgnoreCase))?
                   .GetValue(tObject);
            return (TResult)Convert.ChangeType(obj, typeof(TResult), Application.CurrentCulture);
        }

        private static IEnumerable<MemberInfo> Members<TObject>(this TObject tObject, string memberName, MemberTypes memberType, BindingFlags bindingFlags)
        {
            return tObject
                   .GetType()
                   .GetMembers(bindingFlags)
                   .Where(prop => prop.Name.Equals(memberName, StringComparison.OrdinalIgnoreCase) && prop.MemberType == memberType)
                   .ToList();
        }
    }
}
