using SwissAcademic.Addons.ImportJournals.Properties;
using SwissAcademic.Citavi.Shell;
using SwissAcademic.Controls;
using System.Windows.Forms;

namespace SwissAcademic.Addons.ImportJournals
{
    public class Addon : CitaviAddOn
    {
        #region Constants

        const string Key_Menu_ImportJournals = "SwissAcademic.Addons.ImportJournals.CommandbarMenu";
        const string Key_Button_ImportByFile = "SwissAcademic.Addons.ImportJournals.CommandbarButtonByFile";
        const string Key_Button_ImportByPubmed = "SwissAcademic.Addons.ImportJournals.CommandbarButtonByPubmed";
        const string Key_Button_ImportByWoodward = "SwissAcademic.Addons.ImportJournals.CommandbarButtonByWoodward";

        #endregion

        #region Properties

        public override AddOnHostingForm HostingForm => AddOnHostingForm.PeriodicalList;

        #endregion

        #region Methods

        protected override void OnBeforePerformingCommand(BeforePerformingCommandEventArgs e)
        {
            if (e.Form is PeriodicalList periodicalList)
            {
                e.Handled = true;
                switch (e.Key)
                {
                    case (Key_Button_ImportByFile):
                        {
                            ImportJournalsByFileMacro.Run(periodicalList);
                        }
                        break;

                    case (Key_Button_ImportByPubmed):
                        {
                            ImportJournalsByPubMedMacro.Run(periodicalList);
                        }
                        break;

                    case (Key_Button_ImportByWoodward):
                        {
                            ImportJournalsByWoodwardMacro.Run(periodicalList);
                        }
                        break;
                    default:
                        e.Handled = false;
                        break;
                }
            }

            base.OnBeforePerformingCommand(e);
        }

        protected override void OnHostingFormLoaded(Form form)
        {
            if (form is PeriodicalList periodicalList)
            {
                var menu = periodicalList.GetCommandbar(PeriodicalListCommandbarId.Menu)
                                         .GetCommandbarMenu(PeriodicalListCommandbarMenuId.Periodicals)
                                         .InsertCommandbarMenu(3, Key_Menu_ImportJournals, ImportJournalsResources.ImportJournalsMenu, image: ImportJournalsResources.addon);
                menu?.AddCommandbarButton(Key_Button_ImportByFile, ImportJournalsResources.ImportJournalsByFileCommandText);
                menu?.AddCommandbarButton(Key_Button_ImportByPubmed, ImportJournalsResources.ImportJournalsByPubMedCommandText);
                menu?.AddCommandbarButton(Key_Button_ImportByWoodward, ImportJournalsResources.ImportJournalsByWoodwardLibraryCommandText);
            }

            base.OnHostingFormLoaded(form);
        }

        protected override void OnLocalizing(Form form)
        {

            if (form is PeriodicalList periodicalList)
            {
                var menu = periodicalList.GetCommandbar(PeriodicalListCommandbarId.Menu)
                                         .GetCommandbarMenu(PeriodicalListCommandbarMenuId.Periodicals)
                                         .GetCommandbarMenu(Key_Menu_ImportJournals);

                if (menu != null) menu.Text = ImportJournalsResources.ImportJournalsMenu;

                var button = menu?.GetCommandbarButton(Key_Button_ImportByFile);
                if (button != null) button.Text = ImportJournalsResources.ImportJournalsByFileCommandText;

                button = menu?.GetCommandbarButton(Key_Button_ImportByPubmed);
                if (button != null) button.Text = ImportJournalsResources.ImportJournalsByPubMedCommandText;

                button = menu?.GetCommandbarButton(Key_Button_ImportByWoodward);
                if (button != null) button.Text = ImportJournalsResources.ImportJournalsByWoodwardLibraryCommandText;
            }
            base.OnLocalizing(form);
        }

        #endregion
    }
}