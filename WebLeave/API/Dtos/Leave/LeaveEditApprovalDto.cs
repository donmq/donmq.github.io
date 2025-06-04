namespace API.Dtos.Leave
{
    public class LeaveEditApprovalDto
    {
        public int? slEditApproval {get;set;}
        public int empId {get;set;}
        public int leaveID {get;set;}
        public string notiText {get;set;}
    }
}