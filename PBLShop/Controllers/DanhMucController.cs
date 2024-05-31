using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PBLShop.Models;
using PBLShop.ViewModels;

namespace PBLShop.Controllers
{
    public class DanhMucController : Controller
    {
        private readonly WebShopContext _context;

        public DanhMucController(WebShopContext context)
        {
            _context = context;
        }
        [Authorize(Roles = "Admin, NhanVien")]
        public IActionResult Index()
        {
            var data = _context.DanhMucs
                .Include(p => p.MaDmchaNavigation)
                .Include(p => p.InverseMaDmchaNavigation)
                .Where(p => p .TrangThai == true)
                .Select(dm => new DanhMucVM
                {
                    MaDM = dm.MaDm,
                    TenDM = dm.TenDanhMuc,
                    SoLuong = dm.SanPhams.Where(p => p.TrangThai == true && p.MaDm == dm.MaDm).ToList().Count,
                    TrangThai = dm.TrangThai,
                    TenDMCha = dm.MaDmchaNavigation.TenDanhMuc ?? "",
                    MaDMCha = dm.MaDmcha ?? null,
                    SoDmCon = dm.InverseMaDmchaNavigation.Count,
                })
                .ToList();
            if (data.Count > 0)
            {
                foreach (var danhMuc in data)
                {
                    if (danhMuc.MaDMCha == null && danhMuc.SoDmCon == 0)
                    {
                        danhMuc.CoTheXoa = true;
                    }
                    else if (danhMuc.MaDMCha != null && danhMuc.SoLuong == 0)
                    {
                        danhMuc.CoTheXoa = true;
                    }
                }
            }

            return View(data);
        }

        //[HttpGet]
        //[Authorize(Roles = "Admin, NhanVien")]
        //public IActionResult Create()
        //{
        //    return View();
        //}

        [HttpPost]
        [Authorize(Roles = "Admin, NhanVien")]
        public IActionResult Create(string TenDM, int? MaDMCha)
        {
            if (ModelState.IsValid)
            {
                DanhMuc dm = new DanhMuc();
                dm.TenDanhMuc = TenDM;
                if (MaDMCha != null)
                {
                    dm.MaDmcha = MaDMCha;
                    dm.TrangThai = true;
                }
                _context.Add(dm);
                _context.SaveChanges();
            }
            else
            {
                ModelState.AddModelError("loi", "Thêm không thành công");
                return View();
            }
            return RedirectToAction("Index", "DanhMuc");
        }

        //[HttpGet]
        //[Authorize(Roles = "Admin, NhanVien")]
        //public IActionResult Create(int id)
        //{
        //    var danhmuccha = _context.DanhMucs.FirstOrDefault(p => p.MaDm == id);
        //    return View(danhmuccha);
        //}

        [HttpPost]
        public IActionResult Update(int id, string? TenDM, int? MaDMCha)
        {
            if (ModelState.IsValid)
            {
                var dm = _context.DanhMucs.FirstOrDefault(p => p.MaDm == id);
                if (dm != null)
                {
                    if (TenDM != null)
                    {
                        dm.TenDanhMuc = TenDM;
                    }
                    if (MaDMCha != null)
                    {
                        dm.MaDmcha = MaDMCha;
                    }
                    _context.SaveChanges();
                }
                else
                {
                    TempData["Message"] = "Không tìm thấy danh mục";
                    return Redirect("/404");
                }
            }
            else
            {
                ModelState.AddModelError("loi", "Thêm không thành công");
                return RedirectToAction("Index", "DanhMuc");
            }
            return RedirectToAction("Index", "DanhMuc");
        }

        [Authorize(Roles = "Admin, NhanVien")]
        public IActionResult Delete(int id)
        {
            var danhMuc = _context.DanhMucs.FirstOrDefault(p => p.MaDm == id);
            if (danhMuc == null)
            {
                Redirect("/404");
            }
            else
            {
                danhMuc.TrangThai = false;
                _context.SaveChanges();
            }
            return RedirectToAction("Index");
        }
    }
}
