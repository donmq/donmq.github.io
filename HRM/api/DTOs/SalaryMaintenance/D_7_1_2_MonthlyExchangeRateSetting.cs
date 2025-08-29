namespace API.DTOs.SalaryMaintenance
{
    public class MonthlyExchangeRateSetting_Param
    {
        public string Rate_Month { get; set; }
        public string Rate_Month_Str { get; set; }
        public string Lang { get; set; }
    }
    public class MonthlyExchangeRateSetting_Main
    {
        public string Rate_Month { get; set; }
        public string Rate_Month_Str { get; set; }
        public string Kind { get; set; }
        public string Kind_Name { get; set; }
        public string Currency { get; set; }
        public string Currency_Name { get; set; }
        public string Exchange_Currency { get; set; }
        public string Exchange_Currency_Name { get; set; }
        public string Rate { get; set; }
        public string Rate_Date { get; set; }
        public string Rate_Date_Str { get; set; }
        public string Update_By { get; set; }
        public string Update_Time { get; set; }
        public string Update_Time_Str { get; set; }
        public bool Is_Duplicate { get; set; }
    }
    public class MonthlyExchangeRateSetting_Update
    {
        public MonthlyExchangeRateSetting_Param Param { get; set; }
        public List<MonthlyExchangeRateSetting_Main> Data { get; set; }
    }

}