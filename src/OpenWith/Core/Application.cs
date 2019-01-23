using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;

namespace SwissAcademic.Addons.OpenWith
{
    public class Application
    {
        #region Constructors

        Application(string name, string path, List<string> parameters)
        {
            Id = Guid.NewGuid().ToString();
            Name = name;
            Path = path;
            Parameters = parameters;
        }

        #endregion

        #region Properties

        public string Id { get; }
        public string Name { get; }

        public string Path { get; }

        public IList<string> Parameters { get; }

        #endregion

        #region Methods

        public static Application Analyze(string path)
        {
            var splitPath = path
                .Split(new char[] { '"' }, StringSplitOptions.RemoveEmptyEntries)
                .ToList();

            splitPath.RemoveAll(s => string.IsNullOrEmpty(s) || string.IsNullOrWhiteSpace(s));

            if (splitPath.Count < 2) return null;

            var fileInfo = new FileInfo(splitPath[0].Trim());
            if (!fileInfo.Exists) return null;
            var productName = FileVersionInfo.GetVersionInfo(fileInfo.FullName)?.ProductName;

            if (splitPath.Count == 2) return new Application(productName, fileInfo.FullName, new List<string> { splitPath[1].Trim() });

            var parameters = new List<string>();

            for (int i = 1; i < splitPath.Count; i++)
            {
                parameters.Add(splitPath[i].Trim());
            }

            return new Application(productName, fileInfo.FullName, parameters);
        }

        public override string ToString()
        {
            return Name;
        }


        public void Run(IEnumerable<string> pathes)
        {
            foreach (var path in pathes)
            {
                var startInfo = new ProcessStartInfo(Path)
                {
                    WindowStyle = ProcessWindowStyle.Normal,
                    Arguments = Parameters.Count != 0 ? string.Join(" ", Parameters.Select(parameter => parameter.FormatWithCheck(path))) : string.Empty
                };

                Process.Start(startInfo);
            }
        }

        #endregion
    }
}
