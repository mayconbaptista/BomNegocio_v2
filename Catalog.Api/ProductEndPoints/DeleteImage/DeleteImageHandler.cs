using Amazon.S3;
using Amazon.S3.Model;
using BuildBlocks.Domain.Abstractions.CQRS;
using Catalog.Api.Configuration;
using Catalog.Api.Data.Interfaces;
using MediatR;
using Microsoft.Extensions.Options;

namespace Catalog.Api.ProductEndPoints.DeleteImage;

public record DeleteImageCommand(Guid ProductId, uint imageId) : ICommand;

public class DeleteImageHandler(IUnitOfWork unitOfWork, ILogger<DeleteImageHandler> logger, IAmazonS3 amazonS3, IOptions<AwsServiceS3Config> serviceS3Config) 
    : ICommandHandler<DeleteImageCommand>
{
    public async Task<Unit> Handle(DeleteImageCommand request, CancellationToken cancellationToken)
    {
        var product = await unitOfWork.IProductRepository.GetByIdAsync(request.ProductId)
            ?? throw new Exception($"Não foi possivel encontrar o produto: {request.ProductId}");

        var imagem = product.Images.FirstOrDefault(x => x.Id == request.imageId)
            ?? throw new Exception($"O produto {request.ProductId} não possui imagem {request.imageId}");

        product.Images.Remove(imagem);

        var response = await amazonS3
            .DeleteObjectAsync(new DeleteObjectRequest { BucketName = serviceS3Config.Value.BucketName, Key = imagem.path});

        if (response.HttpStatusCode == System.Net.HttpStatusCode.OK) 
            throw new Exception($"Problemas ao deletar imagem do bucket: {response.HttpStatusCode}.");

        await unitOfWork.SaveChangesAsync();

        return new Unit();
    }
}
