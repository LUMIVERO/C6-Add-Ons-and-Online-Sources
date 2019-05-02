using Newtonsoft.Json;
using SwissAcademic.Citavi.Shell;
using SwissAcademic.Citavi.Settings;
using SwissAcademic.Citavi;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Reflection;
using System.Threading.Tasks;
using SwissAcademic.Addons.ReferenceGridFormWorkSpaceEditor;
using SwissAcademic.Controls;

namespace SwissAcademic.Addons.ReferenceGridFormWorkSpaceEditor
{
    public static class Reflection2
    {
        public static T CreateInstanceOf<T>(params object[] args)
        {
            var type = typeof(T);
            var instance = type.Assembly.CreateInstance(
                type.FullName, false,
                BindingFlags.Instance | BindingFlags.NonPublic,
                null, args, null, null);
            return (T)instance;
        }

        public static B Property<T, B>(this T t, string propertyName) where T : class
        {
            var o = t
                   .Members(propertyName, MemberTypes.Property, BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public)
                   .Cast<PropertyInfo>()
                   .FirstOrDefault(prop => prop.Name.Equals(propertyName, StringComparison.OrdinalIgnoreCase))?
                   .GetValue(t);
            return (B)Convert.ChangeType(o, typeof(B));
        }

        public static B Field<T, B>(this T t, string fieldName) where T : class
        {
            var o = t
                   .Members(fieldName, MemberTypes.Field, BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public)
                   .Cast<FieldInfo>()
                   .FirstOrDefault(field => field.Name.Equals(fieldName, StringComparison.OrdinalIgnoreCase))?
                   .GetValue(t);
            return (B)Convert.ChangeType(o, typeof(B));
        }

        public static void Property<T, B>(this T t, string propertyName, B value) where T : class
        {
            t
            .Members(propertyName, MemberTypes.Property, BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public)
            .Cast<PropertyInfo>()
            .FirstOrDefault(prop => prop.Name.Equals(propertyName, StringComparison.OrdinalIgnoreCase))?
            .SetValue(t, value);
        }

        public static void Field<T, B>(this T t, string fieldName, B value) where T : class
        {
            t
            .Members(fieldName, MemberTypes.Field, BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public)
            .Cast<FieldInfo>()
            .FirstOrDefault(field => field.Name.Equals(fieldName, StringComparison.OrdinalIgnoreCase))?
            .SetValue(t, value);
        }

        public static object Invoke<T>(this T t, string methodName, params object[] parameters) where T : class
        {
            var memberInfos = t.Members(methodName, MemberTypes.Method, BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public);

            return memberInfos.Cast<MethodInfo>().FirstOrDefault(method => method.GetParameters().AreEqual(parameters))?.Invoke(t, parameters);
        }

        public static Task InvokeAsync<T>(this T t, string methodName, params object[] parameters) where T : class
        {
            return (Task)t.Invoke(methodName, parameters);
        }

        private static IEnumerable<MemberInfo> Members<T>(this T t, string memberName, MemberTypes memberType, BindingFlags bindingFlags)
        {
            return t
                   .GetType()
                   .GetMembers(bindingFlags)
                   .Where(prop => prop.Name.Equals(memberName, StringComparison.OrdinalIgnoreCase) && prop.MemberType == memberType)
                   .ToList();
        }

        private static bool AreEqual(this ParameterInfo[] parameterInfos, object[] parameters)
        {
            if (parameterInfos?.Length != parameters?.Length) return false;
            for (int i = 0; i < parameterInfos.Length; i++)
            {
                if (parameterInfos[i].ParameterType != (parameters[i]).GetType()) return false;
            }
            return true;
        }
    }
}

public static class Extensions
{
    public static AddonSettings Load(this string settings)
    {
        if (string.IsNullOrEmpty(settings)) return AddonSettings.Default;
        return JsonConvert.DeserializeObject<AddonSettings>(settings);
    }

    public static string ToJson(this AddonSettings settings)
    {
        if (settings == null) return string.Empty;

        return JsonConvert.SerializeObject(settings);
    }

    public static WorkSpace CreateWorkSpaceByName(this ReferenceGridForm referenceGridForm, string name)
    {
        var workSpace = new WorkSpace { Caption = name };
        var mainGrid = referenceGridForm.Field<ReferenceGridForm, SwissAcademic.Controls.UltraGrid>("mainGrid");
        workSpace.GroupByBoxVisible = !mainGrid.DisplayLayout.GroupByBox.Hidden;

        #region Save Visible Columns

        for (int i = 0; i < mainGrid.DisplayLayout.Bands[0].Columns.Count; i++)
        {
            foreach (var column in mainGrid.DisplayLayout.Bands[0].Columns)
            {
                if (column.Header.VisiblePosition == i)
                {
                    //not only do we have to consider visible columns, but also those "hidden" columns that are currently used for grouping
                    if (column.IsGroupByColumn || !column.Hidden)
                    {
                        workSpace.Columns.Add(new ColumnDescriptor(column.Key, Convert.ToInt32(column.Width / Control2.AutoScaleSize.Width)));
                    }
                    break;
                }
            }
        }

        #endregion

        #region Save Sorted Columns

        var sortedColumns = mainGrid.DisplayLayout.Bands[0].SortedColumns;
        var columnDescriptors = workSpace.Columns;
        var sortIndex = 0;
        for (int i = 0; i < sortedColumns.Count; i++)
        {
            var sortedColumn = sortedColumns[i];
            var columnDescriptor = columnDescriptors.FirstOrDefault(item => item.Key == sortedColumn.Key);
            if (columnDescriptor == null) continue;

            columnDescriptor.IsGroupByColumn = sortedColumn.IsGroupByColumn;
            columnDescriptor.SortIndex = sortIndex++;
            columnDescriptor.SortIndicator = (ColumnSortIndicator)sortedColumn.SortIndicator;
        }

        #endregion

        workSpace.AllowUpdate = Program.Settings.ReferenceGridForm.AllowUpdate;

        return workSpace;
    }
}

