using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PBLShop.Models;
using System.Security.Claims;

namespace PBLShop.Controllers
{
    public class DanhGiaController : Controller
    {
        private readonly WebShopContext _context;

        public DanhGiaController(WebShopContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }

        [Authorize(Roles = "KhachHang")]
        public IActionResult Create(int id,decimal soSao, string noiDung)
        {
            if(ModelState.IsValid)
            {
                var danhGia = new DanhGia
                {
                    MaNguoiDung = Convert.ToInt32(HttpContext.User.FindFirstValue("MaNguoiDung")),
                    MaSp = id,
                    SoSao = soSao,
                    NoiDung = noiDung
                };
                _context.DanhGia.Add(danhGia);
                _context.SaveChanges();
            }
            return RedirectToAction("Detail", "SanPham", new { id = id });
        }
    }
}
