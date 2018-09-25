using Saned.Jazan.Data.Helper;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Saned.Jazan.Data.Persistence.Repositories
{
    public static class ExtensionMethods
    {
        /// <summary>
        /// Be Carefull When Using this Parameter Constructed in the order you sent and in the name of the property you pass . 
        /// </summary>
        /// <param name="procesdureName"></param>
        /// <param name="arr"></param>
        /// <returns></returns>
        public static Tuple<SqlParameter[], string> GenerateParameterArray(this string procesdureName, params KeyValuePair<string, object>[] arr)
        {
            SqlParameter[] retarrr = new SqlParameter[arr.Length];
            procesdureName += " ";
            for (int i = 0; i < arr.Length; i++)
            {
                bool allowNull = false;
                var val = arr[i].Value;
                if (arr[i].Value == null)
                {
                    allowNull = true;
                    val = DBNull.Value;
                }
                retarrr[i] = new SqlParameter(arr[i].Key, val);
                retarrr[i].IsNullable = allowNull;
                procesdureName += ((i == arr.Length - 1) ? "@" + arr[i].Key + "" : "@" + arr[i].Key + " , ");
            }
            return new Tuple<SqlParameter[], string>(retarrr, procesdureName);
        }

        public static string SaveImage(this string base64, string BaseType, string imageExt)
        {
            //string baseFolder = @"E:\Jazan Project\Saned.Jazan\Saned.Jazan.Data\SysFiles"; // for testing Purpose only
            string baseFolder = $"uploads";
            if (!Directory.Exists(HttpContext.Current.Server.MapPath($"~/{baseFolder}")))
            {
                Directory.CreateDirectory(HttpContext.Current.Server.MapPath($"~/{baseFolder}"));
            }
            string imageName = $"{Guid.NewGuid().ToString()}.{imageExt}";
            Byte[] bytes = Convert.FromBase64String(base64);
            File.WriteAllBytes(HttpContext.Current.Server.MapPath($"~/{baseFolder}/{imageName}"), bytes);
            //File.WriteAllBytes(($"{baseFolder}/{imageName}"), bytes);
            return imageName;
        }

        public static KeyValuePair<string, object> KVP(this string key, object value)
        {
            return new KeyValuePair<string, object>(key,value);
        }

    }
}
