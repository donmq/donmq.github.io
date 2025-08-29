using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace API.Models;

public partial class HP_CharacterMap
{
    [Key]
    [StringLength(1)]
    public string SignChar { get; set; }

    [Required]
    [StringLength(1)]
    [Unicode(false)]
    public string UnsignChar { get; set; }
}
