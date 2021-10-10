using Identity.Data.Interfaces;
using Identity.Domain.Models;
using Identity.Service.ResourceParameters;
using Service.Common.Exceptions;
using Service.Common.Paging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Identity.Service
{
    public interface IUserService
    {
        Task<int> CountAsync(Expression<Func<User, bool>> predicate);
        Task<PagedList<User>> GetUsers(UserResourceParameters userResourceParameters, IEnumerable<int> currencies = null);
        Task<User> AddUser(User user, string password);
        Task<User> GetUserById(int id);
        Task<User> GetUserByEmail(string email);
        Task<User> UpdateUser(User user);
        Task<bool> DeleteUser(int id);
        Task<User> Authenticate(string email, string password);
    }
    public class UserService : IUserService
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IGenericRepository<User> userRepository;
        public UserService(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
            userRepository = unitOfWork.GetRepository<User>();
        }
        public async Task<User> AddUser(User user, string password)
        {
            var userExists = await userRepository.FindByAsync(a => a.Email == user.Email);
            if (userExists.Any())
                throw new ApplicationException("Email is arelady taken");

            byte[] passwordHash, passwordSalt;
            CreatePasswordHash(password, out passwordHash, out passwordSalt);

            user.PasswordHash = passwordHash;
            user.PasswordSalt = passwordSalt;

            await userRepository.AddAsync(user);
            await unitOfWork.SaveChangesAsync();
            return user;
        }

        public async Task<int> CountAsync(Expression<Func<User, bool>> predicate)
        {
            return await userRepository.CountAsync(predicate);
        }

        public async Task<bool> DeleteUser(int id)
        {
            var user = await userRepository.GetSingleAsync(id);
            userRepository.Delete(user);
            return await unitOfWork.SaveChangesAsync();
        }

        public async Task<PagedList<User>> GetUsers(UserResourceParameters userResourceParameters, IEnumerable<int> users = null)
        {
            var collection = userRepository.Query();

            if (!string.IsNullOrWhiteSpace(userResourceParameters.SearchQuery))
            {
                var searchQuery = userResourceParameters.SearchQuery.Trim();
                collection = collection.Where(a => a.Email.Contains(searchQuery));
            }

            return await PagedList<User>.ToPagedListAsync(
                collection.Where(a => users == null || users.Contains(a.UserId)).OrderBy(on => on.Email),
                userResourceParameters.PageNumber,
                userResourceParameters.PageSize);
        }

        public async Task<User> GetUserById(int id)
        {
            return await userRepository.GetSingleAsync(id);
        }

        public async Task<User> GetUserByEmail(string email)
        {
            return await userRepository.GetSingleAsync(a => a.Email == email);
        }

        public async Task<User> UpdateUser(User user)
        {
            userRepository.Update(user);
            await unitOfWork.SaveChangesAsync();
            return user;
        }

        public async Task<User> Authenticate(string email, string password)
        {
            var user = await userRepository.GetSingleAsync(u => u.Email == email);
            if (user == null)
                throw new NotFoundException("User not found");

            if (!VerifyPasswordHash(password, user.PasswordHash, user.PasswordSalt))
                throw new ForbiddenException("email or password is incorrect.");

            return user;
        }

        private static void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            if (password == null) throw new BadRequestException("password");
            if (string.IsNullOrWhiteSpace(password)) throw new BadRequestException("password: Value cannot be empty or whitespace only string.");

            using (var hmac = new System.Security.Cryptography.HMACSHA512())
            {
                passwordSalt = hmac.Key;
                passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            }
        }

        private static bool VerifyPasswordHash(string password, byte[] storedHash, byte[] storedSalt)
        {
            if (password == null) throw new BadRequestException("password");
            if (string.IsNullOrWhiteSpace(password)) throw new BadRequestException("password: Value cannot be empty or whitespace only string.");
            if (storedHash.Length != 64) throw new BadRequestException("passwordHash: Invalid length of password hash (64 bytes expected).");
            if (storedSalt.Length != 128) throw new BadRequestException("passwordHash: Invalid length of password salt (128 bytes expected).");

            using (var hmac = new System.Security.Cryptography.HMACSHA512(storedSalt))
            {
                var computedHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
                for (int i = 0; i < computedHash.Length; i++)
                {
                    if (computedHash[i] != storedHash[i]) return false;
                }
            }

            return true;
        }
    }
}
