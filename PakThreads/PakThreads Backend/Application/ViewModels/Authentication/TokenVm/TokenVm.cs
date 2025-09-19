using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.ViewModels.Authentication.TokenVm
{
    public class TokenVm
    {
        public static long UserID { get; set; }
        public static string? UserEmail { get; set; } = "";
        public static string? UserName { get; set; } = "";
    }
}
