using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Windows.Forms;
using ImageProcessor;
using Newtonsoft.Json.Linq;
using PttLib;
using PttLib.Helpers;
using WordpressScraper.Dal;
using WordPressSharp.Models;

namespace WordpressScraper
{

    public partial class frmPublish : Form
    {
        private static List<string> MusicUrls = new List<string>()
        {
            "http://www.bensound.com/royalty-free-music?download=littleidea", 
            "http://www.bensound.com/royalty-free-music?download=theelevatorbossanova", 
            "http://www.bensound.com/royalty-free-music?download=brazilsamba", 
            "http://www.bensound.com/royalty-free-music?download=india", 
            "http://www.bensound.com/royalty-free-music?download=clearday", 
            "http://www.bensound.com/royalty-free-music?download=energy", 
            "http://www.bensound.com/royalty-free-music?download=ukulele", 
            "http://www.bensound.com/royalty-free-music?download=happiness", 
            "http://www.bensound.com/royalty-free-music?download=badass", 
            "http://www.bensound.com/royalty-free-music?download=rumble",
            "http://www.bensound.com/royalty-free-music?download=jazzyfrenchy",
        };
        private static string ListFile = "list.txt";
        private static int FadePercentage = 100; //for fps=30, 30*thisvalue/100 frames to fadein/out
        private static string TempFolder = "temp";
        private static string InputFolder = "input";
        private static string OutputFolder = "output";

        private ProgramOptions _options;
        private IList<Post> _posts;

        public frmPublish()
        {
            InitializeComponent();
        }

        private string MySqlConnectionString
        {
            get
            {
                return string.Format("Server={0};Database={1};Uid={2};Pwd={3}; Allow User Variables=True", _options.DatabaseUrl, _options.DatabaseName, _options.DatabaseUser, _options.DatabasePassword);
            }
        }

        public IList<Post> Posts
        {
            get { return _posts; }
            set
            {
                _posts = value;
            }
        }

        private void btnPublish_Click(object sender, EventArgs e)
        {
            EnDis(false);
            txtStatus.Text = "";

            var postDal = new PostDal(new Dal.Dal(MySqlConnectionString));
            var posts = _posts;
            if (_posts == null)
            {
                posts = postDal.GetPosts((PostOrder)cbCriteria.SelectedIndex, (int)numNumberOfPosts.Value);
            }
            else
            {
                posts = postDal.GetPosts(_posts.Select(p => int.Parse(p.Id)).ToList());

            }
            if (posts == null)
            {
                MessageBox.Show("No posts found to publish!");
                EnDis();
                return;
            }

            foreach (var post in posts)
            {
                AddStatus(string.Format("Publishing '{0}'", post.Title));
                Application.DoEvents();
                postDal.PublishPost(post);
            }


            if (chkCreateSlide.Checked)
            {
                CreateVideos(posts);
                if (chkYoutube.Checked)
                {
                    UploadToYoutube();
                }
            }

            AddStatus("Done");
            EnDis();

        }

        private void AddStatus(string input)
        {
            if (txtStatus.Text.Length > 0)
            {
                txtStatus.AppendText(Environment.NewLine);
            }
            txtStatus.AppendText(input);
            if (txtStatus.Text.Length == 0) return;
            txtStatus.SelectionStart = txtStatus.Text.Length - 1; // add some logic if length is 0
            txtStatus.SelectionLength = 0;
        }

        private void UploadToYoutube()
        {
            if (_videosCreated.Count == 0)
            {
                MessageBox.Show("No videos to publish!");
                return;
            }

            //get google token
            GetGoogleToken();

            //todo:arrange youtubeupload.exe.config for proxy

            //run youtubeupload.exe
            foreach (var videoCreated in _videosCreated)
            {
                var title = txtYoutubeTitle.Text;
                if (chkInheritTitle.Checked)
                {
                    if (videoCreated.Value.Count > 0)
                    {
                        title = videoCreated.Value[0].Title;
                    }
                }

                StartYoutubeUpload(string.Format("-f \"{0}\" -r \"{1}\" -s \"{2}\" -i \"{3}\" -t \"{4}\" -d \"{5}\" -a \"{6}\"",
                    videoCreated.Key, txtRefreshToken.Text, _options.YoutubeClientSecret, _options.YoutubeClient, title, txtYoutubeDescription.Text, txtYoutubeTags.Text));
            }

        }

        private void GetGoogleToken()
        {
            //get token
            var googleTokenForm = new frmGoogleToken();
            googleTokenForm.Url =
                string.Format(
                    "https://accounts.google.com/o/oauth2/auth?client_id={0}&redirect_uri=urn:ietf:wg:oauth:2.0:oob&scope=https://gdata.youtube.com&response_type=code&access_type=offline&approval_prompt=force",
                    _options.YoutubeClient);
            googleTokenForm.ShowDialog();
            var tokenCode = "";
            if (googleTokenForm.DialogResult == DialogResult.OK)
            {
                tokenCode = googleTokenForm.Code;
            }
            //1!ZTQgm8smy2:joelmatthewgr@gmail.com
            if (string.IsNullOrEmpty(tokenCode)) return;
            var json = WebHelper.CurlSimplePost("https://accounts.google.com/o/oauth2/token",
                string.Format("code={0}&client_id={1}&client_secret={2}&redirect_uri=urn:ietf:wg:oauth:2.0:oob&grant_type=authorization_code", tokenCode, _options.YoutubeClient, _options.YoutubeClientSecret),
                "accounts.google.com");
            if (string.IsNullOrEmpty(json)) return;
            dynamic d = JObject.Parse(json);
            if (d.refresh_token == null) return;
            txtRefreshToken.Text = d.refresh_token;
        }

        private void EnDis(bool enable = true)
        {
            btnPublish.Enabled = enable;
            chkCreateSlide.Enabled = enable;
            pnlSlideShow.Enabled = chkCreateSlide.Checked && enable;
            pnlYoutube.Enabled = chkYoutube.Checked && enable;
            chkYoutube.Enabled = chkCreateSlide.Checked && enable;

        }

        Dictionary<string, List<Post>> _videosCreated = new Dictionary<string, List<Post>>();

        private void CreateVideos(IList<Post> posts)
        {
            _videosCreated = new Dictionary<string, List<Post>>();
            var postDal = new PostDal(new Dal.Dal(MySqlConnectionString));
            Application.DoEvents();
            AddStatus("Creating temp directories...");
            Application.DoEvents();

            if (Directory.Exists(TempFolder))
            {
                Directory.Delete(TempFolder, true);
            }
            if (Directory.Exists(OutputFolder))
            {
                Directory.Delete(OutputFolder, true);
            }

            Directory.CreateDirectory(TempFolder);
            Directory.CreateDirectory(OutputFolder);

            Application.DoEvents();
            AddStatus("Getting post images...");
            Application.DoEvents();

            var videoPerPost = (int)numVideoPerPost.Value;
            var imagePerPost = (int)numImagePerPost.Value;
            var pageCount = (int)Math.Ceiling((double)posts.Count / videoPerPost);
            for (int pageNo = 0; pageNo < pageCount; pageNo++)
            {
                if (Directory.Exists(InputFolder))
                {
                    Directory.Delete(InputFolder, true);
                }
                Directory.CreateDirectory(InputFolder);
                var postsToTake = posts.Skip(pageNo * videoPerPost).Take(videoPerPost);
                if (postsToTake.Count() == 0) break;
                var images = postDal.GetImagePostsForPosts(postsToTake.Select(p => Int32.Parse(p.Id)).ToList());

                var imagesToTake = new List<Post>();

                foreach (var imageGroup in images.GroupBy(i => i.ParentId))
                {
                    imagesToTake.AddRange(imageGroup.Take(imagePerPost));
                }

                var counter = 1;
                foreach (var image in imagesToTake)
                {
                    var url = image.Url;
                    if (string.IsNullOrEmpty(url))
                    {
                        continue;
                    }
                    Application.DoEvents();
                    AddStatus("Downloading " + url);
                    Application.DoEvents();

                    var extension = Path.GetExtension(url);
                    using (WebClient webClient = new WebClient())
                    {
                        webClient.DownloadFile(url,
                            AssemblyDirectory + "/" + InputFolder + "\\img" + counter + "." + extension);
                    }
                    counter++;
                }
                var videoFileName = OutputFolder + "/outputwaudio" + pageNo + ".mp4";
                CreateVideo(videoFileName);
                _videosCreated.Add(AssemblyDirectory + "/" + videoFileName, postsToTake.ToList());
            }
            Directory.Delete(InputFolder, true);
            Directory.Delete(TempFolder, true);
        }

        private void CreateVideo(string outputFileName)
        {
            var secondsPerImage = (int)numDurationForEachImage.Value;
            var listFile = AssemblyDirectory + "/" + ListFile;
            File.WriteAllText(listFile, string.Empty);

            Application.DoEvents();
            AddStatus("Downloading music...");
            Application.DoEvents();

            // Create a new WebClient instance.
            var myWebClient = new WebClient();
            myWebClient.DownloadFile(MusicUrls[Helper.GetRandomNumber(0, MusicUrls.Count)],
                AssemblyDirectory + "/" + TempFolder + "/music.mp3");

            for (int i = 0; i < 6; i++)
            {
                File.AppendAllText(listFile, string.Format("file '{0}/music.mp3'\r\n", TempFolder));
            }
            var combineMusicParams = string.Format("-y -f concat -i \"{0}\" -c copy \"{1}/musicShuffled.mp3\"", listFile,
                TempFolder);
            Application.DoEvents();
            AddStatus("Creating shuffled music...");
            Application.DoEvents();

            StartFfmpeg(combineMusicParams, 20);

            File.WriteAllText(listFile, string.Empty);
            var files = Directory.GetFiles(AssemblyDirectory + "/" + InputFolder);
            //first resize images
            var imgFactory = new ImageFactory();

            Application.DoEvents();
            AddStatus("Creating individual clip from each picture...");
            Application.DoEvents();

            var index = 1;
            foreach (var file in files)
            {
                imgFactory.Load(file).Resize(new Size((int)numVideoWidth.Value, (int)numVideoHeight.Value)).Save(file);

                //Console.WriteLine(file);
                var firstArgs =
                    string.Format("-y -framerate 1/{2} -i \"{0}\" -c:v libx264 -r 30 -pix_fmt yuv420p \"{3}/out{1}.mp4\"", file,
                        index, secondsPerImage, TempFolder);
                var secondArgs = string.Format("-y -i \"{3}/out{0}.mp4\" -y -vf fade=in:0:{2} \"{3}/out{1}.mp4\"", index,
                    index + 1, 30 * FadePercentage / 100, TempFolder);
                var thirdArgs = string.Format("-y -i \"{4}/out{0}.mp4\" -y -vf fade=out:{3}:{2} \"{4}/out{1}.mp4\"", index + 1,
                    index + 2, 30 * FadePercentage / 100, secondsPerImage * 30 - 30 * FadePercentage / 100, TempFolder);
                StartFfmpeg(firstArgs);
                StartFfmpeg(secondArgs);
                StartFfmpeg(thirdArgs);
                File.AppendAllText(listFile, string.Format("file '{1}/out{0}.mp4'\r\n", index + 2, TempFolder));
                index += 3;
            }

            Application.DoEvents();
            AddStatus("Combining individual clips...");
            Application.DoEvents();

            //ffmpeg -f concat -i mylist.txt -c copy output.mp4
            var combineParams = string.Format("-y -f concat -i \"{0}\" -c copy \"{1}/output.mp4\"", listFile, TempFolder);
            StartFfmpeg(combineParams, 5);

            Application.DoEvents();
            AddStatus("Arranging audio fade in and out...");
            Application.DoEvents();

            //index*SecondsPerImage/3 secs SecondsPerImage secs;
            var videoSeconds = index * secondsPerImage / 3;
            var audioFadeOutParams =
                string.Format("-y -i \"{2}/musicShuffled.mp3\" -af afade=t=out:st={0}:d={1} \"{2}/audio.mp3\"",
                    videoSeconds - secondsPerImage * 2, secondsPerImage * 2 - 1, TempFolder);
            StartFfmpeg(audioFadeOutParams, 15);

            Application.DoEvents();
            AddStatus("Combining audio and video...");
            Application.DoEvents();
            var audioParams = string.Format("-y -i \"{0}/output.mp4\" -i \"{0}/audio.mp3\" -shortest {1}",
                TempFolder, outputFileName);
            StartFfmpeg(audioParams, 15);


        }

        private static void StartFfmpeg(string firstArgs, int timeout = 20)
        {
            var proc = new Process
            {
                StartInfo = new ProcessStartInfo
                {
                    FileName = AssemblyDirectory + "/ffmpeg/ffmpeg.exe",
                    Arguments = firstArgs,
                    UseShellExecute = false,
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                    CreateNoWindow = true
                }
            };

            proc.Start();

            while (!proc.StandardError.EndOfStream)
            {
                string line = proc.StandardError.ReadLine();
                Console.WriteLine(line);
            }

            proc.WaitForExit(timeout * 1000);
        }
        private void StartYoutubeUpload(string firstArgs, int timeout = 40)
        {
            var proc = new Process
            {
                StartInfo = new ProcessStartInfo
                {
                    FileName = AssemblyDirectory + "/YoutubeUtilities/YoutubeUtilities.exe",
                    Arguments = firstArgs,
                    UseShellExecute = false,
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                    CreateNoWindow = true
                }
            };

            proc.Start();

            while (!proc.StandardError.EndOfStream)
            {
                string line = proc.StandardError.ReadLine();
                AddStatus(line);
            }
            while (!proc.StandardOutput.EndOfStream)
            {
                string line = proc.StandardOutput.ReadLine();
                AddStatus(line);
            }

            proc.WaitForExit(timeout * 1000);
        }

        private void chkCreateSlide_CheckedChanged(object sender, EventArgs e)
        {
            pnlSlideShow.Enabled = chkCreateSlide.Checked;
            chkYoutube.Enabled = chkCreateSlide.Checked;
        }

        private void frmPublish_Load(object sender, EventArgs e)
        {
            cbCriteria.Items.Clear();
            cbCriteria.Items.AddRange(new object[] { "Newest", "Oldest", "Random" });
            cbCriteria.SelectedIndex = 0;
            txtStatus.Text = "";
            if (_posts != null)
            {
                cbCriteria.Items.Add("Selected Items");
                cbCriteria.Enabled = false;
                cbCriteria.SelectedIndex = 3;
                numNumberOfPosts.Enabled = false;
                numNumberOfPosts.Value = _posts.Count;
            }
            var programOptionsFactory = new ProgramOptionsFactory();
            _options = programOptionsFactory.Get();
#if DEBUG
            btnGetGoogleToken.Visible = true;
            txtRefreshToken.Visible = true;
#endif
            txtYoutubeDescription.Text = string.Format("See more on {0}\nMusic by http://www.bensound.com/ royalty free license", _options.BlogUrl);
        }

        private void numNumberOfPosts_ValueChanged(object sender, EventArgs e)
        {
            numVideoPerPost.Maximum = numNumberOfPosts.Value;
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            try
            {
                this.Close();
                this.Dispose();
            }
            catch
            {

            }
        }
        public static string AssemblyDirectory
        {
            get
            {
                string codeBase = Assembly.GetExecutingAssembly().CodeBase;
                UriBuilder uri = new UriBuilder(codeBase);
                string path = Uri.UnescapeDataString(uri.Path);
                return Path.GetDirectoryName(path);
            }
        }

        private void chkYoutube_CheckedChanged(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(_options.YoutubeClientSecret) || string.IsNullOrEmpty(_options.YoutubeClient))
            {
                MessageBox.Show("Please set up youtube settings in settings dialog!");
                return;
            }
            pnlYoutube.Enabled = chkYoutube.Checked;
        }

        private void txtStatus_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = true;
        }

        private void btnGetGoogleToken_Click(object sender, EventArgs e)
        {
            GetGoogleToken();
        }

        private void chkInheritTitle_CheckedChanged(object sender, EventArgs e)
        {
            txtYoutubeTitle.Enabled = !chkInheritTitle.Checked;
        }
    }
}
