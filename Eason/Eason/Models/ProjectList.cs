using Dapper;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace Eason.Models
{
    [Table("ProjectList", "PJT_SERNO")]
    public class ProjectList : baseModel
    {
        public enum ncConditions { PJT_SERNO, PJT_NAME, PJT_TYPE, PJT_PARTICIPANTS, PJT_CUSTOMER }
        public enum ncFields { }

        [Key]
        public string PJT_SERNO { get; set; }
        [Display(Name = "專案代碼")]
        public string PJT_CODE { get; set; }
        [Display(Name = "專案名稱")]
        [Required(ErrorMessage = "專案名稱必須填寫", AllowEmptyStrings = false)]
        public string PJT_NAME { get; set; }
        [Display(Name = "參與人員")]
        public string PJT_PARTICIPANTS { get; set; }
        [Display(Name = "客戶")]
        public string PJT_CUSTOMER { get; set; }
        [Display(Name = "進度")]
        public string PJT_PROGRESS { get; set; }
        [Display(Name = "備註")]
        public string PJT_COMMENT { get; set; }
        [Display(Name = "狀態")]
        public string PJT_STATUS { get; set; }
        [Display(Name = "專案描述")]
        public string PJT_DESCRIPTION { get; set; }
        [Display(Name = "類型")]
        public string PJT_TYPE { get; set; }
        [Display(Name = "起始日期")]
        public DateTime PJT_SDTTM { get; set; }
        [Display(Name = "結束日期")]
        public DateTime PJT_EDTTM { get; set; }

        public ProjectList()
        {
            base.Fields = "*";
            base.From = "ProjectList";
            base.OrderBy = "PJT_CODE";

            base.ConditionDictionary.Add("PJT_SERNO", " PJT_SERNO = N'?' ");
            base.ConditionDictionary.Add("PJT_TYPE", " PJT_TYPE = N'?' ");
            //base.ConditionDictionary.Add("PJT_NAME", " PJT_NAME LIKE N'%?%' ");
            base.ConditionDictionary.Add("PJT_NAME", " PJT_NAME = N'?' ");

            base.ConditionDictionary.Add("PJT_PARTICIPANTS", " PJT_PARTICIPANTS = N'?' ");
            base.ConditionDictionary.Add("PJT_CUSTOMER", " PJT_CUSTOMER = N'?' ");
        }
    }
}