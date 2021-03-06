﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using System.Configuration;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Services;
using Google.Apis.Upload;
using Google.Apis.Util.Store;
using Google.Apis.YouTube.v3;
using Google.Apis.YouTube.v3.Data;
using Newtonsoft.Json;
using System.Diagnostics;
using System.Drawing;

namespace YouTube_kv
{
    public class REST
    {
        public List<YouTubeVideo> lYouTubeResults;
        public static string CallRestMethod(string url)
        {
            HttpWebRequest webrequest = (HttpWebRequest)WebRequest.Create(url);
            webrequest.Method = "GET";
            webrequest.ContentType = "application/x-www-form-urlencoded";
            HttpWebResponse webresponse = (HttpWebResponse)webrequest.GetResponse();
            Encoding enc = System.Text.Encoding.GetEncoding("utf-8");
            StreamReader responseStream = new StreamReader(webresponse.GetResponseStream(),enc);
            string result = string.Empty;
            result = responseStream.ReadToEnd();
            webresponse.Close();
            return result;
        }
        public List<YouTubeVideo> GetVideos(string sPretrazi)
        {
            List<YouTubeVideo> lYTVideos = new List<YouTubeVideo>();
            string url = "https://www.googleapis.com/youtube/v3/search?part=snippet&maxResults=10&q=";
            url += sPretrazi;
            url+= "&regionCode=HR&type=video&key=";
            string sJson = CallRestMethod(url);
            JObject oJson = JObject.Parse(sJson);
            var oVideos = oJson["items"].ToList();
            for (int i = 0; i < oVideos.Count(); i++)
            {
                var sSnippet = oVideos[i]["snippet"];
                var sID = oVideos[i]["id"];
                        lYTVideos.Add(new YouTubeVideo
                        {
                            sVideoLink = (string)sID["videoId"],
                            sVideoTitle = (string)sSnippet["title"],
                            sDescription = (string)sSnippet["description"],
                            sChannelTitle = (string)sSnippet["channelTitle"],
                            sVideoImage = (string)sSnippet["thumbnails"]["default"]["url"],                            
                        });
            }            
            return lYTVideos;     
        }
        
    }
}
