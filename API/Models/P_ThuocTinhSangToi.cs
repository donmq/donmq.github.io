﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace API.Models;

[Keyless]
public partial class P_ThuocTinhSangToi
{
    public int IDViTri { get; set; }

    public int IDThuocTinhChinh { get; set; }

    public bool LoaiThuocTinh { get; set; }
}