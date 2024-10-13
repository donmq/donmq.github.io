using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace API.Models;

public partial class ViTri
{
    [Key]
    [Column("ID")]
    public int Id { get; set; }

    [StringLength(50)]
    public string TenViTri { get; set; }
}
