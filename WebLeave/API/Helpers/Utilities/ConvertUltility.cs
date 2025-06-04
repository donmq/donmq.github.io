namespace API.Helpers.Utilities
{
    public static class ConvertUltility
    {
        public static string ToDetailDay(this string day)
        {
            try
            {
                return Math.Round(decimal.Parse(day), 5) + "d" + " - " + Math.Round(decimal.Parse((Convert.ToDouble(day) * 8).ToString()), 5) + "h";
            }
            catch
            {
                return "0";
            }
        }

        public static string CheckStatus(this int? status)
        {
            return status switch
            {
                1 => "Wait",
                2 => "Approved",
                3 => "Rejected",
                _ => "Finish",
            };
        }

        public static DateTime ConvertToDateTime(this string value)
        {
            var dt = Convert.ToDateTime(value);
            return new DateTime(dt.Year, dt.Month, dt.Day, dt.Hour, dt.Minute, 0);
        }
    }
}
