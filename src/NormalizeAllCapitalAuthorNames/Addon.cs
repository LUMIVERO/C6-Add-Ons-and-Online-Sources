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
            if (e.Key.Equals(Key_Button_NormalizeAllCapitalAuthorNames, StringComparison.OrdinalIgnoreCase))
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
                .InsertCommandbarButton(2, Key_Button_NormalizeAllCapitalAuthorNames, Resources.NormalizeAuthorNamesCommandText, image: Resources.addon);
        }

        public override void OnLocalizing(PersonList personList)
        {
            var button = personList.GetCommandbar(PersonListCommandbarId.Menu)
                                   .GetCommandbarMenu(PersonListCommandbarMenuId.Persons)
                                   .GetCommandbarButton(Key_Button_NormalizeAllCapitalAuthorNames);

            if (button != null)
            {
                button.Text = Resources.NormalizeAuthorNamesCommandText;
            }
        }
    }
}