using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Application.Errors;
using Application.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Application.Actitvities
{
    public class Unattend
    {
        public class Command : IRequest
        {

                public Guid Id;
        }


        public class Handler : IRequestHandler<Command>
        {
            private readonly DataContext _context;
            private readonly IUserAccessor _userAccessor;
            public Handler(DataContext context, IUserAccessor userAccessor)
            {
                this._userAccessor = userAccessor;
                this._context = context;
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

                if (attendance == null)
                {
                    return Unit.Value;

                }
                if (attendance.IsHost)
                {
                    throw new RestException(HttpStatusCode.BadRequest, new { attendance = "You cannot remove yourself as host" });
                }

                _context.UserActivities.Remove(attendance);
                var success = await _context.SaveChangesAsync() > 0;
                if (success) return Unit.Value;//prazan objekt(vraća 200 ok)

                throw new Exception("Probblem saving changes");
            }
        }

    }
}