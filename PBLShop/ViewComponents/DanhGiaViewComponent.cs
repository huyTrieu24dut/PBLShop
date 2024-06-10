using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PBLShop.Models;

namespace PBLShop.ViewComponents
{
    public class DanhGiaViewComponent : ViewComponent
    {
        private readonly WebShopContext _context;

        public DanhGiaViewComponent(WebShopContext context)
        {
            _context = context;
        }
        public IViewComponentResult Invoke()
        {
            var danhGias = _context.DanhGia
                .Include(p => p.MaNguoiDungNavigation)
                .Include(p => p.MaSpNavigation)
                .ToList();
            return View();
        }
    }
}
