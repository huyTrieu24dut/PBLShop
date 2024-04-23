using System;
using System.Collections.Generic;

namespace PBLShop.Models;

public partial class DanhMuc
{
    public string MaDm { get; set; } = null!;

    public string TenDm { get; set; } = null!;

    public string? MoTa { get; set; }

    public virtual ICollection<SanPham> SanPhams { get; set; } = new List<SanPham>();
}
