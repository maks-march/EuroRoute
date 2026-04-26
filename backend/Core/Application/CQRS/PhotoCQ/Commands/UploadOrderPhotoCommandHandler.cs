using Application.Common.Exceptions;
using Application.Interfaces;
using Domain.Models;
using Domain.Models.Abstract;
using Domain.Models.Order;
using MediatR;

namespace Application.CQRS.PhotoCQ.Commands;

public class UploadPhotoCommandHandler<TOwner>(IAppDbContext dbContext, IFileService fileService) 
    : IRequestHandler<UploadPhotoCommand<TOwner>>
    where TOwner : Entity
{
    public async Task Handle(UploadPhotoCommand<TOwner> request, CancellationToken cancellationToken)
    {
        var item = await dbContext.GetDbSet<TOwner>().FindAsync([request.ItemId], cancellationToken);
        if (item == null)
            throw new NotFoundException(nameof(item), request.ItemId);
        if (item is User user)
        {
            if (user.Id != request.UserId)
                throw new ForbiddenException(nameof(user), request.UserId);
        }

        if (item is OrderEntity order)
        {
            if (order.UserId != request.UserId)
                throw new ForbiddenException(nameof(user), request.UserId);
            var save = fileService.SaveFiles(cancellationToken, request.Photos);
            var del = fileService.DeleteFiles(cancellationToken, order.Photos
                .Select(f => f.FilePath)
                .ToArray());
            try
            {
                if (!del.Result)
                    throw new("Files clearing failed.");
            }
            catch (Exception ex)
            {
                throw new Exception("Files update failed", ex);
            }

            UpdateCollection(order.Photos, save.Result, order);
            await dbContext.SaveChangesAsync(cancellationToken);
        }
        
    }
    
    private void UpdateCollection(IList<OrderPhoto> fileCollection, string[] newList, OrderEntity owner)
    {
        for (int i = fileCollection.Count - 1; i >= 0; i--)
        {
            var existing = fileCollection[i];
            if (i < newList.Length)
            {
                existing.FilePath = newList[i];
            }
            else
            {
                dbContext.GetDbSet<OrderPhoto>().Remove(existing);
            }
        }
        
        if (newList.Length > fileCollection.Count)
        {
            for (int i = fileCollection.Count; i < newList.Length; i++)
            {
                var newFile = new OrderPhoto
                {
                    Id = Guid.Empty,
                    Created = DateTime.Now,
                    Updated = DateTime.Now,
                    FilePath = newList[i],
                    OwnerId = owner.Id,
                    Owner = owner,
                    OrderIndex = i
                };
            
                fileCollection.Add(newFile);
            }
        }
    }
}

public class UploadOrderPhotoCommandHandler(
    IAppDbContext dbContext, 
    IFileService fileService
    ) : UploadPhotoCommandHandler<OrderEntity>(dbContext, fileService);