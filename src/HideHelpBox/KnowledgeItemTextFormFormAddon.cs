using SwissAcademic.Citavi.Shell;
using System;
using System.Windows.Forms;

namespace SwissAcademic.Addons.HideHelpBox
{
    public class KnowledgeItemTextFormFormAddon : CitaviAddOn
    {
        public override AddOnHostingForm HostingForm => AddOnHostingForm.KnowledgeItemTextForm;

        #region Methods
        protected override void OnHostingFormLoaded(Form form)
        {
            if (form is KnowledgeItemTextForm knowledgeItemTextForm)
            {
                try
                {
                    knowledgeItemTextForm.HelpBox.Width = 0;
                }
                catch (Exception)
                {
                }
            }

            base.OnHostingFormLoaded(form);
        }

        #endregion
    }
}
