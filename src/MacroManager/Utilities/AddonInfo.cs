namespace SwissAcademic.Addons.MacroManager
{
    public class AddonInfo
    {
        #region Constructors

        public AddonInfo()
        {
            Path = GetType().Assembly.Location;
            Name = System.IO.Path.GetFileNameWithoutExtension(Path);
            Directory = System.IO.Path.GetDirectoryName(Path);
            Configuration = System.IO.Path.Combine(Directory, Name + ".config");
        }

        #endregion

        #region Properties

        public string Configuration { get; }

        public string Directory { get; }

        public string Name { get; }

        public string Path { get; }

        #endregion
    }
}
