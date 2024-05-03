using System.ComponentModel.DataAnnotations;

namespace PBLShop.ViewModels
{
    public class DangNhapVM
    {
        [Display(Name = "Tên đăng nhập")]
        [Required(ErrorMessage = "Vui lòng nhập tên đăng nhập")]
        [MaxLength(20, ErrorMessage = "Tối đa 20 kí tự")]
        public string Email { get; set; }

        [Display(Name = "Mật khẩu")]
        [Required(ErrorMessage = "Vui lòng nhập mật khẩu")]
        [DataType(DataType.Password)]
        public string MatKhau { get; set; }
    }
}
