
namespace Machine_API.DTO
{
    public class AssetsLendMaintainDto
    {
        public int STT { get; set; }
        public string AssnoID { get; set; }
        public string Spec { get; set; }
        public string Dept_ID { get; set; }
        public string Plno { get; set; }
        public string State { get; set; }
        public string MachineName_EN { get; set; }
        public string MachineName_Local { get; set; }
        public string MachineName_CN { get; set; }
        public string Supplier { get; set; }
        public string OwnerFty { get; set; }
        public string UsingFty { get; set; }
        public string IO_Kind { get; set; }
        public string IO_Reason { get; set; }
        public DateTime IO_Date { get; set; }
        public string IO_Confirm { get; set; }
        public DateTime Re_Date { get; set; }
        public string Re_Confirm { get; set; }
        public string Remark { get; set; }
        public string Insert_By { get; set; }
        public DateTime? Insert_At { get; set; }
        public string Update_By { get; set; }
        public DateTime? Update_At { get; set; }
    }
    public class AssetsLendMaintainParam
    {
        public string LendDate { get; set; }
        public string MachineID { get; set; }
        public string LendTo { get; set; }
        public string Return { get; set; }
    }
}