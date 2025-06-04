using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Dtos.SeaHr.ViewConfirmDaily
{
    public class ViewDataModel
    {
        public string DeptCode { get; set; }
        public string DeptName { get; set; }
        public string EmpName { get; set; }
        public string NumberID { get; set; }
        public string CateSym { get; set; }
        public string TimeStart { get; set; }
        public string TimeEnd { get; set; }
        public string DateStart { get; set; }
        public string DateEnd { get; set; }
        public string LeaveDay { get; set; }
        public string Status { get; set; }
        public string Update { get; set; }
        public string EmpNumber { get; set; }
        public string CateNameVN { get; set; }
        public string CateNameEN { get; set; }
        public string CateNameZH { get; set; }
    }
}