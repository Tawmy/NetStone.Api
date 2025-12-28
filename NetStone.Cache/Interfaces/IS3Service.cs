namespace NetStone.Cache.Interfaces;

public interface IS3Service
{
    Task<string> ReuploadAsync(string bucket, string key, Uri remoteFile, CT ct = default);
}