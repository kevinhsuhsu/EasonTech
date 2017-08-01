/*
 * @ Company: EasonTech 
 * @ Author: Kevin Hsu
 * @ Version: 1.0, 2017/05/31
 * @ Framework: .Net 1.1
 * @ Description: 針對XML輸出的Excel報表做簡易的物件化,可以動態組成Style標籤內容,
 *				  取代原先將一大串Style標籤字串塞在程式碼中的方法,並且可以更加彈性的客製化Style,
 *				  將XML Tag放在陣列暫存,取代原先直接將XML Tag丟到Response.Write的方法
 * @ Features: 
 *				1.可以針對字型做變化
 *				2.可以使用Enum值皆選擇想要的Style(Eg. Horizontal, Vertical, KnownColor, FontStyle)
 *				3.可以將Sheet依筆畫順序做正反排序, 也可以將指定的Sheet搬移到指定的Index
 *				4.可以指定儲存格的資料型態(Eg. Integer, Percentage, Decimal)
 *				5.可以針對儲存格內容(Object)做型態判斷, Eg. <Data ss:Type = 'DateTime'>,<Data ss:Type = 'String'>,<Data ss:Type = 'Number'>
 *				6.其餘LineStyle、BorderPosition 因不常用所以還沒實做
 * @ Example: 
 *				base.WorkbookName = "Weekly報表";
 *
 *				base.AddWorksheet("分頁名稱_1");
 *				base.AddWorksheet("分頁名稱_2");
 *
 *				base.AddFont("font_1","標楷體",24,FontStyle.Bold | FontStyle.Italic,KnownColor.Red);
 *				base.AddFont("font_2","新細明體",12,FontStyle.Bold | FontStyle.Underline,KnownColor.BlanchedAlmond);
 *				base.AddFont("font_3","標楷體",18,FontStyle.Underline | FontStyle.Italic,KnownColor.Chartreuse);
 *
 *				base.RowStartTag("分頁名稱_1",35);
 *				base.CellTag("分頁名稱_1", 12, 0, 1, KnownColor.AliceBlue, Horizontal.Center, Vertical.Center, NumberFormat.Percentage,"font_1", "Weekly Report");
 *				base.RowEndTag("分頁名稱_1");
 *
 *				base.RowStartTag("分頁名稱_1");
 *				base.CellTag("分頁名稱_1", 1, 0, 1, KnownColor.AliceBlue, Horizontal.Center, Vertical.Center, NumberFormat.Percentage, "名單敘述");
 *				base.CellTag("分頁名稱_1", 0, 0, 3, KnownColor.AliceBlue, Horizontal.Center, Vertical.Center, NumberFormat.Percentage, "原始名單\r\n(1)");
 *				base.CellTag("分頁名稱_1", 0, 0, 4, KnownColor.AliceBlue, Horizontal.Center, Vertical.Center, NumberFormat.Percentage, "使用名單\r\n(2)");
 *				base.RowEndTag("分頁名稱_1");																						  	
 *
 * 				base.ReverseSheets();
 *				base.MoveSheet("分頁名稱_1",0);
 *				base.ExportExcel();
 *
 */
using System;
using System.Web;
using System.Collections;
using System.Drawing;
using System.Text;
using System.Web.UI.WebControls;
using System.Collections.Generic;

namespace Eason.Utils
{
    #region Enums
    public enum Horizontal { Left, Center, Right }
    public enum Vertical { Top, Center, Bottom }
    public enum LineStyle { Continuous }
    public enum BorderPosition { Bottom, Left, Right, Top }
    public enum NumberFormat { Integer, Decimal, Percentage, ShortDate }
    #endregion

    public class ExcelXmlSuite
    {
        public string ExecScript = "";
        public Hashtable Styles = new Hashtable();

        #region Properties
        private string _workbookName;
        private string _workbookHeader;
        private string _workbookFooter;
        private string _style;
        private string _styleName;
        private string _numberFormat;
        private string _horizontal;
        private string _vertical;
        private string _color;
        private string _cell;
        private string _finalContent;
        private string _dataType;
        private bool _isSorted;
        private bool _italic;
        private bool _bold;
        private bool _underline;

        private Dictionary<string, ArrayList> _worksheet = new Dictionary<string, ArrayList>();
        private Dictionary<string, string> _fonts = new Dictionary<string, string>();
        private ArrayList _row = new ArrayList();
        private ArrayList _sheetList;
        private string StyleTag = "";
        private string StyleStartTag = "<Styles>";
        private string StyleEndTag = "</Styles>";

        private string WorksheetTag = "";
        private string WorksheetStartTag = "";
        private string WorksheetEndTag = @"</Table><WorksheetOptions xmlns='urn:schemas-microsoft-com:office:excel'><Selected/><ProtectObjects>False</ProtectObjects><ProtectScenarios>False</ProtectScenarios><PageSetup><Header x:Data='&amp;L&amp;A'/><Layout x:Orientation='Landscape'/></PageSetup><Unsynced/><FitToPage/></WorksheetOptions></Worksheet>";
        #endregion

        #region Accessors
        private string Style
        {
            set { _style = value; }
            get { return _style; }
        }
        private string StyleName
        {
            set { _styleName = value; }
            get { return _styleName; }
        }
        private string _NumberFormat
        {
            set { _numberFormat = value; }
            get { return _numberFormat; }
        }
        public string WorkbookName
        {
            get { return _workbookName; }
            set { _workbookName = string.Format("{0}.xls",value); }
        }
        public Dictionary<string, ArrayList> WorkSheet
        {
            get { return _worksheet; }
        }
        private Dictionary<string, string> Fonts
        {
            get { return _fonts; }
        }
        private ArrayList SheetList
        {
            get { return _sheetList; }
            set { _sheetList = value; }
        }
        private bool IsSorted
        {
            get { return _isSorted; }
            set { _isSorted = value; }
        }
        private bool Bold
        {
            get { return _bold; }
            set { _bold = value; }
        }
        private bool Italic
        {
            get { return _italic; }
            set { _italic = value; }
        }
        private bool Underline
        {
            get { return _underline; }
            set { _underline = value; }
        }
        private ArrayList Row
        {
            get { return _row; }
            set { _row = value; }
        }
        private string Cell
        {
            get { return _cell; }
            set { _cell = value; }
        }
        private string Color
        {
            get { return _color; }
            set { _color = value; }
        }
        private string _Horizontal
        {
            get { return _horizontal; }
            set { _horizontal = value; }
        }
        private string _Vertical
        {
            get { return _vertical; }
            set { _vertical = value; }
        }
        private string WorkbookHeader
        {
            get { return _workbookHeader; }
            set { _workbookHeader = value; }
        }
        private string WorkbookFooter
        {
            get { return _workbookFooter; }
            set { _workbookFooter = value; }
        }
        public string FinalContent
        {
            get { return _finalContent; }
            set { _finalContent = value; }
        }
        private string DataType
        {
            get { return _dataType; }
            set { _dataType = value; }
        }
        #endregion

        #region Medthods

        #region WorksheetTag
        /// <summary>
        /// 新增分頁
        /// </summary>
        /// <param name="sheetName">分頁名稱</param>
        public void AddWorksheet(string sheetName)
        {
            if (!WorkSheet.ContainsKey(sheetName))
                WorkSheet.Add(sheetName, new ArrayList());
        }
        #endregion

        #region Row Tag
        /// <summary>
        /// 列起始,換列時必需
        /// </summary>
        /// <param name="sheetName">分頁名稱</param>
        public void RowStartTag(string sheetName)
        {
            if (WorkSheet.ContainsKey(sheetName))
            {
                ((ArrayList)WorkSheet[sheetName]).Add("<Row ss:AutofitHeight='0'>");
            }
        }
        /// <summary>
        /// 列起始(可設列高),換列時必需
        /// </summary>
        /// <param name="sheetName">分頁名稱</param>
        /// <param name="height">列高(像素/2)</param>
        public void RowStartTag(string sheetName, double height)
        {
            if (WorkSheet.ContainsKey(sheetName))
            {
                ((ArrayList)WorkSheet[sheetName]).Add(string.Format("<Row ss:AutofitHeight='0' ss:Height='{0}'>", height));
            }
        }
        /// <summary>
        /// 列結尾,換列時必需
        /// </summary>
        /// <param name="sheetName">分頁名稱</param>
        public void RowEndTag(string sheetName)
        {
            if (WorkSheet.ContainsKey(sheetName))
            {
                ((ArrayList)WorkSheet[sheetName]).Add("</Row>");
            }
        }
        #endregion

        #region Column Tag
        /// <summary>
        /// 設定儲存格欄寬
        /// </summary>
        /// <param name="sheetName">分頁名稱</param>
        /// <param name="width">欄寬(像素/2)</param>
        public void ColumnWidth(string sheetName, double width)
        {
            if (WorkSheet.ContainsKey(sheetName))
            {
                ((ArrayList)WorkSheet[sheetName]).Add(string.Format("<Column ss:AutoFitWidth='0' ss:Width='{0}'/>", width));
            }
        }
        #endregion

        #region Cell Tag

        /// <summary>
        /// 設定儲存格
        /// </summary>
        /// <param name="sheetName">分頁名稱</param>
        /// <param name="mergeAcross">向右合併</param>
        /// <param name="mergeDown">向下合併</param>
        /// <param name="cellNumber">儲存格橫向索引</param>
        /// <param name="color">儲存格背景顏色</param>
        /// <param name="horizontal">文字水平對齊</param>
        /// <param name="vertical">文字垂直對齊</param>
        /// <param name="numberformat">文字格式</param>
        /// <param name="content">儲存格內容</param>
        public void CellTag(string sheetName, int mergeAcross, int mergeDown, int cellNumber, KnownColor color, Horizontal horizontal, Vertical vertical, NumberFormat numberformat, string fontStyle, object content)
        {
            if (WorkSheet.ContainsKey(sheetName))
            {
                CheckStyle(color, horizontal, vertical, numberformat, fontStyle);

                CheckDataType(ref content);

                Cell = string.Format(@"<Cell ss:MergeAcross='{0}' ss:MergeDown='{1}' ss:Index='{2}' ss:StyleID='{3}'><Data ss:Type='{4}'>{5}</Data></Cell>"
                    , mergeAcross, mergeDown, cellNumber, StyleName, DataType, content);

                ((ArrayList)WorkSheet[sheetName]).Add(Cell);
            }
        }
        /// <summary>
        /// 設定儲存格
        /// </summary>
        /// <param name="sheetName">分頁名稱</param>
        /// <param name="mergeAcross">向右合併</param>
        /// <param name="mergeDown">向下合併</param>
        /// <param name="cellNumber">儲存格橫向索引</param>
        /// <param name="color">儲存格背景顏色</param>
        /// <param name="horizontal">文字水平對齊</param>
        /// <param name="vertical">文字垂直對齊</param>
        /// <param name="numberformat">文字格式</param>
        /// <param name="content">儲存格內容</param>
        public void CellTag(string sheetName, int mergeAcross, int mergeDown, int cellNumber, KnownColor color, Horizontal horizontal, Vertical vertical, NumberFormat numberformat, object content)
        {
            if (WorkSheet.ContainsKey(sheetName))
            {
                CheckStyle(color, horizontal, vertical, numberformat);

                CheckDataType(ref content);

                Cell = string.Format(@"<Cell ss:MergeAcross='{0}' ss:MergeDown='{1}' ss:Index='{2}' ss:StyleID='{3}'><Data ss:Type='{4}'>{5}</Data></Cell>"
                    , mergeAcross, mergeDown, cellNumber, StyleName, DataType, content);

                ((ArrayList)WorkSheet[sheetName]).Add(Cell);
            }
        }

        /// <summary>
        /// 設定儲存格
        /// </summary>
        /// <param name="sheetName">分頁名稱</param>
        /// <param name="cellNumber">儲存格橫向索引</param>
        /// <param name="color">儲存格背景顏色</param>
        /// <param name="horizontal">文字水平對齊</param>
        /// <param name="vertical">文字垂直對齊</param>
        /// <param name="numberformat">文字格式</param>
        /// <param name="content">儲存格內容</param>
        public void CellTag(string sheetName, int cellNumber, KnownColor color, Horizontal horizontal, Vertical vertical, NumberFormat numberformat, object content)
        {
            if (WorkSheet.ContainsKey(sheetName))
            {
                CheckStyle(color, horizontal, vertical, numberformat);

                CheckDataType(ref content);

                Cell = string.Format(@"<Cell ss:Index='{0}' ss:StyleID='{1}'><Data ss:Type='{2}'>{3}</Data></Cell>"
                    , cellNumber, StyleName, DataType, content);

                ((ArrayList)WorkSheet[sheetName]).Add(Cell);
            }
        }

        /// <summary>
        /// 設定儲存格
        /// </summary>
        /// <param name="sheetName">分頁名稱</param>
        /// <param name="cellNumber">儲存格橫向索引</param>
        /// <param name="numberformat">文字格式</param>
        /// <param name="content">儲存格內容</param>
        public void CellTag(string sheetName, int cellNumber, NumberFormat numberformat, object content)
        {
            if (WorkSheet.ContainsKey(sheetName))
            {
                CheckStyle(numberformat);

                CheckDataType(ref content);

                Cell = string.Format(@"<Cell ss:Index='{0}' ss:StyleID='{1}'><Data ss:Type='{2}'>{3}</Data></Cell>"
                    , cellNumber, StyleName, DataType, content);

                ((ArrayList)WorkSheet[sheetName]).Add(Cell);
            }
        }
        #endregion

        #region Font Tag
        /// <summary>
        /// 新增自訂字型
        /// </summary>
        /// <param name="styleName">自訂字型名稱</param>
        /// <param name="fontName">字型名稱</param>
        /// <param name="fontSize">字體大小</param>
        /// <param name="fontStyle">字體樣式(粗、斜、底)</param>
        /// <param name="color">字體顏色</param>
        public void AddFont(string styleName, string fontName, double fontSize, FontStyle fontStyle, KnownColor color)
        {
            if (!Fonts.ContainsKey(styleName))
            {
                string fontColor = ColorTranslator.ToHtml(System.Drawing.Color.FromArgb(System.Drawing.Color.FromKnownColor(color).ToArgb()));

                Bold = (fontStyle & FontStyle.Bold) == FontStyle.Bold;
                Italic = (fontStyle & FontStyle.Italic) == FontStyle.Italic;
                Underline = (fontStyle & FontStyle.Underline) == FontStyle.Underline;

                Fonts.Add(styleName,
                    string.Format(@"<Font ss:FontName='{0}' x:CharSet='136' x:Family='Roman' ss:Size='{1}' 
                                    ss:Color='{2}' ss:Bold='{3}' ss:Italic='{4}' ss:Underline='{5}'/>"
                                    , fontName
                                    , fontSize
                                    , fontColor
                                    , (Bold == false) ? 0 : 1
                                    , (Italic == false) ? 0 : 1
                                    , (Underline == false) ? "None" : "Single")
                                );
            }
        }
        #endregion

        #region 分頁排序
        /// <summary>
        /// 排序分頁(首字字母筆畫由小到大)
        /// </summary>
        public void SortSheets()
        {
            if (WorkSheet.Count > 0)
            {
                SheetList = new ArrayList(WorkSheet.Keys);
                SheetList.Sort();
                IsSorted = true;
            }
        }
        /// <summary>
        /// 排序分頁(首字字母筆畫由大到小)
        /// </summary>
        public void ReverseSheets()
        {
            if (WorkSheet.Count > 0)
            {
                SheetList = new ArrayList(WorkSheet.Keys);
                SheetList.Sort();
                SheetList.Reverse();
                IsSorted = true;
            }
        }
        /// <summary>
        /// 移動分頁位置
        /// </summary>
        /// <param name="sheetName">要移動的分頁名稱</param>
        /// <param name="destIndex">目標位置</param>
        public void MoveSheet(string sheetName, int destIndex)
        {
            if (SheetList.Contains(sheetName) && destIndex >= 0)
            {
                //SortSheets();
                int currentIndex = SheetList.IndexOf(sheetName, 0, SheetList.Count);

                if (currentIndex > destIndex)
                {
                    SheetList.Insert(destIndex, sheetName);
                    SheetList.RemoveAt(currentIndex + 1);
                }
                else
                {
                    SheetList.Insert(currentIndex, SheetList[destIndex]);
                    SheetList.RemoveAt(destIndex + 1);
                }
            }
        }
        #endregion

        #region 最終輸出
        /// <summary>
        /// 輸出Excel (xls檔)
        /// </summary>
        public void ExportExcel()
        {
            try
            {
                SetWorkbookHeader();
                SetStyles();
                SetWorksheets(IsSorted);
                SetWorkbookFooter();
                FinalContent = string.Format("{0}{1}{2}{3}", WorkbookHeader, StyleTag, WorksheetTag, WorkbookFooter);
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion

        #region Enum 轉換
        private void GetVertical(Vertical vertical)
        {
            switch (vertical)
            {
                case Vertical.Center:
                    _Vertical = "Center";
                    break;
                case Vertical.Bottom:
                    _Vertical = "Bottom";
                    break;
                case Vertical.Top:
                    _Vertical = "Top";
                    break;
            }
        }
        private void GetHorizontal(Horizontal horizontal)
        {
            switch (horizontal)
            {
                case Horizontal.Center:
                    _Horizontal = "Center";
                    break;
                case Horizontal.Left:
                    _Horizontal = "Left";
                    break;
                case Horizontal.Right:
                    _Horizontal = "Right";
                    break;
            }
        }
        private void GetNumberFormat(NumberFormat numberformat)
        {
            switch (numberformat)
            {
                case NumberFormat.Decimal:
                    _NumberFormat = "#,##0.00_ ";
                    break;
                case NumberFormat.Integer:
                    _NumberFormat = "#,##0_ ";
                    break;
                case NumberFormat.Percentage:
                    _NumberFormat = "Percent";
                    break;
                case NumberFormat.ShortDate:
                    _NumberFormat = "yyyy/mm/dd";
                    break;
            }
        }
        #endregion

        #region 檢查資料型態
        /// <summary>
        /// 檢查儲存格內容格式
        /// </summary>
        /// <param name="data">儲存格內容</param>
        private void CheckDataType(ref object data)
        {
            if (data != null)
            {
                Type t = data.GetType();
                string type = t.ToString();

                switch (type)
                {
                    case "System.String":
                        DataType = "String";
                        break;
                    case "System.Double":
                        DataType = "Number";
                        break;
                    case "System.Int32":
                        DataType = "Number";
                        break;
                    case "System.Boolean":
                        DataType = "Boolean";
                        break;
                    case "System.DateTime":
                        DateTimeConvert(ref data);
                        break;
                    default:
                        break;
                }
            }
            else
                DataType = "String";
        }
        private void DateTimeConvert(ref object dt)
        {
            DateTime tmp = Convert.ToDateTime(dt);
            DateTime min = DateTime.MinValue;

            if (DateTime.Compare(tmp, min) > 0)
            {
                dt = tmp.ToString("yyyy-MM-ddThh:MM:ss");
                DataType = "DateTime";
            }
            else
            {
                dt = "";
                DataType = "String";
            }
        }
        #endregion

        #region Style

        /// <summary>
        /// 新增Style字串,不包含字型
        /// </summary>
        private void AddStyle()
        {
            Style = string.Format(@"
                                    <Style ss:ID='{0}'>
                                    <Alignment ss:Horizontal='{1}' ss:Vertical='{2}'/>
                                    <Borders>
                                    <Border ss:Position='Bottom' ss:LineStyle='Continuous' ss:Weight='1'/>
                                    <Border ss:Position='Left' ss:LineStyle='Continuous' ss:Weight='1'/>
                                    <Border ss:Position='Right' ss:LineStyle='Continuous' ss:Weight='1'/>
                                    <Border ss:Position='Top' ss:LineStyle='Continuous' ss:Weight='1'/>
                                    </Borders>
                                    <Interior ss:Color='{3}' ss:Pattern='Solid'/>
                                    <NumberFormat ss:Format='{4}'/>
                                    </Style>",
                StyleName,
                _Horizontal,
                _Vertical,
                Color,
                _NumberFormat
                );
            Styles.Add(StyleName, Style);
        }
        /// <summary>
        /// 新增Style字串,包含字型
        /// </summary>
        /// <param name="fontStyle"></param>
        private void AddStyle(string fontStyle)
        {
            Style = string.Format(@"
                                    <Style ss:ID='{0}'>
                                    <Alignment ss:Horizontal='{1}' ss:Vertical='{2}'/>
                                    <Borders>
                                    <Border ss:Position='Bottom' ss:LineStyle='Continuous' ss:Weight='1'/>
                                    <Border ss:Position='Left' ss:LineStyle='Continuous' ss:Weight='1'/>
                                    <Border ss:Position='Right' ss:LineStyle='Continuous' ss:Weight='1'/>
                                    <Border ss:Position='Top' ss:LineStyle='Continuous' ss:Weight='1'/>
                                    </Borders>
                                    <Interior ss:Color='{3}' ss:Pattern='Solid'/>
                                    <NumberFormat ss:Format='{4}'/>
                                    {5}
                                    </Style>",
                StyleName,
                _Horizontal,
                _Vertical,
                Color,
                _NumberFormat,
                Fonts[fontStyle]
                );
            Styles.Add(StyleName, Style);
        }
        /// <summary>
        /// 若Style不存在則新增Style
        /// </summary>
        /// <param name="color">儲存格背景顏色</param>
        /// <param name="horizontal">文字水平對齊</param>
        /// <param name="vertical">文字垂直對齊</param>
        /// <param name="numberformat">儲存格內容格式</param>
        /// <param name="fontStyle">套用自訂字型名稱</param>
        private void CheckStyle(KnownColor color, Horizontal horizontal, Vertical vertical, NumberFormat numberformat, string fontStyle)
        {
            StyleName = string.Format("{0}_{1}_{2}_{3}", color, horizontal, numberformat, fontStyle);

            if (!Styles.ContainsKey(StyleName))
            {
                GetNumberFormat(numberformat);
                GetVertical(vertical);
                GetHorizontal(horizontal);
                Color = ColorTranslator.ToHtml(System.Drawing.Color.FromArgb(System.Drawing.Color.FromKnownColor(color).ToArgb()));

                #region 自訂字型存在
                if (Fonts.ContainsKey(fontStyle))
                {
                    AddStyle(fontStyle);
                }
                #endregion

                #region 自訂字型不存在
                else
                {
                    AddStyle();
                }
                #endregion
            }
        }

        /// <summary>
        /// 若Style不存在則新增Style
        /// </summary>
        /// <param name="color">儲存格背景顏色</param>
        /// <param name="horizontal">文字水平對齊</param>
        /// <param name="vertical">文字垂直對齊</param>
        /// <param name="numberformat">儲存格內容格式</param>
        private void CheckStyle(KnownColor color, Horizontal horizontal, Vertical vertical, NumberFormat numberformat)
        {
            StyleName = string.Format("{0}_{1}_{2}", color, horizontal, numberformat);

            if (!Styles.ContainsKey(StyleName))
            {
                GetNumberFormat(numberformat);
                GetVertical(vertical);
                GetHorizontal(horizontal);
                Color = ColorTranslator.ToHtml(System.Drawing.Color.FromArgb(System.Drawing.Color.FromKnownColor(color).ToArgb()));
                AddStyle();
            }
        }

        /// <summary>
        /// 若Style不存在則新增Style
        /// </summary>
        /// <param name="numberformat">儲存格內容格式</param>
        private void CheckStyle(NumberFormat numberformat)
        {
            StyleName = string.Format("White_Center_{0}", numberformat);

            if (!Styles.ContainsKey(StyleName))
            {
                GetNumberFormat(numberformat);
                _Horizontal = "Center";
                _Vertical = "Center";
                Color = "#FFFFFF";

                AddStyle();
            }
        }
        #endregion

        #region XML輸出
        /// <summary>
        /// 組合Workbook標頭
        /// </summary>
        private void SetWorkbookHeader()
        {
            WorkbookHeader = string.Format(@"
                                            <?xml version='1.0'?>
                                            <?mso-application progid='Excel.Sheet'?>
                                            <Workbook xmlns='urn:schemas-microsoft-com:office:spreadsheet'
                                            xmlns:o='urn:schemas-microsoft-com:office:office'
                                            xmlns:x='urn:schemas-microsoft-com:office:excel'
                                            xmlns:ss='urn:schemas-microsoft-com:office:spreadsheet'
                                            xmlns:html='http://www.w3.org/TR/REC-html40'>
                                            <DocumentProperties xmlns='urn:schemas-microsoft-com:office:office'>
                                            <Description>Query Range: {0}</Description>
                                            <LastAuthor>Kevin Hsu</LastAuthor>
                                            <Created>{1}</Created>
                                            <Version>1.00</Version>
                                            </DocumentProperties>
                                            <ExcelWorkbook xmlns='urn:schemas-microsoft-com:office:excel'>
                                            <WindowHeight>10005</WindowHeight>
                                            <WindowWidth>10005</WindowWidth>
                                            <WindowTopX>120</WindowTopX>
                                            <WindowTopY>135</WindowTopY>
                                            <ProtectStructure>False</ProtectStructure>
                                            <ProtectWindows>False</ProtectWindows>
                                            </ExcelWorkbook>", "", DateTime.Now.ToString("yyyy/MM/dd"));
        }
        /// <summary>
        /// 組合Styles標籤
        /// </summary>
        private void SetStyles()
        {
            if (Styles.Count > 0)
            {
                foreach (string style in Styles.Values)
                {
                    StyleTag += style;
                }
                StyleTag = string.Format("{0}{1}{2}", StyleStartTag, StyleTag, StyleEndTag);
            }
        }
        /// <summary>
        /// 組合Worksheet標籤
        /// </summary>
        private void SetWorksheets(bool isSorted)
        {
            if (isSorted)
            {
                if (SheetList.Count > 0)
                {
                    foreach (string sheet in SheetList)
                    {
                        WorksheetStartTag = string.Format(@"<Worksheet ss:Name='{0}'><Table ss:DefaultColumnWidth='60' ss:DefaultRowHeight='16.5'>", sheet);

                        StringBuilder tagContent = new StringBuilder("");// 動態串接字串的時候,StringBuilder效率比String相加快上千倍

                        foreach (string row in WorkSheet[sheet])
                        {
                            tagContent.Append(row);
                        }
                        WorksheetTag += string.Format("{0}{1}{2}", WorksheetStartTag, tagContent, WorksheetEndTag);
                    }
                }
            }
            else
            {
                if (WorkSheet.Count > 0)
                {
                    foreach (KeyValuePair<string,ArrayList> sheet in WorkSheet)
                    {
                        WorksheetStartTag = string.Format(@"<Worksheet ss:Name='{0}'><Table ss:DefaultColumnWidth='60' ss:DefaultRowHeight='16.5'>", sheet.Key);

                        StringBuilder tagContent = new StringBuilder("");// 動態串接字串的時候,StringBuilder效率比String相加快上千倍

                        foreach (string row in WorkSheet[sheet.Key])
                        {
                            tagContent.Append(row);
                        }
                        WorksheetTag += string.Format("{0}{1}{2}", WorksheetStartTag, tagContent, WorksheetEndTag);
                    }
                }
            }
        }
        /// <summary>
        /// 組合Workbook結尾
        /// </summary>
        private void SetWorkbookFooter()
        {
            WorkbookFooter = "</Workbook>";
        }
        #endregion

        #endregion
    }
}
