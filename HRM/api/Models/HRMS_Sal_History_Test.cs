using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace API.Models;

[Keyless]
public partial class HRMS_Sal_History_Test
{
    [StringLength(10)]
    [Unicode(false)]
    public string factory { get; set; }

    [Column(TypeName = "date")]
    public DateTime? Effective_Date { get; set; }

    public int? seq { get; set; }

    [StringLength(16)]
    [Unicode(false)]
    public string empno { get; set; }

    [StringLength(10)]
    [Unicode(false)]
    public string Salary_Item { get; set; }

    public int? Amount { get; set; }
}
