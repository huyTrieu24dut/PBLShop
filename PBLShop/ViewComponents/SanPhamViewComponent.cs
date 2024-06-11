using Microsoft.AspNetCore.Mvc;
using PBLShop.Models;
using PBLShop.ViewModels;

namespace PBLShop.ViewComponents
{
    public class SanPhamViewComponent : ViewComponent
    {
        private readonly WebShopContext _context;

        public SanPhamViewComponent(WebShopContext context)
        {
            _context = context;
        }
        public IViewComponentResult Invoke()
        {
            var sanPhams = _context.SanPhams.Where(p => p.TrangThai == true).ToList();
            int i = 1;
            var result = new List<SanPhamVM>();
            foreach (var sanPham in sanPhams)
            {
                if (i <= 4)
                {
                    result.Add(new SanPhamVM
                    {
                        MaSp = sanPham.MaSp,
                        TenSp = sanPham.TenSp,
                        DonGia = sanPham.DonGia,
                        HinhAnh = sanPham.AnhSp,
                    });
                }
                i++;
            }
            return View(result);
        }
    }
}
