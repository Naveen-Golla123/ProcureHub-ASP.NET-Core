
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using ProcureHub_ASP.NET_Core.Models;
using ProcureHub_ASP.NET_Core.Respositories.Interfaces;
using System.Collections;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace ProcureHub_ASP.NET_Core.Services
{
    public class AuthenticationService: IAuthenticationService
    {

        private readonly IAuthenticationRepository _authenticationRepository;
        private readonly IConfiguration _configuration;
        public AuthenticationService(IAuthenticationRepository authenticationRepository, IConfiguration configuration) 
        {
            _configuration = configuration;
            _authenticationRepository = authenticationRepository;
        }

        public async Task<bool> IsEmailAvailable(string email)
        {
            return await _authenticationRepository.IsEmailAvailable(email);
        }

        public async Task<bool> RegisterUser(UserDetails registerDTO)
        {
            PasswordHasher(registerDTO.password, out byte[] hashedPassword, out byte[] salt);
            //userDetails.hashedPassword = hashedPassword;
            //verifyPassword(userDetails.password,);
            UserDetails userDetails = new UserDetails()
            {
                name = registerDTO.name,
                email = registerDTO.email,
                password = string.Empty,
                isBuyer = registerDTO.isBuyer,
                isAdmin = registerDTO.isAdmin,
                isApproved = registerDTO.isApproved,
                mobile = registerDTO.mobile,
                hashedPassword = hashedPassword,
                hashSalt = salt
            };
            return await _authenticationRepository.RegisterUser(userDetails);
        }

        private void PasswordHasher(string password, out byte[] hashedPassword, out byte[] passwordSalt)
        {
            using (var hmca = new HMACSHA512())
            {
                passwordSalt = hmca.Key;
                hashedPassword = hmca.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            }
            
        }

        public bool verifyPassword(string password, byte[] hashedPassword, byte[] salt)
        {
            byte[] newHash;
            using(var hmca = new HMACSHA512(salt))
            {
                newHash = hmca.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            }

            if(hashedPassword.SequenceEqual(newHash))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public async Task<Object> SignIn(string userName, string password)
        {
            if(!await _authenticationRepository.IsEmailAvailable(userName))
            {
                return new { status = false, errorMsg = "Email doest not exists" };
            }
            else
            {
                UserDetails userDetails = await _authenticationRepository.GetUserDetails(userName);
                if (userDetails != null)
                {
                    if(verifyPassword(password, userDetails.hashedPassword, userDetails.hashSalt))
                    {
                        return new { status = true , token = GenerateToken(userDetails) };
                    }
                    else
                    {
                        return new { status = false, errorMsg = "Incorrect Password" };
                    }
                }else
                {
                    return new { status = false, errorMsg = "Use doesnt exists" };
                }
            }
        }

        private string GenerateToken(UserDetails userDetails)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[] {
                    new Claim("partnerCode", "1000"),
                    new Claim("email", userDetails.email.ToString()),
                    new Claim("name", userDetails.name.ToString()),
                    new Claim("isSupplier", userDetails.isApproved.ToString()),
                    new Claim("isAdmin", userDetails.isBuyer.ToString())
                }),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }
}


