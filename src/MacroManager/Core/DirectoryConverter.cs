using SwissAcademic.Addons.MacroManager.Properties;
using SwissAcademic.Controls;
using System;
using System.IO;

namespace SwissAcademic.Addons.MacroManager
{
    internal static class DirectoryConverter
    {
        public static void Travers(CommandbarMenu commandbarMenu, int index, ref int folderCounter, ref int fileCounter, string path, MacroContainer container, bool isFirst)
        {
            try
            {
                foreach (var directory in Directory.GetDirectories(path))
                {
                    var directoryInfo = new DirectoryInfo(directory);

                    if (directoryInfo.GetFiles("*.cs", SearchOption.AllDirectories).Length == 0) continue;

                    folderCounter++;
                    var menu = commandbarMenu.InsertCommandbarMenu(index, Addon.Key_Menu_Directory.FormatString(folderCounter), directoryInfo.Name);

                    index = index + 1;

                    if (isFirst)
                    {
                        menu.Tool.InstanceProps.IsFirstInGroup = true;
                        isFirst = false;
                    }

                    container.Tools.Add(menu.Tool, null);
                    Travers(menu, 0, ref folderCounter, ref fileCounter, directory, container, isFirst);
                }

                foreach (var strFile in Directory.GetFiles(path, "*.cs"))
                {
                    fileCounter++;

                    var key = Addon.Key_Menu_Directory.FormatString(folderCounter) + "." + fileCounter;
                    var menu = commandbarMenu.InsertCommandbarMenu(index, key, Path.GetFileName(strFile), image: Properties.Resources.Macro);
                    index = index + 1;
                    if (isFirst)
                    {
                        menu.Tool.InstanceProps.IsFirstInGroup = true;
                        isFirst = false;
                    }

                    container.Tools.Add(menu.Tool, null);
                    key = Addon.Key_Button_Directory + "." + fileCounter + ".1";
                    var button = menu.AddCommandbarButton(key, Properties.Resources.EditCommand);
                    container.Macros.Add(button.Tool.Key, new MacroCommand(strFile, MacroAction.Edit));
                    container.Tools.Add(button.Tool, "EditCommand");
                    key = Addon.Key_Button_Directory + "." + fileCounter + ".2";
                    button = menu.AddCommandbarButton(Addon.Key_Button_Directory + "." + fileCounter + ".2", Properties.Resources.RunCommand);
                    container.Macros.Add(button.Tool.Key, new MacroCommand(strFile, MacroAction.Run));
                    container.Tools.Add(button.Tool, "RunCommand");
                }
            }
            catch (Exception)
            {

            }
        }
    }
}
