using apiAspNet.Models;
using Bogus;
using DotNetEnv;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace apiAspNet.Data.Seeders
{
    public static class UserSeeder
    {
        public static void SeedUsers(ApplicationDbContext context)
        {
            // Check if there are already users in the database
            if (context.Users.Any())
            {
                return;
            }

            // Generate fake user data using Faker
            var faker = new Faker<User>()
                .RuleFor(u => u.Name, f => f.Person.FullName)
                .RuleFor(u => u.Email, f => f.Internet.Email())
                .RuleFor(u => u.Address, f => f.Address.FullAddress())
                .RuleFor(u => u.Password, f => f.Internet.Password())
                .RuleFor(u => u.Telephone, f => f.Phone.PhoneNumber());

            // Generate a list of fake users
            var users = faker.Generate(50);

            // Add users to the context and save changes
            context.Users.AddRange(users);
            context.SaveChanges();

            // Create personal access tokens for each user
            foreach (var user in users)
            {
                // Generate a token for the user
                string token = GenerateJwtToken(user.Id);

                // Create PersonalAccessToken entity
                var personalAccessToken = new PersonalAccessToken
                {
                    Token = token,
                    UserId = user.Id,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow,
                    IsActive = true
                };

                // Add PersonalAccessToken to the context and save changes
                context.PersonalAccessTokens.Add(personalAccessToken);
                context.SaveChanges();
            }
        }

        private static string GenerateJwtToken(int userId)
        {
            // Load environment variables from .env file
            DotNetEnv.Env.Load();

            // Define the security key using the value from the .env file
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Env.GetString("JWT_SECRET")));

            // Create signing credentials using the security key
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            // Define claims for the JWT token
            var claims = new[]
            {
        new Claim(ClaimTypes.NameIdentifier, userId.ToString())
    };

            // Define token options with no expiration
            var tokenOptions = new JwtSecurityToken(
                issuer: Env.GetString("JWT_ISSUER"),
                audience: Env.GetString("JWT_AUDIENCE"),
                claims: claims,
                expires: DateTime.MaxValue,
                signingCredentials: credentials
            );

            // Generate JWT token
            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.WriteToken(tokenOptions);

            return token;
        }
    }
}
