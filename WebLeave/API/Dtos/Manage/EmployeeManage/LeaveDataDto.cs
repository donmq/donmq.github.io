namespace API.Dtos.Manage.EmployeeManage
{
public class LeaveDataDto
    {
        public int LeaveID { get; set; }
        public string LeaveArchive { get; set; } // mã Lưu trũ
        public int? EmpID { get; set; }
        public int? CateID { get; set; }
        public DateTime? Time_Start { get; set; }  // bắt đầu
        public DateTime? Time_End { get; set; } // Kết thúc
        public DateTime? DateLeave { get; set; }
        public double? LeaveDay { get; set; }
        public int? Approved { get; set; }
        public DateTime? Time_Applied { get; set; } // Thời gian duyệt
        public string TimeLine { get; set; }
        public string Comment { get; set; }
        public bool? LeavePlus { get; set; }
        public bool? LeaveArrange { get; set; }
        public int? UserID { get; set; } //Người gửi
        public int? ApprovedBy { get; set; }  // Người duyệt
        public int? EditRequest { get; set; }
        public bool? Status_Line { get; set; }
        public DateTime? Created { get; set; } // Xin phép lúc 
        public string Created_day { get; set; } 
        public DateTime? Updated { get; set; }
        public bool? Status_Lock { get; set; }
        public string MailContent_Lock { get; set; }
        public string CommentArchive { get; set; }


        // Sử dụng cho Main Detail
        public string NumberDay { get; set; } // Số Ngày
        public string Category { get; set; } // Loại phép
        public string StatusString { get; set; } // Tình Trạng

        // Sử dụng cho Main Sub Detail
        public string FullName { get; set; }

        public int? GBID { get; set; }
        public string PartSym { get; set; }
        public string DeptSym { get; set; }
        public string BuildingSym { get; set; }
        public string AreaSym { get; set; }

        public string Sender { get; set; } // Tên người gửi
        public string ApproveName { get; set; } // Tên người gửi
        public int  RoleEditComments { get; set; } // Quyền được chỉnh sửa ghi chú SEA/HR xác nhận
        public string EmployeeId { get; set; }
        public List<string> CommentList { get; set; } // Danh sách comments
        public string DepartmentName { get; set; } // Bộ phận [A09 - A01-Bảo vệ]
        public string DateIn { get; set; } //Ngày vào
        public List<string> ApproveList  { get; set; } // Danh sách người duyệt


    }
}
	