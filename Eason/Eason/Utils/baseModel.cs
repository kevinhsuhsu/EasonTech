using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Eason
{
    public class baseModel : CRUD
    {
        private string _fields = "";
        private string _from = "";
        private string _fixCondition = "";
        private string _conditions = "";
        private string _orderby = "";
        private string _groupby = "";
        private int _topRecord = 0;
        protected bool _isRowNumber = true;
        private Hashtable _conditionDictionary = new Hashtable();
        private Hashtable _orderByDictionary = new Hashtable();
        protected virtual string Fields
        {
            get { return _fields; }
            set { _fields = value; }
        }
        protected virtual string From
        {
            get { return _from; }
            set { _from = value; }
        }
        protected virtual string FixCondition
        {
            get { return _fixCondition; }
            set { _fixCondition = value; }
        }
        protected virtual string GroupBy
        {
            get { return _groupby; }
            set { _groupby = value; }
        }
        protected virtual Hashtable ConditionDictionary
        {
            get { return _conditionDictionary; }
        }
        protected virtual Hashtable OrderByDictionary
        {
            get { return _orderByDictionary; }
        }
        [Display(AutoGenerateField = false)]
        public virtual string Conditions
        {
            get { return _conditions; }
            set { _conditions = value; }
        }
        [Display(AutoGenerateField = false)]
        public virtual string OrderBy
        {
            get { return _orderby; }
            set { _orderby = value; }
        }
        public virtual void Sort(string orderBy, string currentOrder)
        {
            if (orderBy == currentOrder)
                _orderby = orderBy.Contains(" DESC")
                    ? orderBy.Replace(" DESC", "")
                    : (orderBy != "") ? orderBy + " DESC" : "";
            else
                _orderby = orderBy;
        }
        public virtual string getCondition(string AParamName, string AParamValue)
        {
            return this.getCondition(AParamName, AParamValue, true);
        }
        public virtual string getCondition(string AParamName, string AParamValue, bool bReplaceKeyword)
        {
            if (AParamName != null && ConditionDictionary.ContainsKey(AParamName))
            {
                if (((string)ConditionDictionary[AParamName]).IndexOf(" '?'") > 0)
                    ((string)ConditionDictionary[AParamName]).Replace(" '?'", " N'?'");

                if (AParamName == "EmployeeTree" || AParamName == "MultiContentSerNo" || AParamName == "MultiContentSerNo2" || AParamName == "MultiContentSerNo3")
                    return ((string)ConditionDictionary[AParamName]).Replace("?", AParamValue == null ? "" : AParamValue);
                else
                    return ((string)ConditionDictionary[AParamName]).Replace("?", AParamValue == null ? "" : ((bReplaceKeyword) ? AParamValue.Replace("'", "''") : AParamValue));
            }
            else
                return "";
        }
        public virtual string getSQLStatement()
        {
            string sWhere = "";
            if (FixCondition == "")
            {
                if (Conditions != "")
                    sWhere = "where " + Conditions;
            }
            else
            {
                if (Conditions == "")
                    sWhere = "where " + FixCondition;
                else
                    sWhere = string.Format("where ({0}) AND ({1})", FixCondition, Conditions);
            }

            return string.Format(
            "select {0}{1} from {2} {3} {4} {5}",
            (_topRecord == 0 ? "" : string.Format(" TOP {0} ", _topRecord)),
            Fields,
            From,
            sWhere,
            (GroupBy == "") ? "" : "group by " + GroupBy,
            (OrderBy == "") ? "" : "order by " + OrderBy);
        }
    }
}