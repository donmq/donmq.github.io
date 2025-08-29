namespace API.DTOs.AttendanceMaintenance
{
  public class NewResignedEmployeeDataPrintingParam
  {
    public string Factory { get; set; }
    public string Kind { get; set; }
    public string Date_From { get; set; }
    public string Date_To { get; set; }
    public string Department { get; set; }
    public string Lang { get; set; }
  }
  public class NewResignedEmployeeDataPrintingDto
  {
    public string Department { get; set; }
    public string Department_Name { get; set; }
    public string Employee_ID { get; set; }
    public string Local_Full_Name { get; set; }
    public string Position_Title { get; set; }
    public DateTime? Onboard_Date { get; set; }
    public DateTime? Resign_Date { get; set; }
    public string Resign_Reason { get; set; }
    public string Education { get; set; }
    public string Registered_Address { get; set; }
    public string Registered_Province_Directly { get; set; }
    public string Registered_City { get; set; }
    public DateTime? Birthday { get; set; }
    public string Transportation_Method { get; set; }
    public string Work_Type { get; set; }
    public string Phone_Number { get; set; }
    public string Mobile_Phone_Number { get; set; }
    public string Gender { get; set; }
    public DateTime? Contract_Date { get; set; }
    public DateTime? Insurance_Date { get; set; }
    public double Seniority { get; set; }
    public string Lang { get; set; }
  }
}