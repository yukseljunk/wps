using System;
using System.Threading;
using System.Threading.Tasks;
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

        public string Url { get; set; }

        private void frmGoogleToken_Load(object sender, EventArgs e)
        {
            wbBrowser.Navigate(Url);
        }

        private void wbBrowser_DocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs e)
        {
            if (wbBrowser.Document != null)
            {

                var submitButton = wbBrowser.Document.GetElementById("submit_approve_access");
                if (submitButton != null)
                {
                    timer1.Enabled = true;
                }

                var codeElement = wbBrowser.Document.GetElementById("code");
                if (codeElement != null)
                {
                    this.DialogResult = DialogResult.OK;
                    Code = codeElement.GetAttribute("value");
                    this.Close();
                }

            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            var submitButton = wbBrowser.Document.GetElementById("submit_approve_access");
            if (submitButton != null && submitButton.Enabled)
            {
                timer1.Enabled = false;
             submitButton.InvokeMember("click");


            }
        }

    }
}
