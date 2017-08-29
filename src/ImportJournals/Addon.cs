using SwissAcademic.Addons.ImportJournals.Properties;
using SwissAcademic.Citavi.Shell;
using SwissAcademic.Controls;
using SwissAcademic.Drawing;
using System;
using System.Windows.Forms;

namespace SwissAcademic.Addons.ImportJournals
{
    public class Addon : CitaviAddOn
    {
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
            if (Program.ProjectShells.Count != 0)
            {
                if (e.Key.Equals(AddonKeys.CommandbarButtonByFile, StringComparison.OrdinalIgnoreCase))
                {
                    ImportJournalsByFileMacro.Run(e.Form, Program.ActiveProjectShell.Project);
                    e.Handled = true;
                }
                if (e.Key.Equals(AddonKeys.CommandbarButtonByPubmed, StringComparison.OrdinalIgnoreCase))
                {
                    ImportJournalsByPubMedMacro.Run(e.Form, Program.ActiveProjectShell.Project);
                    e.Handled = true;
                }
                if (e.Key.Equals(AddonKeys.CommandbarButtonByWoodward, StringComparison.OrdinalIgnoreCase))
                {
                    ImportJournalsByWoodwardMacro.Main(Program.ActiveProjectShell, Program.ActiveProjectShell.Project);
                    e.Handled = true;
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
                                                    .InsertCommandbarMenu(3, AddonKeys.CommandbarMenu, ImportJournalsResources.ImportJournalsMenu, image: ImportJournalsResources.addon);
                _importJournalsByFileButton = _importJournalsMenu?.AddCommandbarButton(AddonKeys.CommandbarButtonByFile, ImportJournalsResources.ImportJournalsByFileCommandText);
                _importJournalsByPubMedButton = _importJournalsMenu?.AddCommandbarButton(AddonKeys.CommandbarButtonByPubmed, ImportJournalsResources.ImportJournalsByPubMedCommandText);
                _importJournalsByWoodwardButton = _importJournalsMenu?.AddCommandbarButton(AddonKeys.CommandbarButtonByWoodward, ImportJournalsResources.ImportJournalsByWoodwardLibraryCommandText);
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