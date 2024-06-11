using System.ComponentModel.DataAnnotations;

namespace PBLShop.ViewModels
{
    public class NguoiDungVM
    {
        public int ID { get; set; }
        public string? HoTen {  get; set; }
        public string? Email { get; set; }
        public string? GioiTinh { get; set; }
        public DateTime? NgaySinh { get; set; }
        public string? SoDienThoai { get; set; }
        public string? DiaChi { get; set; }
        public string? vaiTro {  get; set; }
        public int MaVaiTro { get; set; }
        public bool trangThai { get; set; } = true;
    }
    public class ThongTinNguoiDung
    {
        public int ID { get; set; }
        [Display(Name = "Email")]
        [Required(ErrorMessage = "Vui lòng nhập email")]
        [EmailAddress(ErrorMessage = "Chưa đúng định dạng email")]
        public string Email { get; set; }

        [Display(Name = "Họ tên")]
        [Required(ErrorMessage = "Vui lòng nhập họ tên")]
        public string HoTen { get; set; }

        [Display(Name = "Giới tính")]
        public int MaGioiTinh { get; set; }

        [Display(Name = "Ngày sinh")]
        [DataType(DataType.Date)]
        public DateTime? NgaySinh { get; set; }

        [Display(Name = "Địa chỉ")]
        [Required(ErrorMessage = "Vui lòng nhập địa chỉ")]
        [MaxLength(60, ErrorMessage = "Tối đa 60 kí tự")]
        public string DiaChi { get; set; }

        [Display(Name = "Điện thoai")]
        [Required(ErrorMessage = "Vui lòng nhập số điện thoại")]
        [MaxLength(10, ErrorMessage = "Tối đa 10 kí tự")]
        public string DienThoai { get; set; }

        [Display(Name = "Vai trò")]
        public int MaVaiTro { get; set; }
    }
}
