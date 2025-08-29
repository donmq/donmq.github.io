using API.Models;

namespace API.DTOs.AttendanceMaintenance
{
    public class HRMS_Att_Swipe_Card_Excute_Param
    {
        public string Factory { get; set; }

        /// <summary>
        /// Clock Out Date
        /// </summary>
        /// <value></value>
        public string ClockOffDay { get; set; }

        /// <summary>
        /// Clock In Date
        /// </summary>
        /// <value></value>
        public string WorkOnDay { get; set; }
        public string Holiday { get; set; }
        public string NationalHoliday { get; set; }
        public string CurrentUser { get; set; }
    }

    public class Temp_Swipe_Card
    {

        public int RowId { get; set; }
        public string Mark { get; set; }
        public string USER_GUID { get; set; }
        public string Factory { get; set; }
        public string Empno_ID { get; set; }
        public string Overnight { get; set; }
        public int CurrentYear { get; set; }
        public string Card_Time { get; set; }
        public string Card_Date { get; set; }
        public string hm { get; set; }
        public string md { get; set; }
        public DateTime TimeOrderBy
        {
            get
            {
                return new DateTime(CurrentYear, Convert.ToInt16(md[..2]), Convert.ToInt16(md[2..]), Convert.ToInt16(hm[..2]), Convert.ToInt16(hm[2..]), 0);
            }
        }
    }


    public class EmployyeeLeaveApplication
    {
        public string USER_GUID { get; set; }
        public string Factory { get; set; }
        public DateTime Att_Date { get; set; }
        public string Department { get; set; }
        public string Assigned_Department { get; set; }
        // public string LeaveEmployeeId { get; set; }
        // public string PersonalEmployeeId { get; set; }
        public string EmployeeId { get; set; }
        public string Work_Shift_Type { get; set; }
        public string Leave_Code { get; set; }
        public string Clock_In { get; set; }
        public string Clock_Out { get; set; }
        public string Overtime_ClockIn { get; set; }
        public string Overtime_ClockOut { get; set; }
        public int Days { get; set; }
        public string Holiday { get; set; }
        public string Update_By { get; set; }
        public DateTime Update_Time { get; set; }
        public string EmploymentStatus { get; set; }
        public string Assigned_Employee_ID { get; set; }
    }

    public class ExceptionErrorData
    {

        public string Name { get; set; }
        public string Location { get; set; }
        public bool IsSuccess { get; set; }
        public string Error { get; set; } = string.Empty;
        public GeneralData Data { get; set; }

        public ExceptionErrorData() { }
        public ExceptionErrorData(string name, string location, GeneralData data)
        {
            Name = name;
            Location = location;
            IsSuccess = true;
            Data = data;
        }
    }
    public class GeneralData
    {
        public DateTime wk_date { get; set; }
        public HRMS_Att_Yearly HRMS_Att_Yearly { get; set; }
        public HRMS_Att_Overtime_Temp HRMS_Att_Overtime_Temp { get; set; }
        public HRMS_Att_Change_Record HRMS_Att_Change_Record { get; set; }
        public HRMS_Emp_Personal HRMS_Emp_Personal { get; set; }
        public List<HRMS_Att_Swipe_Card> HRMS_Att_Swipe_Card_List { get; set; }
        public List<HRMS_Att_Temp_Record> HRMS_Att_Temp_Record_List { get; set; }

        public GeneralData() { }

        public GeneralData(HRMS_Att_Change_Record att_Change_Record)
        {
            HRMS_Att_Change_Record = att_Change_Record;
        }

        public GeneralData(HRMS_Att_Overtime_Temp att_Overtime_Temp_Values)
        {
            HRMS_Att_Overtime_Temp = att_Overtime_Temp_Values;
        }

        public GeneralData(List<HRMS_Att_Swipe_Card> swipCards)
        {
            HRMS_Att_Swipe_Card_List = swipCards;
        }
        public GeneralData(List<HRMS_Att_Temp_Record> att_Temp_Records)
        {
            HRMS_Att_Temp_Record_List = att_Temp_Records;
        }
        public GeneralData(HRMS_Emp_Personal personal)
        {
            HRMS_Emp_Personal = personal;
        }

        public GeneralData(DateTime wk_date_param, HRMS_Att_Change_Record att_Change_Record)
        {
            wk_date = wk_date_param;
            HRMS_Att_Change_Record = att_Change_Record;
        }
        public GeneralData(DateTime wk_date_param, HRMS_Att_Yearly att_Yearly)
        {
            wk_date = wk_date_param;
            HRMS_Att_Yearly = att_Yearly;
        }

        public GeneralData(HRMS_Att_Overtime_Temp overtime_Temp, HRMS_Att_Change_Record att_Change_Record)
        {
            HRMS_Att_Overtime_Temp = overtime_Temp;
            HRMS_Att_Change_Record = att_Change_Record;
        }


    }
}