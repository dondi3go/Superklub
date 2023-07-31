using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
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
    /// 
    /// </summary>
    [Serializable]
    public class SupersynkClientDTOs
    {
#if UNITY_STANDALONE
        [SerializeField]
#endif
        private List<SupersynkClientDTO> list = new List<SupersynkClientDTO>();
#if !UNITY_STANDALONE
        [JsonPropertyName("list")]
#endif
        public List<SupersynkClientDTO> List { 
            get { return list; }
            set { list = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        public void Add(SupersynkClientDTO dto)
        {
            list.Add(dto);
        }

        /// <summary>
        /// 
        /// </summary>
        public void FromJSONString(string jsonString)
        {
            string adaptedJsonString = "{\"list\":" + jsonString + "}";
#if UNITY_STANDALONE
            var DTOs = JsonUtility.FromJson<SupersynkClientDTOs>(adaptedJsonString);
#else
            var DTOs = JsonSerializer.Deserialize<SupersynkClientDTOs>(adaptedJsonString);
#endif
            if (DTOs != null)
            {
                this.list = DTOs.list;
            }
        }
    }
}
