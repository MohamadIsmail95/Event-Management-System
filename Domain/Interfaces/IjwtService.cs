using Domain.Dtos.Event;
using Domain.Dtos.User;
using Domain.Entities.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Interfaces
{
    public interface IjwtService
    {
        TokenViewModel GenerateToken(LoginViewModel login);
        bool CheckExpiredCookiesRefreshToken();
        CreateUser CreateUser(CreateUser user);
        CurrentUserViewModel GrantUserPermission(Guid userid);
        ClaimsPrincipal GetPrincipalFromExpiredToken(string token);
        string GetClairPassword(string userName);
        User GetUserById(Guid id);
        bool CancelUserTicket(Guid userid,Guid eventid);
        Task<BookDto> EventBook(Guid userId, Guid eventId, int numOfTicket);
    }
}
