using Microsoft.AspNetCore.Mvc;
using PBLShop.Models;
using PBLShop.ViewModels;

namespace PBLShop.ViewComponents
{
    public class DanhMucViewComponent : ViewComponent
    {
        private readonly WebShopContext _context;

        public DanhMucViewComponent(WebShopContext context)
        {
            _context = context;
        }

        public IViewComponentResult Invoke()
        {
            var data = _context.DanhMucs
                .Where(dm => dm.MaDmcha == null)
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


    }
}
