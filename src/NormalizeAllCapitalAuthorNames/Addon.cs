using SwissAcademic.Addons.NormalizeAllCapitalAuthorNamesAddon.Properties;
using SwissAcademic.Citavi.Shell;
using SwissAcademic.Controls;
using System;

namespace SwissAcademic.Addons.NormalizeAllCapitalAuthorNamesAddon
{
    public partial class Addon : CitaviAddOn<PersonList>
    {
        public override void OnBeforePerformingCommand(PersonList personList, BeforePerformingCommandEventArgs e)
        {
            if (e.Key.Equals(ButtonKey, StringComparison.OrdinalIgnoreCase))
            {
                e.Handled = true;
                Macro.Run(personList);
            }
        }

        public override void OnHostingFormLoaded(PersonList personList)
        {
            personList
                .GetCommandbar(PersonListCommandbarId.Menu)
                .GetCommandbarMenu(PersonListCommandbarMenuId.Persons)
                .InsertCommandbarButton(2, ButtonKey, Resources.NormalizeAuthorNamesCommandText, image: Resources.addon);
        }

        public override void OnLocalizing(PersonList personList)
        {
            var button = personList.GetCommandbar(PersonListCommandbarId.Menu)
                                   .GetCommandbarMenu(PersonListCommandbarMenuId.Persons)
                                   .GetCommandbarButton(ButtonKey);

            if (button != null)
            {
                button.Text = Resources.NormalizeAuthorNamesCommandText;
            }
        }
    }
}