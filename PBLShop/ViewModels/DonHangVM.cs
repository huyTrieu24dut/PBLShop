using PBLShop.Models;

namespace PBLShop.ViewModels
{
    public class DonHangVM
    {
        public DonHangVM()
        {
            chiTietDhVMs = new List<ChiTietDhVM>();
        }
        public int MaDh { get; set; }
        public int MaKh { get; set; }
        public string? TenKh { get; set; }
        public string? TenNguoiNhan { get; set; }
        public string? SoDienThoai { get; set; }
        public string? DiaChi { get; set; }
        public string? TrangThai { get; set; }
        public int TongTien { get; set; }
        public string? PhuongThuc { get; set; }
        public DateTime? NgayDat { get; set; }
        public List<ChiTietDhVM> chiTietDhVMs { get; set; }
    }

    public class lichSuDhVM
    {
        public int MaDh { get; set; }
        public string? TenKh { get; set; }
        public string? TenNv { get; set; }
        public List<TrangThaiThoiGian>? LichSu { get; set; }
    }

    public class TrangThaiThoiGian
    {
        public string? TrangThai { get; set; }
        public DateTime? ThoiGian { get; set; }
    }
}
