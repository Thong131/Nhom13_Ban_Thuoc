using GameStore.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace GameStore.Controllers
{
   
		// GET: HealthCheck
		public class HealthCheckController : Controller
		{
			// Hiển thị trang kiểm tra sức khỏe
			public ActionResult Index()
			{
				return View();
			}

			// Nhận thông tin từ form và hiển thị kết quả
			[HttpPost]
			public ActionResult Check(HealthCheck model)
			{
				if (ModelState.IsValid)
				{
					return View("Result", model); // Hiển thị kết quả
				}
				return View("Index"); // Trở lại trang nhập liệu nếu có lỗi
			}
	
	}

}