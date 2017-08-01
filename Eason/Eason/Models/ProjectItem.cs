using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Eason.Models
{
    [Table("ProjectItem", "PJI_SERNO")]
    public class ProjectItem : baseModel
    {
        public enum ncConditions { PJT_SERNO, PJI_SERNO, PJI_EMPNAME }
        public enum ncFields { }

        [Key]
        public string PJI_SERNO { get; set; }
        [Display(Name = "專案序號")]
        public string PJT_SERNO { get; set; }
        [Display(Name = "項目名稱"),Required]
        public string PJI_NAME { get; set; }
        [Display(Name = "負責人")]
        public string PJI_EMPNAME { get; set; }
        [Display(Name = "測試人")]
        public string PJI_UAT { get; set; }
        [Display(Name = "已完成")]
        public bool PJI_ISDONE { get; set; }
        [Display(Name = "已UAT")]
        public bool PJI_ISUAT { get; set; }
        [Display(Name = "已UT")]
        public bool PJI_ISUT { get; set; }
        [Display(Name = "進度")]
        public string PJI_PROGRESS { get; set; }
        [Display(Name = "預計完成日"), DisplayFormat(DataFormatString = "{0:yyyy/MM/dd}")]
        public DateTime PJI_EXPECTDTTM { get; set; }
        [Display(Name = "實際完成日"), DisplayFormat(DataFormatString = "{0:yyyy/MM/dd}")]
        public DateTime PJI_FINALDTTM { get; set; }
        [Display(Name = "備註")]
        public string PJI_COMMENT { get; set; }

        public ProjectItem()
        {
            base.Fields = "*";
            base.From = "ProjectItem";

            base.ConditionDictionary.Add("PJT_SERNO", " PJT_SERNO = N'?' ");
            base.ConditionDictionary.Add("PJI_SERNO", " PJI_SERNO = N'?' ");
            base.ConditionDictionary.Add("PJI_EMPNAME", " PJI_EMPNAME = N'?' ");
        }
    }
}