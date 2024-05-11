using System;
using System.Collections.Generic;

namespace PBLShop.Models;

public partial class GioiTinh
{
    public int MaGioiTinh { get; set; }

    public string TenGioiTinh { get; set; } = null!;

    public virtual ICollection<NguoiDung> NguoiDungs { get; set; } = new List<NguoiDung>();
}
