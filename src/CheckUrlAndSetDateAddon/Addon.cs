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
using CheckUrlAndSetDateAddon.Properties;

namespace CheckUrlAndSetDateAddon
{
    public class Addon : CitaviAddOn
    {
        #region Fields

        CommandbarButton _CheckUrlAndSetDateButton;

        #endregion

        #region Properties
        public override AddOnHostingForm HostingForm
        {
            get { return AddOnHostingForm.MainForm; }
        }

        #endregion

        #region Methods

        protected override void OnApplicationIdle(Form form)
        {
            base.OnApplicationIdle(form);
        }

        protected override void OnBeforePerformingCommand(BeforePerformingCommandEventArgs e)
        {
            if (e.Key.Equals(Ressource.CheckUrlAndSetDateButton, StringComparison.OrdinalIgnoreCase) && (e.Form is MainForm mainForm))
            {
                if (Program.ProjectShells.Count != 0)
                {
                    CheckUrlAndSetDateStringsMacro.Run(mainForm);
                    e.Handled = true;
                }
            }

            base.OnBeforePerformingCommand(e);
        }

        protected override void OnChangingColorScheme(Form form, ColorScheme colorScheme)
        {
            base.OnChangingColorScheme(form, colorScheme);
        }

        protected override void OnHostingFormLoaded(Form form)
        {

            if (form is MainForm mainForm)
            {
                _CheckUrlAndSetDateButton = mainForm
                                                       .GetMainCommandbarManager()
                                                       .GetReferenceEditorCommandbar(MainFormReferenceEditorCommandbarId.Menu)
                                                       .GetCommandbarMenu(MainFormReferenceEditorCommandbarMenuId.References)
                                                       .InsertCommandbarButton(7, Ressource.CheckUrlAndSetDateButton, CheckUrlAndSetDateStrings.CheckUrlAndSetDateCommandText, image: Ressource.addon);
            }

            base.OnHostingFormLoaded(form);
        }

        protected override void OnLocalizing(Form form)
        {
            if (_CheckUrlAndSetDateButton != null) _CheckUrlAndSetDateButton.Text = CheckUrlAndSetDateStrings.CheckUrlAndSetDateCommandText;

            base.OnLocalizing(form);
        }

        #endregion
    }
}