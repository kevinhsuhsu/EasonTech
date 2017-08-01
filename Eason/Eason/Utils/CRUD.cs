using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Dapper;
using Eason.Models;
using System.Reflection;
using System.Collections;

namespace Eason
{
    public class CRUD
    {
        string Fields = "";
        string Values = "";
        string Where = "";
        string Table = "";
        string Key = "";
        string sqlStatement = "";
        SqlConnection conn = SystemManager.DBConnection();
        Dictionary<string, string> fieldsInfo = new Dictionary<string, string>();

        public CRUD()
        {
            
        }
        public void Insert<T>(FormCollection collection)
        {
            try
            {
                GetModelInfo<T>();

                Fields = Key;
                Values = "'" + Guid.NewGuid().ToString().Substring(25, 10) + "'";

                foreach (string field in collection)
                {
                    if (fieldsInfo.ContainsKey(field))
                    {
                        if (Fields == "")
                            Fields = field;
                        else
                            Fields += "," + field;

                        if (Values == "")
                            Values = DataTransform(field, collection[field]);
                        else
                            Values += "," + DataTransform(field, collection[field]);
                    }
                }
                sqlStatement = string.Format("Insert Into {0} ({1}) values({2}) ", Table, Fields, Values);
                conn.Execute(sqlStatement);
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                conn.Close();
            }
        }
        public List<T> View<T>(string sql)
        {
            try
            {
                List<T> view = new List<T>();
                sqlStatement = sql;

                return view = conn.Query<T>(sqlStatement).ToList();
            }
            catch(Exception)
            {
                throw;
            }
            finally
            {
                conn.Close();
            }
        }
        public void Update<T>(string serno, FormCollection collection)
        {
            try
            {
                GetModelInfo<T>();

                foreach (string field in collection)
                {
                    if (fieldsInfo.ContainsKey(field))
                    {
                        if (Fields == "")
                            Fields = string.Format("{0} = {1}", field, DataTransform(field, collection[field]));
                        else
                            Fields += string.Format(",{0} = {1}", field, DataTransform(field, collection[field]));
                    }
                }

                Where = string.Format("{0} = '{1}'", Key, serno);
                sqlStatement = string.Format("Update {0} Set {1} Where {2} ", Table, Fields, Where);
                conn.Execute(sqlStatement);
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                conn.Close();
            }
        }
        public void Delete<T>(string serno, FormCollection collection)
        {
            try
            {
                GetModelInfo<T>();

                Where = string.Format("{0} = '{1}'", Key, serno);
                sqlStatement = string.Format("Delete From {0} Where {1} ", Table, Where);
                conn.Execute(sqlStatement);
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                conn.Close();
            }
        }

        private void GetModelInfo<T>()
        {
            try
            {
                typeof(T).GetCustomAttribute(typeof(TableAttribute), false);
                PropertyInfo[] propertyInfo = typeof(T).GetProperties();

                foreach(PropertyInfo p in propertyInfo)
                {
                    fieldsInfo.Add(p.Name, p.PropertyType.FullName);
                }

                Table = TableAttribute.Table;
                Key = TableAttribute.Key;
            }
            catch (Exception)
            {
                throw;
            }
        }
        private string DataTransform(string field, string value)
        {
            string returnValue = "";

            if (value != "__RequestVerificationToken")
            {
                switch (fieldsInfo[field.ToString()])
                {
                    case "System.String":
                        returnValue = "'" + value + "'";
                        break;
                    case "System.Int":
                        returnValue = "'" + value + "'";
                        break;
                    case "System.Double":
                        returnValue = "'" + value + "'";
                        break;
                    case "System.Boolean":
                        bool IsChecked;
                        returnValue = Boolean.TryParse(value, out IsChecked) ? "0" : "1";
                        break;
                    case "System.DateTime":
                        DateTime result;
                        if (DateTime.TryParse(value, out result))
                        {
                            returnValue = "'" + result.ToString("yyyy/MM/dd HH:mm:ss") + "'";
                        }
                        break;
                    case "System.DBNull":
                        break;
                    default:
                        break;
                }
                return returnValue;
            }
            else
                return "";
        }
    }
}