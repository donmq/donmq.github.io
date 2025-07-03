using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace eTierV2_API.Models
{
    public class MES_Dept_Target
    {
        /// <summary>
        /// 廠別
        /// </summary>
        [Key]
        [StringLength(1)]
        public string Factory_ID { get; set; }
        /// <summary>
        /// &#24180;&#24230;
        /// </summary>
        [Key]
        public short Year_Target { get; set; }
        /// <summary>
        /// &#26376;&#20221;
        /// </summary>
        [Key]
        public byte Month_Target { get; set; }
        /// <summary>
        /// &#21934;&#20301;&#20195;&#34399;
        /// </summary>
        [Key]
        [StringLength(4)]
        public string Dept_ID { get; set; }
        /// <summary>
        /// &#29986;&#37327;&#30446;&#27161;
        /// </summary>
        [Column(TypeName = "numeric(5, 0)")]
        public decimal? Output_Target { get; set; }
        /// <summary>
        /// &#33391;&#29575;&#30446;&#27161;
        /// </summary>
        [Column(TypeName = "numeric(5, 2)")]
        public decimal? RFT_Target { get; set; }
        /// <summary>
        /// BA&#30446;&#27161;
        /// </summary>
        [Column(TypeName = "numeric(3, 1)")]
        public decimal? Star_Target { get; set; }
        [Column(TypeName = "numeric(3, 1)")]
        public decimal? Perform_Target { get; set; }
        [Column(TypeName = "numeric(5, 0)")]
        public decimal? EOLR_Target { get; set; }
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
        [Column(TypeName = "numeric(5, 2)")]
        public decimal? IE_Target { get; set; }
    }
}