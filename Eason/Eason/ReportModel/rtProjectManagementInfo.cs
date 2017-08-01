using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Eason.ReportModel
{
    public class rtProjectManagementInfo:baseModel
    {
        public string PJT_SERNO { get; set; }
        public string PJT_CODE { get; set; }
        public string PJT_NAME { get; set; }
        public string PJT_PARTICIPANTS { get; set; }
        public string PJT_CUSTOMER { get; set; }
        public string PJT_PROGRESS { get; set; }
        public string PJT_COMMENT { get; set; }
        public string PJT_STATUS { get; set; }
        public string PJT_DESCRIPTION { get; set; }
        public string PJT_TYPE { get; set; }
        public DateTime PJT_SDTTM { get; set; }
        public DateTime PJT_EDTTM { get; set; }
        public string PJI_SERNO { get; set; }
        public string PJI_NAME { get; set; }
        public string PJI_EMPNAME { get; set; }
        public string PJI_UAT { get; set; }
        public bool PJI_ISDONE { get; set; }
        public bool PJI_ISUAT { get; set; }
        public bool PJI_ISUT { get; set; }
        public string PJI_PROGRESS { get; set; }
        public DateTime PJI_EXPECTDTTM { get; set; }
        public DateTime PJI_FINALDTTM { get; set; }
        public string PJI_COMMENT { get; set; }

        public rtProjectManagementInfo()
        {
            base.Fields = "*";
            base.From = "ProjectList P LEFT JOIN ProjectItem I ON P.PJT_SERNO = I.PJT_SERNO";
            base.OrderBy = "P.PJT_CODE";

            base.ConditionDictionary.Add("P.PJT_SERNO", " P.PJT_SERNO = N'?' ");
            base.ConditionDictionary.Add("P.PJT_TYPE", " P.PJT_TYPE = N'?' ");
            base.ConditionDictionary.Add("P.PJT_NAME", " P.PJT_NAME LIKE N'%?%' ");
            base.ConditionDictionary.Add("P.PJT_PARTICIPANTS", " P.PJT_PARTICIPANTS = N'?' ");
            base.ConditionDictionary.Add("P.PJT_CUSTOMER", " P.PJT_CUSTOMER = N'?' ");
        }
    }
}