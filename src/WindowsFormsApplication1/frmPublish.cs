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
            var programOptionsFactory = new ProgramOptionsFactory();
            _options = programOptionsFactory.Get();

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
                lblStatus.Text = string.Format("Publishing '{0}'", post.Title);
                Application.DoEvents();
                postDal.PublishPost(post);
            }


            if (chkCreateSlide.Checked)
            {
                CreateVideo(posts);
            }

            lblStatus.Text = "Done";
            EnDis();

        }

        private void EnDis(bool enable = true)
        {
            btnPublish.Enabled = enable;
            chkCreateSlide.Enabled = enable;
            pnlSlideShow.Enabled = chkCreateSlide.Checked && enable;
        }

        private void CreateVideo(IList<Post> posts)
        {
            var secondsPerImage = (int)numDurationForEachImage.Value;
            var postDal = new PostDal(new Dal.Dal(MySqlConnectionString));
            Application.DoEvents();
            lblStatus.Text = "Creating temp directories...";
            Application.DoEvents();
            
            if (Directory.Exists(TempFolder))
            {
                Directory.Delete(TempFolder, true);
            }
            Directory.CreateDirectory(TempFolder);

            if (Directory.Exists(InputFolder))
            {
                Directory.Delete(InputFolder, true);
            }
            Directory.CreateDirectory(InputFolder);

            var listFile = AssemblyDirectory + "/" + ListFile;
            File.WriteAllText(listFile, string.Empty);

            Application.DoEvents();
            lblStatus.Text = "Getting post images...";
            Application.DoEvents();
            
            var images = postDal.GetImagePostsForPosts(posts.Select(p => Int32.Parse(p.Id)).ToList());
            var counter = 1;
            foreach (var image in images)
            {
                var url = image.Url;
                if (string.IsNullOrEmpty(url))
                {
                    continue;
                }
                Application.DoEvents();
                lblStatus.Text = "Downloading " + url;
                Application.DoEvents();

                var extension = Path.GetExtension(url);
                using (WebClient webClient = new WebClient())
                {
                    webClient.DownloadFile(url, AssemblyDirectory + "/" + InputFolder + "\\img" + counter + "." + extension);
                }
                counter++;
            }

            Application.DoEvents();
            lblStatus.Text = "Downloading music...";
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
            lblStatus.Text = "Creating shuffled music...";
            Application.DoEvents();

            StartFfmpeg(combineMusicParams, 20);

            File.WriteAllText(listFile, string.Empty);
            var files = Directory.GetFiles(AssemblyDirectory + "/" + InputFolder);
            //first resize images
            var imgFactory = new ImageFactory();

            Application.DoEvents();
            lblStatus.Text = "Creating individual clip from each picture...";
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
            lblStatus.Text = "Combining individual clips...";
            Application.DoEvents();

            //ffmpeg -f concat -i mylist.txt -c copy output.mp4
            var combineParams = string.Format("-y -f concat -i \"{0}\" -c copy \"{1}/output.mp4\"", listFile, TempFolder);
            StartFfmpeg(combineParams, 5);

            Application.DoEvents();
            lblStatus.Text = "Arranging audio fade in and out...";
            Application.DoEvents();

            //index*SecondsPerImage/3 secs SecondsPerImage secs;
            var videoSeconds = index * secondsPerImage / 3;
            var audioFadeOutParams =
                string.Format("-y -i \"{2}/musicShuffled.mp3\" -af afade=t=out:st={0}:d={1} \"{2}/audio.mp3\"",
                    videoSeconds - secondsPerImage * 2, secondsPerImage * 2 - 1, TempFolder);
            StartFfmpeg(audioFadeOutParams, 15);

            Application.DoEvents();
            lblStatus.Text = "Combining audio and video...";
            Application.DoEvents();
            var audioParams = string.Format("-y -i \"{0}/output.mp4\" -i \"{0}/audio.mp3\" -shortest outputwaudio.mp4",
                TempFolder);
            StartFfmpeg(audioParams, 15);

            Directory.Delete(TempFolder, true);
        }

        private static void StartFfmpeg(string firstArgs, int timeout = 2)
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


        private void chkCreateSlide_CheckedChanged(object sender, EventArgs e)
        {
            pnlSlideShow.Enabled = chkCreateSlide.Checked;
        }

        private void frmPublish_Load(object sender, EventArgs e)
        {
            cbCriteria.Items.Clear();
            cbCriteria.Items.AddRange(new object[] { "Newest", "Oldest", "Random" });
            cbCriteria.SelectedIndex = 0;
            lblStatus.Text = "";
            if (_posts != null)
            {
                cbCriteria.Items.Add("Selected Items");
                cbCriteria.Enabled = false;
                cbCriteria.SelectedIndex = 3;
                numNumberOfPosts.Enabled = false;
                numNumberOfPosts.Value = _posts.Count;
            }
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
    }
}
