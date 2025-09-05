using Amazon.S3;
using Amazon.S3.Model;
using BuildBlocks.Domain.Abstractions.CQRS;
using BuildBlocks.Domain.Exceptions;
using Catalog.Api.Configuration;
using Catalog.Api.Data.Interfaces;
using Catalog.Api.Entities;
using Microsoft.Extensions.Options;

namespace Catalog.Api.ProductEndPoints.InsertOrUpdateImage;

public record InsertImageCommand(Guid productId, string name, MemoryStream pictureStrem) : ICommand<string>;

public class InsertImageHandler(IUnitOfWork unitOfWork, ILogger<InsertImageHandler> logger, IAmazonS3 amazonS3, IOptions<AwsServiceS3Config> serviceS3Config)
    : ICommandHandler<InsertImageCommand, string>
{
    public async Task<string> Handle(InsertImageCommand request, CancellationToken cancellationToken)
    {
        var produto = await unitOfWork.IProductRepository.GetByIdAsync(request.productId)
            ?? throw new NotFoundException("Produto inexistente");

        if (produto.Images.Count >= 6) throw new Exception("O produto já atigou o limite de imagens cadastradas");

        var imageRequest = new PutObjectRequest
        {
            BucketName = serviceS3Config.Value.BucketName,
            Key = $"{request.productId}_{request.name}",
            InputStream = request.pictureStrem,
            CannedACL = S3CannedACL.PublicRead,
        };

        produto.Images.Add(new ImageEntity() {path = imageRequest.Key, name = request.name, ProductId = request.productId });

        var response = await amazonS3.PutObjectAsync(imageRequest);

        if(response.HttpStatusCode != System.Net.HttpStatusCode.OK) throw new Exception($"Erro ao salvar imagem no s3: {response.HttpStatusCode}");

        await unitOfWork.SaveChangesAsync();

        return request.ToString();
    }
}
