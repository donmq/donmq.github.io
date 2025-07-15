using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Machine_API.Models.MT
{
    public partial class SAP_Cost_Center_Changed_Record
    {
        /// <summary>
        /// SID
        /// </summary>
        [Key]
        [StringLength(20)]
        public string SID { get; set; }
        /// <summary>
        /// Company Code
        /// </summary>
        [Key]
        [StringLength(4)]
        public string Company_Code { get; set; }
        /// <summary>
        /// Main Asset Number
        /// </summary>
        [Key]
        [StringLength(12)]
        public string Main_Asset_Number { get; set; }
        /// <summary>
        /// Asset Subnumber
        /// </summary>
        [Key]
        [StringLength(4)]
        public string Asset_Subnumber { get; set; }
        /// <summary>
        /// Asset Location
        /// </summary>
        [StringLength(10)]
        public string Asset_Location { get; set; }
        [StringLength(1)]
        public string Biz_Flag { get; set; }
        [StringLength(1)]
        public string Biz_Tflag { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? Biz_P_Time { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? Biz_Y_Time { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? Biz_Time { get; set; }
    }
}