namespace NetStone.Cache;

internal static class EnvironmentVariables
{
    /// <summary>
    ///     EFCore connection string for Postgres database.
    /// </summary>
    public const string ConnectionString = "CONNECTION_STRING";

    /// <summary>
    ///     Service URL for S3 storage. "https://" prefix is added automatically later.
    /// </summary>
    public const string S3ServiceUrl = "S3_SERVICE_URL";

    /// <summary>
    ///     S3 region. Required for URL construction.
    /// </summary>
    public const string S3Region = "S3_REGION";

    /// <summary>
    ///     S3 bucket under which images will be stored.
    /// </summary>
    public const string S3BucketName = "S3_BUCKET_NAME";
}