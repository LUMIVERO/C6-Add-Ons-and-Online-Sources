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

        #region Fields

        CommandbarMenu _importJournalsMenu;
        CommandbarButton _importJournalsByFileButton;
        CommandbarButton _importJournalsByPubMedButton;
        CommandbarButton _importJournalsByWoodwardButton;

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
                _importJournalsMenu = periodicalList.GetCommandbar(PeriodicalListCommandbarId.Menu)
                                                    .GetCommandbarMenu(PeriodicalListCommandbarMenuId.Periodicals)
                                                    .InsertCommandbarMenu(3, Key_Menu_ImportJournals, ImportJournalsResources.ImportJournalsMenu, image: ImportJournalsResources.addon);
                _importJournalsByFileButton = _importJournalsMenu?.AddCommandbarButton(Key_Button_ImportByFile, ImportJournalsResources.ImportJournalsByFileCommandText);
                _importJournalsByPubMedButton = _importJournalsMenu?.AddCommandbarButton(Key_Button_ImportByPubmed, ImportJournalsResources.ImportJournalsByPubMedCommandText);
                _importJournalsByWoodwardButton = _importJournalsMenu?.AddCommandbarButton(Key_Button_ImportByWoodward, ImportJournalsResources.ImportJournalsByWoodwardLibraryCommandText);
            }

            base.OnHostingFormLoaded(form);
        }

        protected override void OnLocalizing(Form form)
        {
            if (_importJournalsMenu != null) _importJournalsMenu.Text = ImportJournalsResources.ImportJournalsMenu;
            if (_importJournalsByFileButton != null) _importJournalsByFileButton.Text = ImportJournalsResources.ImportJournalsByFileCommandText;
            if (_importJournalsByPubMedButton != null) _importJournalsByPubMedButton.Text = ImportJournalsResources.ImportJournalsByPubMedCommandText;
            if (_importJournalsByWoodwardButton != null) _importJournalsByWoodwardButton.Text = ImportJournalsResources.ImportJournalsByWoodwardLibraryCommandText;

            base.OnLocalizing(form);
        }

        #endregion
    }
}