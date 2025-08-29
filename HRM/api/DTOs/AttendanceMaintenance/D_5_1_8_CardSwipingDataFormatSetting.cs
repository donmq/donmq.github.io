namespace API.DTOs.AttendanceMaintenance;

public partial class HRMS_Att_Swipecard_SetDto
{
    public string Factory { get; set; }

    public int Employee_Start { get; set; }

    public int Employee_End { get; set; }

    public int Time_Start { get; set; }

    public int Time_End { get; set; }

    public int Date_Start { get; set; }

    public int Date_End { get; set; }

    public string Update_By { get; set; }

    public string Update_Time { get; set; }
}

public partial class CardSwipingDataFormatSettingMain
{
    public string Factory { get; set; }
    public string Employee_Id_Card_No { get; set; }
    public string Time { get; set; }
    public string Date { get; set; }

}
