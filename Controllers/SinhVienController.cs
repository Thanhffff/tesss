using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using NguyenNgocThanh.Models;
namespace NguyenNgocThanh.Controllers
{
    public class SinhVienController : Controller
    {
        // GET: SinhVien
        MyDataDataContext data = new MyDataDataContext();
        public ActionResult ListSV()
        {
            var all_SinhVien = from ss in data.SinhViens select ss;
            return View(all_SinhVien);
        }
        public ActionResult Detail(string id)
        {
            var D_sv = data.SinhViens.Where(m => m.MaSV==id).First();
            return View(D_sv);
        }
        public ActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Create(FormCollection collection, SinhVien s)
        {
            var E_tensv = collection["HoTen"];
            var E_hinh = collection["Hinh"];
            var E_ngaysinh =Convert.ToDateTime(collection["NgaySinh"]); 
            var E_gioitinh =collection["GioiTinh"];
            var E_manganh =collection["MaNganh"];
            if (string.IsNullOrEmpty(E_tensv))
            {
                ViewData["Error"] = "Don't empty!";
            }
            else
            {
                s.HoTen = E_tensv.ToString();
                s.Hinh = E_hinh.ToString();
                s.NgaySinh = E_ngaysinh;
                s.GioiTinh = E_gioitinh.ToString();
                s.MaNganh = E_manganh.ToString();
                data.SinhViens.InsertOnSubmit(s);
                data.SubmitChanges();
                return RedirectToAction("ListSV");
            }
            return this.Create();
        }
        public ActionResult Edit(string id)
        {
            var E_sinhvien = data.SinhViens.First(m => m.MaSV == id);
            return View(E_sinhvien);
        }
        [HttpPost]
        public ActionResult Edit(string id, FormCollection collection)
        {
            var E_sv = data.SinhViens.First(m => m.MaSV == id);
            var E_tensv = collection["HoTen"];
            var E_Hinh = collection["Hinh"];
            var E_gioitinh =collection["GioiTinh"];
            var E_ngaysinh = Convert.ToDateTime(collection["NgaySinh"]);
            var E_MaNganh = collection["MaNganh"];
            if (string.IsNullOrEmpty(E_tensv))
            {
                ViewData["Error"] = "Don't empty!";
            }
            else
            {
                E_sv.HoTen = E_tensv;
                E_sv.Hinh = E_Hinh;
                E_sv.GioiTinh = E_gioitinh;
                E_sv.NgaySinh = E_ngaysinh;
                E_sv.MaNganh = E_MaNganh;
                UpdateModel(E_sv);
                data.SubmitChanges();
                return RedirectToAction("ListSach");
            }
            return this.Edit(id);
        }
        //----------------------------------------- 
        public ActionResult Delete(string id)
        {
            var D_sv = data.SinhViens.First(m => m.MaSV == id);
            return View(D_sv);
        }
        [HttpPost]
        public ActionResult Delete(string id, FormCollection collection)
        {
            var D_sach = data.SinhViens.Where(m => m.MaSV == id).First();
            data.SinhViens.DeleteOnSubmit(D_sach);
            data.SubmitChanges();
            return RedirectToAction("ListSV");
        }
        public string ProcessUpload(HttpPostedFileBase file)
        {
            if (file == null)
            {
                return "";
            }
            file.SaveAs(Server.MapPath("~/Content/images/" + file.FileName));
            return "/Content/images/" + file.FileName;
        }
        public ActionResult HocPhan()
        {
            var all_sv = from ss in data.HocPhans select ss;
            return View(all_sv);
        }
        public ActionResult DangKy()
        {
            return View();
        }
        [HttpPost]
        public ActionResult DangKy(FormCollection collection, DangKy kh)
        {
            var madk = collection["MaDk"];
            var ngaydk = Convert.ToString(collection["NgayDK"]);
            var masv = collection["MaSV"];
            if (String.IsNullOrEmpty(madk))
            {
                ViewData["Loi1"] = "Mã đăng ký không được để trống";
            }
            else if (String.IsNullOrEmpty(ngaydk))
            {
                ViewData["Loi2"] = "Ngày đăng ký không được để trống";
            }
            else if (String.IsNullOrEmpty(masv))
            {
                ViewData["Loi3"] = "MSSV không được để trống";
            }
            else
            {
                kh.MaDK = Convert.ToInt32(madk);
                kh.NgayDK = DateTime.Parse(ngaydk);
                kh.MaSV = masv;
                data.DangKies.InsertOnSubmit(kh);
                data.SubmitChanges();
                return RedirectToAction("DangNhap");

            }
            return this.DangKy();
        }
        public ActionResult DangNhap()
        {
            return View();
        }

        [HttpPost]
        public ActionResult DangNhap(FormCollection collection, DangKy kh)
        {
            var madk = collection["MaDk"];
            var ngaydk = Convert.ToString(collection["NgayDK"]);
            var masv = collection["MaSV"];
            if (String.IsNullOrEmpty(masv))
            {
                ViewData["Loi1"] = "Phải nhập tên đăng nhập";
            }
            else
            {
                kh = data.DangKies.SingleOrDefault(n => n.MaSV == masv);
                if (kh != null)
                {
                    ViewBag.ThongBao = "Đăng nhập thành công!!!";
                    Session["TaiKhoan"] = kh;
                }
                else
                {
                    ViewBag.ThongBao = "MSSV không đúng!!!";
                }
                return View();
            }
            return RedirectToAction("ListSV", "SinhVien");

        }
    }
}