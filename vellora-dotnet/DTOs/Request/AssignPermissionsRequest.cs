using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Vellora.ECommerce.API.DTOs.Request
{
    public class AssignPermissionsRequest
    {
        public List<int> PermissionIds { get; set; }
    }
}