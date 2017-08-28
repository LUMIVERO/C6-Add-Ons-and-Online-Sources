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
        public static void Travers(CommandbarMenu commandbarMenu, ref int folderCounter, ref int fileCounter, string path, Dictionary<string, MacroCommand> keyFiles, List<ToolBase> tools,bool isFirst)
        {
            try
            {
                foreach (var directory in Directory.GetDirectories(path))
                {
                    folderCounter++;
                    var menu = commandbarMenu.AddCommandbarMenu(AddonKeys.DirectoryMenu.FormatString(folderCounter), new DirectoryInfo(directory).Name);

                    if (isFirst)
                    {
                        menu.Tool.InstanceProps.IsFirstInGroup = true;
                        isFirst = false;
                    }

                    tools.Add((ToolBase)menu.Tool);
                    Travers(menu, ref folderCounter, ref fileCounter, directory, keyFiles, tools, isFirst);
                }

                foreach (string strFile in Directory.GetFiles(path, "*.cs"))
                {
                    fileCounter++;

                    var key = AddonKeys.DirectoryMenu.FormatString(folderCounter) + "." + fileCounter;
                    var menu = commandbarMenu.AddCommandbarMenu(key, Path.GetFileName(strFile), image: MacroManagerResources.Macro);

                    if (isFirst)
                    {
                        menu.Tool.InstanceProps.IsFirstInGroup = true;
                        isFirst = false;
                    }

                    tools.Add((ToolBase)menu.Tool);
                    key = AddonKeys.DirectoryCommand + "." + fileCounter + ".1";
                    var button = menu.AddCommandbarButton(key, MacroManagerResources.EditCommand);
                    keyFiles.Add(button.Tool.Key, new MacroCommand(strFile, MacroAction.Edit));
                    tools.Add(button.Tool);
                    key = AddonKeys.DirectoryCommand + "." + fileCounter + ".2";
                    button = menu.AddCommandbarButton(AddonKeys.DirectoryCommand + "." + fileCounter + ".2", MacroManagerResources.RunCommand);
                    keyFiles.Add(button.Tool.Key, new MacroCommand(strFile, MacroAction.Run));
                    tools.Add(button.Tool);
                }
            }
            catch (Exception)
            {

            }
        }
    }
}
