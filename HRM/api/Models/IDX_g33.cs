using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace API.Models;

[Keyless]
public partial class IDX_g33
{
    [StringLength(10)]
    [Unicode(false)]
    public string Factory { get; set; }

    [StringLength(16)]
    [Unicode(false)]
    public string Employee_ID { get; set; }

    [Column(TypeName = "date")]
    public DateTime? Leave_Start { get; set; }

    [StringLength(10)]
    [Unicode(false)]
    public string Leave_code { get; set; }

    [StringLength(4)]
    [Unicode(false)]
    public string Min_Start { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? Update_Time { get; set; }

    [StringLength(1)]
    [Unicode(false)]
    public string upcode { get; set; }

    [StringLength(1)]
    [Unicode(false)]
    public string BIZ_FLAG { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? BIZ_TIME { get; set; }
}
