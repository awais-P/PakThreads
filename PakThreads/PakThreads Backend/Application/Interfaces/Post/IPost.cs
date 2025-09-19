using Application.ViewModels.PostVm;
using Application.ViewModels.ResponseModel;
using Application.ViewModels.UserVm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces.Post
{
    public interface IPost
    {
        ResponseVm CreatePost(PostVm model);
        ResponseVm GetAllPosts(PostSearchVm model);
        ResponseVm DeletePost(long PostId);
        ResponseVm GetUserPosts(PostSearchVm model);

    }
}
