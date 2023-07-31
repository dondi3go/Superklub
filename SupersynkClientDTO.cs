using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

#if UNITY_STANDALONE
using UnityEngine;
#endif

namespace Superklub
{
    /// <summary>
    /// 
    /// </summary>
    [Serializable]
    public class SupersynkPropertyDTO
    {
#if UNITY_STANDALONE
        [SerializeField]
#endif
        private string key;
#if !UNITY_STANDALONE
        [JsonPropertyName("key")]
#endif
        public string Key { get { return key; } }

#if UNITY_STANDALONE
        [SerializeField]
#endif
        private string value;
#if !UNITY_STANDALONE
        [JsonPropertyName("value")]
#endif
        public string Value { get { return value; } }

        public SupersynkPropertyDTO(string key, string value)
        {
            this.key = key;
            this.value = value;
        }
    }

    /// <summary>
    /// Store data used for Supersynk Request, 
    /// can be converted into JSON string
    /// </summary>
    [Serializable]
    public class SupersynkClientDTO
    {
#if UNITY_STANDALONE
        [SerializeField]
#endif
        private string client_id = "";
#if !UNITY_STANDALONE
        [JsonPropertyName("client_id")]
#endif
        public string ClientId 
        {
            get { return client_id; }
            private set { client_id = value; } 
        }

#if UNITY_STANDALONE
        [SerializeField]
#endif
        private List<SupersynkPropertyDTO> properties = new List<SupersynkPropertyDTO>();
#if !UNITY_STANDALONE
        [JsonPropertyName("properties")]
#endif
        public List<SupersynkPropertyDTO> Properties 
        {
            get { return properties; }
            set { properties = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        public SupersynkClientDTO(string clientId)
        {
            ClientId = clientId;
        }

        /// <summary>
        /// 
        /// </summary>
        public void AddProperty(string propertyKey, string propertyValue)
        {
            Properties.Add(new SupersynkPropertyDTO(propertyKey, propertyValue));
        }

        /// <summary>
        /// 
        /// </summary>
        public string ToJSONString()
        {
#if UNITY_STANDALONE
            return JsonUtility.ToJson(this);
#else
            return JsonSerializer.Serialize(this);
#endif
        }
    }
}
