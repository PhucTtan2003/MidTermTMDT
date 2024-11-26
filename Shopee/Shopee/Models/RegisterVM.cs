using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace Shopee.Models
{
    public class RegisterVM 
    {
        [Display(Name ="Tên đăng nhập")]
        [Required(ErrorMessage = "*")]
        [MaxLength(20,ErrorMessage ="Tối đa 20 kí tự tôi má")]
        public string Makh { get; set; } = null!;
        [Display(Name = "Mật khẩu")]
        [Required(ErrorMessage = "*")]
        public string Matkhau { get; set; }
        [MaxLength(50, ErrorMessage = "Tối đa 50 kí tự tôi má")]
        [Display(Name = "Họ tên")]
        public string Hoten { get; set; }

        public bool Gioitinh { get; set; } = true;

        public DateTime? Ngaysinh { get; set; }
        [MaxLength(60, ErrorMessage = "Tối đa 60 kí tự tôi má")]
        public string? Diachi { get; set; }
        [MaxLength(11, ErrorMessage = "Tối đa 11 kí tự tôi má")]
        [RegularExpression(@"0[98753]\d{8}", ErrorMessage ="Chưa đúng địng dạng")]
        public string Dienthoai { get; set; }
        [EmailAddress(ErrorMessage ="Chưa đúng định dạng")]
        public string Email { get; set; } 

        public string? Hinh { get; set; }
        // Thêm mã kích hoạt
        public string? ActivationCode { get; set; }

        public int Vaitro { get; set; } = 0; // 0: User, 1: Admin
    }
}
