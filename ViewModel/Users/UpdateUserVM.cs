using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace ViewModel.Users
{
    public class UpdateUserVM
    {
        public string Id { get; set; }
        [Display(Name = "Nick Name")]
        public string UserName { get; set; }

        [Display(Name = "Tên")]
        public string FirstName { get; set; }

        [Display(Name = "Họ")]
        public string LastName { get; set; }

        [Display(Name = "Ngày sinh")]
        [DataType(DataType.Date)]
        public DateTime Dob { get; set; }

        [Display(Name = "Email")]
        public string Email { get; set; }

        [Display(Name = "Số điện thoại")]
        public string PhoneNumber { get; set; }
    }
}
