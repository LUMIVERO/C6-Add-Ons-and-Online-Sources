using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SwissAcademic.Addons.OpenWith
{
    public static class Registry2
    {
        public static IEnumerable<Application> GetOpenWithList(string fileExtension)
        {
            var applications = new List<Application>();

            if (string.IsNullOrEmpty(fileExtension)) return applications;

            var currentregistryKey = Registry
                                     .CurrentUser
                                     .OpenSubKey("Software")
                                     ?.OpenSubKey("Microsoft")
                                     ?.OpenSubKey("Windows")
                                     ?.OpenSubKey("CurrentVersion")
                                     ?.OpenSubKey("Explorer")
                                     ?.OpenSubKey("FileExts")
                                     ?.OpenSubKey(fileExtension)
                                     ?.OpenSubKey("OpenWithList");

            if (currentregistryKey == null) return applications;

            var entries = currentregistryKey
                          .GetValueNames()
                          .Select(name => new StringEntry(name, currentregistryKey.GetValue(name).ToString()))
                          .Where(w => w.IsEmpty == false)
                          .ToList();

            if (entries.Count == 0 || entries.Any(w => w.IsMRUList) == false) return applications;

            var mRUListEntry = entries.FirstOrDefault(w => w.IsMRUList);

            entries.Remove(mRUListEntry);

            var applicationOrderList = mRUListEntry.Value.ToCharArray().Select(c => c.ToString()).ToList();

            currentregistryKey = Registry.ClassesRoot.OpenSubKey("Applications");

            foreach (var applicationEntryName in applicationOrderList)
            {
                var entry = entries.FirstOrDefault(w => w.Name.Equals(applicationEntryName, StringComparison.OrdinalIgnoreCase));
                if (entry == null) continue;
                var applicationPath = currentregistryKey
                    ?.GetSubKeyNames()
                    .Where(name => name.Equals(entry.Value, StringComparison.OrdinalIgnoreCase))
                    ?.Select(n => currentregistryKey?.OpenSubKey(n))
                    ?.FirstOrDefault()
                    ?.OpenSubKey("shell")
                    ?.OpenSubKey("open")
                    ?.OpenSubKey("command")
                    ?.GetValue(string.Empty)
                    ?.ToString();

                if (string.IsNullOrEmpty(applicationPath)) continue;

                applications.Add(Application.Analyze(applicationPath));
            }

            applications.RemoveAll(application => application == null);

            return applications;
        }
    }
}
