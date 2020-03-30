using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Application.Errors;
using MediatR;
using Persistence;

namespace Application.Actitvities
{
    public class Delete
    {
        public class Command : IRequest
        {
            public Guid Id{get;set;}

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

                var activity=await _context.Activities.FindAsync(request.Id);
                if(activity==null){
                    throw new RestException(HttpStatusCode.NotFound,new {activity="NOt found"});
                }

                _context.Remove(activity);


                var success = await _context.SaveChangesAsync() > 0;
                if (success) return Unit.Value;//prazan objekt(vraća 200 ok)

                throw new Exception("Probblem saving changes");
            }


        }

    }
}