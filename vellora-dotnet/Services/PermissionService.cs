using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Vellora.ECommerce.API.Models;
using Vellora.ECommerce.API.Repositories.IRepositories;
using Vellora.ECommerce.API.Services.IServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Vellora.ECommerce.API.Data;
using Vellora.ECommerce.API.DTOs.Response;
using Vellora.ECommerce.API.DTOs.Request;

namespace Vellora.ECommerce.API.Services
{
  public class PermissionService : IPermissionService
    {
        private readonly ApplicationDbContext _dbContext;

        public PermissionService(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IEnumerable<PermissionResponse>> GetAllPermissionsAsync()
        {
            var permissions = await _dbContext.Permissions.ToListAsync();
            return permissions.Select(p => new PermissionResponse
            {
                Id = p.Id,
                Name = p.Name,
                Description = p.Description
            });
        }

        public async Task<PermissionResponse> GetPermissionByIdAsync(int id)
        {
            var permission = await _dbContext.Permissions.FindAsync(id);
            if (permission == null) return null;

            return new PermissionResponse
            {
                Id = permission.Id,
                Name = permission.Name,
                Description = permission.Description
            };
        }

        public async Task<PermissionResponse> CreatePermissionAsync(PermissionRequest request)
        {
            if (await _dbContext.Permissions.AnyAsync(p => p.Name == request.Name))
                throw new Exception("Permission already exists.");

            var permission = new Permission
            {
                Name = request.Name,
                Description = request.Description
            };

            await _dbContext.Permissions.AddAsync(permission);
            await _dbContext.SaveChangesAsync();

            return new PermissionResponse
            {
                Id = permission.Id,
                Name = permission.Name,
                Description = permission.Description
            };
        }

        public async Task<bool> UpdatePermissionAsync(int id, PermissionRequest request)
        {
            var permission = await _dbContext.Permissions.FindAsync(id);
            if (permission == null) return false;

            permission.Name = request.Name;
            permission.Description = request.Description;

            _dbContext.Permissions.Update(permission);
            await _dbContext.SaveChangesAsync();

            return true;
        }

        public async Task<bool> DeletePermissionAsync(int id)
        {
            var permission = await _dbContext.Permissions.FindAsync(id);
            if (permission == null) return false;

            _dbContext.Permissions.Remove(permission);
            await _dbContext.SaveChangesAsync();

            return true;
        }
    }
}

