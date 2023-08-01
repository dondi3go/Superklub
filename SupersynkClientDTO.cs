using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

#if UNITY_STANDALONE
using UnityEngine;
#else
using System.Text.Json;
using System.Text.Json.Serialization;
#endif

namespace Superklub
{
    /// <summary>
    /// Store data used in Supersynk Requests, 
    /// can be converted into Json string
    /// </summary>
    [Serializable]
    public class SupersynkClientDTO
    {
#if UNITY_STANDALONE
        [SerializeField]
#endif
        private string client_id = string.Empty;
#if !UNITY_STANDALONE
        [JsonPropertyName("client_id")]
#endif
        public string ClientId
        {
            get { return client_id; }
        }

#if UNITY_STANDALONE
        [SerializeField]
#endif
        private List<string> data = new List<string>();
#if !UNITY_STANDALONE
        [JsonPropertyName("data")]
#endif
        public List<string> Data
        {
            get { return data; }
            // needed for deserialization
            set { data = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        public SupersynkClientDTO(string clientId)
        {
            client_id = clientId;
        }

        /// <summary>
        /// 
        /// </summary>
        public string ToJsonString()
        {
#if UNITY_STANDALONE
            return JsonUtility.ToJson(this);
#else
            return JsonSerializer.Serialize(this);
#endif
        }
    }
}
