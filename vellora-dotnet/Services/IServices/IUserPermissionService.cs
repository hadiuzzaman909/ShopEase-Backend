using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Vellora.ECommerce.API.Services.IServices
{
    public interface IUserPermissionService
    {
          Task<bool> UserHasPermissionAsync(string userId, string permissionName);
    }
}