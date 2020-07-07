using SwissAcademic.Citavi.Shell;
using SwissAcademic.Controls;

namespace SwissAcademic.Addons.ImportJournalsAddon
{
    public partial class Addon : CitaviAddOn<PeriodicalList>
    {
        public override void OnBeforePerformingCommand(PeriodicalList periodicalList, BeforePerformingCommandEventArgs e)
        {
            e.Handled = true;
            switch (e.Key)
            {
                case ButtonKey_File:
                    {
                        ImportJournalsByFileMacro.Run(periodicalList);
                    }
                    break;

                case ButtonKey_PubMed:
                    {
                        ImportJournalsByPubMedMacro.Run(periodicalList);
                    }
                    break;

                case ButtonKey_Woodward:
                    {
                        ImportJournalsByWoodwardMacro.Run(periodicalList);
                    }
                    break;
                default:
                    e.Handled = false;
                    break;
            }
        }

        public override void OnHostingFormLoaded(PeriodicalList periodicalList)
        {
            var menu = periodicalList.GetCommandbar(PeriodicalListCommandbarId.Menu)
                                     .GetCommandbarMenu(PeriodicalListCommandbarMenuId.Periodicals)
                                     .InsertCommandbarMenu(3, MenuKey, Properties.Resources.ImportJournalsMenu, image: Properties.Resources.addon);
            menu?.AddCommandbarButton(ButtonKey_File, Properties.Resources.ImportJournalsByFileCommandText);
            menu?.AddCommandbarButton(ButtonKey_PubMed, Properties.Resources.ImportJournalsByPubMedCommandText);
            menu?.AddCommandbarButton(ButtonKey_Woodward, Properties.Resources.ImportJournalsByWoodwardLibraryCommandText);
        }

        public override void OnLocalizing(PeriodicalList periodicalList)
        {
            var menu = periodicalList.GetCommandbar(PeriodicalListCommandbarId.Menu)
                                     .GetCommandbarMenu(PeriodicalListCommandbarMenuId.Periodicals)
                                     .GetCommandbarMenu(MenuKey);

            if (menu != null)
            {
                menu.Text = Properties.Resources.ImportJournalsMenu;
            }

            var button = menu?.GetCommandbarButton(ButtonKey_File);
            if (button != null)
            {
                button.Text = Properties.Resources.ImportJournalsByFileCommandText;
            }

            button = menu?.GetCommandbarButton(ButtonKey_PubMed);
            if (button != null)
            {
                button.Text = Properties.Resources.ImportJournalsByPubMedCommandText;
            }

            button = menu?.GetCommandbarButton(ButtonKey_Woodward);
            if (button != null)
            {
                button.Text = Properties.Resources.ImportJournalsByWoodwardLibraryCommandText;
            }

        }
    }
}