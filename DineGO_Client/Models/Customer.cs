using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DineGO_Client.Model
{
    public class Customer
    {
        [Key]
        public int cus_id { get; set; }

        [Required(ErrorMessage = "Tên tài khoản không được để trống")]
        [MaxLength(50, ErrorMessage = "Tên tài khoản không được vượt quá 50 ký tự")]
        public string cus_username { get; set; }

        [Required(ErrorMessage = "Mật khẩu không được để trống")]
        [MinLength(6, ErrorMessage = "Mật khẩu phải có ít nhất 6 ký tự")]
        [MaxLength(100, ErrorMessage = "Mật khẩu không được vượt quá 100 ký tự")]
        public string cus_password { get; set; }

        [Required(ErrorMessage = "Xác nhận mật khẩu không được để trống")]
        [Compare("cus_password", ErrorMessage = "Mật khẩu xác nhận không khớp")]
        [NotMapped] // Không lưu vào database
        public string confirm_password { get; set; }

        [Required(ErrorMessage = "Họ và tên không được để trống")]
        [MaxLength(100, ErrorMessage = "Họ và tên không được vượt quá 100 ký tự")]
        public string cus_name { get; set; }

        [Required(ErrorMessage = "Email không được để trống")]
        [EmailAddress(ErrorMessage = "Email không hợp lệ")]
        [MaxLength(100, ErrorMessage = "Email không được vượt quá 100 ký tự")]
        public string cus_email { get; set; }

        [Required(ErrorMessage = "Số điện thoại không được để trống")]
        [Phone(ErrorMessage = "Số điện thoại không hợp lệ")]
        [RegularExpression(@"^\d{10}$", ErrorMessage = "Số điện thoại phải có đúng 10 chữ số")]
        public string cus_phone { get; set; }

        [MaxLength(200, ErrorMessage = "Địa chỉ không được vượt quá 200 ký tự")]
        public string cus_address { get; set; }

        [Required(ErrorMessage = "Ngày sinh không được để trống")]
        [DataType(DataType.Date, ErrorMessage = "Ngày sinh không hợp lệ")]
        public DateTime cus_birthday { get; set; }

        [Required(ErrorMessage = "Giới tính không được để trống")]
        [StringLength(10, ErrorMessage = "Giới tính không hợp lệ")]
        [RegularExpression(@"^(Nam|Nữ|Khác)$", ErrorMessage = "Giới tính phải là Nam, Nữ hoặc Khác")]
        public string cus_gender { get; set; }

        public string cus_image { get; set; }
        public bool cus_isKYI { get; set; }

        public List<Reservation> reservations { get; set; }
        public List<Payment> payments { get; set; }
        public List<Notification> notifications { get; set; }
        public List<RestaurantOwner> restaurantOwners { get; set; }
    }
}
