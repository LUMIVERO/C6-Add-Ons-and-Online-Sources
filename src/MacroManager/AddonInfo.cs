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
    public class AddonInfo
    {
        #region Constructors

        public AddonInfo()
        {
            Path = this.GetType().Assembly.Location;
            Name = System.IO.Path.GetFileNameWithoutExtension(Path);
            Directory = System.IO.Path.GetDirectoryName(Path);
            Configuration = System.IO.Path.Combine(Directory, Name + ".config");
        }

        #endregion

        #region Properties

        #region Configuration

        public string Configuration { get; private set; }

        #endregion

        #region Directory

        public string Directory { get; private set; }

        #endregion

        #region Name

        public string Name { get; private set; }

        #endregion

        #region Path

        public string Path { get; private set; }

        #endregion


        #endregion
    }
}
