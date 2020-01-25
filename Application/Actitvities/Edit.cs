using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Persistence;

namespace Application.Actitvities
{
    public class Edit
    {
        public class Command : IRequest
        {
            public Guid Id { get; set; }
            public string Title { get; set; }
            public string Description { get; set; }
            public string Category { get; set; }
            public string City { get; set; }
            public DateTime? Date { get; set; }//optional, zbog null
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
                //handler logic
                    var acticity=await _context.Activities.FindAsync(request.Id);
                    if(acticity==null){
                        throw new Exception("No activity");

                    }

                    acticity.Title=request.Title ?? acticity.Title;
                    acticity.Description=request.Description ?? acticity.Description;
                    acticity.City=request.City ?? acticity.City;
                    acticity.venue=request.venue ?? acticity.venue;
                    acticity.Date=request.Date ?? acticity.Date;
                    acticity.Category=request.Category ?? acticity.Category; 

                    




                var success = await _context.SaveChangesAsync() > 0;
                if (success) return Unit.Value;//prazan objekt(vraća 200 ok)

                throw new Exception("Probblem saving changes");
            }


        }

    }
}