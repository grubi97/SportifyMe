using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Application.Errors;
using Application.Interfaces;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Application.Profiles
{
    public class ProfileReader : IProfileReader
    {
        private readonly IUserAccessor _usserAccessor;
        private readonly DataContext _context;
        public ProfileReader(DataContext context, IUserAccessor usserAccessor)
        {
            this._context = context;
            this._usserAccessor = usserAccessor;
        }

        public async Task<Profile> ReadProfile(string username)
        {
            var user=await _context.Users.SingleOrDefaultAsync(x=>x.UserName==username);

            if(user==null){
                throw new RestException(HttpStatusCode.NotFound,new {User="Not Found"});

            }
            var currentUser=await _context.Users.SingleOrDefaultAsync(x=>x.UserName==_usserAccessor.GetCurrentUserName());
            var profile=new Profile
            {
                DisplayName=user.DisplayName,
                UserName=user.UserName,
                Image=user.Photos.FirstOrDefault(x=>x.IsMain)?.Url,
                Photos=user.Photos,
                Bio=user.Bio,
                FollowingCount=user.Followings.Count(),
                FollowersCount=user.Followers.Count()
            };
            if(currentUser.Followings.Any(x=>x.TargetId==user.Id)){
                profile.IsFollowed=true;
            }
            return profile;
        }
    }
}