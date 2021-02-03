using SportsStore.Domain.Models;
using SportsStore.WebUI.Infrastructure;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using PagedList;

namespace SportsStore.WebUI.Controllers
{
    public class ProductController : Controller
    {
        private ISportsStoreDbContext _db = null;

        public ProductController(ISportsStoreDbContext db)
        {
            _db = db;
        }

        // GET: Product
        public ActionResult Index(ProductFilter productFilter)
        {
            IQueryable<Product> products = _db.Products.Include(p => p.Category);

            if (!string.IsNullOrEmpty(productFilter.ProductName))
            {
                products = products.Where(item => item.Name.Contains(productFilter.ProductName));
            }

            if (!string.IsNullOrEmpty(productFilter.CategoryName))
            {
                products = products.Where(item => item.Category.Name.Contains(productFilter.CategoryName));
            }

            if (productFilter.UnitPrice.HasValue && productFilter.UnitPrice > 0)
            {
                products = products.Where(item => item.UnitPrice == productFilter.UnitPrice.Value);
            }

            switch (productFilter.SortColumn)
            {
                case "ProductName":
                    {
                        products = (productFilter.SortOrder == System.Data.SqlClient.SortOrder.Descending) ?
                            products.OrderByDescending(item => item.Name) :
                            products.OrderBy(item => item.Name);
                    }
                    break;

                case "CategoryName":
                    {
                        products = (productFilter.SortOrder == System.Data.SqlClient.SortOrder.Descending) ?
                            products.OrderByDescending(item => item.Category.Name) :
                            products.OrderBy(item => item.Category.Name);
                    }
                    break;

                case "UnitPrice":
                    {
                        products = (productFilter.SortOrder == System.Data.SqlClient.SortOrder.Descending) ?
                            products.OrderByDescending(item => item.UnitPrice) :
                            products.OrderBy(item => item.UnitPrice);
                    }
                    break;

                default:
                    {
                        products = (productFilter.SortOrder == System.Data.SqlClient.SortOrder.Descending) ?
                            products.OrderByDescending(item => item.Name) :
                            products.OrderBy(item => item.Name);
                    }
                    break;
            }

            ViewBag.ProductFilter = productFilter;
            IPagedList<Product> list = products.ToPagedList(productFilter.PageNumber.Value, productFilter.PageSize);
            return View(list);
        }

        // GET: Product/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Product product = _db.Products.Find(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            return View(product);
        }

        // GET: Product/Create
        public ActionResult Create()
        {
            ViewBag.IdCategory = new SelectList(_db.Categories.OrderBy(item => item.Name), "IdCategory", "Name");
            return View();
        }

        // POST: Product/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "IdProduct,IdCategory,Name,UnitPrice")] Product product)
        {
            if (ModelState.IsValid)
            {
                _db.Products.Add(product);
                _db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.IdCategory = new SelectList(_db.Categories.OrderBy(item => item.Name), "IdCategory", "Name", product.IdCategory);
            return View(product);
        }

        // GET: Product/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Product product = _db.Products.Find(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            ViewBag.IdCategory = new SelectList(_db.Categories.OrderBy(item => item.Name), "IdCategory", "Name", product.IdCategory);
            return View(product);
        }

        // POST: Product/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "IdProduct,IdCategory,Name,UnitPrice")] Product product)
        {
            if (ModelState.IsValid)
            {
                _db.Entry(product).State = EntityState.Modified;
                _db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.IdCategory = new SelectList(_db.Categories.OrderBy(item => item.Name), "IdCategory", "Name", product.IdCategory);
            return View(product);
        }

        // GET: Product/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Product product = _db.Products.Find(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            return View(product);
        }

        // POST: Product/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Product product = _db.Products.Find(id);
            _db.Products.Remove(product);
            _db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && _db != null)
            {
                _db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
