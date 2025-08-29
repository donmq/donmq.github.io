
namespace API.DTOs.AttendanceMaintenance
{
    public class FactoryCalendar_MainParam
    {
        public string Division { get; set; }
        public string Factory { get; set; }
        public string Month { get; set; }
        public string Month_Str { get; set; }
        public string Lang { get; set; }
    }
    public class FactoryCalendar_MainData
    {
        public PaginationUtility<FactoryCalendar_Table> Table { get; set; }
        public List<FactoryCalendar_Calendar> Calendar { get; set; }
    }
    public class FactoryCalendar_Table
    {
        public string Division { get; set; }
        public string Factory { get; set; }
        public DateTime Att_Date { get; set; }
        public string Att_Date_Str { get; set; }
        public string Type_Code { get; set; }
        public string Type_Code_Name { get; set; }
        public string Describe { get; set; }
        public string Update_By { get; set; }
        public DateTime Update_Time { get; set; }
        public string Update_Time_Str { get; set; }
        public string Error_Message { get; set; }
    }
    public class FactoryCalendar_Calendar
    {
        public List<Week> Weeks { get; set; }
    }
    public class Week
    {
        public List<Day> Days { get; set; }
    }
    public class Day
    {
        public string Value { get; set; }
        public string Style { get; set; }
    }
    public class FactoryCalendar_MainMemory
    {
        public FactoryCalendar_MainParam Param { get; set; }
        public FactoryCalendar_MainData Data { get; set; }
    }

}