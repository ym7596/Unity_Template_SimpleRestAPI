using System.Collections;
using System.Collections.Generic;
using System.Xml.Linq;
using FatRaccoon.Network;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using UnityEngine;

public class ItemSample
{
    public string name;
    public int price;
}
public class TestMan : MonoBehaviour
{
    private ProtocolRest _protocolRest;

    [SerializeField] private EndPointSO endPoint;
    // Start is called before the first frame update
    void Start()
    {
        _protocolRest = new ProtocolRest();
        Test2();
    }

    public void Test2()
    {
        var url = endPoint.urlDatas.Find(x => x.type
                                              == EndPointType.Test2).url;

        WebPacket packet = new WebPacket(url);

        StartCoroutine(Test2Co(packet));

    }

    private IEnumerator Test2Co(WebPacket pk)
    {   
        ItemSample sample = new ItemSample()
        {
            name = "dduR",
            price = 10000
        };

       string json = JsonConvert.SerializeObject(sample);

        yield return _protocolRest.Post(pk, json,
            (result, obj) =>
            {
                if (result == WebRequestResult.Success)
                {
                    Debug.Log(obj.ToString());
                }
            });
    }

    public void Test()
    {
        var url = endPoint.urlDatas.Find(x =>
            x.type == EndPointType.Test).url;

        WebPacket packet = new WebPacket(url);
       // packet.SetAuthToken(endPoint.authToken);

       StartCoroutine(TestCo(packet));

    }

    private IEnumerator TestCo(WebPacket pk)
    {
        yield return _protocolRest.Get(pk, (result, obj) =>
        {
            if (result == WebRequestResult.Success)
            {
                Debug.Log(obj);
            }
        });
    }
}
