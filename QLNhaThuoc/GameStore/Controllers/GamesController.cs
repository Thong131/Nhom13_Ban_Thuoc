using GameStore.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace GameStore.Controllers
{
    public class GamesController : Controller
    {
        QLThuocSo1VNNGAY1010Entities db = new QLThuocSo1VNNGAY1010Entities();
        // GET: Games
        public ActionResult Index()
        {
            var listThuoc = from sp in db.SanPhams
                           select sp;
            var query = listThuoc.Include("HinhAnhs");
            return View(listThuoc.ToList());
        }

        public ActionResult DanhMuc(int? id)
        {   
            /* nếu không có mã danh mục sẽ báo lỗi*/
            if (id == null)
                return new HttpStatusCodeResult(System.Net.HttpStatusCode.BadRequest);

            /* tìm kiếm sản phẩm theo mã danh mục*/
            var listThuoc = from sp in db.SanPhams
                               where sp.maDM == id
                               select sp;

            /* lấy hình ảnh ứng với sản phẩm*/
            var query = listThuoc.Include("DanhMuc").Include("HinhAnhs");

            /* Nếu không có sản phẩm nào sẽ báo lỗi*/
            if (query!=null)
                return View(listThuoc.ToList());

            return HttpNotFound();

         
            
           
        }

        public ActionResult ChiTietSP(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(System.Net.HttpStatusCode.BadRequest);

            var thuoc = db.SanPhams.Include("DanhMuc").Include("HinhAnhs").FirstOrDefault(sp => sp.maSP == id);
            if (thuoc == null)
                return HttpNotFound();

            var binhLuans = db.BinhLuans.Include("NguoiDung").Where(bl => bl.maSP == id).ToList();
            var danhGias = db.DanhGias.Where(dg => dg.MaSanPham == id).ToList();
            double avgRating = 0;
            if (danhGias.Count > 0)
            {
                avgRating = danhGias.Average(dg => Convert.ToDouble(dg.NoiDung)); // Assuming NoiDung stores the rating as a string
            }
            ViewBag.AvgRating = avgRating; // Pass the average rating to the view

            ViewBag.BinhLuans = binhLuans; // Gửi danh sách bình luận sang View
            return View(thuoc);
        }

        [HttpPost]
        public ActionResult AddComment(int maSP, string NoiDung)
        {
            if (Session["userLogin"] == null)
            {
                return RedirectToAction("Login", "User");
            }

            string username = Session["userLogin"].ToString();
            var nguoiDung = db.NguoiDungs.FirstOrDefault(nd => nd.username == username);

            if (nguoiDung == null)
            {
                return RedirectToAction("Index", "LoginUser");
            }

            BinhLuan binhLuan = new BinhLuan
            {
                maSP = maSP,
                MaNguoiDung = nguoiDung.maNguoiDung,
                NoiDung = NoiDung,
                NgayBinhLuan = DateTime.Now
            };

            db.BinhLuans.Add(binhLuan);
            db.SaveChanges();

            return RedirectToAction("ChiTietSP", new { id = maSP });
        }


        public ActionResult Search(string keyword)
        {
            var listGame = from sp in db.SanPhams
                           select sp;
            var query = listGame.Include("HinhAnhs");

            if (!String.IsNullOrEmpty(keyword))
            {
                keyword = keyword.ToLower();
                query = query.Where(b => b.tenSP.ToLower().Contains(keyword));
            }

            return View(query.ToList());
        }
        [HttpPost]
        public ActionResult AddRating(int maSP, int rating)
        {
            if (Session["userLogin"] == null)
            {
                return RedirectToAction("Login", "User");
            }

            string username = Session["userLogin"].ToString();
            var nguoiDung = db.NguoiDungs.FirstOrDefault(nd => nd.username == username);

            if (nguoiDung == null)
            {
                return RedirectToAction("Index", "LoginUser");
            }

            // Kiểm tra xem người dùng đã mua sản phẩm chưa
            var daMuaHang = db.ChiTietDonHangs.Any(ct => ct.maSP == maSP );

            // Kiểm tra xem người dùng đã đánh giá chưa
            var daDanhGia = db.DanhGias.Any(dg => dg.MaNguoiDung == nguoiDung.maNguoiDung && dg.MaSanPham == maSP);

            if (!daMuaHang)
            {
                TempData["ErrorMessage"] = "Bạn chưa mua hàng, không thể đánh giá.";
                return RedirectToAction("ChiTietSP", new { id = maSP });
            }

            if (daDanhGia)
            {
                TempData["ErrorMessage"] = "Bạn đã đánh giá sản phẩm này rồi.";
                return RedirectToAction("ChiTietSP", new { id = maSP });
            }

            DanhGia danhGia = new DanhGia
            {
                MaSanPham = maSP,
                MaNguoiDung = nguoiDung.maNguoiDung,
                NoiDung = rating.ToString(),
                NgayBinhLuan = DateTime.Now
            };

            db.DanhGias.Add(danhGia);
            db.SaveChanges();

            return RedirectToAction("ChiTietSP", new { id = maSP });
        }

    }
}