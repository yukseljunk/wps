/*
 * Copyright 2015 Google Inc. All Rights Reserved.
 *
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 *
 *     http://www.apache.org/licenses/LICENSE-2.0
 *
 *  Unless required by applicable law or agreed to in writing, software
 *  distributed under the License is distributed on an "AS IS" BASIS,
 *  WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 *  See the License for the specific language governing permissions and
 *  limitations under the License.
 *
 */

using System;
using System.IO;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;

using Google.Apis.Auth.OAuth2;
using Google.Apis.Services;
using Google.Apis.Upload;
using Google.Apis.YouTube.v3;
using Google.Apis.YouTube.v3.Data;

namespace Google.Apis.YouTube.Samples
{

    /// <summary>
    /// YouTube Data API v3 sample: upload a video.
    /// Relies on the Google APIs Client Library for .NET, v1.7.0 or higher.
    /// See https://code.google.com/p/google-api-dotnet-client/wiki/GettingStarted
    /// </summary>
    internal class UploadVideo
    {

        [STAThread]
        static void Main(string[] args)
        {
            Console.WriteLine("YouTube Data API: Upload Video");
            Console.WriteLine("==============================");
            Console.WriteLine("usage:");
            Console.WriteLine("YoutubeUpload ");
            try
            {
                new UploadVideo().Run().Wait();
            }
            catch (AggregateException ex)
            {
                foreach (var e in ex.InnerExceptions)
                {
                    Console.WriteLine("Error: " + e.Message);
                }
            }

            Console.WriteLine("Press any key to continue...");
            Console.ReadKey();
        }
        private string _clientSecretJson;
        private const string ClientSecretJsonDefault = "client_secret.json";
        public string ClientSecretJson
        {
            get
            {
                if (string.IsNullOrEmpty(_clientSecretJson))
                {
                    return ClientSecretJsonDefault;
                }
                return _clientSecretJson;
            }
            set { _clientSecretJson = value; }
        }

        private const string VideoTitleDefault = "My Video Title";
        private string _videoTitle;
        public string VideoTitle
        {
            get
            {
                if (string.IsNullOrEmpty(_videoTitle))
                {
                    return VideoTitleDefault;
                }
                return _videoTitle;
            }
            set { _videoTitle = value; }
        }

        private const string VideoDescriptionDefault = "My Video Title";
        private string _videoDescription;
        private string[] _videoTags;

        public string VideoDescription
        {
            get
            {
                if (string.IsNullOrEmpty(_videoDescription))
                {
                    return VideoDescriptionDefault;
                }
                return _videoDescription;
            }
            set { _videoDescription = value; }
        }

        private string[] VideoTagsDefault = new string[] { };
        private int _categoryId;

        public string[] VideoTags
        {
            get
            {
                if (_videoTags == null) return VideoTagsDefault;
                return _videoTags;
            }
            set { _videoTags = value; }
        }

        private const int CategoryIdDefault = 22;

        /// <summary>
        /// See https://developers.google.com/youtube/v3/docs/videoCategories/list
        /// </summary>
        public int CategoryId
        {
            get
            {
                if (_categoryId == 0) return CategoryIdDefault;
                return _categoryId;
            }
            set { _categoryId = value; }
        }

        public bool Public { get; set; }

        public string File { get; set; }

        private async Task Run()
        {
            UserCredential credential;
            using (var stream = new FileStream(ClientSecretJson, FileMode.Open, FileAccess.Read))
            {
                credential = await GoogleWebAuthorizationBroker.AuthorizeAsync(
                    GoogleClientSecrets.Load(stream).Secrets,
                    // This OAuth 2.0 access scope allows an application to upload files to the
                    // authenticated user's YouTube channel, but doesn't allow other types of access.
                    new[] { YouTubeService.Scope.YoutubeUpload },
                    "user",
                    CancellationToken.None
                );
            }

            var i = new BaseClientService.Initializer()
                        {
                            HttpClientInitializer = credential,
                            ApplicationName = Assembly.GetExecutingAssembly().GetName().Name
                        };

            var youtubeService = new YouTubeService(i);


            var video = new Video();
            video.Snippet = new VideoSnippet();
            video.Snippet.Title = VideoTitle;
            video.Snippet.Description = VideoDescription;
            video.Snippet.Tags = VideoTags;
            video.Snippet.CategoryId = CategoryId.ToString();
            video.Status = new VideoStatus();
            video.Status.PrivacyStatus = Public ? "public" : "private";
            var filePath = File; // Replace with path to actual movie file.

            using (var fileStream = new FileStream(filePath, FileMode.Open))
            {
                var videosInsertRequest = youtubeService.Videos.Insert(video, "snippet,status", fileStream, "video/*");
                videosInsertRequest.ProgressChanged += videosInsertRequest_ProgressChanged;
                videosInsertRequest.ResponseReceived += videosInsertRequest_ResponseReceived;

                await videosInsertRequest.UploadAsync();
            }
        }

        void videosInsertRequest_ProgressChanged(Google.Apis.Upload.IUploadProgress progress)
        {
            switch (progress.Status)
            {
                case UploadStatus.Uploading:
                    Console.WriteLine("{0} bytes sent.", progress.BytesSent);
                    break;

                case UploadStatus.Failed:
                    Console.WriteLine("An error prevented the upload from completing.\n{0}", progress.Exception);
                    break;
            }
        }

        void videosInsertRequest_ResponseReceived(Video video)
        {
            Console.WriteLine("Video id '{0}' was successfully uploaded.", video.Id);
        }
    }
}