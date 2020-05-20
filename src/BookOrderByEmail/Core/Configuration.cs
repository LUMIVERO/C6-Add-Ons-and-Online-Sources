using System.Collections.Generic;

namespace SwissAcademic.Addons.BookOrderByEmailAddon
{
    internal class Configuration
    {
        #region Constants

        const string key_receiver = "SwissAcademic.Addons.BookOrderByEmail.Configuration.Receiver";
        const string key_body = "SwissAcademic.Addons.BookOrderByEmail.Configuration.Body";

        #endregion

        #region Fields

        readonly Dictionary<string, string> _settings;

        #endregion

        #region Constructors

        public Configuration(Dictionary<string, string> settings) => _settings = settings;

        #endregion

        #region Properties

        public string Receiver
        {
            get => _settings.GetSafe(key_receiver, string.Empty);
            set => _settings.AddSafe(key_receiver, value);
        }

        public string Body
        {
            get => _settings.GetSafe(key_body, string.Empty);
            set => _settings.AddSafe(key_body, value);
        }

        #endregion
    }
}
