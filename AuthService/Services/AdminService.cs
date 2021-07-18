using AuthService.Helpers;
using AuthService.Models;
using AuthService.Models.Dto;
using AuthService.Services.Interfaces;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AuthService.Services
{
    public class AdminService : IAdminService
    {
        private readonly UserManager<User> _userManager;
        private readonly IMapper _mapper;

        public AdminService(UserManager<User> userManager, IMapper mapper)
        {
            _userManager = userManager;
            _mapper = mapper;
        }

        public async Task<IEnumerable<RoleDto>> EditUserRoles(string id, IEnumerable<string> editRolesDtos)
        {
            var selectedRoles = editRolesDtos;

            var user = await _userManager.FindByIdAsync(id);

            var currentUserRoles = await _userManager.GetRolesAsync(user);

            var result = await _userManager.AddToRolesAsync(user, selectedRoles.Except(currentUserRoles));

            if (!result.Succeeded)
                return null;

            result = await _userManager.RemoveFromRolesAsync(user, currentUserRoles.Except(selectedRoles));

            if (!result.Succeeded)
                return null;

            return _mapper.Map<IEnumerable<RoleDto>>(await _userManager.GetRolesAsync(user));
        }

        public async Task<PagedList<UserDto>> GetUsersWithRoles(UserParamsDto userParams)
        {
            //pagination
             var query = _userManager.Users.
                 OrderBy(u => u.UserName).
                 Include(r => r.UserRoles).
                 ThenInclude(u => u.Role).
                 Select(u => new UserDto
                 {
                     ConfirmedAccount = u.EmailConfirmed,
                     Email = u.Email,
                     Id = u.Id,
                     Roles = u.UserRoles.Select(r => r.Role.Name).ToList()
                 }).AsNoTracking();

            //pagination+filtering
            /* var query = _userManager.Users.
                 OrderBy(u => u.UserName).
                 Include(r => r.UserRoles).
                 ThenInclude(u => u.Role).
                 Select(u => new UserDto
                 {
                     ConfirmedAccount = u.EmailConfirmed,
                     Email = u.Email,
                     Id = u.Id,
                     Roles = u.UserRoles.Select(r => r.Role.Name).ToList()
                 }).AsNoTracking().AsQueryable();*/

            return await PagedList<UserDto>.CreateAsync(query, userParams.PageNumber, userParams.PageSize);
        }
    }
}
