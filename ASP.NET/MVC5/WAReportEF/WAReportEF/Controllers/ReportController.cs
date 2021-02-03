using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using WAReportEF.Infrastructure;
using WAReportEF.Models;

namespace WAReportEF.Controllers
{
    public class ReportController : Controller
    {
        private DatabaseContext db = new DatabaseContext();

        // GET: Report
        public ActionResult Index()
        {
            ReportView reportView = new ReportView();
            List<Report> data = db.Reports.ToList();

            reportView.Filter = new ReportFilter();
            reportView.Reports = data;
            reportView.Locals = new SelectList(data.GroupBy(item => item.Local).Select(item => item.Key).OrderBy(item => item));

            return View(reportView);
        }

        [HttpPost]
        public ActionResult Index(ReportFilter filter)
        {
            ReportView reportView = new ReportView();
            reportView.Filter = new ReportFilter();
            reportView.Locals = new SelectList(db.Reports.GroupBy(item => item.Local).Select(item => item.Key).OrderBy(item => item));

            if (!string.IsNullOrEmpty(filter.Local) || filter.DateFrom.HasValue || filter.DateTo.HasValue)
            {
                if (filter.DateFrom.HasValue && filter.DateTo.HasValue && filter.DateFrom.Value > filter.DateTo.Value)
                {
                    DateTime tempDateFrom = filter.DateFrom.Value;
                    filter.DateFrom = filter.DateTo;
                    filter.DateTo = tempDateFrom;
                }

                string sql = "SELECT * FROM dbo.Report WHERE";
                string conditions = null;
                List<SqlParameter> parameters = new List<SqlParameter>();
                string parameterName = "";

                if (!string.IsNullOrEmpty(filter.Local))
                {
                    if (!string.IsNullOrEmpty(conditions))
                    {
                        conditions += " AND";
                    }
                    parameterName = PropertyInfoEx.GetPropertyName(() => filter.Local);
                    conditions += string.Format(" {0} = @{0}", parameterName);
                    parameters.Add(new SqlParameter(parameterName, filter.Local));
                }
                if (filter.DateFrom.HasValue)
                {
                    if (!string.IsNullOrEmpty(conditions))
                    {
                        conditions += " AND";
                    }
                    parameterName = PropertyInfoEx.GetPropertyName(() => filter.DateFrom);
                    conditions += string.Format(" Date >= @{0}", parameterName);
                    parameters.Add(new SqlParameter(parameterName, filter.DateFrom));
                }
                if (filter.DateTo.HasValue)
                {
                    if (!string.IsNullOrEmpty(conditions))
                    {
                        conditions += " AND";
                    }
                    parameterName = PropertyInfoEx.GetPropertyName(() => filter.DateTo);
                    conditions += string.Format(" Date <= @{0}", parameterName);
                    parameters.Add(new SqlParameter(parameterName, filter.DateTo));
                }

                sql += conditions;
                reportView.Reports = db.Reports.SqlQuery(sql, parameters.ToArray()).ToList();
            }
            else
            {
                reportView.Reports = db.Reports.ToList();
            }
            return View(reportView);
        }

        // GET: Report/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Report report = db.Reports.Find(id);
            if (report == null)
            {
                return HttpNotFound();
            }
            return View(report);
        }

        // GET: Report/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Report/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Name,Date,User,Local")] Report report)
        {
            if (ModelState.IsValid)
            {
                db.Reports.Add(report);
                db.SaveChanges();
            }

            return View(report);
        }

        // GET: Report/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Report report = db.Reports.Find(id);
            if (report == null)
            {
                return HttpNotFound();
            }
            return View(report);
        }

        // POST: Report/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Name,Date,User,Local")] Report report)
        {
            if (ModelState.IsValid)
            {
                db.Entry(report).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(report);
        }

        // GET: Report/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Report report = db.Reports.Find(id);
            if (report == null)
            {
                return HttpNotFound();
            }
            return View(report);
        }

        // POST: Report/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Report report = db.Reports.Find(id);
            db.Reports.Remove(report);
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
