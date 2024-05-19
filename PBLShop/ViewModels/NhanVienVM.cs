﻿using System.ComponentModel.DataAnnotations;

namespace PBLShop.ViewModels
{
    public class NhanVienVM :DangKyVM
    {
        [Display(Name = "Vai trò")]
        public int MaVaiTro { get; set; }
    }

    public class ThongTinNVVM
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
