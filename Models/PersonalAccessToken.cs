using System;

namespace apiAspNet.Models
{
    public class PersonalAccessToken
    {
        public int Id { get; set; }
        public required string Token { get; set; }
        public int UserId { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public bool IsActive { get; set; }

        // Navigation property
        public User ?User { get; set; } 
    }
}
