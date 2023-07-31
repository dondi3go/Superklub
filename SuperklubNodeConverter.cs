using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace Superklub
{
    public class SuperklubNodeConverter
    {
        /// <summary>
        /// A class dedicated to parsing from and to string
        /// to convert between ISuperklubNode and Supersynk (json properties)
        /// </summary>
        public static SupersynkClientDTO ConvertToSupersynk(string clientId, List<ISuperklubNode> nodes)
        {
            SupersynkClientDTO result = new SupersynkClientDTO(clientId);
            foreach (var node in nodes)
            {
                string propertyValue = ConvertToString(node);
                result.AddProperty(node.Id, propertyValue);
            }
            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        public static List<SuperklubNodeRecord> ConvertFromSupersynk(SupersynkClientDTO dto)
        { 
            List<SuperklubNodeRecord> result = new List<SuperklubNodeRecord>();

            foreach (var propertyDTO in dto.Properties)
            { 
                SuperklubNodeRecord record = new SuperklubNodeRecord();
                record.Id = dto.ClientId + ":" + propertyDTO.Key;

                // Convert from string 'propertyDTO.Value'
                var tokens = propertyDTO.Value.Split(';', StringSplitOptions.RemoveEmptyEntries);
                foreach (var token in tokens)
                {
                    if (!ParseToken(token, record))
                    {
                        continue;
                    }
                }
                result.Add(record);
            }
            
            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        public static string ConvertToString(ISuperklubNode node)
        { 
            string result = string.Empty;
            StringBuilder sb = new StringBuilder();

            var posAsString = StringFromFloat3(
                node.Position.x, node.Position.y, node.Position.z);
            sb.Append( string.Concat("pos=", posAsString, ";") );

            var rotAsString = StringFromFloat4(
                node.Rotation.w, node.Rotation.x, node.Rotation.y, node.Rotation.z);
            sb.Append(string.Concat("rot=", rotAsString, ";") );

            sb.Append(string.Concat("shape=", node.Shape, ";"));

            sb.Append(string.Concat("color=", node.Color));

            return sb.ToString();
        }

        /// <summary>
        /// 
        /// </summary>
        public static string StringFromFloat3(float x, float y, float z)
        {
            string str = string.Empty;
            str += StringFromFloat(x) + ",";
            str += StringFromFloat(y) + ",";
            str += StringFromFloat(z);
            return str.TrimEnd();
        }

        /// <summary>
        /// 
        /// </summary>
        public static string StringFromFloat4(float x, float y, float z, float w)
        {
            string str = string.Empty;
            str += StringFromFloat(x) + ",";
            str += StringFromFloat(y) + ",";
            str += StringFromFloat(z) + ",";
            str += StringFromFloat(w);
            return str.TrimEnd();
        }

        /// <summary>
        /// Convert using '.' as decimal separator, limit decimals to 4
        /// </summary>
        public static string StringFromFloat(float f)
        {
            NumberFormatInfo nfi = GetNumberFormatInfo();
            return f.ToString("0.####", nfi);
        }

        /// <summary>
        /// Provide a NumberFormatInfo having '.' as a decimal separator
        /// Independant from the culture info of the platform
        /// 
        /// Without it, serialization of floats could use '.' or ','
        /// </summary>
        private static NumberFormatInfo numberFormatInfo = null;
        private static NumberFormatInfo GetNumberFormatInfo()
        {
            // Lazy instanciation of numberFormatInfo
            if (numberFormatInfo == null)
            {
                numberFormatInfo = new NumberFormatInfo();
                numberFormatInfo.NumberDecimalSeparator = ".";
            }
            return numberFormatInfo;
        }

        /// <summary>
        /// 
        /// </summary>
        public static bool ParseToken(string token, SuperklubNodeRecord record)
        {
            if(string.IsNullOrEmpty(token))
            {
                return false;
            }

            var nameAndValue = token.Split('=');
            if (nameAndValue.Length != 2)
            {
                return false;
            }

            string parameterName = nameAndValue[0].Trim();
            string parameterValue = nameAndValue[1].Trim();
            switch (parameterName)
            {
                case "pos":
                    try
                    {
                        record.Position = ParseFloat3(parameterValue);
                    }
                    catch (FormatException)
                    { 
                        return false; 
                    }
                    break;
                case "rot":
                    try
                    {
                        record.Rotation = ParseFloat4(parameterValue);
                    }
                    catch (FormatException)
                    {
                        return false;
                    }
                    break;
                case "shape":
                    record.Shape = parameterValue;
                    break;
                case "color":
                    record.Color = parameterValue;
                    break;
            }

            return true;
        }

        /// <summary>
        /// 
        /// </summary>
        public static (float, float, float) ParseFloat3(string s)
        {
            var nfi = GetNumberFormatInfo();
            var tokens = s.Split(',');
            if(tokens.Length != 3)
            {
                throw new FormatException();
            }
            float x = float.Parse(tokens[0].Trim(), nfi);
            float y = float.Parse(tokens[1].Trim(), nfi);
            float z = float.Parse(tokens[2].Trim(), nfi);
            return (x, y, z);
        }

        /// <summary>
        /// 
        /// </summary>
        public static (float, float, float, float) ParseFloat4(string s)
        {
            var nfi = GetNumberFormatInfo();
            var tokens = s.Split(',');
            if (tokens.Length != 4)
            {
                throw new FormatException();
            }
            float x = float.Parse(tokens[0].Trim(), nfi);
            float y = float.Parse(tokens[1].Trim(), nfi);
            float z = float.Parse(tokens[2].Trim(), nfi);
            float w = float.Parse(tokens[3].Trim(), nfi);
            return (x, y, z, w);
        }
    }
}
