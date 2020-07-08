using SwissAcademic.Controls;
using System;
using System.IO;
using System.Linq;

namespace SwissAcademic.Addons.MacroManagerAddon
{
    internal static class DirectoryConverter
    {
        public static void Travers(CommandbarMenu commandbarMenu, int index, ref int folderCounter, ref int fileCounter, string path, MacroContainer container, bool isFirst)
        {
            try
            {
                foreach (var directoryInfo in Directory.GetDirectories(path).Select(directory => new DirectoryInfo(directory)))
                {
                    if (directoryInfo.GetFiles("*.cs", SearchOption.AllDirectories).Length == 0)
                    {
                        continue;
                    }

                    folderCounter++;

                    var menu = commandbarMenu.InsertCommandbarMenu(index, Addon.MenuKey.FormatString(folderCounter), directoryInfo.Name);

                    index += 1;

                    if (isFirst)
                    {
                        menu.Tool.InstanceProps.IsFirstInGroup = true;
                        isFirst = false;
                    }

                    container.Tools.Add(menu.Tool, null);
                    Travers(menu, 0, ref folderCounter, ref fileCounter, directoryInfo.FullName, container, isFirst);
                }

                foreach (var file in Directory.GetFiles(path, "*.cs"))
                {
                    fileCounter++;

                    var key = Addon.MenuKey.FormatString(folderCounter) + "." + fileCounter;
                    var menu = commandbarMenu.InsertCommandbarMenu(index, key, Path.GetFileName(file) , image: Properties.Resources.Macro);
                    index += 1;
                    if (isFirst)
                    {
                        menu.Tool.InstanceProps.IsFirstInGroup = true;
                        isFirst = false;
                    }

                    container.Tools.Add(menu.Tool, null);
                    key = Addon.ButtonKey + "." + fileCounter + ".1";

                    var button = menu.AddCommandbarButton(key, Properties.Resources.EditCommand);
                    container.Macros.Add(button.Tool.Key, new Macro(file, MacroAction.Edit));
                    container.Tools.Add(button.Tool, "EditCommand");
                    key = Addon.ButtonKey + "." + fileCounter + ".2";
                    button = menu.AddCommandbarButton(Addon.ButtonKey + "." + fileCounter + ".2", Properties.Resources.RunCommand);
                    container.Macros.Add(button.Tool.Key, new Macro(file, MacroAction.Run));
                    container.Tools.Add(button.Tool, "RunCommand");
                }
            }
            catch (Exception)
            {

            }
        }
    }
}
