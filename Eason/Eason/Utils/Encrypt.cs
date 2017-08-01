using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;

namespace Eason
{
    public class Encrypt
    {
        private string _result="";
        public string Result
        {
            get { return _result; }
            set { _result = value; }
        }
        public Encrypt(string origin)
        {
            SHA512 sha512 = new SHA512CryptoServiceProvider();
            Result = Convert.ToBase64String(sha512.ComputeHash(Encoding.Default.GetBytes(origin))).Substring(0,20);
        }
    }
}