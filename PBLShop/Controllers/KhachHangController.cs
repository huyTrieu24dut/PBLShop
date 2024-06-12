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
        [Authorize(Roles = "Admin")]
        public IActionResult Index(bool? trangThai)
        {
            var khachhangs = _context.NguoiDungs
                .Include(p => p.MaGioiTinhNavigation)
                .Where(p =>  p.MaVaiTro == 3).ToList();
            List<NguoiDungVM> result = new();
            if (trangThai != null)
            {
                khachhangs = khachhangs.Where(p => p.TrangThai == trangThai).ToList();
                if (trangThai == true)
                {
                    TempData["status"] = "true";
                }
                else
                {
                    TempData["status"] = "false";
                }
            }
            else
            {
                khachhangs = khachhangs.Where(p => p.TrangThai == true).ToList();
            }
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
                    DiaChi = kh.DiaChi,
                    trangThai = kh.TrangThai, 
                });
            }
            return View(result);
        }

        [Authorize(Roles = "Admin")]
        public IActionResult Search(string name)
        {
            var khachhangs = _context.NguoiDungs
                .Include(p => p.MaGioiTinhNavigation)
                .Where(p => p.MaVaiTro == 3 && p.HoTen.Contains(name)).ToList();
            List<NguoiDungVM> result = new();
            foreach (var kh in khachhangs)
            {
                result.Add(new NguoiDungVM
                {
                    ID = kh.MaNguoiDung,
                    HoTen = kh.HoTen,
                    Email = kh.Email,
                    GioiTinh = kh.MaGioiTinhNavigation.TenGioiTinh,
                    NgaySinh = kh.NgaySinh,
                    SoDienThoai = kh.SoDienThoai,
                    DiaChi = kh.DiaChi,
                    trangThai = kh.TrangThai,
                });
            }
            return View(result);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public IActionResult UnBlock(int? id)
        {
            if (id == null)
            {
                RedirectToAction("/404");
            }
            var khachhang = _context.NguoiDungs
                .FirstOrDefault(p => p.MaNguoiDung == id && p.MaVaiTro == 3 && p.TrangThai == false);
            if (khachhang == null)
            {
                TempData["Message"] = "Không tìm thấy khách hàng";
                RedirectToAction("/404");
            }
            else
            {
                khachhang.TrangThai = true;
                _context.SaveChanges();
            }
            return RedirectToAction("Index", "KhachHang");
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public IActionResult Block(int? id)
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
                _context.SaveChanges();
            }
            return RedirectToAction("Index", "KhachHang");
        }
    }
}
