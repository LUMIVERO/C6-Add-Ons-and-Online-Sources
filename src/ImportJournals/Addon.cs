using SwissAcademic.Citavi.Shell;
using SwissAcademic.Controls;

namespace SwissAcademic.Addons.ImportJournalsAddon
{
    public class Addon : CitaviAddOn<PeriodicalList>
    {
        #region Constants

        const string Key_Menu_ImportJournals = "SwissAcademic.Addons.ImportJournals.CommandbarMenu";
        const string Key_Button_ImportByFile = "SwissAcademic.Addons.ImportJournals.CommandbarButtonByFile";
        const string Key_Button_ImportByPubmed = "SwissAcademic.Addons.ImportJournals.CommandbarButtonByPubmed";
        const string Key_Button_ImportByWoodward = "SwissAcademic.Addons.ImportJournals.CommandbarButtonByWoodward";

        #endregion

        #region Methods

        public override void OnBeforePerformingCommand(PeriodicalList periodicalList, BeforePerformingCommandEventArgs e)
        {
            e.Handled = true;
            switch (e.Key)
            {
                case Key_Button_ImportByFile:
                    {
                        ImportJournalsByFileMacro.Run(periodicalList);
                    }
                    break;

                case Key_Button_ImportByPubmed:
                    {
                        ImportJournalsByPubMedMacro.Run(periodicalList);
                    }
                    break;

                case Key_Button_ImportByWoodward:
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
                                         .InsertCommandbarMenu(3, Key_Menu_ImportJournals, Properties.Resources.ImportJournalsMenu, image: Properties.Resources.addon);
            menu?.AddCommandbarButton(Key_Button_ImportByFile, Properties.Resources.ImportJournalsByFileCommandText);
            menu?.AddCommandbarButton(Key_Button_ImportByPubmed, Properties.Resources.ImportJournalsByPubMedCommandText);
            menu?.AddCommandbarButton(Key_Button_ImportByWoodward, Properties.Resources.ImportJournalsByWoodwardLibraryCommandText);
        }

        public override void OnLocalizing(PeriodicalList periodicalList)
        {
            var menu = periodicalList.GetCommandbar(PeriodicalListCommandbarId.Menu)
                                          .GetCommandbarMenu(PeriodicalListCommandbarMenuId.Periodicals)
                                          .GetCommandbarMenu(Key_Menu_ImportJournals);

            if (menu != null)
            {
                menu.Text = Properties.Resources.ImportJournalsMenu;

                var button = menu.GetCommandbarButton(Key_Button_ImportByFile);
                if (button != null)
                {
                    button.Text = Properties.Resources.ImportJournalsByFileCommandText;
                }

                button = menu.GetCommandbarButton(Key_Button_ImportByPubmed);
                if (button != null)
                {
                    button.Text = Properties.Resources.ImportJournalsByPubMedCommandText;
                }

                button = menu.GetCommandbarButton(Key_Button_ImportByWoodward);
                if (button != null)
                {
                    button.Text = Properties.Resources.ImportJournalsByWoodwardLibraryCommandText;
                }
            }
        }

        #endregion
    }
}