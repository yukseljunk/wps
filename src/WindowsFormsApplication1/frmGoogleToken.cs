using System;
using System.Windows.Forms;

namespace WordpressScraper
{
    public partial class frmGoogleToken : Form
    {
        public frmGoogleToken()
        {
            InitializeComponent();
        }
        public string Code { get; set; }

        public string Url{get;set; }

        private void frmGoogleToken_Load(object sender, EventArgs e)
        {
            wbBrowser.Navigate(Url);
        }

        private void wbBrowser_DocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs e)
        {
            if (wbBrowser.Document != null)
            {
                var codeElement=wbBrowser.Document.GetElementById("code");
                if (codeElement != null)
                {
                    this.DialogResult=DialogResult.OK;
                    Code = codeElement.GetAttribute("value");
                    this.Close();
                }

            }
        }
    }
}
