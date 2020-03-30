using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Application.Interfaces;
using Domain;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Application.Photos
{
    public class Add
    {
        public class Command : IRequest<Photo>
        {
            public IFormFile File { get; set; }


        }


        public class Handler : IRequestHandler<Command, Photo>
        {
            private readonly DataContext _context;
            private readonly IUserAccessor _usserAccessor;
            private readonly IPhotoAccessor _photoAccessor;
            public Handler(DataContext context, IUserAccessor usserAccessor, IPhotoAccessor photoAccessor)
            {
                this._photoAccessor = photoAccessor;
                this._usserAccessor = usserAccessor;
                this._context = context;
            }

            public async Task<Photo> Handle(Command request, CancellationToken cancellationToken)
            {
                //handler logic
                var photoResult=_photoAccessor.AddPhoto(request.File);

                var user=await _context.Users.SingleOrDefaultAsync(x=>x.UserName==_usserAccessor.GetCurrentUserName());

                var photo=new Photo{
                    Url=photoResult.Url,
                    Id=photoResult.PublicId
                };

                if(!user.Photos.Any(x=>x.IsMain)){
                    photo.IsMain=true;
                }


                user.Photos.Add(photo);


                var success = await _context.SaveChangesAsync() > 0;
                if (success) return photo;//prazan objekt(vraća 200 ok)

                throw new Exception("Probblem saving changes");
            }
        }

    }
}