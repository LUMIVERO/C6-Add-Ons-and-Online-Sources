// ######################################
// #                                    #
// #    Copyright                       #
// #    Daniel Lutz                     #
// #    Swiss Academic Software GmbH    #
// #    2014                            #
// #                                    #
// ######################################

namespace ManageMacrosAddon
{
    public class MacroCommand
    {
        #region Constructors

        public MacroCommand(string macroPath, MacroAction macroAction)
        {
            MacroPath = macroPath;
            MacroAction = macroAction;
        }

        #endregion

        #region Properties

        #region MacroPath

        public string MacroPath { get; private set; }

        #endregion

        #region MacroAction

        public MacroAction MacroAction { get; private set; }

        #endregion

        #endregion
    }
}
