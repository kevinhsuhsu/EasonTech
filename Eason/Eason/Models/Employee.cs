using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Eason.Models
{
    [Table("Employee", "EMP_SERNO")]
    public class Employee : baseModel
    {
        public enum ncConditions { EMP_SERNO, EMP_NAME }
        public enum ncFields { }

        [Key]
        public string EMP_SERNO { get; set; }
        [Display(Name = "帳號")]
        public string EMP_NAME { get; set; }
        [Display(Name = "中文姓名")]
        public string EMP_CNAME { get; set; }
        [Display(Name = "密碼"), DataType(DataType.Password)]
        public string EMP_PASSWORD { get; set; }
        [Display(Name = "職稱")]
        public string EMP_TITLE { get; set; }
        [Display(Name = "Email"),EmailAddress]
        public string EMP_EMAIL { get; set; }
        [Display(Name = "最後登入時間")]
        public DateTime EMP_LASTLOGINDTTM { get; set; }
        [Display(Name = "最後登出時間")]
        public DateTime EMP_LASTLOGOUTDTTM { get; set; }


        public Employee()
        {
            base.Fields = "*";
            base.From = "Employee";

            base.ConditionDictionary.Add("EMP_SERNO", " EMP_SERNO = N'?' ");
            base.ConditionDictionary.Add("EMP_NAME", " EMP_Name = N'?' ");
        }
    }
}