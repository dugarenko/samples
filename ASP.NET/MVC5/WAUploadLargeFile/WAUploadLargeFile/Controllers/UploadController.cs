using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace WAUploadLargeFile.Controllers
{
    public class UploadController : Controller
    {
        // GET: Upload
        public ActionResult Index()
        {
            return View();
        }

		[HttpPost]
		public async Task<ActionResult> UploadFiles()
		{
			HttpFileCollectionBase files = Request.Files;
			for (int i = 0; i < files.Count; i++)
			{
				HttpPostedFileBase file = files[i];
				string fname = Path.GetTempFileName();
				//file.SaveAs(fname);
			}
			return RedirectToAction("Index");
		}
    }
}