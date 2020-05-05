using SwissAcademic.Citavi;
using SwissAcademic.Citavi.Shell;
using SwissAcademic.Citavi.Shell.Controls.Preview;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;

namespace SwissAcademic.Addons.ExportAttachmentsToCategoryFolderStructure
{
    internal class Preview
    {
        #region Fields

        readonly PreviewControl _previewControl;
        readonly Location _location;

        #endregion

        #region Constructors

        public Preview(PreviewControl previewControl, Location location)
        {
            _previewControl = previewControl;
            _location = location;
        }

        #endregion

        #region Method

        public void Close()
        {
            _previewControl.ShowNoPreview();
        }


        public void Open()
        {
            _previewControl.ShowLocationPreview(_location, PreviewBehaviour.SkipEntryPage, false);
        }

        #endregion
    }

    internal class Previews : List<Preview>
    {
        #region Constructors

        public Previews() : base()
        {

        }

        #endregion

        #region Methods

        public void Close()
        {
            ForEach(preview => preview.Close());
        }

        public void Open()
        {
            ForEach(preview => preview.Open());
        }

        public static Previews Create()
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

        #endregion
    }

    internal static class PreviewExtensions
    {
        public static TResult Field<TObject, TResult>(this TObject t, string fieldName) where TObject : class
        {
            var o = t
                   .Members(fieldName, MemberTypes.Field, BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public)
                   .Cast<FieldInfo>()
                   .FirstOrDefault(field => field.Name.Equals(fieldName, StringComparison.OrdinalIgnoreCase))?
                   .GetValue(t);
            return (TResult)Convert.ChangeType(o, typeof(TResult), Application.CurrentCulture);
        }

        private static IEnumerable<MemberInfo> Members<T>(this T t, string memberName, MemberTypes memberType, BindingFlags bindingFlags)
        {
            return t
                   .GetType()
                   .GetMembers(bindingFlags)
                   .Where(prop => prop.Name.Equals(memberName, StringComparison.OrdinalIgnoreCase) && prop.MemberType == memberType)
                   .ToList();
        }
    }
}
