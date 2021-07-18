using AuthService.Helpers;
using AuthService.Models.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AuthService.Services.Interfaces
{
    public interface IAdminService
    {
        public Task<PagedList<UserDto>> GetUsersWithRoles(UserParamsDto userParams);
        public Task<IEnumerable<RoleDto>> EditUserRoles(string id, IEnumerable<string> editRolesDtos);
    }
}
