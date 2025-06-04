using API.Models;

namespace API.Dtos.Leave.Personal
{
    public class EmployeeDataDto : Employee
    {
        public string DeptCode { get; set; }
        public string PartNameVN { get; set; }
        public string PartNameEN { get; set; }
        public string PartNameTW { get; set; }
        public string PositionNameVN { get; set; }
        public string PositionNameEN { get; set; }
        public string PositionNameTW { get; set; }
        public string DateInVN { get; set; }
        public string DateInEN { get; set; }
        public string DateInTW { get; set; }
        public double CountRestAgent { get; set; }
        public bool CheckUser { get; set; }
    }
}