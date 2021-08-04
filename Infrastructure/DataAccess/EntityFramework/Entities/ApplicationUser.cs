using Domain.Entities.Users;
using Microsoft.AspNetCore.Identity;
using System;

namespace Infrastructure.DataAccess.EntityFramework.Entities
{
    public class ApplicationUser : IdentityUser<Guid>
    {
        // Properties
        public Guid UserId { get; private set; }
        public User User { get; private set; }

        // Constructors
        public ApplicationUser(User user)
            : base(user.UniqueMasterCitizenNumber)
        {
            UserId = user.Id;
            User = user;
        }

        private ApplicationUser() { }

        // Helper Methods
        public static string GenerateRandomPin(int length)
        {
            var random = new Random();
            int maxValue= (int)Math.Pow(10, length);
            string pin = random
                .Next(0, maxValue)
                .ToString("D6");
            return pin;
        }
    }
}
