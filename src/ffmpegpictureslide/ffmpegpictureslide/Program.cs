using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Net;
using System.Reflection;
using ImageProcessor;

namespace ffmpegpictureslide
{
    class Program
    {
        private static string ListFile = "list.txt";
        private static int SecondsPerImage = 3;
        private static int FadePercentage = 100; //for fps=30, 30*thisvalue/100 frames to fadein/out
        private static int VideoWidth = 600;
        private static int VideoHeight = 600;
        private static string TempFolder = "temp";

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

        static void Main(string[] args)
        {
            Console.WriteLine("create slides from input folder image...");
            Console.WriteLine("Downloading Music, add credits for http://www.bensound.com/");

            Directory.CreateDirectory(TempFolder);
            var listFile = AssemblyDirectory + "/" + ListFile;
            File.WriteAllText(listFile, string.Empty);
            
            var rand = new Random(DateTime.Now.Millisecond);
            // Create a new WebClient instance.
            var myWebClient = new WebClient();
            myWebClient.DownloadFile(MusicUrls[rand.Next(0, MusicUrls.Count)], AssemblyDirectory + "/" + TempFolder + "/music.mp3");

            for (int i = 0; i < 6; i++)
            {
                File.AppendAllText(listFile, string.Format("file '{0}/music.mp3'\r\n", TempFolder));                
            }

            var combineMusicParams = string.Format("-y -f concat -i \"{0}\" -c copy \"{1}/musicShuffled.mp3\"", listFile, TempFolder);
            StartFfmpeg(combineMusicParams, 20);


            File.WriteAllText(listFile, string.Empty);
            var files = Directory.GetFiles(AssemblyDirectory + "/input");

            //first resize images
            var imgFactory = new ImageFactory();

            var index = 1;
            foreach (var file in files)
            {
                imgFactory.Load(file).Resize(new Size(VideoWidth, VideoHeight)).Save(file);

                //Console.WriteLine(file);
                var firstArgs = string.Format("-y -framerate 1/{2} -i \"{0}\" -c:v libx264 -r 30 -pix_fmt yuv420p \"{3}/out{1}.mp4\"", file, index, SecondsPerImage, TempFolder);
                var secondArgs = string.Format("-y -i \"{3}/out{0}.mp4\" -y -vf fade=in:0:{2} \"{3}/out{1}.mp4\"", index, index + 1, 30 * FadePercentage / 100, TempFolder);
                var thirdArgs = string.Format("-y -i \"{4}/out{0}.mp4\" -y -vf fade=out:{3}:{2} \"{4}/out{1}.mp4\"", index + 1, index + 2, 30 * FadePercentage / 100, SecondsPerImage * 30 - 30 * FadePercentage / 100, TempFolder);
                StartFfmpeg(firstArgs);
                StartFfmpeg(secondArgs);
                StartFfmpeg(thirdArgs);
                File.AppendAllText(listFile, string.Format("file '{1}/out{0}.mp4'\r\n", index + 2, TempFolder));
                index += 3;
            }

            //ffmpeg -f concat -i mylist.txt -c copy output.mp4
            var combineParams = string.Format("-y -f concat -i \"{0}\" -c copy \"{1}/output.mp4\"", listFile, TempFolder);
            StartFfmpeg(combineParams, 5);

            //index*SecondsPerImage/3 secs SecondsPerImage secs;
            var videoSeconds = index * SecondsPerImage / 3;
            var audioFadeOutParams = string.Format("-y -i \"{2}/musicShuffled.mp3\" -af afade=t=out:st={0}:d={1} \"{2}/audio.mp3\"", videoSeconds - SecondsPerImage * 2, SecondsPerImage * 2 - 1, TempFolder);
            StartFfmpeg(audioFadeOutParams, 15);

            var audioParams = string.Format("-y -i \"{0}/output.mp4\" -i \"{0}/audio.mp3\" -shortest outputwaudio.mp4", TempFolder);
            StartFfmpeg(audioParams, 15);
            Directory.Delete(TempFolder, true);

            Console.WriteLine("\r\nREADY!\r\n");
            Console.ReadLine();
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
