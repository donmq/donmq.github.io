namespace API.DTOs.OrganizationManagement
{
    public class OrganizationChartParam
    {
        public string division { get; set; }
        public string factory { get; set; }
        public string department { get; set; }
        public string level { get; set; }
        public string lang { get; set; }
        public string parent_Code { get; set; }
        public string root_Code { get; set; }
        public int approved_Headcount { get; set; }
        public int actual_Headcount { get; set; }
        public bool is_dept_Layer { get; set; }
        public string supervisor_Employee_ID { get; set; }
        public string supervisor_Type { get; set; }
        public string name { get; set; }
        public string cssClass { get; set; }
        public string image { get; set; }
    }
    public class INodeDto : OrganizationChartParam
    {
        public int total_sub_approved_Headcount
        {
            get
            {
                return
                childs.Count > 0 ?
                childs.Sum(x => x.total_sub_approved_Headcount) :
                is_dept_Layer ?
                0 :
                approved_Headcount;
            }
        }
        public int total_sub_actual_Headcount
        {
            get
            {
                return
                childs.Count > 0 ?
                childs.Sum(x => x.total_sub_actual_Headcount) :
                is_dept_Layer ?
                0 :
                actual_Headcount;
            }
        }
        public string title
        {
            get
            {
                return
                supervisor_Employee_ID != null && supervisor_Type != null ?
                $"{supervisor_Employee_ID}\n{supervisor_Type}\n{total_sub_approved_Headcount}/{total_sub_actual_Headcount}" :
                supervisor_Employee_ID != null && supervisor_Type == null ?
                $"{supervisor_Employee_ID}\n{total_sub_approved_Headcount}/{total_sub_actual_Headcount}" :
                supervisor_Employee_ID == null && supervisor_Type != null ?
                $"{supervisor_Type}\n{total_sub_approved_Headcount}/{total_sub_actual_Headcount}" :
                $"{total_sub_approved_Headcount}/{total_sub_actual_Headcount}";
            }
        }
        public List<INodeDto> childs { get; set; }
    }
    public class INodeDtoFinal : OrganizationChartParam
    {
        public int total_sub_approved_Headcount { get; set; }
        public int total_sub_actual_Headcount { get; set; }
        public string title { get; set; }
        public List<INodeDtoFinal> childs { get; set; }
    }
}