
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestAuth.Models;

namespace TestAuth.Data
{
    public class UserCredentialsRepository : IUserCredentialsRepository
    {
        private readonly UserCredentialsContext _context;
        private readonly ILogger<UserCredentialsRepository> _logger;

        public UserCredentialsRepository(UserCredentialsContext context, ILogger<UserCredentialsRepository> logger)
        {
            _context = context;
            _logger = logger;
        }


        public void Add<T>(T entity) where T : class
        {
            _logger.LogInformation($"Adding an object of type {entity.GetType()} to the context.");
            _context.Add(entity);
        }

        public void Delete<T>(T entity) where T : class
        {
            _logger.LogInformation($"Removing an object of type {entity.GetType()} to the context.");
            _context.Remove(entity);
        }


        public async Task<UserCredentials[]> GetAllCredentialsAsync()
        {
            
            IQueryable<UserCredentials> query = _context.UserCredentials
                .Include(c => c.Id);
            // Order It
            //query = query.OrderByDescending(c => c.EventDate);
            return await query.ToArrayAsync();
        }
        public async Task<UserCredentials> GetCredentialAsync(string username)
        {
            IQueryable<UserCredentials> queryForUsername = _context.UserCredentials
                .Where(credential => credential.UserName == username);
            return await queryForUsername.FirstAsync();
        }

        public Task<bool> SaveChangesAsync()
        {
            throw new NotImplementedException();
        }
    }
}
