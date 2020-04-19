using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Application.Errors;
using AutoMapper;
using Domain;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Application.Comments
{
    public class Create
    {
        public class Command : IRequest<CommentDTO>
        {
            public string Body { get; set; }
            public Guid AcitvityId { get; set; }
            public string UserName { get; set; }

        }


        public class Handler : IRequestHandler<Command, CommentDTO>
        {
            private readonly DataContext _context;
            private readonly IMapper _mapper;
            public Handler(DataContext context, IMapper mapper)
            {
                this._mapper = mapper;
                this._context = context;
            }

            public async Task<CommentDTO> Handle(Command request, CancellationToken cancellationToken)
            {
                //handler logic
                var activity=await _context.Activities.FindAsync(request.AcitvityId);
                if(activity==null){
                              throw new Exception("NOt found");

                }

                var user=await _context.Users.SingleOrDefaultAsync(x=>x.UserName==request.UserName);

                var comment=new Comment{
                    Author=user,
                    Activity=activity,
                    Body=request.Body,
                    CreatedAt=DateTime.Now
                };
                activity.Comments.Add(comment);
                var success = await _context.SaveChangesAsync() > 0;
                if (success) return _mapper.Map<CommentDTO>(comment);//prazan objekt(vraća 200 ok)

                throw new Exception("Probblem saving changes");
            }
        }

    }
}