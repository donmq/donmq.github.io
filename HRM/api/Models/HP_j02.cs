using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace API.Models;

[PrimaryKey("Factory", "Type_Seq", "Code", "Status")]
public partial class HP_j02
{
    [Key]
    [StringLength(5)]
    [Unicode(false)]
    public string Type_Seq { get; set; }

    [Key]
    [StringLength(10)]
    [Unicode(false)]
    public string Code { get; set; }

    [StringLength(100)]
    [Unicode(false)]
    public string Code_Name { get; set; }

    [StringLength(50)]
    [Unicode(false)]
    public string HP { get; set; }

    [Key]
    [StringLength(10)]
    [Unicode(false)]
    public string Factory { get; set; }

    [Key]
    [StringLength(1)]
    [Unicode(false)]
    public string Status { get; set; }

    [StringLength(20)]
    [Unicode(false)]
    public string Update_By { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? Update_Time { get; set; }
}
