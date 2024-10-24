using GameStore.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.IO;
using iTextSharp.text;
using iTextSharp.text.pdf;

namespace GameStore.Controllers
{
    public class HomeController : Controller
    {
        QLThuocSo1VNNGAY1010Entities db = new QLThuocSo1VNNGAY1010Entities();
        public ActionResult Index()
        {

            return View();
        }
        public ActionResult ViTriNhaThuoc()
        {

            return View();
        }

        public JsonResult GetData()
        {
            int cartCount = 0;
            if (Session["ShoppingCart"] != null) // Nếu giỏ hàng chưa được khởi tạo
            {
                List<Cart> ShoppingCart = Session["ShoppingCart"] as List<Cart>;
                cartCount = ShoppingCart.Count();

            }
           
            return Json(cartCount,JsonRequestBehavior.AllowGet);
        }

        public ActionResult DonHang()
        {
         
            if (Session["userLogin"] == null)
            {
                return RedirectToAction("Login", "User");
            }
            else
            {
                string userID = (string)Session["userLogin"];
                var donhang = from dh in db.DonHangs
                           where dh.username == userID
                           select dh;

                donhang = donhang.Include("ChiTietDonHangs").OrderByDescending(e=>e.trangThai);

                /*.Include("SanPham").Include("HinhAnh");*/



                return View(donhang.ToList());
            }
              
        }


        public RedirectToRouteResult HuyDonHang(string maDH)
        {
            if (Session["userLogin"] == null)
            {
                return RedirectToAction("Login", "User");
            }
            else
            {
                string userID = (string)Session["userLogin"];
                var dh = from donhang in db.DonHangs
                         where donhang.maDH == maDH && donhang.username ==userID
                         select donhang;
                dh.FirstOrDefault().updatedAt = DateTime.Now;
                dh.FirstOrDefault().trangThai = "Đã hủy";
                db.SaveChanges();
                return RedirectToAction("DonHang","Home");
            }
                
            
        }





        public ActionResult XuatHoaDonPDF(string maDH)
        {
            // Kiểm tra người dùng đã đăng nhập chưa
            if (Session["userLogin"] == null)
            {
                return RedirectToAction("Login", "User");
            }

            string userID = (string)Session["userLogin"];
            var donhang = db.DonHangs.Include("ChiTietDonHangs")
                        .FirstOrDefault(dh => dh.maDH == maDH && dh.username == userID);

            if (donhang == null)
            {
                return HttpNotFound(); // Trả về 404 nếu không tìm thấy đơn hàng
            }

            // Tạo PDF
            using (MemoryStream stream = new MemoryStream())
            {
                Document pdfDoc = new Document(PageSize.A4);
                PdfWriter.GetInstance(pdfDoc, stream);
                pdfDoc.Open();

                // Thêm tiêu đề
                pdfDoc.Add(new Paragraph("Hóa Đơn Đơn Hàng"));
                pdfDoc.Add(new Paragraph($"Mã Đơn Hàng: {donhang.maDH}"));
                pdfDoc.Add(new Paragraph($"Tên Khách Hàng: {donhang.HoTen}"));
                pdfDoc.Add(new Paragraph($"Số Điện Thoại: {donhang.Sdt}"));
                pdfDoc.Add(new Paragraph($"Địa Chỉ: {donhang.diachi}"));
                pdfDoc.Add(new Paragraph($"Trạng Thái: {donhang.trangThai}"));

                // Thêm tiêu đề cho bảng chi tiết
                pdfDoc.Add(new Paragraph("Chi Tiết Đơn Hàng"));
                PdfPTable table = new PdfPTable(4);
                table.AddCell("Tên Sản Phẩm");
                table.AddCell("Số Lượng");
                table.AddCell("Giá");
                table.AddCell("Tổng");

                // Thêm thông tin chi tiết đơn hàng
                //foreach (var item in donhang.ChiTietDonHangs)
                //{
                //    table.AddCell(item.TenSanPham);
                //    table.AddCell(item.SoLuong.ToString());
                //    table.AddCell(item.Gia.ToString("C"));
                //    table.AddCell((item.Gia * item.SoLuong).ToString("C"));
                //}

                //pdfDoc.Add(table);
                //pdfDoc.Add(new Paragraph($"Tổng Giá Trị Đơn Hàng: {donhang.TongGiaTri.ToString("C")}"));

                pdfDoc.Close();

                // Trả về PDF cho người dùng
                byte[] bytes = stream.ToArray();
                return File(bytes, "application/pdf", "HoaDon_" + donhang.maDH + ".pdf");
            }
        }
    }
}