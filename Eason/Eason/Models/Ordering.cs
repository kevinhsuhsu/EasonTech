using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using Dapper;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Collections;

namespace Eason.Models
{
    [Table("Ordering", "ORD_SERNO")]
    public class Ordering : baseModel
    {
        public enum ncConditions { }
        public enum ncFields { }

        [Key]
        public string ORD_SERNO { get; set; }

        public Ordering()
        {
            base.Fields = "*";
            base.From = "Ordering";
        }
    }
}