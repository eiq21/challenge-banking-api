using System;
using System.Collections.Generic;
using System.Text;

namespace Identity.Domain.Models
{
    public class User
    {
        public User()
        {
            IsActive = true;
            CreatedAt = DateTime.Now;

        }
        public int UserId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public byte[] PasswordHash { get; set; }
        public byte[] PasswordSalt { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public bool? IsActive { get; set; }

    }
}
