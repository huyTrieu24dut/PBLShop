using PBLShop.Models;
using System.ComponentModel.DataAnnotations;

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
        public string? TenNv { get; set; }

        [Display(Name = "Tên người nhận")]
        [Required(ErrorMessage = "Vui lòng nhập tên người nhận")]
        public string? TenNguoiNhan { get; set; }

        [Display(Name = "Điện thoai")]
        [Required(ErrorMessage = "Vui lòng nhập số điện thoại")]
        [MaxLength(10, ErrorMessage = "Tối đa 10 kí tự")]
        public string? SoDienThoai { get; set; }

        public string? SdtNguoiDung { get; set; }

        public string? Email { get; set; }

        public string? DiaChi { get; set; }
        public string? TrangThai { get; set; }
        public int TongTien { get; set; }
        public string? PhuongThuc { get; set; }
        
        [Display(Name = "Ngày đặt")]
        [DataType(DataType.Date)]
        public DateTime? NgayDat { get; set; }
        public DateTime? NgayHoanThanh { get; set; }
        public string? FileHoaDon { get; set; }
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
