using Newtonsoft.Json.Linq;
using UnityEngine.Networking;

namespace FatRaccoon.Network
{
    public interface IWebPacket
    {
        string Url { get; }
        string Authorization { get; set; }
        
        void SetAuthToken(string token);
        T Deserialize<T>();

        JObject Deserialize();
    }
}