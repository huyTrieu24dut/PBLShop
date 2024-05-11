using System;
using System.Collections.Generic;

namespace PBLShop.Models;

public partial class DanhMuc
{
    public int MaDm { get; set; }

    public string TenDanhMuc { get; set; } = null!;

    public string? MoTa { get; set; }

    public int? MaDmcha { get; set; }

    public virtual ICollection<DanhMuc> InverseMaDmchaNavigation { get; set; } = new List<DanhMuc>();

    public virtual DanhMuc? MaDmchaNavigation { get; set; }

    public virtual ICollection<SanPham> SanPhams { get; set; } = new List<SanPham>();
}
