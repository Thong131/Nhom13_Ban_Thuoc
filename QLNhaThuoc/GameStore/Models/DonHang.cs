//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace GameStore.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class DonHang
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public DonHang()
        {
            this.ChiTietDonHangs = new HashSet<ChiTietDonHang>();
            this.ThanhToans = new HashSet<ThanhToan>();
        }
    
        public string maDH { get; set; }
        public string Sdt { get; set; }
        public string HoTen { get; set; }
        public string ghiChu { get; set; }
        public string username { get; set; }
        public string diachi { get; set; }
        public int MaKhuyenMai { get; set; }
        public double tongTien { get; set; }
        public int soLuong { get; set; }
        public string trangThai { get; set; }
        public Nullable<System.DateTime> createdAt { get; set; }
        public Nullable<System.DateTime> updatedAt { get; set; }
        public Nullable<int> maNguoiDung { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ChiTietDonHang> ChiTietDonHangs { get; set; }
        public virtual KhuyenMai KhuyenMai { get; set; }
        public virtual NguoiDung NguoiDung { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ThanhToan> ThanhToans { get; set; }
    }
}
