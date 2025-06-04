namespace API.Dtos.Manage.EmployeeManage

{
    public partial class EmployeeDto
    {
        
        public int EmpID { get; set; }
        public string EmpName { get; set; }
        public string EmpNumber { get; set; }
        public string DateIn { get; set; }
        public int? PositionID { get; set; }
        public string Descript { get; set; }
        public int? GBID { get; set; }
        public bool Visible { get; set; }
        public int? PartID { get; set; }
    }
    public partial class ListEmployeeDto
    {
        public string DeptCode { get; set; }

        //Cot 2
        public string PositionSym { get; set; }
        //Cột 3 + 4 
        public int EmpID { get; set; }
        public string EmpName { get; set; }
        public string EmpNumber { get; set; }
        //Cột 5
        public bool? Visible { get; set; }
        public int? PartID { get; set; }
        public string PartName { get; set; }
    }

}