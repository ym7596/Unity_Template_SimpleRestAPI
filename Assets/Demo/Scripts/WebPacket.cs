using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using UnityEngine.Networking;

namespace FatRaccoon.Network
{
    public class WebPacket : IWebPacket
    {
        public string Url { get; }
        public string Authorization { get; set; }

        public Dictionary<string, string> Headers { get; private set; } = new Dictionary<string, string>();

        public DownloadHandler downloadHandler { get; protected set; }
        
        public WebPacket(string url, string authorizationToken = "")
        {
            Url = url;
            SetAuthToken(authorizationToken);
            SetHeadersDefault();
        }

        public void SetAuthToken(string token)
        {
            if (string.IsNullOrEmpty(token))
                return;

            Authorization = token;
            Headers.Add("Authorization",token);
        }
        
       

        private void SetHeadersDefault()
        {
            Headers.Add("Content-Type","application/json");
        }
      
    

        public virtual T Deserialize<T>()
        {
            var json = downloadHandler?.text;

            return string.IsNullOrEmpty(json) ? default : JsonConvert.DeserializeObject<T>(json);
        }

        public virtual JObject Deserialize()
        {
            var json = downloadHandler?.text;

            return string.IsNullOrEmpty(json) ? null : JObject.Parse(json);
        }

        public void SetDownHandler(DownloadHandler handler)
        {
            downloadHandler = handler;
        }
    }
}