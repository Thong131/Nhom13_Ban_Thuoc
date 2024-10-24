using GameStore.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity; // Nếu bạn đang sử dụng Entity Framework
// Hoặc

namespace GameStore.Controllers
{
    public class BenhController : Controller
    {
        QLThuocSo1VNNGAY1010Entities db = new QLThuocSo1VNNGAY1010Entities();

        // GET: Benh
        public ActionResult Index()
        {
            // Lấy danh sách các bệnh từ database và chuyển vào view
            var listBenh = from benh in db.Benhs select benh;
            return View(listBenh.ToList());
        }
        public ActionResult TiemChung()
        {
            
            return View();
        }
        public ActionResult Benh1()
        {
            var benhs = db.Benhs.ToList();
            return View(benhs);
        }
        // GET: Benh/Details/5
        public ActionResult Details(string tenBenh)
        {
            if (string.IsNullOrEmpty(tenBenh))
            {
                return new HttpStatusCodeResult(System.Net.HttpStatusCode.BadRequest);
            }

            // Tìm bệnh dựa theo tên bệnh
            Benh benh = db.Benhs
                .Include(b => b.SanPhams)
                .FirstOrDefault(b => b.tenBenh.Equals(tenBenh, StringComparison.OrdinalIgnoreCase)); // So sánh không phân biệt chữ hoa chữ thường

            if (benh == null)
            {
                return HttpNotFound();
            }

            // Lấy danh sách sản phẩm khác cùng loại bệnh
            var danhSachSanPhamKhac = db.SanPhams
                .Where(sp => sp.maBenh == benh.maBenh && sp.maSP != null)
                .ToList();

            ViewBag.DanhSachSanPhamKhac = danhSachSanPhamKhac;

            return View(benh);
        }



    }
}
