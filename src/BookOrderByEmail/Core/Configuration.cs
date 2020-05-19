using System.Collections.Generic;

namespace SwissAcademic.Addons.BookOrderByEmailAddon
{
    internal class Configuration
    {
        #region Fields

        readonly Dictionary<string, string> _settings;

        #endregion

        #region Constants

        const string key_receiver = "SwissAcademic.Addons.BookOrderByEmail.Configuration.Receiver";
        const string key_body = "SwissAcademic.Addons.BookOrderByEmail.Configuration.Body";

        #endregion

        #region Constructors

        public Configuration(Dictionary<string, string> settings) => _settings = settings;

        #endregion

        #region Properties

        public string Receiver
        {
            get
            {
                if (_settings.ContainsKey(key_receiver)) return _settings[key_receiver];
                return string.Empty;
            }
            set
            {
                if (_settings.ContainsKey(key_receiver)) { _settings[key_receiver] = value; return; }
                _settings.Add(key_receiver, value);
            }
        }

        public string Body
        {
            get
            {
                if (_settings.ContainsKey(key_body)) return _settings[key_body];
                return string.Empty;
            }
            set
            {
                if (_settings.ContainsKey(key_body)) { _settings[key_body] = value; return; }
                _settings.Add(key_body, value);
            }
        }

        #endregion
    }
}
