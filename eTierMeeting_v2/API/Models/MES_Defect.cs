using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace eTierV2_API.Models
{
    /// <summary>
    /// QC&#32570;&#40670;&#20195;&#30908;&#27284;
    /// </summary>
    public partial class MES_Defect
    {
        /// <summary>
        /// 廠別
        /// </summary>
        [Key]
        [StringLength(1)]
        public string Factory_ID { get; set; }
        /// <summary>
        /// &#32570;&#22833;&#32232;&#34399;
        /// </summary>
        [Key]
        [StringLength(3)]
        public string Def_ID { get; set; }
        /// <summary>
        /// &#32570;&#22833;&#33521;&#25991;&#35498;&#26126;
        /// </summary>
        [StringLength(80)]
        public string Def_Desc { get; set; }
        /// <summary>
        /// &#32570;&#22833;&#20013;&#25991;&#35498;&#26126;
        /// </summary>
        [StringLength(80)]
        public string Def_DescZW { get; set; }
        /// <summary>
        /// &#32570;&#22833;&#36234;&#25991;&#35498;&#26126;
        /// </summary>
        [StringLength(80)]
        public string Def_DescVN { get; set; }
        /// <summary>
        /// &#32570;&#22833;&#31777;&#23531;&#33521;&#25991;
        /// </summary>
        [StringLength(80)]
        public string Def_Sname { get; set; }
        /// <summary>
        /// &#20998;&#20139;&#35338;&#24687;
        /// </summary>
        [StringLength(1024)]
        public string Share_Msg { get; set; }
        /// <summary>
        /// &#30064;&#21205;&#26178;&#38291;
        /// </summary>
        [Column(TypeName = "datetime")]
        public DateTime Update_Time { get; set; }
        /// <summary>
        /// &#30064;&#21205;&#32773;
        /// </summary>
        [Required]
        [StringLength(16)]
        public string Updated_By { get; set; }
        public int Sort { get; set; }
    }
}
