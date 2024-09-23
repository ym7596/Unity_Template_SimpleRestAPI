using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using FatRaccoon.Network;
using Newtonsoft.Json.Linq;
using UnityEngine;
using UnityEngine.Networking;

public enum WebRequestResult
{
    Success = 200,
    BadRequest = 400,
    Forbidden = 403,
    NotFound = 404,
    ServerError = 500
}

public enum ProtocolMethod
{
    Get,
    Post,
    Put,
}

public class ProtocolRest
{
   
   //프로토콜 통신을 위한 준비 나열
    // 웹패킷 필요 (보내고 받을 데이터가 있는 스크립트)
    // WebRequest를 만드는 곳.

    protected void SetHeaders(UnityWebRequest req, Dictionary<string, string> headers)
    {
        if (req == null || headers == null || headers.Count == 0)
            return;

        foreach (var header in headers)
        {
            var headerKey = req.GetRequestHeader(header.Key);
            
            if(string.IsNullOrEmpty(headerKey))
                req.SetRequestHeader(header.Key,header.Value);
        }
    }

    protected void SetBody(UnityWebRequest req, string body)
    {
        if (req == null || string.IsNullOrEmpty(body))
            return;

        byte[] data = Encoding.UTF8.GetBytes(body);

        if (data.Length > 0)
        {
            req.uploadHandler = new UploadHandlerRaw(data);
            req.SetRequestHeader("Content-Type","application/json");
        }
    }
    
    public virtual IEnumerator Post(WebPacket packet,string body, Action<WebRequestResult,JObject> onComplete)
    {
        yield return RequestBufferCo(ProtocolMethod.Post , packet,body, 
            (result, buffer) =>
            {
                WebRequestResult reqResult;
                JObject data = null;
                packet.SetDownHandler(buffer);
                if (result == UnityWebRequest.Result.Success)
                {
                    data = packet.Deserialize();
                    reqResult = WebRequestResult.Success;
                    onComplete?.Invoke(WebRequestResult.Success,data);
                }
                else
                {
                    //get error
                    data = default;
                    Debug.Log($"Protocol Error : {result.ToString()}");
                }
                
            });
    }

    public virtual IEnumerator Get(WebPacket packet, Action<WebRequestResult,JObject> onComplete)
    {
        yield return RequestBufferCo(ProtocolMethod.Get, packet,null, 
            (result, buffer) =>
            {
                WebRequestResult reqResult;
                JObject data = null;
                packet.SetDownHandler(buffer);
                if (result == UnityWebRequest.Result.Success)
                {
                    data = packet.Deserialize();
                    onComplete?.Invoke(WebRequestResult.Success,data);
                }
                else
                {
                    //get error
                    data = default;
                    Debug.Log($"Protocol Error : {result.ToString()}");
                }
                
            });
    }
    
    public virtual IEnumerator GetArray(WebPacket packet, Action<WebRequestResult, JArray> onComplete)
    {
        yield return RequestBufferCo(ProtocolMethod.Get, packet, null,
            (result, buffer) =>
            {
                JArray data = null;
                packet.SetDownHandler(buffer);

                if (result == UnityWebRequest.Result.Success)
                {
                    try
                    {
                        // JSON 배열로 디시리얼라이즈 시도
                        data = packet.DeserializeArray();
                        onComplete?.Invoke(WebRequestResult.Success, data);
                    }
                    catch (Exception ex)
                    {
                        Debug.LogError($"JSON Parsing Error: {ex.Message}");
                        //적절히 에러 enum은 수정할것.
                        onComplete?.Invoke(WebRequestResult.NotFound, null);
                    }
                }
                else
                {
                    // 에러 처리
                    data = default;
                    Debug.Log($"Protocol Error : {result.ToString()}");
                    //적절히 에러 enum은 수정할것.
                    onComplete?.Invoke(WebRequestResult.ServerError, null);
                }
            });
    }

    public virtual IEnumerator Put(WebPacket packet, Action<WebRequestResult,JObject> onComplete)
    {
        yield return RequestBufferCo(ProtocolMethod.Put , packet,null, 
            (result, buffer) =>
            {
                WebRequestResult reqResult;
                JObject data = null;
                packet.SetDownHandler(buffer);
                if (result == UnityWebRequest.Result.Success)
                {
                    data = packet.Deserialize();
                    reqResult = WebRequestResult.Success;
                    onComplete?.Invoke(WebRequestResult.Success,data);
                }
                else
                {
                    //get error
                    data = default;
                    Debug.Log($"Protocol Error : {result.ToString()}");
                    onComplete?.Invoke(WebRequestResult.BadRequest,null);
                }
                
            });
    }

    protected IEnumerator RequestBufferCo(ProtocolMethod method, WebPacket packet,
         string body,
        Action<UnityWebRequest.Result, DownloadHandlerBuffer> onComplete)
    {
        if(packet == null)
            yield break;

        var url = packet.Url;
        var headers = packet.Headers;
        using var req = new UnityWebRequest(url, method.ToString());

        switch (method)
        {
            case ProtocolMethod.Post:
            case ProtocolMethod.Put:
            {
                SetBody(req,body);
                break;
            }
        }
      
        SetHeaders(req,headers);
        req.downloadHandler = new DownloadHandlerBuffer();

        yield return req.SendWebRequest();

        DownloadHandlerBuffer downloadHandler = null;

        if (req.result == UnityWebRequest.Result.Success)
            downloadHandler = (DownloadHandlerBuffer)req.downloadHandler;
        
        onComplete?.Invoke(req.result,downloadHandler);
    }
    
}
