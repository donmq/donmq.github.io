using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace eTierV2_API.Models
{
    public partial class MES_IPQC_Defect
    {
        /// <summary>
        /// 異常代碼
        /// </summary>
        [Key]
        public int Def_Id { get; set; }
        /// <summary>
        /// &#24288;&#21029;
        /// </summary>
        [Key]
        [StringLength(5)]
        public string Factory_ID { get; set; }
        /// <summary>
        /// &#21697;&#29260;&#20195;&#30908;
        /// </summary>
        [Key]
        [StringLength(5)]
        public string Brand_Code { get; set; }
        /// <summary>
        /// &#19981;&#33391;&#21407;&#22240;&#22823;&#39006;
        /// </summary>
        [Key]
        public int Parent_ID { get; set; }
        /// <summary>
        /// &#19981;&#33391;&#21407;&#22240;&#32048;&#38917;
        /// </summary>
        [Key]
        [StringLength(3)]
        public string Def_Code { get; set; }
        /// <summary>
        /// &#19981;&#33391;&#20195;&#30908;&#33521;&#25991;&#35498;&#26126;
        /// </summary>
        [Required]
        [StringLength(255)]
        public string Def_Name { get; set; }
        /// <summary>
        /// &#19981;&#33391;&#20195;&#30908;&#20013;&#25991;&#35498;&#26126;
        /// </summary>
        [Required]
        [StringLength(100)]
        public string Def_Name_ZW { get; set; }
        /// <summary>
        /// &#19981;&#33391;&#20195;&#30908;&#36234;&#25991;&#35498;&#26126;
        /// </summary>
        [Required]
        [StringLength(100)]
        public string Def_Name_Location { get; set; }
        /// <summary>
        /// &#26159;&#21542;&#26377;&#25928;(1/0)
        /// </summary>
        public int IsEnable { get; set; }
        /// <summary>
        /// &#26368;&#24460;&#26356;&#26032;&#26178;&#38291;
        /// </summary>
        [Column(TypeName = "datetime")]
        public DateTime? Update_Time { get; set; }
        /// <summary>
        /// &#26368;&#24460;&#26356;&#26032;&#32773;
        /// </summary>
        [StringLength(16)]
        public string Updated_By { get; set; }
    }
}
