using System.Collections.Generic;

namespace SwissAcademic.Addons.SendReferenceByEmail
{
    public class Mail
    {
        #region Constructors

        public Mail()
        {
            Attachments = new List<string>();
        }

        #endregion

        #region Properties

        public string Body { get; set; }

        public string Subject { get; set; }

        public List<string> Attachments { get; }

        #endregion
    }
}
