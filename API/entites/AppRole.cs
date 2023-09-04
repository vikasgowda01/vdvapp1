using Microsoft.AspNetCore.Identity;

namespace API.entites
{
    public class AppRole : IdentityRole<int>
    {
        public ICollection<AppUserRole> UserRoles { get; set; }
    }
}