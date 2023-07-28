using Microsoft.EntityFrameworkCore;
using UserManagement.Data.Common;
using UserManagement.Model;

namespace UserManagement.Data.Repositories
{
    public interface IUserRepository : IRepository<User>
    {

    }
    public class UserRepository : GenericRepository<User>, IUserRepository
    {
        public UserRepository(DbContext context, ILogger logger, User user) : base(context, logger, user)
        {
        }
    }
}