using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;
using WAUploadSmallFile.Models;
using System.Linq;

namespace WAUploadSmallFile.Controllers
{
	public class UploadController : Controller
	{
		// GET: Upload
		public ActionResult Index()
		{
			return View();
		}

		[HttpPost]
		public async Task<ActionResult> UploadFile(HttpPostedFileBase file)
		{
			//var image = WebImage.GetImageFromRequest();

			if (file != null && file.ContentLength > 0)
			{
				string filePath = Path.GetTempFileName();

				using (var stream = new FileStream(filePath, FileMode.Create))
				{
					await file.InputStream.CopyToAsync(stream);
				}
			}
			return RedirectToAction("Index");
		}

		[HttpPost]
		public async Task<ActionResult> UploadMultipleFiles(IEnumerable<HttpPostedFileBase> files)
		{
			if (files != null)
			{
				foreach (HttpPostedFileBase file in files)
				{
					if (file.ContentLength > 0)
					{
						string filePath = Path.GetTempFileName();

						using (var stream = new FileStream(filePath, FileMode.Create))
						{
							await file.InputStream.CopyToAsync(stream);
						}
					}
				}
			}
			return RedirectToAction("Index");
		}

		// ===================
		// Lub
		// ===================
		//[HttpPost]
		//public async Task<ActionResult> UploadMultipleFiles()
		//{
		//	foreach (string name in Request.Files.AllKeys.Distinct())
		//	{
		//		foreach (var file in Request.Files.GetMultiple(name))
		//		{
		//			if (file.ContentLength > 0)
		//			{
		//				string filePath = Path.GetTempFileName();

		//				using (var stream = new FileStream(filePath, FileMode.Create))
		//				{
		//					await file.InputStream.CopyToAsync(stream);
		//				}
		//			}
		//		}
		//	}
		//	return RedirectToAction("Index");
		//}
	}
}