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
        public IActionResult Index(bool? trangThai)
        {
            var nhanviens = _context.NguoiDungs
                .Include(p => p.MaGioiTinhNavigation)
                .Include(p => p.MaVaiTroNavigation)
                .Where(p => p.MaVaiTro == 1).ToList();
            List<NguoiDungVM> result = new List<NguoiDungVM>();
            if (trangThai != null)
            {
                nhanviens = nhanviens.Where(p => p.TrangThai == trangThai).ToList();
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
                nhanviens = nhanviens.Where(p => p.TrangThai == true).ToList();
            }
            foreach (var nv in nhanviens)
            {
                result.Add(new NguoiDungVM
                {
                    ID = nv.MaNguoiDung,
                    HoTen = nv.HoTen,
                    Email = nv.Email,
                    GioiTinh = nv.MaGioiTinhNavigation.TenGioiTinh,
                    NgaySinh = nv.NgaySinh,
                    SoDienThoai = nv.SoDienThoai,
                    DiaChi = nv.DiaChi,
                    trangThai = nv.TrangThai,
                    vaiTro = nv.MaVaiTroNavigation.TenVaiTro 
                });
            }
            return View(result);
        }

        [Authorize(Roles = "Admin")]
        public IActionResult Search(string name)
        {
            var nhanviens = _context.NguoiDungs
                .Include(p => p.MaGioiTinhNavigation)
                .Include(p => p.MaVaiTroNavigation)
                .Where(p => p.MaVaiTro == 1 && p.HoTen.Contains(name)).ToList();
            List<NguoiDungVM> result = new List<NguoiDungVM>();
            foreach (var nv in nhanviens)
            {
                result.Add(new NguoiDungVM
                {
                    ID = nv.MaNguoiDung,
                    HoTen = nv.HoTen,
                    Email = nv.Email,
                    GioiTinh = nv.MaGioiTinhNavigation.TenGioiTinh,
                    NgaySinh = nv.NgaySinh,
                    SoDienThoai = nv.SoDienThoai,
                    DiaChi = nv.DiaChi,
                    trangThai = nv.TrangThai,
                    vaiTro = nv.MaVaiTroNavigation.TenVaiTro
                });
            }
            return View(result);
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
                    .Any(p => p.Email == model.Email);
                if (!emailExists)
                {
                    NguoiDung nhanvien = new()
                    {
                        Email = model.Email,
                        MatKhau = model.MatKhau,
                        HoTen = model.HoTen,
                        MaGioiTinh = model.MaGioiTinh,
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
        public IActionResult Update(int id)
        {
            var nhanVien = _context.NguoiDungs.FirstOrDefault(p => p.MaNguoiDung == id);
            if (nhanVien != null)
            {
                var nv = new ThongTinNVVM
                {
                    ID = nhanVien.MaNguoiDung,
                    Email = nhanVien.Email,
                    HoTen = nhanVien.HoTen,
                    MaGioiTinh = (int)nhanVien.MaGioiTinh,
                    NgaySinh = nhanVien.NgaySinh,
                    DiaChi = nhanVien.DiaChi,
                    DienThoai = nhanVien.SoDienThoai,
                    MaVaiTro = nhanVien.MaVaiTro,
                };
                return View(nv);
            }
            else
            {
                TempData["Message"] = "Không tìm thấy nhân viên";
                RedirectToAction("/404");
            }
            return View();
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public IActionResult Update(int id, ThongTinNVVM model)
        {
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
                    nhanVien.Email = model.Email;
                    nhanVien.HoTen = model.HoTen;
                    nhanVien.MaGioiTinh = model.MaGioiTinh;
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

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public IActionResult UnBlock(int? id)
        {
            if (id == null)
            {
                RedirectToAction("/404");
            }
            var nhanvien = _context.NguoiDungs
                .FirstOrDefault(p => p.MaNguoiDung == id && p.MaVaiTro != 3 && p.TrangThai == false);
            if (nhanvien == null)
            {
                TempData["Message"] = "Không tìm thấy nhân viên";
                RedirectToAction("/404");
            }
            else
            {
                nhanvien.TrangThai = true;
                _context.SaveChanges();
            }
            return RedirectToAction("Index", "NhanVien");
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public IActionResult Block(int? id)
        {
            if (id == null)
            {
                RedirectToAction("/404");
            }
            var nhanvien = _context.NguoiDungs
                .FirstOrDefault(p => p.MaNguoiDung == id && p.MaVaiTro != 3 && p.TrangThai == true);
            if (nhanvien == null)
            {
                TempData["Message"] = "Không tìm thấy nhân viên";
                RedirectToAction("/404");
            }
            else
            {
                nhanvien.TrangThai = false;
                //_context.NguoiDungs.Update(khachhang);
                _context.SaveChanges();
            }
            return RedirectToAction("Index", "NhanVien");
        }

    }
}
