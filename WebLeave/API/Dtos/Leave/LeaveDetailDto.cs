namespace API.Dtos.Leave
{
    public class LeaveDetailDto
    {
        public bool EditCommentArchive { get; set; }
        public bool NotiUser { get; set; }
        public bool RoleApproved { get; set; }
        public bool EnablePreviousMonthEditRequest { get; set; }
        public LeaveDataDto LeaveData { get; set; }
        public List<string> ApprovalPersons { get; set; }
        public List<LeaveDataDto> DeletedLeaves { get; set; }
    }
}