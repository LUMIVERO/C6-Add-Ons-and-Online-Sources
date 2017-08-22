// ######################################
// #                                    #
// #    Copyright                       #
// #    Daniel Lutz                     #
// #    Swiss Academic Software GmbH    #
// #    2014                            #
// #                                    #
// ######################################

using Infragistics.Win.UltraWinToolbars;
using SwissAcademic.Controls;
using System;
using System.Collections.Generic;
using System.IO;

namespace ManageMacrosAddon
{
    public static class DirectoryConverter
    {
        public static void Travers(CommandbarMenu root, ref int folderCounter, ref int fileCounter, string path, Dictionary<string, MacroCommand> keyFiles, List<ToolBase> tools,bool isFirst)
        {
            try
            {
                foreach (string strDirectory in Directory.GetDirectories(path))
                {
                    folderCounter++;
                    var menu = root.AddCommandbarMenu(AddonKeys.DirectoryMenu.FormatString(folderCounter), new DirectoryInfo(strDirectory).Name);

                    if (isFirst)
                    {
                        menu.Tool.InstanceProps.IsFirstInGroup = true;
                        isFirst = false;
                    }

                    tools.Add(menu.Tool);
                    Travers(menu, ref folderCounter, ref fileCounter, strDirectory, keyFiles, tools, isFirst);
                }

                foreach (string strFile in Directory.GetFiles(path, "*.cs"))
                {
                    fileCounter++;

                    var key = AddonKeys.DirectoryMenu.FormatString(folderCounter) + "." + fileCounter;
                    var menu = root.AddCommandbarMenu(key, Path.GetFileName(strFile), image: Properties.Resources.Macro);

                    if (isFirst)
                    {
                        menu.Tool.InstanceProps.IsFirstInGroup = true;
                        isFirst = false;
                    }

                    tools.Add(menu.Tool);
                    key = AddonKeys.DirectoryCommand + "." + fileCounter + ".1";
                    var button = menu.AddCommandbarButton(key, "Bearbeiten");
                    keyFiles.Add(button.Tool.Key, new MacroCommand(strFile, MacroAction.Edit));
                    tools.Add(button.Tool);
                    key = AddonKeys.DirectoryCommand + "." + fileCounter + ".2";
                    button = menu.AddCommandbarButton(AddonKeys.DirectoryCommand + "." + fileCounter + ".2", "Ausführen");
                    keyFiles.Add(button.Tool.Key, new MacroCommand(strFile, MacroAction.Run));
                    tools.Add(button.Tool);
                }
            }
            catch (Exception ex)
            {

            }
        }
    }
}
