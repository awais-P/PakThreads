using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.ViewModels.UserVm
{
    public class UserDataVm
    {
        public long Id { get; set; }
        public string FirstName { get; set; } = "";
        public string LastName { get; set; } = "";
        public string Gender { get; set; } = "";
        public string PhoneNumber { get; set; } = "";
        public string UserBio { get; set; } = "";
        public DateTime DateOfBirth { get; set; } = DateTime.Now;
        public string Country { get; set; } = "";
        public string AddedBy { get; set; } = "";
        public DateTime AddedDate { get; set; }
        public string UpdatedBy { get; set; } = "";
        public DateTime UpdatedDate { get; set; }
    }
}
