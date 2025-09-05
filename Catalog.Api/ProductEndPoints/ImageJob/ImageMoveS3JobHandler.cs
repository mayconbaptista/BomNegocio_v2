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

public class ImageMoveS3JobHandler(IUnitOfWork unitOfWork, IAmazonS3 amazonS3, IOptions<AwsServiceS3Config> s3Config, HttpClient httpClient, ILogger<ImageMoveS3JobHandler> logger, IConfiguration config) 
    : ICommandHandler<ImageMoveS3JobCommand>
{
    public async Task<Unit> Handle(ImageMoveS3JobCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var images = await unitOfWork.IProductRepository.GeImages();

            var serviceUrl = config.GetValue<string>("AWS:S3:ServiceURL") 
                ?? throw new ArgumentNullException("A service url do s3 é nula");

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
                        CannedACL = S3CannedACL.PublicRead,
                        ContentType = "image/png"
                    };

                    var response = await amazonS3.PutObjectAsync(imageRequest);
                };

                image.path = $"/{s3Config.Value.BucketName}/{image.name}";
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
