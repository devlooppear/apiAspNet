using apiAspNet.Data;
using apiAspNet.Models;
using BCrypt.Net;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using DotNetEnv;

namespace apiAspNet.Services
{
    public class UserService : IUserService
    {
        private readonly ApplicationDbContext _context;

        public UserService(ApplicationDbContext context)
        {
            _context = context;
        }

        public IEnumerable<UserDto> GetAllUsers()
        {
            return _context.Users.Select(u => new UserDto
            {
                Id = u.Id,
                Name = u.Name,
                Email = u.Email,
                Address = u.Address,
                Telephone = u.Telephone,
                SecondTelephone = u.SecondTelephone
            }).ToList();
        }

        public UserDto GetUserById(int id)
        {
            var user = _context.Users.FirstOrDefault(u => u.Id == id);
            if (user == null)
            {
                throw new InvalidOperationException($"User with ID {id} not found.");
            }
            return new UserDto
            {
                Id = user.Id,
                Name = user.Name,
                Email = user.Email,
                Address = user.Address,
                Telephone = user.Telephone,
                SecondTelephone = user.SecondTelephone
            };
        }

        public void CreateUser(CreateUserDto userDto)
        {
            string hashedPassword = BCrypt.Net.BCrypt.HashPassword(userDto.Password);

            var user = new User
            {
                Name = userDto.Name,
                Email = userDto.Email,
                Address = userDto.Address,
                Password = hashedPassword,
                Telephone = userDto.Telephone,
                SecondTelephone = userDto.SecondTelephone
            };

            // Add the user to the context
            _context.Users.Add(user);
            _context.SaveChanges();

            // Create a personal access token for the user
            CreatePersonalAccessToken(user.Id);
        }

        public void UpdateUser(int id, UpdateUserDto userDto)
        {
            var user = _context.Users.FirstOrDefault(u => u.Id == id);
            if (user == null)
            {
                throw new InvalidOperationException($"User with ID {id} not found.");
            }

            if (userDto.Name != null)
            {
                user.Name = userDto.Name;
            }
            if (userDto.Email != null)
            {
                user.Email = userDto.Email;
            }
            if (userDto.Address != null)
            {
                user.Address = userDto.Address;
            }
            if (userDto.Telephone != null)
            {
                user.Telephone = userDto.Telephone;
            }
            if (userDto.SecondTelephone != null)
            {
                user.SecondTelephone = userDto.SecondTelephone;
            }

            _context.SaveChanges();
        }

        public void DeleteUser(int id)
        {
            var user = _context.Users.Find(id);
            if (user != null)
            {
                // Delete associated personal access tokens
                var tokens = _context.PersonalAccessTokens.Where(t => t.UserId == id);
                _context.PersonalAccessTokens.RemoveRange(tokens);

                // Remove the user
                _context.Users.Remove(user);
                _context.SaveChanges();
            }
        }

        private void CreatePersonalAccessToken(int userId)
        {
            // Generate JWT token
            string token = GenerateJwtToken(userId);

            // Save the token in PersonalAccessToken table
            var personalAccessToken = new PersonalAccessToken
            {
                Token = token,
                UserId = userId,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
                IsActive = true
            };

            _context.PersonalAccessTokens.Add(personalAccessToken);
            _context.SaveChanges();
        }

        private string GenerateJwtToken(int userId)
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
