using Application.Interfaces.Post;
using Application.ViewModels.Authentication.TokenVm;
using Application.ViewModels.PostVm;
using Application.ViewModels.ResponseModel;
using CommonOperations.CommonMethods;
using CommonOperations.Enums;
using Dapper;
using Domain.Models.Entities.PostEntities;
using Infrastructure.Context;
using Infrastructure.Services.TokenServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Services.PostServices
{
    public class PostServices : IPost
    {
        private readonly AppDbContext _appDbContext;
        private readonly TokenService _tokenService;
        public PostServices(AppDbContext appDbContext, TokenService tokenService)
        {
            _appDbContext = appDbContext;
            _tokenService = tokenService;
        }

        public ResponseVm CreatePost(PostVm model)
        {
            ResponseVm response = ResponseVm.Instance;                        

            if (model.Id != 0)
            {
                var exisitingPost = _appDbContext.Post.FirstOrDefault(op => op.Id == model.Id && !op.IsDeleted);
                if (exisitingPost != null)
                {
                    exisitingPost.Title = model.Title;
                    exisitingPost.ContentUrl = Path.Combine("D:\\repos\\PakthreadsData\\Posts", model.Title, $"{model.Title}File{DateTime.Now.Ticks}");
                    CommonMethod.UploadBase64Data(model.ContentUrl, exisitingPost.ContentUrl);
                    exisitingPost.Upvotes = model.Upvotes;
                }

                if (model.PostTagsVms != null)
                {
                    var existingTags = _appDbContext.PostTags.Where(op => op.PostId == model.Id).ToList();
                    foreach (var tag in existingTags)
                    {
                        if(!model.PostTagsVms.Any(a => a.Id == tag.Id))
                        {
                            _appDbContext.PostTags.Remove(tag);
                        }
                        else
                        {
                            var matchedTags = model.PostTagsVms.FirstOrDefault(a => a.Id == tag.Id);
                            tag.UpdatedDate = DateTime.Now;
                            tag.TagName = matchedTags.TagName;
                            _appDbContext.PostTags.Update(tag);

                        }
                    }

                    foreach (var newTag in model.PostTagsVms)
                    {
                        if (newTag.Id == 0)
                        {
                            PostTags newTagEntity = new PostTags
                            {
                                PostId = model.Id,
                                TagName = newTag.TagName,
                                AddedDate = DateTime.Now
                            };
                            _appDbContext.PostTags.Add(newTagEntity);

                        }
                    }
                }
                else
                {
                    response.responseCode = ResponseCode.BadRequest;
                    response.errorMessage = "Invalid PostTag Data";
                    return response;
                }

            }
            else
            {
                var existingPost = _appDbContext.Post.FirstOrDefault(op => op.Title.ToLower().Trim() == 
                model.Title.ToLower().Trim() && !op.IsDeleted);

                if (existingPost != null)
                {
                    response.data = existingPost;
                    response.responseCode = ResponseCode.BadRequest;
                    response.errorMessage = "Post Already Exists";
                }
                else
                {
                    Post newPost = new Post();
                    
                    newPost.Title = model.Title;
                    newPost.PostType = model.PostType;
                    newPost.Upvotes = model.Upvotes;
                    newPost.Downvotes = model.Downvotes;
                    newPost.ContentUrl = Path.Combine("D:\\repos\\PakthreadsData\\Posts", model.Title, $"{model.Title}File{DateTime.Now.Ticks}");
                    CommonMethod.UploadBase64Data(model.ContentUrl, newPost.ContentUrl);
                    newPost.AddedBy = _tokenService.UserID.ToString();
                    newPost.AddedDate = DateTime.Now;
                    _appDbContext.Post.Add(newPost);
                    _appDbContext.SaveChanges();

                    if (model.PostTagsVms != null)
                    {
                        foreach (var tag in model.PostTagsVms)
                        {
                            PostTags newTag = new PostTags
                            {
                                PostId = newPost.Id,
                                TagName = tag.TagName,
                                AddedDate = DateTime.Now
                            };
                            _appDbContext.PostTags.Add(newTag);
                        }
                        _appDbContext.SaveChanges();
                    }
                    else
                    {
                        response.responseCode = ResponseCode.BadRequest;
                        response.errorMessage = "Invalid PostTag Data";
                        return response;
                    }

                }

            }
            response.responseCode = ResponseCode.Success;
            response.responseMessage = "Post Created Successfully";
            return response;
        }
        public ResponseVm DeletePost(long PostId)
        {
            ResponseVm response = ResponseVm.Instance;

            var post = _appDbContext.Post.FirstOrDefault(a => a.Id == PostId && !a.IsDeleted);
            if (post == null)
            {
                response.responseCode = ResponseCode.BadRequest;
                response.errorMessage = "No Post Found";
            }
            else
            {
                string TableName = "Post";
                if (CommonMethod.SoftDeleteRecord(post.Id, TableName))
                {
                    response.responseCode = ResponseCode.Success;
                    response.responseMessage = $"Post deleted";
                }
                else
                {
                    response.responseCode = ResponseCode.BadRequest;
                    response.errorMessage = $"Error Occured While deleting Post";

                }
            }

            return response;
        }
        public ResponseVm GetAllPosts(PostSearchVm model)
        {
            ResponseVm response = ResponseVm.Instance;

            var parameters = new DynamicParameters();
            parameters.Add("@SearchText", model.SearchText);
            parameters.Add("@Filter", model.Filter);
            

            List<GetPostsVm> postVms = new List<GetPostsVm>();
            var postData = CommonMethod.ExecuteStoredProcedure(parameters, $"SP_GetPosts");

            if (postData == null || !postData.Any())
            {
                response.responseCode = ResponseCode.Success;
                response.responseMessage = "No Posts found.";
                response.data = postVms;
                return response;
            }

            foreach (var item in postData)
            {
                var postVm = new GetPostsVm
                {
                    Id = item.Id,
                    Title = item.Title,
                    PostType = item.PostType,
                    Downvotes = item.Downvotes,
                    Upvotes = item.Upvotes,
                    ContentUrl = CommonMethod.RetrieveBase64Data(item.ContentUrl),
                    PostTagsVms = CommonMethod.JsonToListClass<PostTagsVm>(item.PostTags),
                    UserName = item.UserName,
                    ProfileImageUrl = item.ProfileImageUrl,
                    AddedDate = item.AddedDate,
                };
                postVms.Add(postVm);
            }

            response.data = postVms;
            response.responseCode = ResponseCode.Success;
            response.responseMessage = $"Showing results for GetPosts";
            return response;
        }

        public ResponseVm GetUserPosts(PostSearchVm model)
        {
            ResponseVm response = ResponseVm.Instance;

            var parameters = new DynamicParameters();
            parameters.Add("@SearchText", model.SearchText);
            parameters.Add("@Filter", model.Filter);
            parameters.Add("@UserId", _tokenService.UserID);


            List<GetPostsVm> postVms = new List<GetPostsVm>();
            var postData = CommonMethod.ExecuteStoredProcedure(parameters, $"SP_GetUserPosts");

            if (postData == null || !postData.Any())
            {
                response.responseCode = ResponseCode.Success;
                response.responseMessage = "No Posts found.";
                response.data = postVms;
                return response;
            }

            foreach (var item in postData)
            {
                var postVm = new GetPostsVm
                {
                    Id = item.Id,
                    Title = item.Title,
                    PostType = item.PostType,
                    Downvotes = item.Downvotes,
                    Upvotes = item.Upvotes,
                    ContentUrl = CommonMethod.RetrieveBase64Data(item.ContentUrl),
                    PostTagsVms = CommonMethod.JsonToListClass<PostTagsVm>(item.PostTags),
                    UserName = item.UserName,
                    ProfileImageUrl = item.ProfileImageUrl,
                    AddedDate = item.AddedDate,
                };
                postVms.Add(postVm);
            }

            response.data = postVms;
            response.responseCode = ResponseCode.Success;
            response.responseMessage = $"Showing results for {model.SearchText}";
            return response;
        }


    }
}
