using SwissAcademic.Citavi.Shell;
using System.Collections.Generic;

namespace SwissAcademic.Addons.HideHelpBox
{

    public class HideHelpBoxAddon : CitaviAddOn<MainForm>
    {
        public override void OnApplicationIdle(MainForm form)
        {
            var openForms = form.OpenForms();

            if (openForms != null)
            {
                openForms.FormsOf<KnowledgeItemFileForm>().ForEach(k => k.HelpBox.Width = 0);
                openForms.FormsOf<KnowledgeItemTextForm>().ForEach(k => k.HelpBox.Width = 0);
            }
            base.OnApplicationIdle(form);
        }

    }
}