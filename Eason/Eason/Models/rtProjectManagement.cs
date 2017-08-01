using Eason.ReportModel;
using Eason.Utils;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Web;

namespace Eason.Models
{
    public class rtProjectManagement:ExcelXmlSuite
    {
        public rtProjectManagement()
        {
            rtProjectManagementInfo vProj = new rtProjectManagementInfo();
            List<rtProjectManagementInfo> ProjList = vProj.View<rtProjectManagementInfo>(vProj.getSQLStatement());

            string sheet1 = "總表";
            base.WorkbookName = "專案控管表";
            base.AddWorksheet(sheet1);

            #region 總表

            #region 欄位標題
            base.RowStartTag(sheet1);
            base.CellTag(sheet1, 1, KnownColor.SkyBlue, Horizontal.Center, Vertical.Center, NumberFormat.Integer, "起始日期");
            base.CellTag(sheet1, 2, KnownColor.SkyBlue, Horizontal.Center, Vertical.Center, NumberFormat.Integer, "客戶");
            base.CellTag(sheet1, 3, KnownColor.SkyBlue, Horizontal.Center, Vertical.Center, NumberFormat.Integer, "編號");
            base.CellTag(sheet1, 4, KnownColor.SkyBlue, Horizontal.Center, Vertical.Center, NumberFormat.Integer, "專案簡述");
            base.CellTag(sheet1, 5, KnownColor.SkyBlue, Horizontal.Center, Vertical.Center, NumberFormat.Integer, "類別");
            base.CellTag(sheet1, 6, KnownColor.SkyBlue, Horizontal.Center, Vertical.Center, NumberFormat.Integer, "負責人");
            base.CellTag(sheet1, 7, KnownColor.SkyBlue, Horizontal.Center, Vertical.Center, NumberFormat.Integer, "進度");
            base.CellTag(sheet1, 8, KnownColor.SkyBlue, Horizontal.Center, Vertical.Center, NumberFormat.Integer, "狀態");
            base.CellTag(sheet1, 9, KnownColor.SkyBlue, Horizontal.Center, Vertical.Center, NumberFormat.Integer, "備註");
            base.RowEndTag(sheet1);
            #endregion

            var query = from proj in ProjList
                        group proj by proj.PJT_CODE;

            foreach (var q in query)
            {
                var project = q.FirstOrDefault();

                base.RowStartTag(sheet1);
                base.CellTag(sheet1, 1, NumberFormat.ShortDate, project.PJT_SDTTM);
                base.CellTag(sheet1, 2, NumberFormat.Integer, project.PJT_CUSTOMER);
                base.CellTag(sheet1, 3, NumberFormat.Integer, project.PJT_CODE);
                base.CellTag(sheet1, 4, NumberFormat.Integer, project.PJT_NAME);
                base.CellTag(sheet1, 5, NumberFormat.Integer, project.PJT_TYPE);
                base.CellTag(sheet1, 6, NumberFormat.Integer, project.PJT_PARTICIPANTS);
                base.CellTag(sheet1, 7, NumberFormat.Percentage, project.PJT_PROGRESS);
                base.CellTag(sheet1, 8, NumberFormat.Integer, project.PJT_STATUS);
                base.CellTag(sheet1, 9, NumberFormat.Integer, project.PJT_COMMENT);
                base.RowEndTag(sheet1);
            }
            #endregion

            #region 分頁

            foreach (var project in ProjList)
            {
                string sheet = project.PJT_CODE;

                if (base.WorkSheet.ContainsKey(sheet))
                    continue;
                else
                    base.AddWorksheet(sheet);

                #region 欄位標題

                #region 第一列
                base.RowStartTag(sheet);
                base.CellTag(sheet, 6, 0, 1, KnownColor.White, Horizontal.Center, Vertical.Center, NumberFormat.Integer, "專案控管表");
                base.RowEndTag(sheet);
                #endregion

                #region 第二列
                base.RowStartTag(sheet);
                base.CellTag(sheet, 1, 0, 1, KnownColor.White, Horizontal.Center, Vertical.Center, NumberFormat.Integer, "客戶名稱");
                base.CellTag(sheet, 0, 0, 3, KnownColor.White, Horizontal.Center, Vertical.Center, NumberFormat.Integer, project.PJT_CUSTOMER);
                base.CellTag(sheet, 0, 0, 4, KnownColor.White, Horizontal.Center, Vertical.Center, NumberFormat.Integer, "起始日期");
                base.CellTag(sheet, 0, 0, 5, KnownColor.White, Horizontal.Center, Vertical.Center, NumberFormat.ShortDate, project.PJT_SDTTM);
                base.CellTag(sheet, 0, 0, 6, KnownColor.White, Horizontal.Center, Vertical.Center, NumberFormat.Integer, "內部測試");
                base.CellTag(sheet, 0, 0, 7, KnownColor.White, Horizontal.Center, Vertical.Center, NumberFormat.Integer, "");
                base.RowEndTag(sheet);
                #endregion

                #region 第三列
                base.RowStartTag(sheet);
                base.CellTag(sheet, 1, 0, 1, KnownColor.White, Horizontal.Center, Vertical.Center, NumberFormat.Integer, "專案編號");
                base.CellTag(sheet, 0, 0, 3, KnownColor.White, Horizontal.Center, Vertical.Center, NumberFormat.Integer, project.PJT_CODE);
                base.CellTag(sheet, 0, 0, 4, KnownColor.White, Horizontal.Center, Vertical.Center, NumberFormat.Integer, "預計完成日期");
                base.CellTag(sheet, 0, 0, 5, KnownColor.White, Horizontal.Center, Vertical.Center, NumberFormat.ShortDate, "");
                base.CellTag(sheet, 0, 0, 6, KnownColor.White, Horizontal.Center, Vertical.Center, NumberFormat.Integer, "交測客戶");
                base.CellTag(sheet, 0, 0, 7, KnownColor.White, Horizontal.Center, Vertical.Center, NumberFormat.Integer, "");
                base.RowEndTag(sheet);
                #endregion

                #region 第四列
                base.RowStartTag(sheet);
                base.CellTag(sheet, 1, 0, 1, KnownColor.White, Horizontal.Center, Vertical.Center, NumberFormat.Integer, "預估工時");
                base.CellTag(sheet, 0, 0, 3, KnownColor.White, Horizontal.Center, Vertical.Center, NumberFormat.Integer, "");
                base.CellTag(sheet, 0, 0, 4, KnownColor.White, Horizontal.Center, Vertical.Center, NumberFormat.Integer, "實際工時");
                base.CellTag(sheet, 0, 0, 5, KnownColor.White, Horizontal.Center, Vertical.Center, NumberFormat.Integer, "");
                base.CellTag(sheet, 0, 0, 6, KnownColor.White, Horizontal.Center, Vertical.Center, NumberFormat.Integer, "上線日期");
                base.CellTag(sheet, 0, 0, 7, KnownColor.White, Horizontal.Center, Vertical.Center, NumberFormat.ShortDate, "");
                base.RowEndTag(sheet);
                #endregion

                #region 第五列
                base.RowStartTag(sheet);
                base.CellTag(sheet, 1, 0, 1, KnownColor.White, Horizontal.Center, Vertical.Center, NumberFormat.Integer, "專案簡述");
                base.CellTag(sheet, 4, 0, 3, KnownColor.White, Horizontal.Center, Vertical.Center, NumberFormat.Integer, project.PJT_NAME);
                base.RowEndTag(sheet);
                #endregion

                #region 第六列
                base.RowStartTag(sheet);
                base.CellTag(sheet, 3, 0, 1, KnownColor.White, Horizontal.Center, Vertical.Center, NumberFormat.Integer, "工作項目");
                base.CellTag(sheet, 0, 0, 5, KnownColor.White, Horizontal.Center, Vertical.Center, NumberFormat.Integer, "負責人員");
                base.CellTag(sheet, 0, 0, 6, KnownColor.White, Horizontal.Center, Vertical.Center, NumberFormat.Integer, "預計完成日");
                base.CellTag(sheet, 0, 0, 7, KnownColor.White, Horizontal.Center, Vertical.Center, NumberFormat.Integer, "進度狀態");
                base.RowEndTag(sheet);
                #endregion

                #region 工作項目
                var items = from proj in ProjList
                            where proj.PJT_SERNO == project.PJT_SERNO
                            select proj;

                int index = 1;

                foreach (var item in items)
                {
                    base.RowStartTag(sheet);
                    base.CellTag(sheet, 0, 0, 1, KnownColor.White, Horizontal.Center, Vertical.Center, NumberFormat.Integer, index);
                    base.CellTag(sheet, 2, 0, 2, KnownColor.White, Horizontal.Center, Vertical.Center, NumberFormat.Integer, item.PJI_NAME);
                    base.CellTag(sheet, 0, 0, 5, KnownColor.White, Horizontal.Center, Vertical.Center, NumberFormat.Integer, item.PJI_EMPNAME);
                    base.CellTag(sheet, 0, 0, 6, KnownColor.White, Horizontal.Center, Vertical.Center, NumberFormat.ShortDate, item.PJI_EXPECTDTTM);
                    base.CellTag(sheet, 0, 0, 7, KnownColor.White, Horizontal.Center, Vertical.Center, NumberFormat.Percentage, item.PJI_PROGRESS);
                    base.RowEndTag(sheet);

                    index++;
                }

                #endregion

                #region 工時統計
                base.RowStartTag(sheet);
                base.CellTag(sheet, 1, 0, 1, KnownColor.White, Horizontal.Center, Vertical.Center, NumberFormat.Integer, "參與人員");
                base.CellTag(sheet, 0, 0, 3, KnownColor.White, Horizontal.Center, Vertical.Center, NumberFormat.Integer, "個人預估工時");
                base.CellTag(sheet, 0, 0, 4, KnownColor.White, Horizontal.Center, Vertical.Center, NumberFormat.Integer, "個人實際工時");
                base.CellTag(sheet, 0, 0, 5, KnownColor.White, Horizontal.Center, Vertical.Center, NumberFormat.Integer, "達成率");
                base.CellTag(sheet, 1, 0, 6, KnownColor.White, Horizontal.Center, Vertical.Center, NumberFormat.Integer, "備註說明");
                base.RowEndTag(sheet);

                var employees = from proj in ProjList
                                where proj.PJT_SERNO == project.PJT_SERNO
                                group proj by proj.PJT_PARTICIPANTS;

                foreach (var employee in employees)
                {
                    var emp = employee.FirstOrDefault();

                    base.RowStartTag(sheet);
                    base.CellTag(sheet, 1, 0, 1, KnownColor.White, Horizontal.Center, Vertical.Center, NumberFormat.Integer, emp.PJT_PARTICIPANTS);
                    base.CellTag(sheet, 0, 0, 3, KnownColor.White, Horizontal.Center, Vertical.Center, NumberFormat.Integer, "");
                    base.CellTag(sheet, 0, 0, 4, KnownColor.White, Horizontal.Center, Vertical.Center, NumberFormat.Integer, "");
                    base.CellTag(sheet, 0, 0, 5, KnownColor.White, Horizontal.Center, Vertical.Center, NumberFormat.Integer, "");
                    base.CellTag(sheet, 1, 0, 6, KnownColor.White, Horizontal.Center, Vertical.Center, NumberFormat.Integer, "");
                    base.RowEndTag(sheet);
                }
                #endregion

                #endregion
            }

            #endregion

            base.ExportExcel();
        }
    }
}