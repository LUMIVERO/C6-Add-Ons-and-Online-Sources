namespace SwissAcademic.Addons.ScrollSpeedInPdfPreview
{
    partial class Addon
    {
        void SaveScrollSpeedInSettings()
        {
            if (Settings.ContainsKey(SETTINGS_KEY_SPEED))
            {
                Settings[SETTINGS_KEY_SPEED] = _scrollSpeed.ToString();
            }
            else
            {
                Settings.Add(SETTINGS_KEY_SPEED, _scrollSpeed.ToString());
            }

            if (Settings.ContainsKey(SETTINGS_KEY_ONLYINFULLSCREENMODE))
            {
                Settings[SETTINGS_KEY_ONLYINFULLSCREENMODE] = _onlyInFullScreenMode.ToString();
            }
            else
            {
                Settings.Add(SETTINGS_KEY_ONLYINFULLSCREENMODE, _onlyInFullScreenMode.ToString());
            }
        }

        void LoadScrollSpeedFromSettings()
        {

            if (Settings.TryGetValue(SETTINGS_KEY_SPEED, out string value1) && double.TryParse(value1, out double scrollSpeed))
            {
                _scrollSpeed = scrollSpeed;
            }
            else
            {
                _scrollSpeed = 1;
            }

            if (Settings.TryGetValue(SETTINGS_KEY_ONLYINFULLSCREENMODE, out string value2) && bool.TryParse(value2, out bool onlyInFullScreenMode))
            {
                _onlyInFullScreenMode = onlyInFullScreenMode;
            }
            else
            {
                _onlyInFullScreenMode = false;
            }
        }
    }
}
