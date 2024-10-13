using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace API.Models;

public partial class ThongTin
{
    [Key]
    [Column("ID")]
    public int Id { get; set; }

    [StringLength(50)]
    public string Ten { get; set; }

    [StringLength(50)]
    public string Tuoi { get; set; }

    [Column("CLTT")]
    public int? Cltt { get; set; }

    [Column("ViTriID")]
    public int? ViTriId { get; set; }
}
