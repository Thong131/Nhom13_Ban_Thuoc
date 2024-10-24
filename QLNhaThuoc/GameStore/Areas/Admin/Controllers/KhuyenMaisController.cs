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
    public class KhuyenMaisController : Controller
    {
        private QLThuocSo1VNNGAY1010Entities db = new QLThuocSo1VNNGAY1010Entities();

        // GET: Admin/KhuyenMais
        public ActionResult Index()
        {
            return View(db.KhuyenMais.ToList());
        }

        // GET: Admin/KhuyenMais/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            KhuyenMai khuyenMai = db.KhuyenMais.Find(id);
            if (khuyenMai == null)
            {
                return HttpNotFound();
            }
            return View(khuyenMai);
        }

        // GET: Admin/KhuyenMais/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Admin/KhuyenMais/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "MaKhuyenMai,GiaTri,ThoiGianBatDau,ThoiGianKetThuc,TrangThai,NgayTao,DieuKienApDung,SoLuong")] KhuyenMai khuyenMai)
        {
            if (ModelState.IsValid)
            {
                db.KhuyenMais.Add(khuyenMai);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(khuyenMai);
        }

        // GET: Admin/KhuyenMais/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            KhuyenMai khuyenMai = db.KhuyenMais.Find(id);
            if (khuyenMai == null)
            {
                return HttpNotFound();
            }
            return View(khuyenMai);
        }

        // POST: Admin/KhuyenMais/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "MaKhuyenMai,GiaTri,ThoiGianBatDau,ThoiGianKetThuc,TrangThai,NgayTao,DieuKienApDung,SoLuong")] KhuyenMai khuyenMai)
        {
            if (ModelState.IsValid)
            {
                db.Entry(khuyenMai).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(khuyenMai);
        }

        // GET: Admin/KhuyenMais/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            KhuyenMai khuyenMai = db.KhuyenMais.Find(id);
            if (khuyenMai == null)
            {
                return HttpNotFound();
            }
            return View(khuyenMai);
        }

        // POST: Admin/KhuyenMais/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            KhuyenMai khuyenMai = db.KhuyenMais.Find(id);
            db.KhuyenMais.Remove(khuyenMai);
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
