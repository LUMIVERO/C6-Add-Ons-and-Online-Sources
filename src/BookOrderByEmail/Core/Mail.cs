using System.Collections.Generic;

namespace SwissAcademic.Addons.BookOrderByEmail
{
    public class Mail
    {
        #region Constructors

        public Mail()
        {
            Attachments = new List<string>();
            To = new List<string>();
        }

        #endregion

        #region Properties

        public string Body { get; set; }

        public string Subject { get; set; }

        public List<string> To { get; }

        public List<string> Attachments { get; }

        #endregion
    }
}
