using Aspose.Cells;
using Aspose.Cells.Drawing;

namespace API.Helpers.Utilities
{
    public static class AsposeUtility
    {
        public static void Install()
        {
            try
            {
                License cellLicense = new();
                string filePath = Directory.GetCurrentDirectory() + "\\wwwroot\\Resources\\" + "Aspose.Total.lic";
                cellLicense.SetLicense(filePath);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public static Style SetAllBorders(this Style style)
        {
            style.Borders[BorderType.BottomBorder].LineStyle = CellBorderType.Thin;
            style.Borders[BorderType.TopBorder].LineStyle = CellBorderType.Thin;
            style.Borders[BorderType.LeftBorder].LineStyle = CellBorderType.Thin;
            style.Borders[BorderType.RightBorder].LineStyle = CellBorderType.Thin;
            return style;
        }

        public static Style SetAlignCenter(this Style style)
        {
            style.IsTextWrapped = true;
            style.HorizontalAlignment = TextAlignmentType.Center;
            style.VerticalAlignment = TextAlignmentType.Center;
            return style;
        }

        public static Picture SetPictureSize(this Picture picture)
        {
            int originWidth = picture.Width;
            int originHeight = picture.Height;
            int customWidth = 120;
            int customHeight = (int)(originHeight * customWidth / originWidth);

            picture.Top = 5;
            picture.Left = 5;
            picture.Width = customWidth;
            picture.Height = customHeight;

            return picture;
        }

        public static void ApplyBorders(Worksheet worksheet)
        {
            // Lấy hàng và cột cuối cùng có dữ liệu
            int lastRow = worksheet.Cells.MaxDataRow;
            int lastColumn = worksheet.Cells.MaxDataColumn;

            // Tạo đối tượng Style và đặt đường viền cho tất cả các cạnh
            Style style = worksheet.Workbook.CreateStyle();
            style.Borders[BorderType.TopBorder].LineStyle = CellBorderType.Thin;
            style.Borders[BorderType.BottomBorder].LineStyle = CellBorderType.Thin;
            style.Borders[BorderType.LeftBorder].LineStyle = CellBorderType.Thin;
            style.Borders[BorderType.RightBorder].LineStyle = CellBorderType.Thin;

            // Tạo StyleFlag để chỉ định rằng Border sẽ được áp dụng
            StyleFlag styleFlag = new() { Borders = true };

            // Áp dụng style cho toàn bộ vùng dữ liệu
            Aspose.Cells.Range range = worksheet.Cells.CreateRange(1, 0, lastRow, lastColumn + 1);
            range.ApplyStyle(style, styleFlag);
        }
    }
}