using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Application.Errors;
using Application.Interfaces;
using Domain;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Application.Actitvities
{
    public class Attend
    {
        public class Command : IRequest
        {
            public Guid Id { get; set; }


        }


        public class Handler : IRequestHandler<Command>
        {
            private readonly DataContext _context;
            private readonly IUserAccessor _userAccessor;

            public Handler(DataContext context, IUserAccessor userAccessor)
            {
                this._context = context;
                this._userAccessor = userAccessor;
            }

            public async Task<Unit> Handle(Command request, CancellationToken cancellationToken)
            {
                //handler logic

                var activity = await _context.Activities.FindAsync(request.Id);
                if (activity == null)
                {
                    throw new RestException(HttpStatusCode.NotFound, new { Activity = "colul not find activity." });
                }
                var user = await _context.Users.SingleOrDefaultAsync(x => x.UserName == _userAccessor.GetCurrentUserName());

                var attendance = await _context.UserActivities.SingleOrDefaultAsync(x => x.ActivityId == activity.Id && x.AppUserId == user.Id);

                if(attendance!=null){
                    throw new RestException(HttpStatusCode.BadRequest,new{attendance="Already attending"});
                }

                attendance=new UserActivity{
                    Activity=activity,
                    AppUser=user,
                    IsHost=false,
                    DateJoined=DateTime.Now
                };

                _context.UserActivities.Add(attendance);
                var success = await _context.SaveChangesAsync() > 0;
                if (success) return Unit.Value;//prazan objekt(vraća 200 ok)

                throw new Exception("Probblem saving changes");
            }
        }

    }
}