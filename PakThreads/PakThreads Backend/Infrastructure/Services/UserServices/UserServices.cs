using Application.Interfaces.User;
using Application.ViewModels.CommonVm;
using Application.ViewModels.ResponseModel;
using Application.ViewModels.UserVm;
using CommonOperations.CommonMethods;
using CommonOperations.Enums;
using Dapper;
using Domain.Models.Entities.User;
using Infrastructure.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Services.UserServices
{
    public class UserServices : IUser
    {
        private readonly IConfiguration _config;
        private readonly AppDbContext _context;
        public UserServices(IConfiguration config, AppDbContext context)
        {
            _config = config;
            _context = context;
        }

        public ResponseVm CreateUser(UserVm user)
        {
            ResponseVm response = ResponseVm.Instance;

            if (user.Id == 0) // Create new user
            {
                var existingUser = _context.User
                    .FirstOrDefault(x => (x.UserEmail == user.Email || x.UserName == user.UserName) && !x.IsDeleted);

                if (existingUser != null)
                {
                    response.responseCode = ResponseCode.BadRequest;
                    response.errorMessage = existingUser.UserEmail == user.Email
                        ? "User Already Exists with this Email"
                        : "Username Already Exists";
                    return response;
                }

                var userModel = new User
                {
                    Password = CommonMethod.EncrypthePassword(user.Password),
                    AddedBy = user.Email,
                    AddedDate = DateTime.UtcNow,
                    UpdatedDate = DateTime.UtcNow,
                    UserEmail = user.Email,
                    UserName = user.UserName,
                    IsSiteUser = user.IsSiteUser,
                    UserType = "User",

                };

               
                _context.User.Add(userModel);
                _context.SaveChanges();
                

                response.responseCode = ResponseCode.Success;
                response.responseMessage = "Account created successfully";
                return response;

            }
            else // Update existing user
            {
                var existingUser = _context.User.FirstOrDefault(u => u.Id == user.Id && !u.IsDeleted);
                if (existingUser == null)
                {
                    response.responseCode = ResponseCode.BadRequest;
                    response.errorMessage = "No User Exists";
                    return response;
                }

                var conflictingUser = _context.User
                    .FirstOrDefault(x => (x.UserEmail == user.Email || x.UserName == user.UserName) && x.Id != user.Id && x.IsSiteUser == user.IsSiteUser);

                if (conflictingUser != null)
                {
                    response.responseCode = ResponseCode.BadRequest;
                    response.errorMessage = conflictingUser.UserEmail == user.Email
                        ? "User Already Exists with this Email"
                        : "Username Already Exists";
                    return response;
                }

                /*if (existingUser.Password != CommonMethod.EncrypthePassword(user.Password))
                {
                    response.responseCode = ResponseCode.BadRequest;
                    response.responseMessage = "Incorrect Password";
                    return response;
                }*/
                string imagePath = Path.Combine("D:\\repos\\PakthreadsData", user.UserName, $"{user.Email}{DateTime.Now.Ticks}");
                CommonMethod.UploadBase64Data(user.ProfileImageUrl, imagePath);
                existingUser.UserName = user.UserName;
               // existingUser.UpdatedBy = TokenVm.UserName;
                existingUser.UpdatedDate = DateTime.UtcNow;
                existingUser.UserEmail = user.Email;
                existingUser.IsSiteUser = user.IsSiteUser;
                existingUser.ProfileImageUrl = imagePath;
                existingUser.UserType = user.UserType;
                existingUser.Gender = user.Gender;
                existingUser.PhoneNumber = user.PhoneNumber;
                existingUser.DateOfBirth = user.DateOfBirth;


                //CommonMethod.RemoveFile(existingUser.ProfileImageUrl);

                _context.User.Update(existingUser);
                _context.SaveChanges();

                response.responseCode = ResponseCode.Success;
                response.responseMessage = "User Updated Successfully";
            }

            return response;
        }
        public ResponseVm GetUsers(UserSearchVm model)
        {
            ResponseVm response = ResponseVm.Instance;

            var parameters = new DynamicParameters();
            parameters.Add("@SearchText", model.SearchText);
            parameters.Add("@Gender", model.Gender);
            parameters.Add("@UserType", model.UserType);
            parameters.Add("@PageNumber", model.PageNumber);
            parameters.Add("@PageSize", model.PageSize);

            // Retrieve the user data directly as a list of GetUserVm objects
            var userVms = CommonMethod.ExecuteStoredProcedureAndMaptoModel<GetUserVm>($"SP_Get{model.GetName}Users", parameters);

            if (userVms == null || !userVms.Any())
            {
                response.responseCode = ResponseCode.Success;
                response.responseMessage = "No users found.";
                response.data = new List<GetUserVm>(); 
                return response;
            }

            var userList = new List<GetUserVm>();

            foreach (var user in userVms)
            {
                if (!string.IsNullOrEmpty(user.ProfileImageUrl)) // Ensure ImagePath is not null
                {
                    user.ProfileImageUrl = CommonMethod.RetrieveBase64Data(user.ProfileImageUrl);
                }
                userList.Add(user);
            }

            // Return the response with user data
            response.data = userList;
            response.responseCode = ResponseCode.Success;
            response.responseMessage = $"Showing results for {model.SearchText}";
            return response;
        }
        public ResponseVm DeleteUser(long Id)
        {
            ResponseVm response = ResponseVm.Instance;

            // Check if the user exists and is not already deleted
            var user = _context.User.FirstOrDefault(a => a.Id == Id && !a.IsDeleted);

            if (user == null)
            {
                response.responseCode = ResponseCode.BadRequest;
                response.errorMessage = "No active user found";
                return response;  // Return early if user is not found or is deleted
            }

            try
            {
                // Soft delete the user (mark as deleted)
                user.IsDeleted = true;
                _context.User.Update(user);
                _context.SaveChanges();

                response.responseCode = ResponseCode.Success;
                response.responseMessage = "User deleted successfully";
            }
            catch (Exception ex)
            {
                response.responseCode = ResponseCode.InternalServerError;
                response.responseMessage = $"Unable to delete user: {ex.Message}";
            }

            return response;
        }
        public ResponseVm BlockUser(DeclineVm model)
        {
            ResponseVm response = ResponseVm.Instance;

            var user = _context.User.FirstOrDefault(a => a.Id == model.Id && !a.IsDeleted);
            if (user == null)
            {
                response.responseCode = ResponseCode.BadRequest;
                response.errorMessage = "No active user found with the provided ID.";
                return response;
            }
            user.IsBlocked = true;
            user.BlockedReason = model.Reason;

            _context.User.Update(user);
            _context.SaveChanges();

            response.responseCode = ResponseCode.Success;
            response.responseMessage = "User successfully blocked.";
            return response;
        }
        public ResponseVm UnBlockUser(long Id)
        {
            ResponseVm response = ResponseVm.Instance;

            // Check if the user exists and is not deleted
            var user = _context.User.FirstOrDefault(a => a.Id == Id && !a.IsDeleted);

            if (user == null)
            {
                response.responseCode = ResponseCode.BadRequest;
                response.errorMessage = "No active user found";
                return response;  // Return early if user is not found or is deleted
            }

            // Unblock the user
            user.IsBlocked = false;
            user.BlockedReason = "";

            try
            {
                // Update the user synchronously and save changes
                _context.User.Update(user);
                _context.SaveChanges();

                response.responseCode = ResponseCode.Success;
                response.responseMessage = "User UnBlocked";
            }
            catch (Exception ex)
            {
                response.responseCode = ResponseCode.BadRequest;
                response.responseMessage = $"Error unblocking user: {ex.Message}";
            }

            return response;
        }
        public ResponseVm GetUserCount()
        {
            ResponseVm response = ResponseVm.Instance;

            var result = CommonMethod.ExecuteStoredProcedure(null, "SP_GetUserCounts");

            if (result == null || !result.Any())
            {
                response.data = null;
                response.responseCode = ResponseCode.Success;
                response.responseMessage = "No data found.";
                return response;
            }

            var record = result.FirstOrDefault();

            response.data = new
            {
                totalUsers = record.TotalUsers,
                totalActiveUsers = record.TotalActiveUsers,
                totalBlockedUsers = record.TotalBlockedUsers,
                lastMonth = new
                {
                    usersAdded = record.UsersAddedLastMonth,
                    activeUsers = record.ActiveUsersLastMonth,
                    blockedUsers = record.BlockedUsersLastMonth
                }
            };

            response.responseCode = ResponseCode.Success;
            response.responseMessage = "Total Users Count";
            return response;
        }
        public ResponseVm SaveAndUpdateUserDetail(UserDataVm user)
        {
            ResponseVm response = ResponseVm.Instance;
            //long userId = TokenVm.UserID;
            long? userId = _context.User.FirstOrDefault(a => a.Id == user.Id)?.Id;
            //long userId = 1;
            if (userId <= 0)
            {
                response.responseCode = ResponseCode.BadRequest;
                response.responseMessage = "Invalid User ID!";
                return response;
            }

            try
            {
                var existingUser = _context.User.FirstOrDefault(x => x.Id == userId);

                if (existingUser == null)
                {
                    // Add a new user
                    var newUser = new User
                    {
                        FirstName = user.FirstName,
                        LastName = user.LastName,
                        Gender = user.Gender,
                        PhoneNumber = user.PhoneNumber,
                        DateOfBirth = user.DateOfBirth,
                        UserBio = user.UserBio,
                        Country = user.Country,
                       // AddedBy = TokenVm.UserEmail,
                        AddedDate = DateTime.Now
                    };
                    _context.User.Add(newUser);
                }
                else
                {
                    // Update existing user
                    existingUser.FirstName = user.FirstName;
                    existingUser.LastName = user.LastName;
                    existingUser.Gender = user.Gender;
                    existingUser.PhoneNumber = user.PhoneNumber;
                    existingUser.DateOfBirth = user.DateOfBirth;
                    existingUser.UserBio = user.UserBio;
                    existingUser.Country = user.Country;
                    //existingUser.UpdatedBy = TokenVm.UserEmail;
                    existingUser.UpdatedDate = DateTime.Now;
                }

                _context.SaveChanges();
                response.responseCode = ResponseCode.Success;
                response.responseMessage = "User Data Saved Successfully";
            }
            catch (Exception ex)
            {
                response.responseCode = ResponseCode.InternalServerError;
                response.responseMessage = $"An error occurred: {ex.Message}";
            }

            return response;
        }
        public ResponseVm SaveAndUpdateUserImage(UserImageVm model)
        {
            ResponseVm response = ResponseVm.Instance;

            try
            {
                // Retrieve the user and check if they are deleted
                var user = _context.User.FirstOrDefault(x => x.Id == model.UserId && !x.IsDeleted);

                // Validate user existence and check if the user is deleted
                if (user == null)
                {
                    response.responseCode = ResponseCode.BadRequest;
                    response.responseMessage = "User not found or has been deleted.";
                    return response;
                }

                // Check if the image is provided
                if (string.IsNullOrEmpty(model.ImageBase64))
                {
                    response.responseCode = ResponseCode.BadRequest;
                    response.responseMessage = "Please Provide Image";
                    return response;
                }

                // Save the image  we need to upload on wasabi 
                string imagePath = Path.Combine("C:\\ATGData\\AdmissiomLelo\\ApplicationUser", user.UserName, $"{user.UserEmail}{DateTime.Now.Ticks}");
                CommonMethod.UploadBase64Data(model.ImageBase64, imagePath);

                // Update user profile image URL
                user.ProfileImageUrl = imagePath;
                _context.User.Update(user);
                _context.SaveChanges();

                response.responseCode = ResponseCode.Success;
                response.responseMessage = " Image Saved Successfully!";
                return response;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception: {ex}");
                response.responseCode = ResponseCode.InternalServerError;
                response.responseMessage = "An error occurred while processing your request.";
                return response;
            }
        }
        public ResponseVm LoginUser(LoginUserVm model)
        {
            ResponseVm response = ResponseVm.Instance;

            var user = _context.User
                .FirstOrDefault(x => (x.UserEmail == model.Email || x.UserName == model.Email) && !x.IsDeleted);

            if (user == null)
            {
                response.responseCode = ResponseCode.BadRequest;
                response.responseMessage = "There is no user with this Email or Username, or the account is deleted.";
                return response;
            }


            //// Check if the user email is verified
            //if (!user.IsEmailVerified && user.IsSiteUser)
            //{
            //    response.responseCode = ResponseCode.BadRequest;
            //    response.responseMessage = "Please verify your Email first.";
            //    return response;
            //}


            // Check if the user is blocked
            if (user.IsBlocked)
            {
                response.responseCode = ResponseCode.BadRequest;
                response.responseMessage = "Account is Blocked";
                return response;
            }

            // Validate the password
            if (user.Password != CommonMethod.EncrypthePassword(model.Password))
            {
                response.responseCode = ResponseCode.BadRequest;
                response.responseMessage = "Invalid password.";
                return response;
            }


            // Prepare the response data on successful login
            var loginData = new LoginVm
            {
                Token = CommonMethod.GenerateJwtToken(user.UserEmail, user.Id, user.UserName, _config),
                Email = user.UserEmail,
                UserName = user.UserName,
                FirstName = user.FirstName,
                LastName = user.LastName,
                UserType = user.UserType,
                ProfileBase64 = CommonMethod.RetrieveBase64Data(user.ProfileImageUrl)
            };

            response.data = loginData;
            response.responseCode = ResponseCode.Success;
            response.responseMessage = "Login successful.";

            return response;
        }
    }
}
