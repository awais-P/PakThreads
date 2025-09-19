using Application.ViewModels.BaseVm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.ViewModels.UserVm
{
    public class GetUserVm : PaginationVm
    {
        public long Id { get; set; }
        public string FirstName { get; set; } = "";
        public string LastName { get; set; } = "";
        public string UserName { get; set; } = "";
        public string UserEmail { get; set; } = "";
        public string Password { get; set; } = "";
        public string BlockedReason { get; set; } = "";
        public string Status { get; set; } = "";
        public string UserType { get; set; } = "";
        public string Gender { get; set; } = "";
        public string Country { get; set; } = "";
        public string PhoneNumber { get; set; } = "";
        public DateTime DateOfBirth { get; set; } = DateTime.Now;
        public string ProfileImageUrl { get; set; } = "";
        public string AddedBy { get; set; } = "";
        public DateTime AddedDate { get; set; }
        public string UpdatedBy { get; set; } = "";
        public DateTime UpdatedDate { get; set; }
    }
}
