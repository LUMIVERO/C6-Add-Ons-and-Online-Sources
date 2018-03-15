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

namespace PushAndMerge
{
    public class Addon : CitaviAddOn
    {
        #region Properties
        public override AddOnHostingForm HostingForm
        {
            get { return AddOnHostingForm.MainForm; }
        }

        #endregion

        #region Methods

        protected override void OnApplicationIdle(System.Windows.Forms.Form form)
        {
            base.OnApplicationIdle(form);
        }

        protected override void OnBeforePerformingCommand(BeforePerformingCommandEventArgs e)
        {
            base.OnBeforePerformingCommand(e);
        }

        protected override void OnChangingColorScheme(System.Windows.Forms.Form form, ColorScheme colorScheme)
        {
            base.OnChangingColorScheme(form, colorScheme);
        }

        protected override void OnHostingFormLoaded(System.Windows.Forms.Form form)
        {
            base.OnHostingFormLoaded(form);
        }

        protected override void OnLocalizing(System.Windows.Forms.Form form)
        {
            base.OnLocalizing(form);
        }

        #endregion
    }
}