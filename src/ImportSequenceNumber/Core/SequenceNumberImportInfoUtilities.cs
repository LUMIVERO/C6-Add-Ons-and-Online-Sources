using SwissAcademic.Citavi;
using SwissAcademic.Citavi.Metadata;
using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;

namespace SwissAcademic.Addons.ImportSequenceNumberAddon
{
    public static class SequenceNumberImportInfoUtilities
    {
        public static List<SequenceNumberImportInfo> Load(string projectPath)
        {
            var results = new List<SequenceNumberImportInfo>();

            using (var connection = new SQLiteConnection($"Data Source={projectPath};"))
            {
                connection.Open();
                var sql = "select ID, SequenceNumber from reference";
                using (var command = new SQLiteCommand(sql, connection))
                {
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var sequenceNumber = reader["SequenceNumber"].ToString();
                            var id = reader["id"].ToString();

                            if (string.IsNullOrEmpty(sequenceNumber) || Guid.TryParse(id, out Guid guid) == false) continue;

                            results.Add(
                                new SequenceNumberImportInfo
                                {
                                    SourceId = guid,
                                    Number = sequenceNumber
                                }
                                );
                        }
                    }
                }
            }

            return results;
        }

        public static void LoadTargetReferences(this List<SequenceNumberImportInfo> sequenceNumberImportInfos, ProjectReferenceCollection references)
        {
            foreach (var sequenceNumberInfo in sequenceNumberImportInfos)
            {
                sequenceNumberInfo.TargetReference = references.FirstOrDefault(reference => reference.Id == sequenceNumberInfo.SourceId);
            }

            sequenceNumberImportInfos.RemoveAll(reference => reference.TargetReference == null);
        }

        public static void StoreTargetReferences(this List<SequenceNumberImportInfo> sequenceNumberImportInfos, ReferencePropertyId propertyId)
        {
            foreach (var sequenceNumberInfo in sequenceNumberImportInfos)
            {
                var currentValue = sequenceNumberInfo.TargetReference.GetValue(propertyId).ToString().Trim();

                if (string.IsNullOrEmpty(currentValue))
                {
                    sequenceNumberInfo.TargetReference.SetValue(propertyId, sequenceNumberInfo.Number);
                    sequenceNumberInfo.Success = true;
                }
                else
                {

                    if (currentValue.Equals(sequenceNumberInfo.Number, StringComparison.OrdinalIgnoreCase))
                    {
                        sequenceNumberInfo.Success = true;
                    }
                    else
                    {
                        var newValue = currentValue + " | " + sequenceNumberInfo.Number;
                        sequenceNumberInfo.TargetReference.SetValue(propertyId, newValue);
                        sequenceNumberInfo.Success = true;
                    }
                }

            }
        }

        public static int GetSuccesImportCount(this List<SequenceNumberImportInfo> sequenceNumberImportInfos) => sequenceNumberImportInfos.Count(s => s.Success);
    }
}
