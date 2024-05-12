using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PBLShop.Models;
using PBLShop.ViewModels;

namespace PBLShop.Controllers
{
    public class NhanVienController : Controller
    {
        private readonly WebShopContext _context;

        public NhanVienController(WebShopContext context)
        {
            _context = context;
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public IActionResult Index()
        {
            var khachhangs = _context.NguoiDungs
                .Include(p => p.MaGioiTinhNavigation)
                .Where(p => p.MaVaiTro == 1 && p.TrangThai == true).ToList();
            List<NguoiDungVM> result = new List<NguoiDungVM>();
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
                    DiaChi = kh.DiaChi
                });
            }
            return View(khachhangs);
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public IActionResult Create(NhanVienVM model)
        {
            if (ModelState.IsValid)
            {
                var emailExists = _context.NguoiDungs
                    .Any(p => p.Email == p.Email);
                if (!emailExists)
                {
                    var gioiTinh = _context.GioiTinhs.FirstOrDefault(gt => gt.TenGioiTinh == model.GioiTinh);
                    NguoiDung nhanvien = new()
                    {
                        Email = model.Email,
                        MatKhau = model.MatKhau,
                        HoTen = model.HoTen,
                        MaGioiTinh = gioiTinh != null ? gioiTinh.MaGioiTinh : 1,
                        NgaySinh = model.NgaySinh,
                        DiaChi = model.DiaChi,
                        SoDienThoai = model.DienThoai,
                        MaVaiTro = model.MaVaiTro,
                        TrangThai = true,
                    };
                    _context.NguoiDungs.Add(nhanvien);
                    _context.SaveChanges();
                }
                else
                {
                    ModelState.AddModelError("loi", "Email đã tồn tại");
                    return View(model);
                }
            }
            return RedirectToAction("Index");
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public IActionResult Update()
        {
            return View();
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public IActionResult Update(int? id, NhanVienVM model)
        {
            if (id == null)
            {
                TempData["Message"] = "Không tìm thấy nhân viên";
                RedirectToAction("/404");
            }

            if (ModelState.IsValid)
            {
                var nhanVien = _context.NguoiDungs.FirstOrDefault(p => p.MaNguoiDung == id);
                if (nhanVien == null)
                {
                    TempData["Message"] = "Không tìm thấy nhân viên";
                    RedirectToAction("/404");
                }

                var emailExists = _context.NguoiDungs.Any(p => p.Email == model.Email && p.MaNguoiDung != id);
                if (!emailExists)
                {
                    var gioiTinh = _context.GioiTinhs.FirstOrDefault(gt => gt.TenGioiTinh == model.GioiTinh);
                    nhanVien.Email = model.Email;
                    nhanVien.MatKhau = model.MatKhau;
                    nhanVien.HoTen = model.HoTen;
                    nhanVien.MaGioiTinh = gioiTinh != null ? gioiTinh.MaGioiTinh : 1;
                    nhanVien.NgaySinh = model.NgaySinh;
                    nhanVien.DiaChi = model.DiaChi;
                    nhanVien.SoDienThoai = model.DienThoai;
                    nhanVien.MaVaiTro = model.MaVaiTro;

                    _context.SaveChanges();
                }
                else
                {
                    ModelState.AddModelError("loi", "Email đã tồn tại");
                    return View(model);
                }
            }
            return RedirectToAction("Index");
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
                .FirstOrDefault(p => p.MaNguoiDung == id && p.MaVaiTro == 1 && p.TrangThai == true);
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
