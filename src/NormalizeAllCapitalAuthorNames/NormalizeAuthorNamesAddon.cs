using System;
using SwissAcademic.Citavi.Shell;
using SwissAcademic.Controls;
using SwissAcademic.Drawing;
using System.Windows.Forms;
using NormalizeAllCapitalAuthorNamesAddon.Properties;

namespace NormalizeAllCapitalAuthorNamesAddon
{
    public class NormalizeAuthorNamesAddon : CitaviAddOn
    {
        #region fields

        CommandbarButton _normalizeAuthorNamesButton;

        #endregion

        #region Properties
        public override AddOnHostingForm HostingForm
        {
            get { return AddOnHostingForm.PersonList; }
        }

        #endregion

        #region Methods

        protected override void OnApplicationIdle(System.Windows.Forms.Form form)
        {
            base.OnApplicationIdle(form);
        }

        protected override void OnBeforePerformingCommand(BeforePerformingCommandEventArgs e)
        {
            if (e.Key.Equals(AddonKeys.NormalizeAuthorNamesButton, StringComparison.OrdinalIgnoreCase))
            {
                NormalizeAllCapitalAuthorNamesMacro.Run(e.Form, Program.ActiveProjectShell.Project);
                e.Handled = true;
            }

            base.OnBeforePerformingCommand(e);
        }

        protected override void OnChangingColorScheme(System.Windows.Forms.Form form, ColorScheme colorScheme)
        {
            base.OnChangingColorScheme(form, colorScheme);
        }

        protected override void OnHostingFormLoaded(System.Windows.Forms.Form form)
        {
            if (form is PersonList personListFrom)
            {
                _normalizeAuthorNamesButton = personListFrom
                                                .GetCommandbar(PersonListCommandbarId.Menu)
                                                .GetCommandbarMenu(PersonListCommandbarMenuId.Persons)
                                                .InsertCommandbarButton(2, AddonKeys.NormalizeAuthorNamesButton, AddonStrings.NormalizeAuthorNamesCommandText, image: Resources.addon);
            }

            base.OnHostingFormLoaded(form);
        }

        protected override void OnLocalizing(System.Windows.Forms.Form form)
        {
            if (_normalizeAuthorNamesButton != null) _normalizeAuthorNamesButton.Text = AddonStrings.NormalizeAuthorNamesCommandText;

            base.OnLocalizing(form);
        }

        #endregion
    }
}