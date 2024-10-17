using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using GameStore.Models;

namespace GameStore.Areas.Admin.Controllers
{
    public class SanPhamController : Controller
    {
        private QLThuocSo1VNNGAY1010Entities db = new QLThuocSo1VNNGAY1010Entities();

        // GET: Admin/SanPham
        public ActionResult Index()
        {
            var sanPham = db.SanPhams.Include(s => s.DanhMuc);
            return View(sanPham.ToList());
        }

        // GET: Admin/SanPham/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SanPham sanPham = db.SanPhams.Find(id);
            if (sanPham == null)
            {
                return HttpNotFound();
            }
            return View(sanPham);
        }

        public ActionResult Create()
        {
            // Cung cấp danh sách cho dropdown
            ViewBag.maDM = new SelectList(db.DanhMucs, "maDM", "tenDM");
            ViewBag.maBenh = new SelectList(db.Benhs, "maBenh", "tenBenh");
            ViewBag.MaNhaCungCap = new SelectList(db.NhaCungCaps, "MaNhaCungCap", "TenNhaCungCap");
            ViewBag.MaGiamGia = new SelectList(db.GiamGias, "MaGiamGia", "GiaTri");
            return View();
        }

        // POST: Admin/SanPham/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "maSP,tenSP,giaTien,chitietSP,maDM,soLuong,maBenh,MaNhaCungCap,MaGiamGia,thanhPhan,donVi,hansuDung")] SanPham sanPham,
            HttpPostedFileBase hinh1, HttpPostedFileBase hinh2, HttpPostedFileBase hinh3, HttpPostedFileBase hinh4)
        {
            if (ModelState.IsValid)
            {
                var idImage = Guid.NewGuid().ToString();
                var idImage2 = Guid.NewGuid().ToString();
                var idImage3 = Guid.NewGuid().ToString();
                var idImage4 = Guid.NewGuid().ToString();

                string _FileName = idImage + Path.GetExtension(hinh1.FileName);
                string _FileName2 = idImage2 + Path.GetExtension(hinh2.FileName);
                string _FileName3 = idImage3 + Path.GetExtension(hinh3.FileName);
                string _FileName4 = idImage4 + Path.GetExtension(hinh4.FileName);

                string path1 = Path.Combine(Server.MapPath("~/UploadFile"), _FileName);
                string path2 = Path.Combine(Server.MapPath("~/UploadFile"), _FileName2);
                string path3 = Path.Combine(Server.MapPath("~/UploadFile"), _FileName3);
                string path4 = Path.Combine(Server.MapPath("~/UploadFile"), _FileName4);

                // Lưu hình ảnh nếu có
                if (hinh1 != null)
                {
                    hinh1.SaveAs(path1);
                    sanPham.hinhAnh1 = _FileName;
                }
                if (hinh2 != null)
                {
                    hinh2.SaveAs(path2);
                    sanPham.hinhAnh2 = _FileName2;
                }
                if (hinh3 != null)
                {
                    hinh3.SaveAs(path3);
                    sanPham.hinhAnh3 = _FileName3;
                }
                if (hinh4 != null)
                {
                    hinh4.SaveAs(path4);
                    sanPham.hinhAnh4 = _FileName4;
                }

                // Thêm sản phẩm vào cơ sở dữ liệu
                db.SanPhams.Add(sanPham);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            // Cung cấp lại danh sách nếu model không hợp lệ
            ViewBag.maDM = new SelectList(db.DanhMucs, "maDM", "tenDM", sanPham.maDM);
            ViewBag.maBenh = new SelectList(db.Benhs, "maBenh", "tenBenh", sanPham.maBenh);
            ViewBag.MaNhaCungCap = new SelectList(db.NhaCungCaps, "MaNhaCungCap", "TenNhaCungCap", sanPham.MaNhaCungCap);
            ViewBag.MaGiamGia = new SelectList(db.GiamGias, "MaGiamGia", "TenGiamGia", sanPham.MaGiamGia);
            return View(sanPham);
        }



        // GET: Admin/SanPham/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SanPham sanPham = db.SanPhams.Find(id);
            if (sanPham == null)
            {
                return HttpNotFound();
            }
            ViewBag.maDM = new SelectList(db.DanhMucs, "maDM", "tenDM", sanPham.maDM);
            return View(sanPham);
        }

        // POST: Admin/SanPham/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "maSP,tenSP,giaTien,chitietSP,maDM,soLuong,hinhAnh1,hinhAnh2,hinhAnh3,hinhAnh4")] SanPham sanPham,
            HttpPostedFileBase hinh1, HttpPostedFileBase hinh2, HttpPostedFileBase hinh3, HttpPostedFileBase hinh4)
        {
            if (ModelState.IsValid)
            {
                var idImage = Guid.NewGuid().ToString();
                var idImage2 = Guid.NewGuid().ToString();
                var idImage3 = Guid.NewGuid().ToString();
                var idImage4 = Guid.NewGuid().ToString();
                string _FileName = "";
                string _FileName2 = "";
                string _FileName3 = "";
                string _FileName4 = "";

                int index = hinh1.FileName.IndexOf('.');
                int index2 = hinh2.FileName.IndexOf('.');
                int index3 = hinh3.FileName.IndexOf('.');
                int index4 = hinh4.FileName.IndexOf('.');

                _FileName = idImage.ToString() + "." + hinh1.FileName.Substring(index + 1);
                _FileName2 = idImage2.ToString() + "." + hinh2.FileName.Substring(index2 + 1);
                _FileName3 = idImage3.ToString() + "." + hinh3.FileName.Substring(index3 + 1);
                _FileName4 = idImage4.ToString() + "." + hinh4.FileName.Substring(index4 + 1);

                string path1 = Path.Combine(Server.MapPath("~/UploadFile"), _FileName);
                string path2 = Path.Combine(Server.MapPath("~/UploadFile"), _FileName2);
                string path3 = Path.Combine(Server.MapPath("~/UploadFile"), _FileName3);
                string path4 = Path.Combine(Server.MapPath("~/UploadFile"), _FileName4);

                hinh1.SaveAs(path1);
                hinh2.SaveAs(path2);
                hinh3.SaveAs(path3);
                hinh4.SaveAs(path4);

                sanPham.hinhAnh1 = _FileName;
                sanPham.hinhAnh2 = _FileName2;
                sanPham.hinhAnh3 = _FileName3;
                sanPham.hinhAnh4 = _FileName4;
                db.Entry(sanPham).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.maDM = new SelectList(db.DanhMucs, "maDM", "tenDM", sanPham.maDM);
            return View(sanPham);
        }

        // GET: Admin/SanPham/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SanPham sanPham = db.SanPhams.Find(id);
            if (sanPham == null)
            {
                return HttpNotFound();
            }
            return View(sanPham);
        }

        // POST: Admin/SanPham/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            SanPham sanPham = db.SanPhams.Find(id);
            db.SanPhams.Remove(sanPham);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
