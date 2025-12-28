using System.Security.Cryptography;
using Amazon.S3;
using Amazon.S3.Model;
using AspNetCoreExtensions;
using Microsoft.Extensions.Configuration;
using NetStone.Cache.Interfaces;

namespace NetStone.Cache.Services;

internal class S3Service(IAmazonS3 s3, HttpClient httpClient, IConfiguration configuration) : IS3Service
{
    public async Task<string> ReuploadAsync(string bucket, string key, Uri remoteFile, CT ct = default)
    {
        using var remoteResponse = await httpClient.GetAsync(remoteFile, ct);
        remoteResponse.EnsureSuccessStatusCode();
        var stream = await remoteResponse.Content.ReadAsStreamAsync(ct);
        var checksum = GetChecksum(stream);

        var putRequest = new PutObjectRequest
        {
            BucketName = bucket,
            Key = key,
            InputStream = stream,
            ContentType = remoteResponse.Content.Headers.ContentType?.MediaType,
            ChecksumSHA256 = checksum
        };

        await s3.PutObjectAsync(putRequest, ct);
        return CreateS3Url(bucket, key);
    }

    private static string GetChecksum(Stream stream)
    {
        var sha = SHA256.Create();
        var checksum = sha.ComputeHash(stream);
        return Convert.ToBase64String(checksum);
    }

    private string CreateS3Url(string bucket, string key)
    {
        var s3Url = configuration.GetGuardedConfiguration(EnvironmentVariables.S3ServiceUrl);
        return $"https://{bucket}.{s3Url}/{key}";
    }
}