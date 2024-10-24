using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using GameStore.Models;

namespace GameStore.Areas.Admin.Controllers
{
    public class DanhGiasController : Controller
    {
        private QLThuocSo1VNNGAY1010Entities db = new QLThuocSo1VNNGAY1010Entities();

        // GET: Admin/DanhGias
        public ActionResult Index()
        {
            var danhGias = db.DanhGias.Include(d => d.NguoiDung).Include(d => d.SanPham);
            return View(danhGias.ToList());
        }

        // GET: Admin/DanhGias/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DanhGia danhGia = db.DanhGias.Find(id);
            if (danhGia == null)
            {
                return HttpNotFound();
            }
            return View(danhGia);
        }

        // GET: Admin/DanhGias/Create
        public ActionResult Create()
        {
            ViewBag.MaNguoiDung = new SelectList(db.NguoiDungs, "maNguoiDung", "username");
            ViewBag.MaSanPham = new SelectList(db.SanPhams, "maSP", "tenSP");
            return View();
        }

        // POST: Admin/DanhGias/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "MaDanhGia,MaSanPham,MaNguoiDung,NoiDung,NgayBinhLuan")] DanhGia danhGia)
        {
            if (ModelState.IsValid)
            {
                db.DanhGias.Add(danhGia);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.MaNguoiDung = new SelectList(db.NguoiDungs, "maNguoiDung", "username", danhGia.MaNguoiDung);
            ViewBag.MaSanPham = new SelectList(db.SanPhams, "maSP", "tenSP", danhGia.MaSanPham);
            return View(danhGia);
        }

        // GET: Admin/DanhGias/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DanhGia danhGia = db.DanhGias.Find(id);
            if (danhGia == null)
            {
                return HttpNotFound();
            }
            ViewBag.MaNguoiDung = new SelectList(db.NguoiDungs, "maNguoiDung", "username", danhGia.MaNguoiDung);
            ViewBag.MaSanPham = new SelectList(db.SanPhams, "maSP", "tenSP", danhGia.MaSanPham);
            return View(danhGia);
        }

        // POST: Admin/DanhGias/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "MaDanhGia,MaSanPham,MaNguoiDung,NoiDung,NgayBinhLuan")] DanhGia danhGia)
        {
            if (ModelState.IsValid)
            {
                db.Entry(danhGia).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.MaNguoiDung = new SelectList(db.NguoiDungs, "maNguoiDung", "username", danhGia.MaNguoiDung);
            ViewBag.MaSanPham = new SelectList(db.SanPhams, "maSP", "tenSP", danhGia.MaSanPham);
            return View(danhGia);
        }

        // GET: Admin/DanhGias/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DanhGia danhGia = db.DanhGias.Find(id);
            if (danhGia == null)
            {
                return HttpNotFound();
            }
            return View(danhGia);
        }

        // POST: Admin/DanhGias/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            DanhGia danhGia = db.DanhGias.Find(id);
            db.DanhGias.Remove(danhGia);
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
