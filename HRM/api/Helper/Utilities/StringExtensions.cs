namespace API.Helper.Utilities
{
    public static class StringExtensions
    {
        public static bool CheckDecimalValue(this string value, int totalDigits = 15, int digitsAfterDot = 4)
        {
            {
                if (string.IsNullOrEmpty(value))
                    return false;

                // Kiểm tra xem chuỗi có phải là số thập phân hợp lệ không (có thể là số nguyên hoặc số thập phân)
                if (!decimal.TryParse(value, out _))
                    return false;

                int dotPosition = value.IndexOf('.');
                if (dotPosition == -1) // Nếu là số nguyên
                {
                    return value.Length <= totalDigits;
                }
                else // Nếu là số thập phân
                {
                    int totalDigitsValue = value.Length - 1; // Trừ đi 1 vì có một dấu chấm
                    int digitsAfterDotValue = totalDigitsValue - dotPosition;

                    // Kiểm tra số lượng chữ số tổng cộng và số lượng chữ số sau dấu chấm thập phân
                    return totalDigitsValue <= totalDigits && digitsAfterDotValue <= digitsAfterDot;
                }
            }
        }
        public static bool IsTimeSpanFormat(this string value) // Format: "HHmm"
        {
            if (string.IsNullOrWhiteSpace(value))
                return false;
            else if (value == "9999")
                return true;
            else
            {
                if (value.Length != 4)
                    return false;
                else
                {
                    if (!value.All(char.IsNumber))
                        return false;
                    else
                    {
                        var hour = int.Parse(value[..2]);
                        var minute = int.Parse(value[2..]);
                        if (hour < 0 || hour >= 24 || minute < 0 || minute >= 60)
                            return false;
                        return TimeSpan.TryParse($"{hour}:{minute}", out _);
                    }
                }
            }
        }
        public static string GetFileName(this string value) => string.Concat(string.Join("", value[..value.LastIndexOf(".")].Split(Path.GetInvalidFileNameChars().Append('.').ToArray())), value[value.LastIndexOf(".")..]);

        public static TimeSpan ToTimeSpan(this string value) => new(int.Parse(value[..2]), int.Parse(value[2..]), 0);
        public static DateTime ToDateTime(this string value) => Convert.ToDateTime(value);
        public static bool isIntRange(this string value, int min = 0, int max = 99) => !string.IsNullOrWhiteSpace(value) && int.TryParse(value, out int intValue) && intValue >= min && intValue <= max;

        /// <summary>
        /// Trả về ngày bắt đầu trong tháng
        /// </summary>
        /// <param name="dateTime"></param>
        /// <returns> </returns>
        public static DateTime ToFirstDateOfMonth(this DateTime dateTime) => new(dateTime.Year, dateTime.Month, 1);

        /// <summary>
        /// Trả về ngày cuối cùng của tháng
        /// </summary>
        /// <param name="dateTime"></param>
        /// <returns></returns>
        public static DateTime ToLastDateOfMonth(this DateTime dateTime) => new(dateTime.Year, dateTime.Month, DateTime.DaysInMonth(dateTime.Year, dateTime.Month));

        public static string ToHourMinuteString(this TimeSpan time) => $"{(int)time.TotalMinutes / 60:D2}{(int)time.TotalMinutes % 60:D2}";
        public static string ToMonthDayString(this DateTime dt) => $"{dt:MMdd}";

        /// <summary>
        /// Chuyển đổi ngày tháng thành dạng YYYYMM
        /// </summary>
        /// <param name="dateTime"></param>
        /// <returns></returns>
        public static string ToYearMonthString(this DateTime dateTime) => $"{dateTime:YYYYMM}";
        public static string GetVariableName(this string keyPrefix, string key) => keyPrefix.Replace("{0}", key);


        /// <summary>
        /// Chuyển đổi 1 số thành string có dạng số thập phân 2 chữ số, VD: 100.12123123 => 100.12
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string ToNumberHasTwoDecimal(this object value)
        {
            if (((double)value).HasDecimal())
                return ((double)value).ToString("F2");
            return value.ToString();
        }
    }

    public static class NumberExtention
    {
        /// <summary>
        /// Kiểm tra 1 số có phải số thập phân hay không ?
        /// </summary>
        /// <param name="number"></param>
        /// <returns></returns>
        public static bool HasDecimal(this double number) => number % 1 != 0;
        public static bool HasDecimal(string number) => number.ToString().Contains(".");

        public static int ToRoundInt(this decimal number)
        {
            // Lấy phần nguyên và phần thập phân
            int integerPart = (int)number; // Phần nguyên
            decimal decimalPart = number - integerPart; // Phần thập phân

            // Kiểm tra phần thập phân
            if (decimalPart >= 0.5m)
            {
                // Làm tròn lên
                return integerPart + 1;
            }
            else
            {
                // Làm tròn xuống
                return integerPart;
            }
        }
    }
}