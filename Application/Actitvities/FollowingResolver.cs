using System.Linq;
using Application.Interfaces;
using AutoMapper;
using Domain;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Application.Actitvities
{
    public class FollowingResolver : IValueResolver<UserActivity, AttendeeDTO, bool>
    {
        private readonly IUserAccessor _userAccessor;
        private readonly DataContext _context;
        public FollowingResolver(DataContext context, IUserAccessor userAccessor)
        {
            this._context = context;
            this._userAccessor = userAccessor;
        }

        public bool Resolve(UserActivity source, AttendeeDTO destination, bool destMember, ResolutionContext context)
        {
            var currentUser=_context.Users.SingleOrDefaultAsync(x=>x.UserName==_userAccessor.GetCurrentUserName()).Result;

            if(currentUser.Followings.Any(x=>x.TargetId==source.AppUserId)){
                return true;
            }

            return false;

        }
    }
    
}