using Microsoft.AspNetCore.Identity;

namespace ListaTareas.Models.Users
{
    public class ManageUserModel
    {
        public IdentityUser[]? Adminstradors { get; set; }
        public IdentityUser[]? Everyone { get; set; }

    }
}
