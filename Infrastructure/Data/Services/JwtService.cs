using AutoMapper;
using ClinicSystem.Dtos;
using Domain.Dtos.Event;
using Domain.Dtos.User;
using Domain.Entities.Events;
using Domain.Entities.Roles;
using Domain.Entities.Users;
using Domain.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Data.Services
{
    public class JwtService: IjwtService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IMapper _mapper;
        private readonly IConfiguration _configuration;
        private readonly AppDbContext _dbContext;
        private readonly IPasswordCryption _passwordCryption;
        private static TokenUserDto tokenUser = new TokenUserDto();

        public JwtService(IHttpContextAccessor httpContextAccessor, IMapper mapper,
            IConfiguration configuration, AppDbContext appDbContext, IPasswordCryption passwordCryption)
        {
            _httpContextAccessor = httpContextAccessor;
            _mapper = mapper;
            _configuration = configuration;
            _dbContext = appDbContext;
            _passwordCryption = passwordCryption;
        }


          
        public TokenViewModel GenerateToken(LoginViewModel login)
        {
            TokenViewModel data = new TokenViewModel();
            var user = _dbContext.Users.Include(x=>x.UserRoles).ThenInclude(x=>x.Role)
                .FirstOrDefault(x=>x.FullName==login.UserName && x.Password==_passwordCryption.EncodePasswordToBase64(login.Password));
            if(user==null)
            {
                return null;
            }
            var tokenHandler = new JwtSecurityTokenHandler();
            var tokenKey = Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
              {
                   new Claim(ClaimTypes.Name, user.FullName),
                   new Claim(ClaimTypes.Sid, user.Id.ToString()),
                   new Claim(ClaimTypes.Role, user.UserRoles.Count!=0?user.UserRoles.FirstOrDefault().Role.Name:"Denied Access")

              }),
                Expires = DateTime.Now.AddMinutes(Convert.ToDouble(_configuration["Jwt:ExpireTokenByMin"])),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(tokenKey), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            RefreshToken refTokent = GenerateRefreshToken();
            SetRefreshToken(refTokent);
            data.token = tokenHandler.WriteToken(token);
            data.expiredDate = DateTime.Now.AddMinutes(Convert.ToDouble(_configuration["Jwt:ExpireTokenByMin"]));
            data.refreshToken = refTokent.Token;
            return data;
        }
        public bool CheckExpiredCookiesRefreshToken()
        {
            string cookiesToken = _httpContextAccessor.HttpContext.Request.Cookies["refreshToken"];
            if (!tokenUser.refreshToken.Equals(cookiesToken) || (tokenUser.tokenExpired < DateTime.Now))
            {
                return false;
            }
            return true;

        }
        public CreateUser CreateUser(CreateUser user)
        {
            user.Password = _passwordCryption.EncodePasswordToBase64(user.Password);
            User input = new User(user);
            _dbContext.Users.Add(input);
            _dbContext.SaveChanges();
            return user;

        }
        public CurrentUserViewModel GrantUserPermission(Guid userid)
        {
            var user = _dbContext.Users.FirstOrDefault(x => x.Id == userid);
            var role = _dbContext.Roles.FirstOrDefault(x=>x.Name=="Normal");
            if(user == null)
            {
                return null;
            }
            UserRole userRole = new UserRole(userid,role.Id);
            CurrentUserViewModel response = new CurrentUserViewModel();
            _dbContext.UserRoles.Add(userRole);
            _dbContext.SaveChanges();
            response.UserId= userid;
            response.UserName = user.FullName;
            response.RoleName = role.Name;
            return response;
        }
        public ClaimsPrincipal GetPrincipalFromExpiredToken(string token)
        {
            var Key = Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]);

            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = false,
                ValidateAudience = false,
                ValidateLifetime = false,
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Key),
                ClockSkew = TimeSpan.Zero
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out SecurityToken securityToken);
            JwtSecurityToken jwtSecurityToken = securityToken as JwtSecurityToken;
            if (jwtSecurityToken == null || !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
            {
                throw new SecurityTokenException("Invalid token");
            }

            return principal;
        }
        public string GetClairPassword(string userName)
        {
            var user = _dbContext.Users.FirstOrDefault(x=>x.FullName== userName);
            if(user==null)
            {
                return null;
            }

            return _passwordCryption.DecodeFrom64(user.Password);
        }
        public User GetUserById(Guid id)
        {
            var user = _dbContext.Users.Include(x=>x.EventBooks).ThenInclude(x=>x.Event).FirstOrDefault(x => x.Id == id);

            return user;
        }
        public bool CancelUserTicket(Guid userid, Guid eventid)
        {
            var userTicket = _dbContext.EventBooks.FirstOrDefault(x=>x.UserId== userid && x.EventId==eventid);
            if(userTicket==null)
            {
                return false;
            }
            _dbContext.EventBooks.Remove(userTicket);
            _dbContext.SaveChanges();
            var canceldEvent = _dbContext.Events.FirstOrDefault(x=>x.Id==eventid);
            canceldEvent.AvailableTikect = canceldEvent.AvailableTikect + userTicket.NumberOfTicket;
            _dbContext.Events.Update(canceldEvent);
            _dbContext.SaveChanges();
            return true;
        }
        public async Task<BookDto> EventBook(Guid userId, Guid eventId, int numOfTicket)
        {
            var user = await _dbContext.Users.Include(x => x.EventBooks).ThenInclude(x => x.Event).
                FirstOrDefaultAsync(x => x.Id == userId);
            EventBook input = new EventBook(userId, eventId, numOfTicket);
           await  _dbContext.EventBooks.AddAsync(input);
            await _dbContext.SaveChangesAsync();


            return new BookDto() { Id = input.Id, UserName = user.FullName, EventName = user.EventBooks.FirstOrDefault(x => x.EventId == eventId).Event.Name, NumOfBookingTicket = numOfTicket };
        }
        private RefreshToken GenerateRefreshToken()
        {
            var refreshToken = new RefreshToken()
            {
                Token = Convert.ToBase64String(RandomNumberGenerator.GetBytes(64)),
                Expired = DateTime.Now.AddHours(Convert.ToDouble(_configuration["Jwt:ExpireRefreshTokenByDays"]))
            };
            return refreshToken;
        }
        private void SetRefreshToken(RefreshToken newRefreshToken)
        {
            var cookieOptions = new CookieOptions
            {
                HttpOnly = true,
                Expires = newRefreshToken.Expired
            };
            _httpContextAccessor.HttpContext.Response.Cookies.Append("refreshToken", newRefreshToken.Token, cookieOptions);
            tokenUser.refreshToken = newRefreshToken.Token;
            tokenUser.tokenCreated = newRefreshToken.Created;
            tokenUser.tokenExpired = newRefreshToken.Expired;
        }


    }
}
