using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using SwissAcademic;
using SwissAcademic.Citavi;
using SwissAcademic.Citavi.Shell;
using SwissAcademic.Controls;
using SwissAcademic.Drawing;
using System.Windows.Forms;
using ImportJournalsAddon.Properties;

namespace ImportJournalsAddon
{
    public class ImportJournalsAddon : CitaviAddOn
    {
        #region Fields

        CommandbarMenu _importJournalsMenu;
        CommandbarButton _importJournalsByFileButton;
        CommandbarButton _importJournalsByPubMedButton;
        CommandbarButton _importJournalsByWoodwardButton;

        #endregion

        #region Properties
        public override AddOnHostingForm HostingForm
        {
            get { return AddOnHostingForm.PeriodicalList; }
        }

        #endregion

        #region Methods

        protected override void OnApplicationIdle(System.Windows.Forms.Form form)
        {
            base.OnApplicationIdle(form);
        }

        protected override void OnBeforePerformingCommand(BeforePerformingCommandEventArgs e)
        {

            if (Program.ProjectShells.Count != 0)
            {
                var form = e.Form;
                var project = Program.ActiveProjectShell.Project;

                if (e.Key.Equals(AddonKeys.ImportJournalsByFile, StringComparison.OrdinalIgnoreCase))
                {
                    ImportJournalsByFileMacro.Run(form, project);
                    e.Handled = true;
                }
                if (e.Key.Equals(AddonKeys.ImportJournalsByPubMed, StringComparison.OrdinalIgnoreCase))
                {
                    ImportJournalsByPubMedMacro.Run(form, project);
                    e.Handled = true;
                }
                if (e.Key.Equals(AddonKeys.ImportJournalsByWoodward, StringComparison.OrdinalIgnoreCase))
                {
                    ImportJournalsByWoodwardMacro.Main(Program.ActiveProjectShell, project);
                    e.Handled = true;
                }
            }

            base.OnBeforePerformingCommand(e);
        }

        protected override void OnChangingColorScheme(System.Windows.Forms.Form form, ColorScheme colorScheme)
        {
            base.OnChangingColorScheme(form, colorScheme);
        }

        protected override void OnHostingFormLoaded(System.Windows.Forms.Form form)
        {
            if (form is PeriodicalList periodicalList)
            {
                _importJournalsMenu = periodicalList.GetCommandbar(PeriodicalListCommandbarId.Menu).GetCommandbarMenu(PeriodicalListCommandbarMenuId.Periodicals).InsertCommandbarMenu(3, AddonKeys.ImportJournalsAddonMenu, ImportJournalsStrings.ImportJournalsMenu, image: Resources.addon);
                _importJournalsByFileButton = _importJournalsMenu?.AddCommandbarButton(AddonKeys.ImportJournalsByFile, ImportJournalsStrings.ImportJournalsByFileCommandText);
                _importJournalsByPubMedButton = _importJournalsMenu?.AddCommandbarButton(AddonKeys.ImportJournalsByPubMed, ImportJournalsStrings.ImportJournalsByPubMedCommandText);
                _importJournalsByWoodwardButton = _importJournalsMenu?.AddCommandbarButton(AddonKeys.ImportJournalsByWoodward, ImportJournalsStrings.ImportJournalsByWoodwardLibraryCommandText);
            }

            base.OnHostingFormLoaded(form);
        }

        protected override void OnLocalizing(System.Windows.Forms.Form form)
        {
            if (_importJournalsMenu != null) _importJournalsMenu.Text = ImportJournalsStrings.ImportJournalsMenu;
            if (_importJournalsByFileButton != null) _importJournalsByFileButton.Text = ImportJournalsStrings.ImportJournalsByFileCommandText;
            if (_importJournalsByPubMedButton != null) _importJournalsByPubMedButton.Text = ImportJournalsStrings.ImportJournalsByPubMedCommandText;
            if (_importJournalsByWoodwardButton != null) _importJournalsByWoodwardButton.Text = ImportJournalsStrings.ImportJournalsByWoodwardLibraryCommandText;

            base.OnLocalizing(form);
        }

        #endregion
    }
}