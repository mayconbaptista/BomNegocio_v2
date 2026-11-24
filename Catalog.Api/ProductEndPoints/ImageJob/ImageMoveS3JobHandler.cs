using Amazon.S3;
using Amazon.S3.Model;
using BuildBlocks.Domain.Abstractions.CQRS;
using Catalog.Api.Configuration;
using Catalog.Api.Data.Interfaces;
using Grpc.Net.Client.Configuration;
using MassTransit.Configuration;
using MediatR;
using Microsoft.Extensions.Options;

namespace Catalog.Api.ProductEndPoints.ImageJob;

public record ImageMoveS3JobCommand(string patch) : ICommand;

public class ImageMoveS3JobHandler(
    IUnitOfWork unitOfWork, 
    IAmazonS3 amazonS3, 
    IOptions<AwsServiceS3Config> s3Config, 
    HttpClient httpClient, 
    ILogger<ImageMoveS3JobHandler> logger) 
    : ICommandHandler<ImageMoveS3JobCommand>
{
    public async Task<Unit> Handle(ImageMoveS3JobCommand request, CancellationToken cancellationToken)
    {
        try
        {

            var images = await unitOfWork.IProductRepository.GeImages();

            // Buckets novos usam Object Ownership = BucketOwnerEnforced (ACLs
            // desativadas): CannedACL.PublicRead falharia na AWS real. A leitura
            // publica acontece via CloudFront + OAC, nunca direto no bucket.

            foreach (var image in images)
            {
                byte[] imageBytes = await httpClient.GetByteArrayAsync(image.path);

                using(var imageStream = new MemoryStream(imageBytes))
                {
                    var imageRequest = new PutObjectRequest
                    {
                        BucketName = s3Config.Value.BucketName,
                        Key = image.name,
                        InputStream = imageStream,
                        ContentType = "image/png"
                    };

                    var response = await amazonS3.PutObjectAsync(imageRequest);
                };

                // Persiste somente a chave do objeto. Quem monta a URL publica
                // (CDN, LocalStack) e a borda/front, nao o dominio.
                image.path = image.name;
            }

            await unitOfWork.SaveChangesAsync();
            return Unit.Value;
        }
        catch (HttpRequestException e)
        {
            logger.LogError(e, "Erro ao baixar imagem: {message}", e.Message);

            throw;
        }
        catch (AmazonS3Exception e)
        {
            logger.LogError(e, "Erro no S3: {message}", e.Message);

            throw;
        }
        catch (Exception e)
        {
            logger.LogError(e, "Um erro inesperado: {message}", e.Message);

            throw;
        }
    }
}
