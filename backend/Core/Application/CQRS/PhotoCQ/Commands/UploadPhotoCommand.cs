using Domain.Models.Abstract;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace Application.CQRS.PhotoCQ.Commands;

public class UploadPhotoCommand<T>(Guid id, Guid userId, IFormFile[] photos) : IRequest
    where T : Entity
{
    public Guid ItemId = id;
    public Guid UserId =  userId;
    public IFormFile[] Photos = photos;
}