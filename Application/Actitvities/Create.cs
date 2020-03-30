using System;
using System.ComponentModel.DataAnnotations;
using System.Threading;
using System.Threading.Tasks;
using Application.Interfaces;
using Domain;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Application.Actitvities
{
    public class Create
    {
        public class Command : IRequest
        {

            public Guid Id { get; set; }
            public string Title { get; set; }
            public string Description { get; set; }
            public string Category { get; set; }
            public string City { get; set; }
            public DateTime Date { get; set; }
            public string venue { get; set; }

        }
        public class CommandValidator : AbstractValidator<Command>
        {
            public CommandValidator()
            {
                RuleFor(x => x.Title).NotEmpty();
                RuleFor(x => x.Description).NotEmpty();
                RuleFor(x => x.Category).NotEmpty();
                RuleFor(x => x.Date).NotEmpty();
                RuleFor(x => x.venue).NotEmpty();
                RuleFor(x => x.City).NotEmpty();



            }
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
                var activity = new Activity
                {
                    Id = request.Id,
                    Title = request.Title,
                    Description = request.Description,
                    Category = request.Category,
                    Date = request.Date,
                    City = request.City,
                    venue = request.venue
                };
                _context.Activities.Add(activity);//ne async metdoa jer ne korsitimo posebni value gen

                var user=await _context.Users.SingleOrDefaultAsync(x=>x.UserName==_userAccessor.GetCurrentUserName());

                var attendee=new UserActivity{
                    AppUser=user,
                    Activity=activity,
                    IsHost=true,
                    DateJoined=DateTime.Now
                };
                _context.UserActivities.Add(attendee);

                var success = await _context.SaveChangesAsync() > 0;
                if (success) return Unit.Value;//prazan objekt(vraća 200 ok)

                throw new Exception("Probblem saving changes");
            }
        }
    }
}