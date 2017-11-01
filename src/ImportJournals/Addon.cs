using SwissAcademic.Addons.ImportJournals.Properties;
using SwissAcademic.Citavi.Shell;
using SwissAcademic.Controls;
using System.Windows.Forms;

namespace SwissAcademic.Addons.ImportJournals
{
    public class Addon : CitaviAddOn
    {
        #region Constants

        const string Key_CommandbarMenu = "SwissAcademic.Addons.ImportJournals.CommandbarMenu";
        const string Key_CommandbarButtonByFile = "SwissAcademic.Addons.ImportJournals.CommandbarButtonByFile";
        const string Key_CommandbarButtonByPubmed = "SwissAcademic.Addons.ImportJournals.CommandbarButtonByPubmed";
        const string Key_CommandbarButtonByWoodward = "SwissAcademic.Addons.ImportJournals.CommandbarButtonByWoodward";

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
            if (e.Form is MainForm mainForm)
            {
                e.Handled = true;
                switch (e.Key)
                {
                    case (Key_CommandbarButtonByFile):
                        {
                            ImportJournalsByFileMacro.Run(mainForm);
                        }
                        break;

                    case (Key_CommandbarButtonByPubmed):
                        {
                            ImportJournalsByPubMedMacro.Run(mainForm);
                        }
                        break;

                    case (Key_CommandbarButtonByWoodward):
                        {
                            ImportJournalsByWoodwardMacro.Run(mainForm);
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
                                                    .InsertCommandbarMenu(3, Key_CommandbarMenu, ImportJournalsResources.ImportJournalsMenu, image: ImportJournalsResources.addon);
                _importJournalsByFileButton = _importJournalsMenu?.AddCommandbarButton(Key_CommandbarButtonByFile, ImportJournalsResources.ImportJournalsByFileCommandText);
                _importJournalsByPubMedButton = _importJournalsMenu?.AddCommandbarButton(Key_CommandbarButtonByPubmed, ImportJournalsResources.ImportJournalsByPubMedCommandText);
                _importJournalsByWoodwardButton = _importJournalsMenu?.AddCommandbarButton(Key_CommandbarButtonByWoodward, ImportJournalsResources.ImportJournalsByWoodwardLibraryCommandText);
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