using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Eason
{
    /// <summary>
    /// 產生下拉選單內容
    /// </summary>
    /// <typeparam name="Target">Model類型</typeparam>
    public class DropDownList<Target> where Target : new()
    {
        public List<SelectListItem> SelectListItem;
        /// <summary>
        /// 從已有的Model產生下拉選單
        /// </summary>
        /// <param name="t">Model Instance, 可設定篩選條件</param>
        /// <param name="key">下拉選單Key</param>
        /// <param name="value">下拉選單Value</param>
        public DropDownList(Target t, string key, string value)
        {
            CRUD crud = new CRUD();

            List<Target> list = crud.View<Target>(typeof(Target).GetMethod("getSQLStatement").Invoke(t, null).ToString());
            SelectListItem = new List<SelectListItem>();

            foreach (var i in list)
            {
                SelectListItem.Add(new SelectListItem()
                {
                    Text = i.GetType().GetProperty(key).GetValue(i).ToString(),
                    Value = i.GetType().GetProperty(value).GetValue(i).ToString()
                });
            }
        }
    }
    /// <summary>
    /// 自定義下拉選單, 無須參考Model
    /// </summary>
    public class DropDownList
    {
        public List<SelectListItem> SelectListItem;
        public DropDownList(Dictionary<string,string> members)
        {
            SelectListItem = new List<SelectListItem>();

            foreach (var member in members)
            {
                SelectListItem.Add(new SelectListItem()
                {
                    Text = member.Key,
                    Value = member.Value
                });
            }
        }
    }
}