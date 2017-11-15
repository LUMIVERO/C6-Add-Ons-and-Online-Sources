using SwissAcademic;
using SwissAcademic.Citavi;
using SwissAcademic.Citavi.Shell;
using SwissAcademic.Controls;
using SwissAcademic.Drawing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace SwissAcademic.Addons.ReferenceEvaluation
{
    public class Addon : CitaviAddOn
    {
        #region Properties
        public override AddOnHostingForm HostingForm => AddOnHostingForm.MainForm;

        #endregion

        #region Methods

        protected override void OnApplicationIdle(Form form)
        {
            base.OnApplicationIdle(form);
        }

        protected override void OnBeforePerformingCommand(BeforePerformingCommandEventArgs e)
        {
            base.OnBeforePerformingCommand(e);
        }

        protected override void OnChangingColorScheme(Form form, ColorScheme colorScheme)
        {
            base.OnChangingColorScheme(form, colorScheme);
        }

        protected override void OnHostingFormLoaded(Form form)
        {
            base.OnHostingFormLoaded(form);
        }

        protected override void OnLocalizing(Form form)
        {
            base.OnLocalizing(form);
        }

        #endregion
    }
}