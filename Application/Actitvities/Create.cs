using System;
using System.Threading;
using System.Threading.Tasks;
using Domain;
using MediatR;
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


        public class Handler : IRequestHandler<Command>
        {
            private readonly DataContext _context;
            public Handler(DataContext context)
            {
                this._context = context;
            }

            public async Task<Unit> Handle(Command request, CancellationToken cancellationToken)
            {
                var activity=new Activity
                {
                    Id=request.Id,
                    Title=request.Title,
                    Description=request.Description,
                    Category=request.Category,
                    Date=request.Date,
                    City=request.City,
                    venue=request.venue
                };
                _context.Activities.Add(activity);//ne async metdoa jer ne korsitimo posebni value gen
                 var success= await _context.SaveChangesAsync()>0;
                 if(success)return Unit.Value;//prazan objekt(vraća 200 ok)

                 throw new Exception("Probblem saving changes");
            }
        }
    }
}