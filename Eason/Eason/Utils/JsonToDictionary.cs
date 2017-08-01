using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Eason
{
    public class JsonToDictionary
    {
        private string _json = "";
        private Dictionary<string, string> _dictionary;
        public string Json
        {
            get { return _json; }
            set { _json = value; }
        }
        public Dictionary<string, string> Dictionary
        {
            get { return _dictionary; }
            set { _dictionary = value; }
        }
        public JsonToDictionary(string JsonArray)
        {
            try
            {
                Json = JsonArray.Replace("name", "key").Replace("SearchParameter.", "");
                List<KeyValuePair<string, string>> collection = JsonConvert.DeserializeObject<List<KeyValuePair<string, string>>>(Json);
                Dictionary = collection.ToDictionary(x => x.Key, x => x.Value);
            }
            catch
            {
                throw new Exception("Json To Dictionary Error.");
            }
        }
    }
}