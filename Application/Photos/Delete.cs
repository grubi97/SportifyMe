using System;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Application.Errors;
using Application.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Application.Photos
{
    public class Delete
    {
        public class Command : IRequest
        {
            public string Id { get; set; }

        }


        public class Handler : IRequestHandler<Command>
        {
            private readonly DataContext _context;
            private readonly IPhotoAccessor _photoAccessor;
            private readonly IUserAccessor _usserAccessor;
            public Handler(DataContext context, IUserAccessor usserAccessor, IPhotoAccessor photoAccessor)
            {
                this._usserAccessor = usserAccessor;
                this._photoAccessor = photoAccessor;
                this._context = context;
            }

            public async Task<Unit> Handle(Command request, CancellationToken cancellationToken)
            {
                //handler logic
                var user=await _context.Users.SingleOrDefaultAsync(x=>x.UserName==_usserAccessor.GetCurrentUserName());

                var photo=user.Photos.FirstOrDefault(x=>x.Id==request.Id);
                if(photo==null){
                    throw new Exception("no photo to delete");
                }


                if(photo.IsMain){
                    throw new Exception("cant delete main photo");

                }

                var result=_photoAccessor.DeletePhoto(photo.Id);
                if(result==null){
                    throw new Exception("prob delteing photo");
                }

                


                user.Photos.Remove(photo);
                var success = await _context.SaveChangesAsync() > 0;
                if (success) return Unit.Value;//prazan objekt(vraća 200 ok)

                throw new Exception("Probblem saving changes");
            }
        }

    }
}