namespace PBLShop.ViewModels
{
    public class ChiTietGhVM
    {
        public string MaKh { get; set; } = null!;
        public string MaSp { get; set; } = null!;
        public string TenSp { get; set; } = null!;
        public int DonGia { get; set; }
        public int? SoLuong { get; set; }
        public string? HinhAnh { get; set; }
        public string DanhMuc { get; set; } = null!;
    }
}
