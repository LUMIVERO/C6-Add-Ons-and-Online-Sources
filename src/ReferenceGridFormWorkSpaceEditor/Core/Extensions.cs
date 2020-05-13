using Infragistics.Win.UltraWinGrid;
using Newtonsoft.Json;
using SwissAcademic.Citavi;
using SwissAcademic.Citavi.Settings;
using SwissAcademic.Citavi.Shell;
using SwissAcademic.Controls;
using System;
using System.Linq;
using UltraGrid = SwissAcademic.Controls.UltraGrid;

namespace SwissAcademic.Addons.ReferenceGridFormWorkSpaceEditorAddon
{
    internal static class Extensions
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
            var mainGrid = referenceGridForm.Field<ReferenceGridForm, UltraGrid>("mainGrid");
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

        public static void LoadWorkSpace(this ReferenceGridForm referenceGridForm, WorkSpace workSpace)
        {
            Program.Settings.ReferenceGridForm.AllowUpdate = workSpace.AllowUpdate;
            Program.Settings.ReferenceGridForm.GroupByBoxVisible = workSpace.GroupByBoxVisible;
            Program.Settings.ReferenceGridForm.ColumnDescriptors.Clear();
            workSpace.Columns.ForEach(cl => Program.Settings.ReferenceGridForm.ColumnDescriptors.Add(cl));
#pragma warning disable CA2000 // Objekte verwerfen, bevor Bereich verloren geht
            referenceGridForm.Invoke("mainGrid_InitializeLayout", new object(), new InitializeLayoutEventArgs(new UltraGridLayout()));
#pragma warning restore CA2000 // Objekte verwerfen, bevor Bereich verloren geht
            referenceGridForm.Invoke("Refresh");
        }
    }
}
