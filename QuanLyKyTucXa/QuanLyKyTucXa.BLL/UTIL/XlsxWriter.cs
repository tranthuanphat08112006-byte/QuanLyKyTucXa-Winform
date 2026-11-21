using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;

namespace QuanLyKyTucXa.BLL.UTIL
{
    /// <summary>
    /// Chỉ số style cố định dùng cho XlsxWriter.
    /// </summary>
    public static class XlsxStyle
    {
        public const int Default = 0;
        public const int Title = 1;
        public const int Subtitle = 2;
        public const int Header = 3;
        public const int Text = 4;
        public const int TextCenter = 5;
        public const int Money = 6;
        public const int Date = 7;
        public const int TotalLabel = 8;
        public const int TotalMoney = 9;
        public const int Integer = 10;
        public const int TotalInteger = 11;
    }

    /// <summary>
    /// Bộ ghi file Excel .xlsx (Office Open XML) gọn nhẹ, không phụ thuộc thư viện ngoài.
    /// Chỉ dùng System.IO.Compression có sẵn trong .NET Framework 4.5+.
    /// Hỗ trợ: nhiều sheet, chuỗi, số, ngày, định dạng tiền VNĐ, gộp ô, độ rộng cột, cố định dòng tiêu đề.
    /// </summary>
    public class XlsxWriter
    {
        private readonly List<XlsxSheet> sheets = new List<XlsxSheet>();

        public XlsxSheet AddSheet(string name)
        {
            string safe = new string((name ?? "Sheet").Where(ch => "[]:*?/\\".IndexOf(ch) < 0).ToArray());
            if (string.IsNullOrWhiteSpace(safe)) safe = "Sheet" + (sheets.Count + 1);
            if (safe.Length > 31) safe = safe.Substring(0, 31);
            XlsxSheet sheet = new XlsxSheet(safe);
            sheets.Add(sheet);
            return sheet;
        }

        public void Save(string filePath)
        {
            if (string.IsNullOrWhiteSpace(filePath))
                throw new ArgumentException("Đường dẫn file không hợp lệ.");
            if (sheets.Count == 0) AddSheet("Sheet1");

            string dir = Path.GetDirectoryName(filePath);
            if (!string.IsNullOrEmpty(dir) && !Directory.Exists(dir)) Directory.CreateDirectory(dir);

            using (FileStream fs = new FileStream(filePath, FileMode.Create, FileAccess.Write))
            using (ZipArchive zip = new ZipArchive(fs, ZipArchiveMode.Create))
            {
                WriteEntry(zip, "[Content_Types].xml", BuildContentTypes());
                WriteEntry(zip, "_rels/.rels",
                    "<?xml version=\"1.0\" encoding=\"UTF-8\" standalone=\"yes\"?>" +
                    "<Relationships xmlns=\"http://schemas.openxmlformats.org/package/2006/relationships\">" +
                    "<Relationship Id=\"rId1\" Type=\"http://schemas.openxmlformats.org/officeDocument/2006/relationships/officeDocument\" Target=\"xl/workbook.xml\"/>" +
                    "</Relationships>");
                WriteEntry(zip, "xl/workbook.xml", BuildWorkbook());
                WriteEntry(zip, "xl/_rels/workbook.xml.rels", BuildWorkbookRels());
                WriteEntry(zip, "xl/styles.xml", BuildStyles());
                for (int i = 0; i < sheets.Count; i++)
                {
                    WriteEntry(zip, "xl/worksheets/sheet" + (i + 1) + ".xml", sheets[i].BuildXml());
                }
            }
        }

        private static void WriteEntry(ZipArchive zip, string name, string content)
        {
            ZipArchiveEntry entry = zip.CreateEntry(name, CompressionLevel.Optimal);
            using (StreamWriter sw = new StreamWriter(entry.Open(), new UTF8Encoding(false)))
            {
                sw.Write(content);
            }
        }

        private string BuildContentTypes()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("<?xml version=\"1.0\" encoding=\"UTF-8\" standalone=\"yes\"?>");
            sb.Append("<Types xmlns=\"http://schemas.openxmlformats.org/package/2006/content-types\">");
            sb.Append("<Default Extension=\"rels\" ContentType=\"application/vnd.openxmlformats-package.relationships+xml\"/>");
            sb.Append("<Default Extension=\"xml\" ContentType=\"application/xml\"/>");
            sb.Append("<Override PartName=\"/xl/workbook.xml\" ContentType=\"application/vnd.openxmlformats-officedocument.spreadsheetml.sheet.main+xml\"/>");
            sb.Append("<Override PartName=\"/xl/styles.xml\" ContentType=\"application/vnd.openxmlformats-officedocument.spreadsheetml.styles+xml\"/>");
            for (int i = 0; i < sheets.Count; i++)
            {
                sb.Append("<Override PartName=\"/xl/worksheets/sheet" + (i + 1) + ".xml\" ContentType=\"application/vnd.openxmlformats-officedocument.spreadsheetml.worksheet+xml\"/>");
            }
            sb.Append("</Types>");
            return sb.ToString();
        }

        private string BuildWorkbook()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("<?xml version=\"1.0\" encoding=\"UTF-8\" standalone=\"yes\"?>");
            sb.Append("<workbook xmlns=\"http://schemas.openxmlformats.org/spreadsheetml/2006/main\" xmlns:r=\"http://schemas.openxmlformats.org/officeDocument/2006/relationships\"><sheets>");
            for (int i = 0; i < sheets.Count; i++)
            {
                sb.Append("<sheet name=\"" + XlsxSheet.Escape(sheets[i].Name) + "\" sheetId=\"" + (i + 1) + "\" r:id=\"rId" + (i + 1) + "\"/>");
            }
            sb.Append("</sheets></workbook>");
            return sb.ToString();
        }

        private string BuildWorkbookRels()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("<?xml version=\"1.0\" encoding=\"UTF-8\" standalone=\"yes\"?>");
            sb.Append("<Relationships xmlns=\"http://schemas.openxmlformats.org/package/2006/relationships\">");
            for (int i = 0; i < sheets.Count; i++)
            {
                sb.Append("<Relationship Id=\"rId" + (i + 1) + "\" Type=\"http://schemas.openxmlformats.org/officeDocument/2006/relationships/worksheet\" Target=\"worksheets/sheet" + (i + 1) + ".xml\"/>");
            }
            sb.Append("<Relationship Id=\"rId" + (sheets.Count + 1) + "\" Type=\"http://schemas.openxmlformats.org/officeDocument/2006/relationships/styles\" Target=\"styles.xml\"/>");
            sb.Append("</Relationships>");
            return sb.ToString();
        }

        private static string BuildStyles()
        {
            return "<?xml version=\"1.0\" encoding=\"UTF-8\" standalone=\"yes\"?>" +
                "<styleSheet xmlns=\"http://schemas.openxmlformats.org/spreadsheetml/2006/main\">" +
                "<numFmts count=\"2\">" +
                "<numFmt numFmtId=\"164\" formatCode=\"#,##0&quot; ₫&quot;\"/>" +
                "<numFmt numFmtId=\"165\" formatCode=\"dd/mm/yyyy\"/>" +
                "</numFmts>" +
                "<fonts count=\"5\">" +
                "<font><sz val=\"11\"/><name val=\"Calibri\"/><family val=\"2\"/></font>" +
                "<font><b/><sz val=\"16\"/><color rgb=\"FF1E3A8A\"/><name val=\"Calibri\"/><family val=\"2\"/></font>" +
                "<font><i/><sz val=\"10\"/><color rgb=\"FF475569\"/><name val=\"Calibri\"/><family val=\"2\"/></font>" +
                "<font><b/><sz val=\"11\"/><color rgb=\"FFFFFFFF\"/><name val=\"Calibri\"/><family val=\"2\"/></font>" +
                "<font><b/><sz val=\"11\"/><name val=\"Calibri\"/><family val=\"2\"/></font>" +
                "</fonts>" +
                "<fills count=\"4\">" +
                "<fill><patternFill patternType=\"none\"/></fill>" +
                "<fill><patternFill patternType=\"gray125\"/></fill>" +
                "<fill><patternFill patternType=\"solid\"><fgColor rgb=\"FF2980B9\"/><bgColor indexed=\"64\"/></patternFill></fill>" +
                "<fill><patternFill patternType=\"solid\"><fgColor rgb=\"FFE2E8F0\"/><bgColor indexed=\"64\"/></patternFill></fill>" +
                "</fills>" +
                "<borders count=\"2\">" +
                "<border><left/><right/><top/><bottom/><diagonal/></border>" +
                "<border><left style=\"thin\"><color rgb=\"FF94A3B8\"/></left><right style=\"thin\"><color rgb=\"FF94A3B8\"/></right>" +
                "<top style=\"thin\"><color rgb=\"FF94A3B8\"/></top><bottom style=\"thin\"><color rgb=\"FF94A3B8\"/></bottom><diagonal/></border>" +
                "</borders>" +
                "<cellStyleXfs count=\"1\"><xf numFmtId=\"0\" fontId=\"0\" fillId=\"0\" borderId=\"0\"/></cellStyleXfs>" +
                "<cellXfs count=\"12\">" +
                /*0 Default    */ "<xf numFmtId=\"0\" fontId=\"0\" fillId=\"0\" borderId=\"0\" xfId=\"0\"/>" +
                /*1 Title      */ "<xf numFmtId=\"0\" fontId=\"1\" fillId=\"0\" borderId=\"0\" xfId=\"0\" applyFont=\"1\" applyAlignment=\"1\"><alignment horizontal=\"center\" vertical=\"center\"/></xf>" +
                /*2 Subtitle   */ "<xf numFmtId=\"0\" fontId=\"2\" fillId=\"0\" borderId=\"0\" xfId=\"0\" applyFont=\"1\"/>" +
                /*3 Header     */ "<xf numFmtId=\"0\" fontId=\"3\" fillId=\"2\" borderId=\"1\" xfId=\"0\" applyFont=\"1\" applyFill=\"1\" applyBorder=\"1\" applyAlignment=\"1\"><alignment horizontal=\"center\" vertical=\"center\" wrapText=\"1\"/></xf>" +
                /*4 Text       */ "<xf numFmtId=\"0\" fontId=\"0\" fillId=\"0\" borderId=\"1\" xfId=\"0\" applyBorder=\"1\" applyAlignment=\"1\"><alignment vertical=\"center\"/></xf>" +
                /*5 TextCenter */ "<xf numFmtId=\"0\" fontId=\"0\" fillId=\"0\" borderId=\"1\" xfId=\"0\" applyBorder=\"1\" applyAlignment=\"1\"><alignment horizontal=\"center\" vertical=\"center\"/></xf>" +
                /*6 Money      */ "<xf numFmtId=\"164\" fontId=\"0\" fillId=\"0\" borderId=\"1\" xfId=\"0\" applyNumberFormat=\"1\" applyBorder=\"1\"/>" +
                /*7 Date       */ "<xf numFmtId=\"165\" fontId=\"0\" fillId=\"0\" borderId=\"1\" xfId=\"0\" applyNumberFormat=\"1\" applyBorder=\"1\" applyAlignment=\"1\"><alignment horizontal=\"center\"/></xf>" +
                /*8 TotalLabel */ "<xf numFmtId=\"0\" fontId=\"4\" fillId=\"3\" borderId=\"1\" xfId=\"0\" applyFont=\"1\" applyFill=\"1\" applyBorder=\"1\"/>" +
                /*9 TotalMoney */ "<xf numFmtId=\"164\" fontId=\"4\" fillId=\"3\" borderId=\"1\" xfId=\"0\" applyNumberFormat=\"1\" applyFont=\"1\" applyFill=\"1\" applyBorder=\"1\"/>" +
                /*10 Integer   */ "<xf numFmtId=\"3\" fontId=\"0\" fillId=\"0\" borderId=\"1\" xfId=\"0\" applyNumberFormat=\"1\" applyBorder=\"1\" applyAlignment=\"1\"><alignment horizontal=\"center\"/></xf>" +
                /*11 TotalInt  */ "<xf numFmtId=\"3\" fontId=\"4\" fillId=\"3\" borderId=\"1\" xfId=\"0\" applyNumberFormat=\"1\" applyFont=\"1\" applyFill=\"1\" applyBorder=\"1\" applyAlignment=\"1\"><alignment horizontal=\"center\"/></xf>" +
                "</cellXfs>" +
                "<cellStyles count=\"1\"><cellStyle name=\"Normal\" xfId=\"0\" builtinId=\"0\"/></cellStyles>" +
                "</styleSheet>";
        }
    }

    public class XlsxSheet
    {
        private class Cell
        {
            public object Value;
            public int Style;
        }

        private readonly SortedDictionary<int, SortedDictionary<int, Cell>> rows = new SortedDictionary<int, SortedDictionary<int, Cell>>();
        private readonly SortedDictionary<int, double> colWidths = new SortedDictionary<int, double>();
        private readonly List<string> merges = new List<string>();
        private int freezeRow = 0;

        public string Name { get; private set; }

        internal XlsxSheet(string name)
        {
            Name = name;
        }

        /// <summary>Ghi giá trị vào ô (row, col bắt đầu từ 1).</summary>
        public void SetCell(int row, int col, object value, int style = XlsxStyle.Default)
        {
            if (row < 1 || col < 1) throw new ArgumentOutOfRangeException();
            SortedDictionary<int, Cell> r;
            if (!rows.TryGetValue(row, out r))
            {
                r = new SortedDictionary<int, Cell>();
                rows[row] = r;
            }
            r[col] = new Cell { Value = value, Style = style };
        }

        public void SetColumnWidth(int col, double width)
        {
            colWidths[col] = width;
        }

        public void Merge(int row1, int col1, int row2, int col2)
        {
            merges.Add(CellRef(row1, col1) + ":" + CellRef(row2, col2));
        }

        /// <summary>Cố định các dòng phía trên dòng chỉ định khi cuộn.</summary>
        public void FreezeRowsAbove(int row)
        {
            freezeRow = row;
        }

        internal string BuildXml()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("<?xml version=\"1.0\" encoding=\"UTF-8\" standalone=\"yes\"?>");
            sb.Append("<worksheet xmlns=\"http://schemas.openxmlformats.org/spreadsheetml/2006/main\" xmlns:r=\"http://schemas.openxmlformats.org/officeDocument/2006/relationships\">");

            if (freezeRow > 1)
            {
                sb.Append("<sheetViews><sheetView workbookViewId=\"0\"><pane ySplit=\"" + (freezeRow - 1) +
                          "\" topLeftCell=\"A" + freezeRow + "\" activePane=\"bottomLeft\" state=\"frozen\"/></sheetView></sheetViews>");
            }

            if (colWidths.Count > 0)
            {
                sb.Append("<cols>");
                foreach (var kv in colWidths)
                {
                    sb.Append("<col min=\"" + kv.Key + "\" max=\"" + kv.Key + "\" width=\"" +
                              kv.Value.ToString(CultureInfo.InvariantCulture) + "\" customWidth=\"1\"/>");
                }
                sb.Append("</cols>");
            }

            sb.Append("<sheetData>");
            foreach (var rowKv in rows)
            {
                sb.Append("<row r=\"" + rowKv.Key + "\">");
                foreach (var cellKv in rowKv.Value)
                {
                    AppendCell(sb, rowKv.Key, cellKv.Key, cellKv.Value);
                }
                sb.Append("</row>");
            }
            sb.Append("</sheetData>");

            if (merges.Count > 0)
            {
                sb.Append("<mergeCells count=\"" + merges.Count + "\">");
                foreach (string m in merges) sb.Append("<mergeCell ref=\"" + m + "\"/>");
                sb.Append("</mergeCells>");
            }

            sb.Append("<pageMargins left=\"0.5\" right=\"0.5\" top=\"0.6\" bottom=\"0.6\" header=\"0.3\" footer=\"0.3\"/>");
            sb.Append("</worksheet>");
            return sb.ToString();
        }

        private static void AppendCell(StringBuilder sb, int row, int col, Cell cell)
        {
            string r = CellRef(row, col);
            string s = cell.Style > 0 ? " s=\"" + cell.Style + "\"" : "";
            object v = cell.Value;

            if (v == null || v == DBNull.Value)
            {
                sb.Append("<c r=\"" + r + "\"" + s + "/>");
                return;
            }

            if (v is DateTime)
            {
                double oa = ((DateTime)v).ToOADate();
                sb.Append("<c r=\"" + r + "\"" + s + "><v>" + oa.ToString(CultureInfo.InvariantCulture) + "</v></c>");
                return;
            }

            if (v is decimal || v is double || v is float || v is int || v is long || v is short || v is byte)
            {
                string num = Convert.ToString(v, CultureInfo.InvariantCulture);
                sb.Append("<c r=\"" + r + "\"" + s + "><v>" + num + "</v></c>");
                return;
            }

            sb.Append("<c r=\"" + r + "\"" + s + " t=\"inlineStr\"><is><t xml:space=\"preserve\">" +
                      Escape(v.ToString()) + "</t></is></c>");
        }

        internal static string CellRef(int row, int col)
        {
            string letters = "";
            int c = col;
            while (c > 0)
            {
                int m = (c - 1) % 26;
                letters = (char)('A' + m) + letters;
                c = (c - 1) / 26;
            }
            return letters + row;
        }

        internal static string Escape(string text)
        {
            if (string.IsNullOrEmpty(text)) return string.Empty;
            StringBuilder sb = new StringBuilder(text.Length);
            foreach (char ch in text)
            {
                switch (ch)
                {
                    case '&': sb.Append("&amp;"); break;
                    case '<': sb.Append("&lt;"); break;
                    case '>': sb.Append("&gt;"); break;
                    case '"': sb.Append("&quot;"); break;
                    case '\'': sb.Append("&apos;"); break;
                    default:
                        // Loại bỏ ký tự điều khiển không hợp lệ trong XML
                        if (ch < 0x20 && ch != '\t' && ch != '\n' && ch != '\r') continue;
                        sb.Append(ch);
                        break;
                }
            }
            return sb.ToString();
        }
    }
}

