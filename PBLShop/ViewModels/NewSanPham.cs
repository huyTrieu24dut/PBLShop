using System.ComponentModel.DataAnnotations;

namespace PBLShop.ViewModels
{
    public class NewSanPham
    {
        public NewSanPham()
        {
            danhMucs = new List<DanhMucList>();
        }
        [Display(Name = "Tên sản phẩm")]
        [Required(ErrorMessage = "Vui lòng nhập tên sản phẩm")]
        public string? TenSp { get; set; }

        [Display(Name = "Đơn giá")]
        [Required(ErrorMessage = "Vui lòng nhập đơn giá")]
        [RegularExpression(@"^\d+$", ErrorMessage = "Đơn giá phải là một số")]
        public int DonGia { get; set; }
        public string? MoTa { get; set; }

        [Display(Name = "Danh Mục")]
        [Required(ErrorMessage = "Vui lòng nhập danh mục")]
        [RegularExpression(@"^\d+$", ErrorMessage = "Danh mục phải là một số")]
        public int MaDm { get; set; }
        public string? AnhSp { get; set; }
        public string? MauSac { get; set; }
        public string? Size { get; set; }
        public List<DanhMucList> danhMucs { get; set; }
    }
}
