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

            var applicationNames =GetApplicationNames(fileExtension);

            foreach (var applicationName in applicationNames)
            {
                var subKeyName = Registry.ClassesRoot.OpenSubKey("Applications")?.GetSubKeyNames().FirstOrDefault(name => name.Equals(applicationName, StringComparison.OrdinalIgnoreCase));
                if (string.IsNullOrEmpty(subKeyName)) continue;

                var applicationPath = Registry.ClassesRoot.OpenSubKey("Applications").OpenSubKey(subKeyName)?.OpenSubKey("shell")?.OpenSubKey("open")?.OpenSubKey("command")?.GetValue(string.Empty)?.ToString();
                if (string.IsNullOrEmpty(applicationPath))
                {
                    applicationPath = Registry.ClassesRoot.OpenSubKey("Applications").OpenSubKey(subKeyName)?.OpenSubKey("shell")?.OpenSubKey("edit")?.OpenSubKey("command")?.GetValue(string.Empty)?.ToString();
                }

                if (string.IsNullOrEmpty(applicationPath))
                    continue;
                applications.Add(Application.Analyze(applicationPath));
            }

            applications.RemoveAll(application => application == null);

            return applications;
        }

        static IEnumerable<string> GetApplicationNames(string fileExtension)
        {
            if (string.IsNullOrEmpty(fileExtension)) return Enumerable.Empty<string>();

            var applicationNames = new List<string>();
            applicationNames.AddRange(GetApplicationNamesFromOpenWithLists(fileExtension));
            applicationNames.AddRange(GetApplicationNamesFromOpenWithListSubKeys(fileExtension));
            return applicationNames.Distinct().ToList();
        }
        
        static IEnumerable<string> GetApplicationNamesFromOpenWithLists(string fileExtension)
        {
            var applicationNames = new List<string>();

            foreach (var key in GetOpenWithListsRegistryKeys(fileExtension))
            {
                var names = key
                              .GetValueNames()
                              .Where(name => !name.Equals("MRUList",StringComparison.OrdinalIgnoreCase))
                              .Select(name => key.GetValue(name).ToString())
                              .Where(name =>!string.IsNullOrEmpty(name) && name.EndsWith(".exe",StringComparison.OrdinalIgnoreCase))
                              .ToList();

                foreach (var name in names)
                {
                    if (applicationNames.Any(applicationName => applicationName.Equals(name,StringComparison.OrdinalIgnoreCase)) == false) applicationNames.Add(name);
                }
            }

            return applicationNames;
        }

        static IEnumerable<string> GetApplicationNamesFromOpenWithListSubKeys(string fileExtension)
        {
            var applicationNames = new List<string>();

            foreach (var key in GetOpenWithListsRegistryKeys(fileExtension))
            {
                var names = key
                              .GetSubKeyNames()
                              .Where(name => !string.IsNullOrEmpty(name) && name.EndsWith(".exe", StringComparison.OrdinalIgnoreCase))
                              .Select(name => name)
                              .ToList();

                foreach (var name in names)
                {
                    if (applicationNames.Any(applicationName => applicationName.Equals(name, StringComparison.OrdinalIgnoreCase)) == false) applicationNames.Add(name);
                }
            }

            return applicationNames;
        }

        static IEnumerable<RegistryKey> GetOpenWithListsRegistryKeys(string fileExtension)
        {
            var keys = new List<RegistryKey>();

            //Computer\HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\FileExts\[fileExtensions]\OpenWithList
            var key = Registry
                      .CurrentUser
                      .OpenSubKey("Software")
                      ?.OpenSubKey("Microsoft")
                      ?.OpenSubKey("Windows")
                      ?.OpenSubKey("CurrentVersion")
                      ?.OpenSubKey("Explorer")
                      ?.OpenSubKey("FileExts")
                      ?.OpenSubKey(fileExtension)
                      ?.OpenSubKey("OpenWithList");

            if (key != null) keys.Add(key);

            //Computer\HKEY_CURRENT_USER\Software\Classes\.pdf\OpenWithList
            key = Registry
                  .CurrentUser
                  .OpenSubKey("Software")
                  ?.OpenSubKey("Classes")
                  ?.OpenSubKey(fileExtension)
                  ?.OpenSubKey("OpenWithList");

            if (key != null) keys.Add(key);

            //Computer\HKEY_LOCAL_MACHINE\SOFTWARE\Classes\.pdf\OpenWithList
            key = Registry
                 .LocalMachine
                 .OpenSubKey("Software")
                 ?.OpenSubKey("Classes")
                 ?.OpenSubKey(fileExtension)
                 ?.OpenSubKey("OpenWithList");

            if (key != null) keys.Add(key);

            return keys;
        }
    }
}
