using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PBLShop.Models;
using PBLShop.ViewModels;

namespace PBLShop.Controllers
{
    public class KhachHangController : Controller
    {
        private readonly WebShopContext _context;

        public KhachHangController(WebShopContext context)
        {
            _context = context;
        }

        [HttpGet]
        [Authorize(Roles = "Admin, NhanVien")]
        public IActionResult Index()
        {
            var khachhangs = _context.NguoiDungs
                .Include(p => p.MaGioiTinhNavigation)
                .Where(p =>  p.MaVaiTro == 3 && p.TrangThai == true).ToList();
            List<NguoiDungVM> result = new List<NguoiDungVM>();
            foreach (var kh in  khachhangs)
            {
                result.Add(new NguoiDungVM
                {
                    ID = kh.MaNguoiDung,
                    HoTen = kh.HoTen,
                    Email = kh.Email,
                    GioiTinh = kh.MaGioiTinhNavigation.TenGioiTinh,
                    NgaySinh = kh.NgaySinh,
                    SoDienThoai = kh.SoDienThoai,
                    DiaChi = kh.DiaChi
                });
            }
            return View(khachhangs);
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public IActionResult Delete()
        {
            return View();
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public IActionResult Delete(int? id)
        {
            if (id == null)
            {
                RedirectToAction("/404");
            }
            var khachhang = _context.NguoiDungs
                .FirstOrDefault(p => p.MaNguoiDung == id && p.MaVaiTro == 3 && p.TrangThai == true);
            if (khachhang == null)
            {
                TempData["Message"] = "Không tìm thấy khách hàng";
                RedirectToAction("/404");
            }
            else
            {
                khachhang.TrangThai = false;
                //_context.NguoiDungs.Update(khachhang);
                _context.SaveChanges();
            }
            return Redirect("Index");
        }

    }
}
