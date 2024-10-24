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
    public class BenhsController : Controller
    {
        private QLThuocSo1VNNGAY1010Entities db = new QLThuocSo1VNNGAY1010Entities();

        // GET: Admin/Benhs
        public ActionResult Index()
        {
            return View(db.Benhs.ToList());
        }

        // GET: Admin/Benhs/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Benh benh = db.Benhs.Find(id);
            if (benh == null)
            {
                return HttpNotFound();
            }
            return View(benh);
        }

        // GET: Admin/Benhs/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Admin/Benhs/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "maBenh,tenBenh,moTa")] Benh benh)
        {
            if (ModelState.IsValid)
            {
                db.Benhs.Add(benh);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(benh);
        }

        // GET: Admin/Benhs/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Benh benh = db.Benhs.Find(id);
            if (benh == null)
            {
                return HttpNotFound();
            }
            return View(benh);
        }

        // POST: Admin/Benhs/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "maBenh,tenBenh,moTa")] Benh benh)
        {
            if (ModelState.IsValid)
            {
                db.Entry(benh).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(benh);
        }

        // GET: Admin/Benhs/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Benh benh = db.Benhs.Find(id);
            if (benh == null)
            {
                return HttpNotFound();
            }
            return View(benh);
        }

        // POST: Admin/Benhs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Benh benh = db.Benhs.Find(id);
            db.Benhs.Remove(benh);
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
