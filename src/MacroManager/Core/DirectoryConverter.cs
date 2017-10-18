using Infragistics.Win.UltraWinToolbars;
using SwissAcademic.Addons.MacroManager.Properties;
using SwissAcademic.Controls;
using System;
using System.Collections.Generic;
using System.IO;

namespace SwissAcademic.Addons.MacroManager
{
    public static class DirectoryConverter
    {
        public static void Travers(CommandbarMenu commandbarMenu, int index, ref int folderCounter, ref int fileCounter, string path, Dictionary<string, MacroCommand> macroCommands, Dictionary<ToolBase,string> tools, bool isFirst)
        {
            try
            {
                foreach (var directory in Directory.GetDirectories(path))
                {
                    var directoryInfo = new DirectoryInfo(directory);

                    if (directoryInfo.GetFiles("*.cs", SearchOption.AllDirectories).Length == 0) continue;

                    folderCounter++;
                    var menu = commandbarMenu.InsertCommandbarMenu(index, AddonKeys.DirectoryMenu.FormatString(folderCounter), directoryInfo.Name);

                    index = index + 1;

                    if (isFirst)
                    {
                        menu.Tool.InstanceProps.IsFirstInGroup = true;
                        isFirst = false;
                    }

                    tools.Add(menu.Tool,null);
                    Travers(menu, 0, ref folderCounter, ref fileCounter, directory, macroCommands, tools, isFirst);
                }

                foreach (var strFile in Directory.GetFiles(path, "*.cs"))
                {
                    fileCounter++;

                    var key = AddonKeys.DirectoryMenu.FormatString(folderCounter) + "." + fileCounter;
                    var menu = commandbarMenu.InsertCommandbarMenu(index, key, Path.GetFileName(strFile), image: MacroManagerResources.Macro);
                    index = index + 1;
                    if (isFirst)
                    {
                        menu.Tool.InstanceProps.IsFirstInGroup = true;
                        isFirst = false;
                    }

                    tools.Add(menu.Tool,null);
                    key = AddonKeys.DirectoryCommand + "." + fileCounter + ".1";
                    var button = menu.AddCommandbarButton(key, MacroManagerResources.EditCommand);
                    macroCommands.Add(button.Tool.Key, new MacroCommand(strFile, MacroAction.Edit));
                    tools.Add(button.Tool, "EditCommand");
                    key = AddonKeys.DirectoryCommand + "." + fileCounter + ".2";
                    button = menu.AddCommandbarButton(AddonKeys.DirectoryCommand + "." + fileCounter + ".2", MacroManagerResources.RunCommand);
                    macroCommands.Add(button.Tool.Key, new MacroCommand(strFile, MacroAction.Run));
                    tools.Add(button.Tool, "RunCommand");
                }
            }
            catch (Exception)
            {

            }
        }
    }
}
