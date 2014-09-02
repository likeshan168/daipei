using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
//using System.Web.Security.AntiXss.CodeCharts


namespace cosen.Models
{
    //public class AccountModel
    //{
    //}
    public class UsersContext : DbContext
    {
        public UsersContext()
            : base("DefaultConnection")
        { 
        }

        public DbSet<UserProfile> UserProfiles { get; set; }
    }

    [Table("UserProfile")]
    public class UserProfile
    {
        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public int UserId { get; set; }
        public string UserName { get; set; }
    }
    public class LoginModel
    { 
        [Required]
        [Display(Name="用户名")]
        public string  UserName { get; set; }

        [Required]
        [Display(Name="密码")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }

    public class AddUserModel
    {
        [Required]
        [Display(Name = "用户名")]
        public string UserName { get; set; }

        [Required]
        [Display(Name = "密码")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required]
        [Display(Name="角色")]
        public string RoleName { get; set; }
    }
}