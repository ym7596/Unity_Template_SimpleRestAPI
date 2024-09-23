using System;
using System.Collections;
using Newtonsoft.Json.Linq;
using UnityEngine;
using UnityEngine.Networking;

namespace FatRaccoon.Network
{
    public class ProtocolUse : ProtocolRest
    {
        public override IEnumerator Get(WebPacket packet,
            Action<WebRequestResult, JObject> onComplete)
        {

            yield return RequestBufferCo(ProtocolMethod.Get, packet, null,
                (result, buffer) =>
                {
                    WebRequestResult reqResult;
                    JObject data = null;
                    packet.SetDownHandler(buffer);
                    if (result == UnityWebRequest.Result.Success)
                    {
                        data = packet.Deserialize();

                        onComplete?.Invoke(WebRequestResult.Success, data);
                    }
                    else
                    {
                        //get error
                        data = default;
                        Debug.Log($"Protocol Error : {result.ToString()}");
                    }

                });
        }


    }

}