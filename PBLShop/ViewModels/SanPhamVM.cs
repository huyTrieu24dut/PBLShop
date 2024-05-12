namespace PBLShop.ViewModels
{
    public class SanPhamVM
    {
        public int MaSp { get; set; }
        public string TenSp { get; set; } = null!;
        public int DonGia { get; set; }
        public int SoLuong { get; set; }
        public string? HinhAnh { get; set; }
        public string? DanhMuc { get; set; }
    }

    public class ChiTietSanPhamVM
    {
        public int MaSp { get; set; }
        public string TenSp { get; set; } = null!;
        public int DonGia { get; set; }
        public int SoLuong { get; set; }
        public string? HinhAnh { get; set; }
        public string? MoTa { get; set; }
        public decimal? SoSao { get; set; }
        public List<string> MauSac { get; set; }
        public List<string> KichThuoc { get; set; }
        public string DanhMuc { get; set; } = null!;
    }

    public class SanPhamMoi
    {
        public string TenSp { get; set; } = null!;
        public int DonGia { get; set; }
        public string? HinhAnh { get; set; }
        public string? MoTa { get; set; }
        public List<string> MauSac { get; set; }
        public List<string> KichThuoc { get; set; }
        public int MaDm { get; set; }
    }
}
