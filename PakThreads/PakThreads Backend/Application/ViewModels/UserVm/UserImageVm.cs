using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.ViewModels.UserVm
{
    public class UserImageVm
    {
        public long UserId { get; set; }

        public string ImageBase64 { get; set; } = "";
    }
}
