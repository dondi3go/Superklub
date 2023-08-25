using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

#if UNITY_STANDALONE
using UnityEngine;

public static class JsonHelper
{
    public static List<T> FromJsonArray<T>(string json)
    {
        json = "{\"Items\":" + json + "}";
        Wrapper<T> wrapper = JsonUtility.FromJson<Wrapper<T>>(json);
        return wrapper.Items;
    }

    [Serializable]
    private class Wrapper<T>
    {
        public List<T> Items;
    }
}

#else
using System.Text.Json;
using System.Text.Json.Serialization;
#endif

namespace Superklub
{
    /// <summary>
    /// Store data from Supersynk server response, 
    /// is converted from Json string
    /// </summary>
    [Serializable]
    public class SupersynkClientDTOs : List<SupersynkClientDTO>
    {
#if UNITY_STANDALONE
        /// <summary>
        /// Only because there is another constructor
        /// </summary>
        public SupersynkClientDTOs() { }

        /// <summary>
        /// Only used for Unity Json deserialization
        /// </summary>
        private SupersynkClientDTOs(List<SupersynkClientDTO> list)
        {
            AddRange(list);
        }
#endif

        /// <summary>
        /// 
        /// </summary>
        public static SupersynkClientDTOs? FromJsonString(string jsonString)
        {
#if UNITY_STANDALONE
            var list = JsonHelper.FromJsonArray<SupersynkClientDTO>(jsonString);
            return new SupersynkClientDTOs(list);
#else
            return JsonSerializer.Deserialize<SupersynkClientDTOs>(jsonString);
#endif
        }
    }
}
