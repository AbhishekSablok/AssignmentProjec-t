
using Microsoft.AspNetCore.Identity;
using System;

namespace AssignmentProject.Business
{
    public class ApplicationUser : IdentityUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public bool IsActive { get; set; }
        public string Roles { get; set; }
    }
}
