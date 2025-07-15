using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Machine_API.Models.MT
{
    public partial class Control_File_Temp
    {
        /// <summary>
        /// 拋轉程式碼
        /// </summary>
        [Key]
        [StringLength(50)]
        public string SPEC_ID { get; set; }
        /// <summary>
        /// &#21807;&#19968;&#24207;&#34399;
        /// </summary>
        [Key]
        [StringLength(20)]
        public string SID { get; set; }
        /// <summary>
        /// &#36039;&#26009;&#34920;&#21517;&#31281;
        /// </summary>
        [Key]
        [StringLength(50)]
        public string Table_Name { get; set; }
        /// <summary>
        /// &#24288;&#21029;
        /// </summary>
        [Key]
        [StringLength(50)]
        public string Factory_ID { get; set; }
        /// <summary>
        /// &#36039;&#26009;&#34920;&#25291;&#36681;&#25976;
        /// </summary>
        [Column(TypeName = "decimal(9, 0)")]
        public decimal? Count { get; set; }
        /// <summary>
        /// &#36039;&#26009;&#34920;&#25976;
        /// </summary>
        [Column(TypeName = "decimal(9, 0)")]
        public decimal? Table_Count { get; set; }
        /// <summary>
        /// &#25291;&#36681;&#30908;
        /// </summary>
        [StringLength(1)]
        public string Control_Flag { get; set; }
        /// <summary>
        /// &#30064;&#21205;&#32773;
        /// </summary>
        [StringLength(50)]
        public string Update_By { get; set; }
        /// <summary>
        /// &#30064;&#21205;&#26178;&#38291;
        /// </summary>
        [Column(TypeName = "datetime")]
        public DateTime? Update_Time { get; set; }
    }
}