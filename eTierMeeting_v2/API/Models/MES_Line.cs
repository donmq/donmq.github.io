using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace eTierV2_API.Models
{
    /// <summary>
    /// 生產線代碼檔
    /// </summary>
    public partial class MES_Line
    {

        /// <summary>
        /// 廠別
        /// </summary>
        [Key]
        [StringLength(1)]
        public string Factory_ID { get; set; }

        /// <summary>
        /// &#29983;&#29986;&#32218;&#21029;
        /// </summary>
        [Key]
        [StringLength(3)]
        public string Line_ID { get; set; }

        /// <summary>
        /// &#32218;&#21029;&#21517;&#31281;&#33521;&#25991;
        /// </summary>
        [StringLength(40)]
        public string Line_Desc { get; set; }

        /// <summary>
        /// &#32218;&#21029;&#21517;&#31281;&#20013;&#25991;
        /// </summary>
        [StringLength(40)]
        public string Line_DescZW { get; set; }

        /// <summary>
        /// &#32218;&#21029;&#21517;&#31281;&#36234;&#25991;
        /// </summary>
        [StringLength(40)]
        public string Line_DescVN { get; set; }

        /// <summary>
        /// &#32218;&#21029;&#31777;&#31281;&#33521;&#25991;
        /// </summary>
        [StringLength(10)]
        public string Line_Sname { get; set; }
        [StringLength(1)]
        public string Line_Model { get; set; }

        /// <summary>
        /// &#30064;&#21205;&#26178;&#38291;
        /// </summary>
        [Column(TypeName = "datetime")]
        public DateTime? Update_Time { get; set; }

        /// <summary>
        /// &#30064;&#21205;&#32773;
        /// </summary>
        [StringLength(16)]
        public string Updated_By { get; set; }
        [StringLength(1)]
        public string Line_AB { get; set; }
    }
}
