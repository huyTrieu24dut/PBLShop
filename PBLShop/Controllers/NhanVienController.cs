using Microsoft.AspNetCore.Mvc;
using PBLShop.Models;

namespace PBLShop.Controllers
{
    public class NhanVienController : Controller
    {
        private readonly WebShopContext _context;

        public NhanVienController(WebShopContext context)
        {
            _context = context;
        }

        public IActionResult CustomerList()
        {
            var khachhangs = _context.NguoiDungs.Where(p => p.MaVaiTro == 3).ToList();
            return View(khachhangs);
        }
    }
}
