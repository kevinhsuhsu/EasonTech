using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Eason.Models
{
    [Table("Customer","CST_SERNO")]
    public class Customer:baseModel
    {
        public enum ncConditions { CST_SERNO, CST_NAME}
        public enum ncFields { }
        [Key]
        public string CST_SERNO { get; set; }
        [Display(Name = "客戶名稱")]
        public string CST_NAME { get; set; }
        [Display(Name = "客戶代碼")]
        public string CST_CODE { get; set; }

        public Customer()
        {
            base.Fields = "*";
            base.From = "Customer";

            base.ConditionDictionary.Add("CST_SERNO", " CST_SERNO = N'?' ");
            base.ConditionDictionary.Add("CST_NAME", " CST_NAME = N'?' ");
        }
    }
}