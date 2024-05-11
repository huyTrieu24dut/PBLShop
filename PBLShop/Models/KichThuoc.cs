using System;
using System.Collections.Generic;

namespace PBLShop.Models;

public partial class KichThuoc
{
    public int MaKt { get; set; }

    public string Size { get; set; } = null!;

    public virtual ICollection<ChiTietDh> ChiTietDhs { get; set; } = new List<ChiTietDh>();

    public virtual ICollection<ChiTietGh> ChiTietGhs { get; set; } = new List<ChiTietGh>();

    public virtual ICollection<QuanLySanPham> QuanLySanPhams { get; set; } = new List<QuanLySanPham>();
}
