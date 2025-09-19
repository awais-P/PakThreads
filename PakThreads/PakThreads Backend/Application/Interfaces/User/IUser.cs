using Application.ViewModels.CommonVm;
using Application.ViewModels.ResponseModel;
using Application.ViewModels.UserVm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces.User
{
    public interface IUser
    {
        ResponseVm CreateUser(UserVm user);      
        ResponseVm GetUsers(UserSearchVm model);
        ResponseVm UnBlockUser(long Id);
        ResponseVm BlockUser(DeclineVm model);
        ResponseVm DeleteUser(long Id);
        ResponseVm GetUserCount();
        ResponseVm SaveAndUpdateUserImage(UserImageVm model);
        ResponseVm SaveAndUpdateUserDetail(UserDataVm user);

        ResponseVm LoginUser(LoginUserVm model);

        //ResponseVm VerifyUserEmail(string VerifyToken);
        //Task<ResponseVm> ForgetUserPassword(string email);
        //ResponseVm ResetUserPassword(string ResetToken, string password);
        //ResponseVm ChangeUserPassword(ChangeUserPasswordVm model);
        //ResponseVm ResendVerificationEmail(ResendUserEmailVm model);
    }
}


