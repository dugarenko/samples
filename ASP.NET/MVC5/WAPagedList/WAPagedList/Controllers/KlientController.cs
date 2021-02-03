using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using WAPagedList.Infrastructure;
using WAPagedList.Models;
using PagedList;

namespace WAPagedList.Controllers
{
    public class KlientController : Controller
    {
        private TestEntities db = new TestEntities();

        // GET: Klient
        public ActionResult Index(FilterKlient filter)
        {
            IQueryable<Klient> klienci;

            if (string.IsNullOrEmpty(filter.Nazwa))
            {
                klienci = db.Klient.AsQueryable();
            }
            else
            {
                klienci = db.Klient.Where(item => item.Nazwa.Contains(filter.Nazwa));
            }

            switch (filter.SortColumn)
            {
                case "Nazwa":
                    {
                        klienci = (filter.SortOrder == System.Data.SqlClient.SortOrder.Descending) ?
                            klienci.OrderByDescending(item => item.Nazwa) :
                            klienci.OrderBy(item => item.Nazwa);
                    }
                    break;

                default:
                    {
                        klienci = (filter.SortOrder == System.Data.SqlClient.SortOrder.Descending) ?
                            klienci.OrderByDescending(item => item.Nazwa) :
                            klienci.OrderBy(item => item.Nazwa);
                    }
                    break;
            }

            ViewBag.FilterKlient = filter;
            int pageSize = 4;
            int pageNumber = (filter.Page ?? 1);
            return View(klienci.ToPagedList(pageNumber, pageSize));
        }

        // GET: Klient/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Klient klient = db.Klient.Find(id);
            if (klient == null)
            {
                return HttpNotFound();
            }
            return View(klient);
        }

        // GET: Klient/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Klient/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "IdKlient,Nazwa")] Klient klient)
        {
            if (ModelState.IsValid)
            {
                db.Klient.Add(klient);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(klient);
        }

        // GET: Klient/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Klient klient = db.Klient.Find(id);
            if (klient == null)
            {
                return HttpNotFound();
            }
            return View(klient);
        }

        // POST: Klient/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "IdKlient,Nazwa")] Klient klient)
        {
            if (ModelState.IsValid)
            {
                db.Entry(klient).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(klient);
        }

        // GET: Klient/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Klient klient = db.Klient.Find(id);
            if (klient == null)
            {
                return HttpNotFound();
            }
            return View(klient);
        }

        // POST: Klient/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Klient klient = db.Klient.Find(id);
            db.Klient.Remove(klient);
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
