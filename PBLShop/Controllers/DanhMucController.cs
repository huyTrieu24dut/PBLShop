using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
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
                .Select(dm => new DanhMucVM
                {
                    MaDM = dm.MaDm,
                    TenDM = dm.TenDanhMuc,
                    SoLuong = dm.SanPhams.Count,
                    DanhMucCon = dm.InverseMaDmchaNavigation.Select(con => new DanhMucVM
                    {
                        MaDM = con.MaDm,
                        TenDM = con.TenDanhMuc,
                        SoLuong = con.SanPhams.Count,
                    }).ToList()
                })
                .ToList();

            return View(data);
        }

        [HttpGet]
        [Authorize(Roles = "Admin, NhanVien")]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [Authorize(Roles = "Admin, NhanVien")]
        public IActionResult Create(string name)
        {
            if (ModelState.IsValid)
            {
                DanhMuc dm = new DanhMuc();
                dm.TenDanhMuc = name;
                _context.Add(dm);
                _context.SaveChanges();
            }
            else
            {
                ModelState.AddModelError("loi", "Thêm không thành công");
                return View();
            }
            return RedirectToAction("Index");
        }

        [HttpGet]
        [Authorize(Roles = "Admin, NhanVien")]
        public IActionResult Create(int id)
        {
            var danhmuccha = _context.DanhMucs.FirstOrDefault(p => p.MaDm == id);
            return View(danhmuccha);
        }

        [HttpPost]
        [Authorize(Roles = "Admin, NhanVien")]
        public IActionResult Create(string name, int id)
        {
            if (ModelState.IsValid)
            {
                DanhMuc dm = new DanhMuc();
                dm.TenDanhMuc = name;
                dm.MaDmcha = id;
                _context.Add(dm);
                _context.SaveChanges();
            }
            else
            {
                ModelState.AddModelError("loi", "Thêm không thành công");
                return View();
            }
            return RedirectToAction("Index");
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
                _context.Remove(danhMuc);
                _context.SaveChanges();
            }
            return RedirectToAction("Index");
        }
    }
}
