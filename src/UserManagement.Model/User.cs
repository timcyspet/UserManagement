using UserManagement.Model.Interface;

namespace UserManagement.Model
{
    public abstract class BaseObject :IBaseObject 
    {
        public long Id { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime UpdatedOn { get; set;}
        public bool IsDeleted { get; set; }
        public bool IsActive { get; set; }

    }
    public class User :BaseObject
    {
        public string UserName { get ; set ; }
        public string FirstName { get; set ; }
        public string LastName { get; set; }
        public string  email { get; set; }
        public List<Role> Roles { get; set; }
        public List<UserGroup> UserGroups { get; set; }


    }

    public class Role : BaseObject
    {
        public string RoleName { get; set;}

        public string RoleCd { get; set;}

    }

    public class UserGroup : BaseObject
    {
        public string Name { get; set; }
        public string GroupCd { get; set; }

    }

    public class Policy : BaseObject
    {
        public string Name { get; set; }
        public string PolicyCd { get; set; }
        public string Description { get; set; }
    }

    public class PolicyRules : BaseObject
    {
        public string Name { get; set; }
        public string Rule { get; set; }
        public string Description { get; set; }
        public string Condition { get; set; }
        public Policy Policy { get; set; }

    }

    public class PolicyRoleMapping : BaseObject
    {
        public Role Role { get; set; }
        public Policy Policy { get; set; }
    }




}