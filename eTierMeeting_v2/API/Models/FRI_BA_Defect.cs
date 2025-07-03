using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace eTierV2_API.Models
{
    /// <summary>
    /// BA&#25104;&#21697;&#32570;&#22833;&#27284;&#20027;&#27284;
    /// </summary>
    public partial class FRI_BA_Defect
    {
        /// <summary>
        /// 廠別
        /// </summary>
        [Key]
        [StringLength(5)]
        public string Factory_ID { get; set; }
        /// <summary>
        /// BA&#32570;&#22833;&#20195;&#30908;
        /// </summary>
        [Key]
        [StringLength(3)]
        public string BA_Defect_ID { get; set; }
        /// <summary>
        /// BA&#32570;&#22833;&#25551;&#36848;
        /// </summary>
        [Required]
        [StringLength(100)]
        public string BA_Defect_Desc { get; set; }
        /// <summary>
        /// &#30064;&#21205;&#32773;
        /// </summary>
        [Required]
        [StringLength(20)]
        public string Updated_By { get; set; }
        /// <summary>
        /// &#30064;&#21205;&#26178;&#38291;
        /// </summary>
        [Column(TypeName = "datetime")]
        public DateTime Update_Time { get; set; }
    }
}
