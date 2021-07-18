using AuthService.Extensions;
using AuthService.Models.Dto;
using AuthService.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AuthService.Controllers
{
    public class AdminController : ControllerBase
    {
        private readonly IAdminService _adminService;

        public AdminController(IAdminService adminService)
        {
            _adminService = adminService;
        }

        //[Authorize(Policy = "RequireAdminRole")]
        [HttpGet("users-with-roles")]
        public async Task<IActionResult> GetUserWithRoles([FromQuery]UserParamsDto userParams)
        {
            try
            {
                var result = await _adminService.GetUsersWithRoles(userParams);

                Response.AddPaginationHeader(result.CurrentPage, result.PageSize, result.TotalCount, result.TotalPages);

                return Ok(result);

            }catch(Exception e)
            {
                Console.WriteLine(e);
                return BadRequest("Something went wrong");
            }
        }


        [HttpPost("edit-roles/{id}")]
        public async Task<ActionResult> GetPhotosFroModeration(string id, [FromQuery] IEnumerable<string> roles)
        {
            try
            {
                var result = await _adminService.EditUserRoles(id, roles);

                if (result is null)
                    return BadRequest("Something went wrong");

                return Ok(result);
            }catch(Exception e)
            {
                Console.WriteLine(e);
                return BadRequest("Something went wrong");
            }
        }
    }
}
