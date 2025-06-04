namespace API.Dtos.Common
{
    public class HistoryEmpDto
    {
        public int HisrotyID { get; set; }
        public int? EmpID { get; set; }
        public int? YearIn { get; set; }
        public double? TotalDay { get; set; }
        public double? Arrange { get; set; }
        public double? Agent { get; set; }
        public double? CountArran { get; set; }
        public double? CountRestArran { get; set; }
        public double? CountAgent { get; set; }
        public double? CountRestAgent { get; set; }
        public double? CountTotal { get; set; }
        public double? CountLeave { get; set; }
        public DateTime? Updated { get; set; }

        // ------ AREA ------
        public int? AreaID { get; set; } // Mã khu vực

        // ------ DEPARTMENT ------
        public int? DeptID { get; set; } // Mã phòng ban
        public string DeptCode { get; set; } // Mã Code phòng ban
        public string DeptName { get; set; } // Tên phòng ban

        // ------ PART ------
        public int? PartId { get; set; } // Mã phòng
        public string PartName { get; set; } // Tên Phòng

        // ------ EMPLOYEE ------
        public bool? Visible { get; set; } // Trạng thái // Đang làm việc, Disable(Vô hiệu hóa)
        public string EmpName { get; set; }
        public string EmpNumber { get; set; }
        public DateTime? DateIn { get; set; } // Ngày vào
        public string GroupBaseName { get; set; } // Tên Nhóm
    }
}