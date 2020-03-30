using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;

namespace Domain
{
    public class AppUser : IdentityUser
    {
        public string DisplayName { get; set; }
        public string Bio { get; set; }
        virtual public ICollection<Photo> Photos { get; set; }

        virtual public ICollection<UserActivity> UserActivities { get; set; }
        virtual public ICollection<UserFollowing> Followings { get; set; }
        virtual public ICollection<UserFollowing> Followers { get; set; }



    }
}