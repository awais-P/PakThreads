using Application.ViewModels.BaseVm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.ViewModels.UserVm
{
    public class UserVm : PaginationVm
    {
        public int Id { get; set; }
        public string FirstName { get; set; } = "";
        public string LastName { get; set; } = "";
        public string UserName { get; set; } = "";
        public string Email { get; set; } = "";
        public string Password { get; set; } = "";
        public string UserType { get; set; } = "";
        public string Country { get; set; } = "";
        public string BlockedReason { get; set; } = "";
        public string ProfileImageUrl { get; set; } = "";
        public string Gender { get; set; } = "";
        public DateTime DateOfBirth { get; set; } 
        public string PhoneNumber { get; set; } = "";
        public bool IsSiteUser { get; set; }
        public string AddedBy { get; set; } = "";
        public DateTime AddedDate { get; set; }
        public string UpdatedBy { get; set; } = "";
        public DateTime UpdatedDate { get; set; }
    }
}
