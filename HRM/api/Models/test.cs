using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace API.Models;

[Keyless]
public partial class test
{
    public int Seq { get; set; }

    [Required]
    [StringLength(10)]
    [Unicode(false)]
    public string Code { get; set; }
}
