using GameStore.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

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

        // GET: Benh/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(System.Net.HttpStatusCode.BadRequest);
            }

            // Tìm bệnh dựa theo id
            Benh benh = db.Benhs.Find(id);
            if (benh == null)
            {
                return HttpNotFound();
            }

            return View(benh);  // Trả về view chi tiết của bệnh
        }
    }
}
