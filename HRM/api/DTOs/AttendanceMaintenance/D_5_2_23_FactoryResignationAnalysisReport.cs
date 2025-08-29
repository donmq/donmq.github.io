

namespace API.DTOs.AttendanceMaintenance
{
    public class FactoryResignationAnalysisReport_Param
    {
        public string Factory { get; set; }
        public string Date_From_Str { get; set; }
        public string Date_To_Str { get; set; }
        public DateTime Date_From_Date { get; set; }
        public DateTime Date_To_Date { get; set; }
        public List<string> Permission_Group { get; set; }
        public string Lang { get; set; }
        public int Total_Rows { get; set; }
        public string Function_Type { get; set; }
    }
    public class FactoryResignationAnalysisReport_Excel
    {
        public string Factory { get; set; }
        public string Permission_Group { get; set; }
        public string Date_From { get; set; }
        public string Date_To { get; set; }
        public string Print_By { get; set; }
        public string Print_Date { get; set; }
        public List<FactoryResignationAnalysisReport_Detail> Detail { get; set; }
    }
    public class FactoryResignationAnalysisReport_Detail
    {
        public DateTime Att_Date { get; set; }
        public decimal totemp { get; set; }
        public decimal absent { get; set; }
        public decimal absentr { get; set; }
        public decimal vacu { get; set; }
        public decimal vacuer { get; set; }
        public decimal vacj { get; set; }
        public decimal vacjer { get; set; }
        public decimal aff { get; set; }
        public decimal affer { get; set; }
        public decimal aff_q { get; set; }
        public decimal affer_q { get; set; }
        public decimal dis { get; set; }
        public decimal diser { get; set; }
        public decimal leave { get; set; }
        public decimal leaver { get; set; }
        public decimal wkoff_i { get; set; }
        public decimal wkoffr_i { get; set; }
        public decimal wkoff_k { get; set; }
        public decimal wkoffr_k { get; set; }
        public decimal wkoff_l { get; set; }
        public decimal wkoffr_l { get; set; }
        public decimal wkoff_t { get; set; }
        public decimal wkoffr_t { get; set; }
        public decimal resign { get; set; }
        public decimal resignr { get; set; }
        public decimal actemp { get; set; }
        public decimal actemp2 { get; set; }
        public decimal totabsr { get; set; }
        public decimal totabsr2 { get; set; }
        public decimal totabsr3 { get; set; }
        public decimal pregn { get; set; }
        public decimal newop { get; set; }
        public decimal oldres { get; set; }
        public decimal wc1 { get; set; }
        public decimal wc2 { get; set; }
        public decimal wc3 { get; set; }
        public decimal wc4 { get; set; }
        public decimal totalwc { get; set; }
        public decimal Total { get; set; }
    }
}