using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FatRaccoon.Network
{
    [CreateAssetMenu(fileName = "EndPointSO", menuName = "ProtocolData/EndPointSO", order = 1)]

    public class EndPointSO : ScriptableObject
    {
        public string authToken;
        public List<EndPointData> urlDatas;
    }

    [Serializable]
    public class EndPointData
    {
        public EndPointType type;
        public string url;
    }

    public enum EndPointType
    {
        Test = 0,
        Test2,
    }
}