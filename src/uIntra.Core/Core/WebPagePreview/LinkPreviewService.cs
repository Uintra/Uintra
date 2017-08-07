using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Threading;
using System.Windows.Forms;

namespace uIntra.Core.WebPagePreview
{
    public class LinkPreviewService: ILinkPreviewService
    {
        private const int HtmlPagePreviewImageWidthConst = 1500;
        private const int HtmlPagePreviewImageHeightConst = 3000;

        public byte[] GetHtmlPreviewByteArray(string url)
        {
            byte[] result = null;
            if (!string.IsNullOrEmpty(url))
            {
                var th = new Thread(() => {
                    result = Get(url);
                });
                th.SetApartmentState(ApartmentState.STA);
                th.Start();
                th.Join();
            }
            return result;
        }

        private byte[] Get(string htmlPageUrl)
        {
            using (WebBrowser browser = new WebBrowser())
            {
                browser.Width = HtmlPagePreviewImageWidthConst;
                browser.Height = HtmlPagePreviewImageHeightConst;
                browser.ScrollBarsEnabled = false;
                browser.ScriptErrorsSuppressed = true;
                browser.Navigate(htmlPageUrl);

                while (browser.ReadyState != WebBrowserReadyState.Complete)
                {
                    Application.DoEvents();
                }

                return GetScreenByteArray(browser);
            }

        }

        private byte[] GetScreenByteArray(WebBrowser browser)
        {
            using (Graphics graphics = browser.CreateGraphics())
            using (Bitmap bitmap = new Bitmap(browser.Width, browser.Height, graphics))
            {
                Rectangle bounds = new Rectangle(0, 0, bitmap.Width, bitmap.Height);
                browser.DrawToBitmap(bitmap, bounds);
                MemoryStream ms = new MemoryStream();
                bitmap.Save(ms, ImageFormat.Jpeg);
                return ms.ToArray();
            }
        }

    }
}
