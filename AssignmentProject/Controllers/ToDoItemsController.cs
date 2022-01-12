using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using AssignmentProject.Authentication;
using AssignmentProject.Business;
using Microsoft.AspNetCore.Authorization;
using AssignmentProject.Model.Paging;
using AssignmentProject.Model.Response;
using AssignmentProject.Model;

namespace AssignmentProject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ToDoItemsController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly RoleManager<IdentityRole> roleManager;
        private readonly IConfiguration _configuration;

        public ToDoItemsController(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager, IConfiguration configuration)
        {
            this.userManager = userManager;
            this.roleManager = roleManager;
            _configuration = configuration;

        }
        /// <summary>
        /// Create methos is ued to create a new entry of the user
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPut]
        [Authorize(Roles = "Admin")]
        [Route("[action]", Name = "Create")]
        public async Task<IActionResult> Create([FromBody] RegisterModel model)
        {
            try
            {
                var userExists = await userManager.FindByNameAsync(model.UserName);//Find whether the user is available or not
                if (userExists != null)//if user not available it will show error message
                    return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "Error", Message = "User already exists!" });

                ApplicationUser user = new()//Take the value from the form 
                {
                    SecurityStamp = Guid.NewGuid().ToString(),
                    UserName = model.UserName,
                    FirstName = model.FirstName,
                    LastName = model.LastName,
                    IsActive = model.IsActive,
                    Email = model.Email,
                    Roles = model.Role
                };
                var result = await userManager.CreateAsync(user, model.Password);//Insert the user data into the db.
                if (!result.Succeeded)//if record inserted then success message will show otherwise error message will show
                    return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "Error", Message = "User creation failed! Please check user details and try again." });

                return Ok(new Response { Status = "Success", Message = "User created successfully!" });

            }


            catch (Exception ex)
            {
                return BadRequest(new
                {
                    Status = AssignmentProject.Model.Constants.Failure,
                    StatusMessage = ex.Message,
                    HttpStatus = System.Net.HttpStatusCode.BadRequest,
                    Data = ""
                });
            }


        }
        /// <summary>
        /// UpdateUserDetails method is use dto update the user details only admin can update the user details
        /// </summary>
        /// <param name="model"></param>
        /// <param name="userName"></param>
        /// <returns></returns>
        [HttpPut]
        [Authorize(Roles = "Admin")]
        [Route("[action]", Name = "UpdateUserDetails")]
        public async Task<IActionResult> UpdateUserDetails([FromBody] RegisterModel model, string userName)
        {
            try
            {
                var user = await userManager.FindByNameAsync(userName);//find the user

                //user.UserName = model.UserName;
                user.FirstName = model.FirstName;
                user.LastName = model.LastName;
                user.Email = model.Email;
                user.IsActive = model.IsActive;
                user.Roles = model.Role;

                await userManager.UpdateAsync(user);

                return Ok(new Response { Status = "Success", Message = "User details updated successfully!" });
            }
            catch (Exception ex)
            {
                return BadRequest(new
                {
                    Status = AssignmentProject.Model.Constants.Failure,
                    StatusMessage = ex.Message,
                    HttpStatus = System.Net.HttpStatusCode.BadRequest,
                    Data = ""
                });
            }
        }

        /// <summary>
        /// DeleteUser mthod is used to delete the user 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [Authorize(Roles = "Admin")]
        [HttpDelete]
        [Route("[action]", Name = "DeleteUser")]
        public async Task<IActionResult> DeleteUser(string id)
        {
            try
            {
                var user = await userManager.FindByIdAsync(id);//find the user based on ID
                if (user == null)//if user is available then procedd further otherwise it will show an error message
                {
                    return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "Error", Message = "User not found!" });
                }
                else
                {
                    var result = await userManager.DeleteAsync(user);
                }
                return Ok(new Response { Status = "Success", Message = "User delete successfully!" });
            }
            catch (Exception ex)
            {
                return BadRequest(new
                {
                    Status = AssignmentProject.Model.Constants.Failure,
                    StatusMessage = ex.Message,
                    HttpStatus = System.Net.HttpStatusCode.BadRequest,
                    Data = ""
                });
            }
        }

        /// <summary>
        /// GetAllUser is used to dipslay all users details
        /// </summary>
        /// <param name="pagingParameters"></param>
        /// <returns></returns>


        [HttpGet]
        [Route("[action]", Name = "GetAllUser")]
        public async Task<IActionResult> GetAllUser([FromQuery] PagingParameters pagingParameters)
        {
            try
            {
                var user = userManager.Users;
                var response = new PagedApiResponse
                {
                    Status = Constants.Success,
                    StatusMessage = "Success while getting pallets data.",
                    HttpStatus = System.Net.HttpStatusCode.OK,
                    Paging = new PagingData
                    {
                        CurrentPage = pagingParameters.PageNumber,
                        PageSize = pagingParameters.PageSize,
                        ItemsCount = user.AsQueryable().Count()
                    },
                    Data = user.AsQueryable()
                };
                return Ok(response);
            }
            catch (Exception ex)
            {
                return BadRequest(new
                {
                    Status = AssignmentProject.Model.Constants.Failure,
                    StatusMessage = ex.Message,
                    HttpStatus = System.Net.HttpStatusCode.BadRequest,
                    Data = ""
                });
            }
        }
        /// <summary>
        /// GetUserByaAme is used to dispaly a particular user details
        /// </summary>
        /// <param name="pagingParameters"></param>
        /// <param name="Username"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("[action]", Name = "GetUserByaAme")]
        public async Task<IActionResult> GetUserByaAme([FromQuery] PagingParameters pagingParameters, string Username)
        {
            try
            {
                var user = userManager.Users.Where(x => x.UserName == Username);
                var response = new PagedApiResponse
                {
                    Status = Constants.Success,
                    StatusMessage = "Success while getting pallets data.",
                    HttpStatus = System.Net.HttpStatusCode.OK,
                    Paging = new PagingData
                    {
                        CurrentPage = pagingParameters.PageNumber,
                        PageSize = pagingParameters.PageSize,
                        ItemsCount = user.AsQueryable().Count()
                    },
                    Data = user.AsQueryable()
                };
                return Ok(response);
            }
            catch (Exception ex)
            {
                return BadRequest(new
                {
                    Status = AssignmentProject.Model.Constants.Failure,
                    StatusMessage = ex.Message,
                    HttpStatus = System.Net.HttpStatusCode.BadRequest,
                    Data = ""
                });
            }
        }
    }
}
