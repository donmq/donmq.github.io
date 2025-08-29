using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace API.Models;

[PrimaryKey("ID", "Factory", "Employee_ID", "upcode")]
public partial class IDX_HRMS_Emp_Personal_g01
{
    [Key]
    public int ID { get; set; }

    [Key]
    [StringLength(10)]
    [Unicode(false)]
    public string Factory { get; set; }

    [Key]
    [StringLength(16)]
    [Unicode(false)]
    public string Employee_ID { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? Update_Time { get; set; }

    [Key]
    [StringLength(1)]
    [Unicode(false)]
    public string upcode { get; set; }

    [StringLength(1)]
    [Unicode(false)]
    public string BIZ_FLAG { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? BIZ_TIME { get; set; }
}
