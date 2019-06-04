using SwissAcademic.Addons.NormalizeAllCapitalAuthorNames.Properties;
using SwissAcademic.Citavi.Shell;
using SwissAcademic.Controls;
using System;
using System.Windows.Forms;

namespace SwissAcademic.Addons.NormalizeAllCapitalAuthorNames
{
    public class Addon : CitaviAddOn
    {
        #region Constants

        const string Key_Button_NormalizeAllCapitalAuthorNames = "SwissAcademic.Addons.NormalizeAllCapitalAuthorNames.CommandbarButton";

        #endregion

        #region Properties

        public override AddOnHostingForm HostingForm => AddOnHostingForm.PersonList;

        #endregion

        #region Methods

        protected override void OnBeforePerformingCommand(BeforePerformingCommandEventArgs e)
        {
            if (e.Form is PersonList personList && e.Key.Equals(Key_Button_NormalizeAllCapitalAuthorNames, StringComparison.OrdinalIgnoreCase))
            {
                Macro.Run(personList);
                e.Handled = true;
            }

            base.OnBeforePerformingCommand(e);
        }

        protected override void OnHostingFormLoaded(Form form)
        {
            if (form is PersonList personListFrom)
            {
                personListFrom.GetCommandbar(PersonListCommandbarId.Menu)
                              .GetCommandbarMenu(PersonListCommandbarMenuId.Persons)
                              .InsertCommandbarButton(2, Key_Button_NormalizeAllCapitalAuthorNames, NormalizeAllCapitalAuthorNamesResources.NormalizeAuthorNamesCommandText, image: NormalizeAllCapitalAuthorNamesResources.addon);
            }

            base.OnHostingFormLoaded(form);
        }

        protected override void OnLocalizing(Form form)
        {
            if (form is PersonList personListFrom)
            {
                var button = personListFrom.GetCommandbar(PersonListCommandbarId.Menu)
                                           .GetCommandbarMenu(PersonListCommandbarMenuId.Persons)
                                           .GetCommandbarButton(Key_Button_NormalizeAllCapitalAuthorNames);

                if (button != null) button.Text = NormalizeAllCapitalAuthorNamesResources.NormalizeAuthorNamesCommandText;
            }

            base.OnLocalizing(form);
        }

        #endregion
    }
}