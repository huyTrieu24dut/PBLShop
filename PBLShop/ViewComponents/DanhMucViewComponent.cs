using Microsoft.AspNetCore.Mvc;
using PBLShop.Models;
using PBLShop.ViewModels;

namespace PBLShop.ViewComponents
{
    public class DanhMucViewComponent : ViewComponent
    {
        private readonly PblshopContext _context;

        public DanhMucViewComponent(PblshopContext context)
        {
            _context = context;
        }

        public IViewComponentResult Invoke()
        {
            var data = _context.DanhMucs.Select(dm => new DanhMucVM
            {
               MaDM = dm.MaDm,
               TenDM = dm.TenDm,
               SoLuong = dm.SanPhams.Count
            });
            return View(data);
        }
    }
}
