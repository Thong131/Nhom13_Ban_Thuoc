using GameStore.Models;
using PayPal.Api;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace GameStore.Controllers
{
    public class GioHangController : Controller
    {
        QLThuocSo1VNNGAY1010Entities db = new QLThuocSo1VNNGAY1010Entities();
        // GET: GioHang
        public ActionResult Index()
        {
            List<Cart> ShoppingCart = Session["ShoppingCart"] as List<Cart>;
            return View(ShoppingCart);
        }
        //Thêm sản phẩm vào giỏ hàng
        public RedirectToRouteResult AddToCart(int maSP)
        {

            if (Session["ShoppingCart"] == null) // Nếu giỏ hàng chưa được khởi tạo
            {
                Session["ShoppingCart"] = new List<Cart>();  // Khởi tạo Session["giohang"] là 1 List<CartItem>
            }

            List<Cart> ShoppingCart = Session["ShoppingCart"] as List<Cart>;  // Gán qua biến giohang dễ code

            // Kiểm tra xem sản phẩm khách đang chọn đã có trong giỏ hàng chưa

            if (ShoppingCart.FirstOrDefault(m => m.Id == maSP) == null) // ko co sp nay trong gio hang
            {
                // tim sp theo sanPhamID

                var sanpham = from sp in db.SanPhams
                              where sp.maSP == maSP
                              select sp;

                var query = sanpham.Include("HinhAnhs").ToList();

                Cart newItem = new Cart()
                {
                    Id = maSP,
                    Name = query.FirstOrDefault().tenSP,
                    Amount = 1,
                    Photo = query.FirstOrDefault().hinhAnh1,
                    Price = (int)query.FirstOrDefault().giaTien,

                };  // Tạo ra 1 CartItem mới

                ShoppingCart.Add(newItem);  // Thêm CartItem vào giỏ 
            }
            else
            {
                // Nếu sản phẩm khách chọn đã có trong giỏ hàng thì không thêm vào giỏ nữa mà tăng số lượng lên.
                Cart cardItem = ShoppingCart.FirstOrDefault(m => m.Id == maSP);
                cardItem.Amount++;
            }


            return RedirectToAction("ChiTietSP", "games", new { id = maSP });


        }
        //cập nhật sp trong giỏ hàng
        public RedirectToRouteResult UpdateAmount(int maSP, int newAmount)
        {
            // tìm carditem muon sua
            List<Cart> ShoppingCart = Session["ShoppingCart"] as List<Cart>;
            Cart EditAmount = ShoppingCart.FirstOrDefault(m => m.Id == maSP);
            if (EditAmount != null)
            {
                EditAmount.Amount = newAmount;
            }
            return RedirectToAction("Index");

        }
        //xóa sp trong giỏ hàng
        public RedirectToRouteResult RemoveItem(int maSP)
        {
            List<Cart> shoppingCart = Session["ShoppingCart"] as List<Cart>;
            Cart DelItem = shoppingCart.FirstOrDefault(m => m.Id == maSP);
            if (DelItem != null)
            {
                shoppingCart.Remove(DelItem);
            }
            return RedirectToAction("Index");
        }

        //thanh toán

        public ActionResult ThanhToan()
        {
            if (Session["userLogin"] != null)
            {
                if (Session["ShoppingCart"] != null)
                {
                    List<Cart> ShoppingCart = Session["ShoppingCart"] as List<Cart>;
                    if (ShoppingCart.Count() <= 0)
                    {
                        return RedirectToAction("Index", "GioHang");
                    }
                    else
                    {
                        return View(ShoppingCart);
                    }
                }
                return RedirectToAction("Index", "GioHang");


            }
            else
                return RedirectToAction("Login", "User");
        }


        [HttpPost]
        public JsonResult ThanhToan(string address)
        {
            try
            {
                List<Cart> ShoppingCart = Session["ShoppingCart"] as List<Cart>;

                string username = "";
                if (Session["userLogin"] != null)
                    username = (string)Session["userLogin"];


                double tongTien = 0;
                int soLuong = ShoppingCart.Count();
                string maDonHang = Guid.NewGuid().ToString();

                foreach (var item in ShoppingCart)
                {
                    tongTien += item.Money;
                }
                var nguoiDung = db.NguoiDungs.FirstOrDefault(nd => nd.username == username);

                var donhang = new DonHang();
                donhang.maNguoiDung = nguoiDung.maNguoiDung;
                donhang.trangThai = "Đang chờ";
                donhang.tongTien = tongTien;
                donhang.username = username;
                donhang.soLuong = soLuong;
                donhang.diachi = address;
                donhang.maDH = maDonHang;
                donhang.createdAt = DateTime.Now;
                donhang.updatedAt = DateTime.Now;
                donhang.MaKhuyenMai = 1;

                db.Configuration.ValidateOnSaveEnabled = false;
                db.DonHangs.Add(donhang);
                db.SaveChanges();

                foreach (var item in ShoppingCart)
                {
                    var chitiet = new ChiTietDonHang();
                    
                    chitiet.maDH = maDonHang;
                    chitiet.maSP = item.Id;
                    chitiet.soLuong = item.Amount;
                    chitiet.tongTien = (int)item.Money;

                    db.ChiTietDonHangs.Add(chitiet);
                    db.SaveChanges();

                }
                Session["ShoppingCart"] = null;
                return Json("succes", JsonRequestBehavior.AllowGet);

            }
            catch (Exception ex)
            {
                return Json(ex, JsonRequestBehavior.AllowGet);
            }




        }
        //=============================
        public ActionResult Payment()
        {
            // Lấy giỏ hàng từ session
            List<Cart> ShoppingCart = Session["ShoppingCart"] as List<Cart>;

            // Giả sử bạn đã lưu thông tin khách hàng trong session
            if (Session["hoTen"] != null && Session["sdt"] != null)
            {
                // Truyền hoTen và sdt qua ViewBag
                ViewBag.HoTen = Session["hoTen"].ToString();
                ViewBag.Sdt = Session["sdt"].ToString();
            }

            // Đảm bảo trả về một IEnumerable<Cart> không null
            return View(ShoppingCart ?? new List<Cart>());
        }

        [HttpPost]
        public ActionResult ProcessPayment(string fullName, decimal amount)
        {
            var apiContext = new APIContext(new OAuthTokenCredential(
                System.Configuration.ConfigurationManager.AppSettings["PayPal:ClientId"],
                System.Configuration.ConfigurationManager.AppSettings["PayPal:ClientSecret"]).GetAccessToken());

            var payer = new Payer() { payment_method = "paypal" };

            var redirectUrls = new RedirectUrls()
            {
                cancel_url = Url.Action("Cancel", "GioHang", null, Request.Url.Scheme),
                return_url = Url.Action("Success", "GioHang", null, Request.Url.Scheme)
            };

            var itemList = new ItemList() { items = new List<Item>() };
            itemList.items.Add(new Item()
            {
                name = "Đơn hàng của " + fullName,
                currency = "USD",
                price = amount.ToString(),
                quantity = "1",
                sku = "sku"
            });

            var amountDetails = new Amount() { currency = "USD", total = amount.ToString() };

            var transactionList = new List<Transaction>()
        {
            new Transaction()
            {
                description = "Thanh toán đơn hàng",
                invoice_number = Guid.NewGuid().ToString(), // Thay thế bằng mã đơn hàng của bạn
                amount = amountDetails,
                item_list = itemList
            }
        };

            var payment = new Payment()
            {
                intent = "sale",
                payer = payer,
                transactions = transactionList,
                redirect_urls = redirectUrls
            };

            var createdPayment = payment.Create(apiContext);

            // Chuyển hướng người dùng tới PayPal
            var approvalUrl = createdPayment.links.FirstOrDefault(x => x.rel.ToLower().Trim() == "approval_url").href;

            return Redirect(approvalUrl);
        }

        public ActionResult Success(string paymentId, string token, string PayerID)
        {
            try
            {
                // Lấy thông tin giỏ hàng từ session
                List<Cart> ShoppingCart = Session["ShoppingCart"] as List<Cart>;

                if (ShoppingCart == null || !ShoppingCart.Any())
                {
                    return RedirectToAction("Index", "GioHang");
                }
                int soLuong = ShoppingCart.Count();

                string username = "";
                if (Session["userLogin"] != null)
                {
                    username = (string)Session["userLogin"];
                }
                int soLuongNe = ShoppingCart.Count();

                string maDonHang = Guid.NewGuid().ToString();
                var nguoiDung = db.NguoiDungs.FirstOrDefault(nd => nd.username == username);

                // Tạo đơn hàng mới với tổng tiền là 0₫ và trạng thái "Đã thanh toán"
                var donhang = new DonHang
                {    maNguoiDung=nguoiDung.maNguoiDung,
                    trangThai = "Đã thanh toán",
                    tongTien = 0,  // Tổng tiền thành 0₫
                    username = username,
                    soLuong = soLuong,
                    diachi = "Địa chỉ giao hàng từ PayPal",  // Bạn có thể lấy địa chỉ từ API của PayPal
                    maDH = maDonHang,
                    ghiChu = "Thanh toán trực tuyến đã hoàn tất.",  // Ghi chú là đã thanh toán
                    createdAt = DateTime.Now,
                    updatedAt = DateTime.Now,
                    MaKhuyenMai = 1
                };

                // Lưu đơn hàng vào database
                db.Configuration.ValidateOnSaveEnabled = false;
                db.DonHangs.Add(donhang);
                db.SaveChanges();

                // Lưu chi tiết đơn hàng
                foreach (var item in ShoppingCart)
                {
                    var chitiet = new ChiTietDonHang
                    {
                        maDH = maDonHang,
                        maSP = item.Id,
                        soLuong = item.Amount,
                        tongTien = 0  // Tổng tiền cho mỗi sản phẩm cũng là 0₫
                    };

                    db.ChiTietDonHangs.Add(chitiet);

                    // Cập nhật số lượng đã bán cho sản phẩm
                    var sanPham = db.SanPhams.Find(item.Id);
                    if (sanPham != null)
                    {
                        sanPham.soLuong += item.Amount; // Tăng số lượng bán
                    }
                }

                db.SaveChanges();

                // Xóa giỏ hàng sau khi thanh toán thành công
                Session["ShoppingCart"] = null;

                return View();
            }
            catch (Exception ex)
            {
                // Xử lý lỗi nếu có
                return View("Error", new HandleErrorInfo(ex, "GioHang", "Success"));
            }
        }



        public ActionResult Cancel()
        {
            // Xử lý khi người dùng hủy thanh toán
            return View();
        }

    }
}