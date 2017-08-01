using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Eason
{
    [AttributeUsage(AttributeTargets.All)]
    public class TableAttribute:Attribute
    {
        private static string _table = "";
        private static string _key = "";
        public static string Table 
        { 
            get { return _table; } 
            set { _table = value; } 
        }
        public static string Key
        {
            get { return _key; }
            set { _key = value; }
        }
        public TableAttribute(string table, string key) 
        {
            Table = table;
            Key = key;
        }
    }
}