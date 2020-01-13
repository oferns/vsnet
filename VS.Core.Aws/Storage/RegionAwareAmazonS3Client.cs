
namespace VS.Core.Aws.Storage {
    using Amazon;
    using Amazon.Runtime;
    using Amazon.S3;
    using Amazon.S3.Model;
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Threading;
    using System.Threading.Tasks;
    using VS.Abstractions;

    public class RegionAwareAmazonS3Client : IAmazonS3 {

        private readonly IAmazonS3 chosenClient;
        private readonly IContext context;

        public RegionAwareAmazonS3Client(IAmazonS3[] clients, IContext context) {
            if (clients is null) {
                throw new System.ArgumentNullException(nameof(clients));
            }
            this.context = context ?? throw new System.ArgumentNullException(nameof(context));
            chosenClient = clients.Length == 0 ? new AmazonS3Client(RegionEndpoint.EUWest2) : clients[0];
        }

        public IClientConfig Config => chosenClient.Config;

        public Task<AbortMultipartUploadResponse> AbortMultipartUploadAsync(string bucketName, string key, string uploadId, CancellationToken cancellationToken = default) {
            return chosenClient.AbortMultipartUploadAsync(bucketName, key, uploadId, cancellationToken);
        }

        public Task<AbortMultipartUploadResponse> AbortMultipartUploadAsync(AbortMultipartUploadRequest request, CancellationToken cancellationToken = default) {
            return chosenClient.AbortMultipartUploadAsync(request, cancellationToken);
        }

        public Task<CompleteMultipartUploadResponse> CompleteMultipartUploadAsync(CompleteMultipartUploadRequest request, CancellationToken cancellationToken = default) {
            return chosenClient.CompleteMultipartUploadAsync(request, cancellationToken);
        }

        public Task<CopyObjectResponse> CopyObjectAsync(string sourceBucket, string sourceKey, string destinationBucket, string destinationKey, CancellationToken cancellationToken = default) {
            return chosenClient.CopyObjectAsync(sourceBucket, sourceKey, destinationBucket, destinationKey, cancellationToken);
        }

        public Task<CopyObjectResponse> CopyObjectAsync(string sourceBucket, string sourceKey, string sourceVersionId, string destinationBucket, string destinationKey, CancellationToken cancellationToken = default) {
            return chosenClient.CopyObjectAsync(sourceBucket, sourceKey, sourceVersionId, destinationBucket, destinationKey, cancellationToken);
        }

        public Task<CopyObjectResponse> CopyObjectAsync(CopyObjectRequest request, CancellationToken cancellationToken = default) {
            return chosenClient.CopyObjectAsync(request, cancellationToken);
        }

        public Task<CopyPartResponse> CopyPartAsync(string sourceBucket, string sourceKey, string destinationBucket, string destinationKey, string uploadId, CancellationToken cancellationToken = default) {
            return chosenClient.CopyPartAsync(sourceBucket, sourceKey, destinationBucket, destinationKey, uploadId, cancellationToken);
        }

        public Task<CopyPartResponse> CopyPartAsync(string sourceBucket, string sourceKey, string sourceVersionId, string destinationBucket, string destinationKey, string uploadId, CancellationToken cancellationToken = default) {
            return chosenClient.CopyPartAsync(sourceBucket, sourceKey, sourceVersionId, destinationBucket, destinationKey, uploadId, cancellationToken);
        }

        public Task<CopyPartResponse> CopyPartAsync(CopyPartRequest request, CancellationToken cancellationToken = default) {
            return chosenClient.CopyPartAsync(request, cancellationToken);
        }

        public Task DeleteAsync(string bucketName, string objectKey, IDictionary<string, object> additionalProperties, CancellationToken cancellationToken = default) {
            return chosenClient.DeleteAsync(bucketName, objectKey, additionalProperties, cancellationToken);
        }

        public Task<DeleteBucketAnalyticsConfigurationResponse> DeleteBucketAnalyticsConfigurationAsync(DeleteBucketAnalyticsConfigurationRequest request, CancellationToken cancellationToken = default) {
            return chosenClient.DeleteBucketAnalyticsConfigurationAsync(request, cancellationToken);
        }

        public Task<DeleteBucketResponse> DeleteBucketAsync(string bucketName, CancellationToken cancellationToken = default) {
            return chosenClient.DeleteBucketAsync(bucketName, cancellationToken);
        }

        public Task<DeleteBucketResponse> DeleteBucketAsync(DeleteBucketRequest request, CancellationToken cancellationToken = default) {
            return chosenClient.DeleteBucketAsync(request, cancellationToken);
        }

        public Task<DeleteBucketEncryptionResponse> DeleteBucketEncryptionAsync(DeleteBucketEncryptionRequest request, CancellationToken cancellationToken = default) {
            return chosenClient.DeleteBucketEncryptionAsync(request, cancellationToken);
        }

        public Task<DeleteBucketInventoryConfigurationResponse> DeleteBucketInventoryConfigurationAsync(DeleteBucketInventoryConfigurationRequest request, CancellationToken cancellationToken = default) {
            return chosenClient.DeleteBucketInventoryConfigurationAsync(request, cancellationToken);
        }

        public Task<DeleteBucketMetricsConfigurationResponse> DeleteBucketMetricsConfigurationAsync(DeleteBucketMetricsConfigurationRequest request, CancellationToken cancellationToken = default) {
            return chosenClient.DeleteBucketMetricsConfigurationAsync(request, cancellationToken);
        }

        public Task<DeleteBucketPolicyResponse> DeleteBucketPolicyAsync(string bucketName, CancellationToken cancellationToken = default) {
            return chosenClient.DeleteBucketPolicyAsync(bucketName, cancellationToken);
        }

        public Task<DeleteBucketPolicyResponse> DeleteBucketPolicyAsync(DeleteBucketPolicyRequest request, CancellationToken cancellationToken = default) {
            return chosenClient.DeleteBucketPolicyAsync(request, cancellationToken);
        }

        public Task<DeleteBucketReplicationResponse> DeleteBucketReplicationAsync(DeleteBucketReplicationRequest request, CancellationToken cancellationToken = default) {
            return chosenClient.DeleteBucketReplicationAsync(request, cancellationToken);
        }

        public Task<DeleteBucketTaggingResponse> DeleteBucketTaggingAsync(string bucketName, CancellationToken cancellationToken = default) {
            return chosenClient.DeleteBucketTaggingAsync(bucketName, cancellationToken);
        }

        public Task<DeleteBucketTaggingResponse> DeleteBucketTaggingAsync(DeleteBucketTaggingRequest request, CancellationToken cancellationToken = default) {
            return chosenClient.DeleteBucketTaggingAsync(request, cancellationToken);
        }

        public Task<DeleteBucketWebsiteResponse> DeleteBucketWebsiteAsync(string bucketName, CancellationToken cancellationToken = default) {
            return chosenClient.DeleteBucketWebsiteAsync(bucketName, cancellationToken);
        }

        public Task<DeleteBucketWebsiteResponse> DeleteBucketWebsiteAsync(DeleteBucketWebsiteRequest request, CancellationToken cancellationToken = default) {
            return chosenClient.DeleteBucketWebsiteAsync(request, cancellationToken);
        }

        public Task<DeleteCORSConfigurationResponse> DeleteCORSConfigurationAsync(string bucketName, CancellationToken cancellationToken = default) {
            return chosenClient.DeleteCORSConfigurationAsync(bucketName, cancellationToken);
        }

        public Task<DeleteCORSConfigurationResponse> DeleteCORSConfigurationAsync(DeleteCORSConfigurationRequest request, CancellationToken cancellationToken = default) {
            return chosenClient.DeleteCORSConfigurationAsync(request, cancellationToken);
        }

        public Task<DeleteLifecycleConfigurationResponse> DeleteLifecycleConfigurationAsync(string bucketName, CancellationToken cancellationToken = default) {
            return chosenClient.DeleteLifecycleConfigurationAsync(bucketName, cancellationToken);
        }

        public Task<DeleteLifecycleConfigurationResponse> DeleteLifecycleConfigurationAsync(DeleteLifecycleConfigurationRequest request, CancellationToken cancellationToken = default) {
            return chosenClient.DeleteLifecycleConfigurationAsync(request, cancellationToken);
        }

        public Task<DeleteObjectResponse> DeleteObjectAsync(string bucketName, string key, CancellationToken cancellationToken = default) {
            return chosenClient.DeleteObjectAsync(bucketName, key, cancellationToken);
        }

        public Task<DeleteObjectResponse> DeleteObjectAsync(string bucketName, string key, string versionId, CancellationToken cancellationToken = default) {
            return chosenClient.DeleteObjectAsync(bucketName, key, versionId, cancellationToken);
        }

        public Task<DeleteObjectResponse> DeleteObjectAsync(DeleteObjectRequest request, CancellationToken cancellationToken = default) {
            return chosenClient.DeleteObjectAsync(request, cancellationToken);
        }

        public Task<DeleteObjectsResponse> DeleteObjectsAsync(DeleteObjectsRequest request, CancellationToken cancellationToken = default) {
            return chosenClient.DeleteObjectsAsync(request, cancellationToken);
        }

        public Task<DeleteObjectTaggingResponse> DeleteObjectTaggingAsync(DeleteObjectTaggingRequest request, CancellationToken cancellationToken = default) {
            return chosenClient.DeleteObjectTaggingAsync(request, cancellationToken);
        }

        public Task<DeletePublicAccessBlockResponse> DeletePublicAccessBlockAsync(DeletePublicAccessBlockRequest request, CancellationToken cancellationToken = default) {
            return chosenClient.DeletePublicAccessBlockAsync(request, cancellationToken);
        }

        public Task DeletesAsync(string bucketName, IEnumerable<string> objectKeys, IDictionary<string, object> additionalProperties, CancellationToken cancellationToken = default) {
            return chosenClient.DeletesAsync(bucketName, objectKeys, additionalProperties, cancellationToken);
        }

        public void Dispose() {
            Dispose(true);            
        }
        protected virtual void Dispose(bool disposing) { 
        
        }
        public Task<bool> DoesS3BucketExistAsync(string bucketName) {
            return chosenClient.DoesS3BucketExistAsync(bucketName);
        }

        public Task DownloadToFilePathAsync(string bucketName, string objectKey, string filepath, IDictionary<string, object> additionalProperties, CancellationToken cancellationToken = default) {
            return chosenClient.DownloadToFilePathAsync(bucketName, objectKey, filepath, additionalProperties, cancellationToken);
        }

        public Task EnsureBucketExistsAsync(string bucketName) {
            return chosenClient.EnsureBucketExistsAsync(bucketName);
        }

        public string GeneratePreSignedURL(string bucketName, string objectKey, DateTime expiration, IDictionary<string, object> additionalProperties) {
            return chosenClient.GeneratePreSignedURL(bucketName, objectKey, expiration, additionalProperties);
        }

        public Task<GetACLResponse> GetACLAsync(string bucketName, CancellationToken cancellationToken = default) {
            return chosenClient.GetACLAsync(bucketName, cancellationToken);
        }

        public Task<GetACLResponse> GetACLAsync(GetACLRequest request, CancellationToken cancellationToken = default) {
            return chosenClient.GetACLAsync(request, cancellationToken);
        }

        public Task<IList<string>> GetAllObjectKeysAsync(string bucketName, string prefix, IDictionary<string, object> additionalProperties) {
            return chosenClient.GetAllObjectKeysAsync(bucketName, prefix, additionalProperties);
        }

        public Task<GetBucketAccelerateConfigurationResponse> GetBucketAccelerateConfigurationAsync(string bucketName, CancellationToken cancellationToken = default) {
            return chosenClient.GetBucketAccelerateConfigurationAsync(bucketName, cancellationToken);
        }

        public Task<GetBucketAccelerateConfigurationResponse> GetBucketAccelerateConfigurationAsync(GetBucketAccelerateConfigurationRequest request, CancellationToken cancellationToken = default) {
            return chosenClient.GetBucketAccelerateConfigurationAsync(request, cancellationToken);
        }

        public Task<GetBucketAnalyticsConfigurationResponse> GetBucketAnalyticsConfigurationAsync(GetBucketAnalyticsConfigurationRequest request, CancellationToken cancellationToken = default) {
            return chosenClient.GetBucketAnalyticsConfigurationAsync(request, cancellationToken);
        }

        public Task<GetBucketEncryptionResponse> GetBucketEncryptionAsync(GetBucketEncryptionRequest request, CancellationToken cancellationToken = default) {
            return chosenClient.GetBucketEncryptionAsync(request, cancellationToken);
        }

        public Task<GetBucketInventoryConfigurationResponse> GetBucketInventoryConfigurationAsync(GetBucketInventoryConfigurationRequest request, CancellationToken cancellationToken = default) {
            return chosenClient.GetBucketInventoryConfigurationAsync(request, cancellationToken);
        }

        public Task<GetBucketLocationResponse> GetBucketLocationAsync(string bucketName, CancellationToken cancellationToken = default) {
            return chosenClient.GetBucketLocationAsync(bucketName, cancellationToken);
        }

        public Task<GetBucketLocationResponse> GetBucketLocationAsync(GetBucketLocationRequest request, CancellationToken cancellationToken = default) {
            return chosenClient.GetBucketLocationAsync(request, cancellationToken);
        }

        public Task<GetBucketLoggingResponse> GetBucketLoggingAsync(string bucketName, CancellationToken cancellationToken = default) {
            return chosenClient.GetBucketLoggingAsync(bucketName, cancellationToken);
        }

        public Task<GetBucketLoggingResponse> GetBucketLoggingAsync(GetBucketLoggingRequest request, CancellationToken cancellationToken = default) {
            return chosenClient.GetBucketLoggingAsync(request, cancellationToken);
        }

        public Task<GetBucketMetricsConfigurationResponse> GetBucketMetricsConfigurationAsync(GetBucketMetricsConfigurationRequest request, CancellationToken cancellationToken = default) {
            return chosenClient.GetBucketMetricsConfigurationAsync(request, cancellationToken);
        }

        public Task<GetBucketNotificationResponse> GetBucketNotificationAsync(string bucketName, CancellationToken cancellationToken = default) {
            return chosenClient.GetBucketNotificationAsync(bucketName, cancellationToken);
        }

        public Task<GetBucketNotificationResponse> GetBucketNotificationAsync(GetBucketNotificationRequest request, CancellationToken cancellationToken = default) {
            return chosenClient.GetBucketNotificationAsync(request, cancellationToken);
        }

        public Task<GetBucketPolicyResponse> GetBucketPolicyAsync(string bucketName, CancellationToken cancellationToken = default) {
            return chosenClient.GetBucketPolicyAsync(bucketName, cancellationToken);
        }

        public Task<GetBucketPolicyResponse> GetBucketPolicyAsync(GetBucketPolicyRequest request, CancellationToken cancellationToken = default) {
            return chosenClient.GetBucketPolicyAsync(request, cancellationToken);
        }

        public Task<GetBucketPolicyStatusResponse> GetBucketPolicyStatusAsync(GetBucketPolicyStatusRequest request, CancellationToken cancellationToken = default) {
            return chosenClient.GetBucketPolicyStatusAsync(request, cancellationToken);
        }

        public Task<GetBucketReplicationResponse> GetBucketReplicationAsync(GetBucketReplicationRequest request, CancellationToken cancellationToken = default) {
            return chosenClient.GetBucketReplicationAsync(request, cancellationToken);
        }

        public Task<GetBucketRequestPaymentResponse> GetBucketRequestPaymentAsync(string bucketName, CancellationToken cancellationToken = default) {
            return chosenClient.GetBucketRequestPaymentAsync(bucketName, cancellationToken);
        }

        public Task<GetBucketRequestPaymentResponse> GetBucketRequestPaymentAsync(GetBucketRequestPaymentRequest request, CancellationToken cancellationToken = default) {
            return chosenClient.GetBucketRequestPaymentAsync(request, cancellationToken);
        }

        public Task<GetBucketTaggingResponse> GetBucketTaggingAsync(GetBucketTaggingRequest request, CancellationToken cancellationToken = default) {
            return chosenClient.GetBucketTaggingAsync(request, cancellationToken);
        }

        public Task<GetBucketVersioningResponse> GetBucketVersioningAsync(string bucketName, CancellationToken cancellationToken = default) {
            return chosenClient.GetBucketVersioningAsync(bucketName, cancellationToken);
        }

        public Task<GetBucketVersioningResponse> GetBucketVersioningAsync(GetBucketVersioningRequest request, CancellationToken cancellationToken = default) {
            return chosenClient.GetBucketVersioningAsync(request, cancellationToken);
        }

        public Task<GetBucketWebsiteResponse> GetBucketWebsiteAsync(string bucketName, CancellationToken cancellationToken = default) {
            return chosenClient.GetBucketWebsiteAsync(bucketName, cancellationToken);
        }

        public Task<GetBucketWebsiteResponse> GetBucketWebsiteAsync(GetBucketWebsiteRequest request, CancellationToken cancellationToken = default) {
            return chosenClient.GetBucketWebsiteAsync(request, cancellationToken);
        }

        public Task<GetCORSConfigurationResponse> GetCORSConfigurationAsync(string bucketName, CancellationToken cancellationToken = default) {
            return chosenClient.GetCORSConfigurationAsync(bucketName, cancellationToken);
        }

        public Task<GetCORSConfigurationResponse> GetCORSConfigurationAsync(GetCORSConfigurationRequest request, CancellationToken cancellationToken = default) {
            return chosenClient.GetCORSConfigurationAsync(request, cancellationToken);
        }

        public Task<GetLifecycleConfigurationResponse> GetLifecycleConfigurationAsync(string bucketName, CancellationToken cancellationToken = default) {
            return chosenClient.GetLifecycleConfigurationAsync(bucketName, cancellationToken);
        }

        public Task<GetLifecycleConfigurationResponse> GetLifecycleConfigurationAsync(GetLifecycleConfigurationRequest request, CancellationToken cancellationToken = default) {
            return chosenClient.GetLifecycleConfigurationAsync(request, cancellationToken);
        }

        public Task<GetObjectResponse> GetObjectAsync(string bucketName, string key, CancellationToken cancellationToken = default) {
            return chosenClient.GetObjectAsync(bucketName, key, cancellationToken);
        }

        public Task<GetObjectResponse> GetObjectAsync(string bucketName, string key, string versionId, CancellationToken cancellationToken = default) {
            return chosenClient.GetObjectAsync(bucketName, key, versionId, cancellationToken);
        }

        public Task<GetObjectResponse> GetObjectAsync(GetObjectRequest request, CancellationToken cancellationToken = default) {
            return chosenClient.GetObjectAsync(request, cancellationToken);
        }

        public Task<GetObjectLegalHoldResponse> GetObjectLegalHoldAsync(GetObjectLegalHoldRequest request, CancellationToken cancellationToken = default) {
            return chosenClient.GetObjectLegalHoldAsync(request, cancellationToken);
        }

        public Task<GetObjectLockConfigurationResponse> GetObjectLockConfigurationAsync(GetObjectLockConfigurationRequest request, CancellationToken cancellationToken = default) {
            return chosenClient.GetObjectLockConfigurationAsync(request, cancellationToken);
        }

        public Task<GetObjectMetadataResponse> GetObjectMetadataAsync(string bucketName, string key, CancellationToken cancellationToken = default) {
            return chosenClient.GetObjectMetadataAsync(bucketName, key, cancellationToken);
        }

        public Task<GetObjectMetadataResponse> GetObjectMetadataAsync(string bucketName, string key, string versionId, CancellationToken cancellationToken = default) {
            return chosenClient.GetObjectMetadataAsync(bucketName, key, versionId, cancellationToken);
        }

        public Task<GetObjectMetadataResponse> GetObjectMetadataAsync(GetObjectMetadataRequest request, CancellationToken cancellationToken = default) {
            return chosenClient.GetObjectMetadataAsync(request, cancellationToken);
        }

        public Task<GetObjectRetentionResponse> GetObjectRetentionAsync(GetObjectRetentionRequest request, CancellationToken cancellationToken = default) {
            return chosenClient.GetObjectRetentionAsync(request, cancellationToken);
        }

        public Task<Stream> GetObjectStreamAsync(string bucketName, string objectKey, IDictionary<string, object> additionalProperties, CancellationToken cancellationToken = default) {
            return chosenClient.GetObjectStreamAsync(bucketName, objectKey, additionalProperties, cancellationToken);
        }

        public Task<GetObjectTaggingResponse> GetObjectTaggingAsync(GetObjectTaggingRequest request, CancellationToken cancellationToken = default) {
            return chosenClient.GetObjectTaggingAsync(request, cancellationToken);
        }

        public Task<GetObjectTorrentResponse> GetObjectTorrentAsync(string bucketName, string key, CancellationToken cancellationToken = default) {
            return chosenClient.GetObjectTorrentAsync(bucketName, key, cancellationToken);
        }

        public Task<GetObjectTorrentResponse> GetObjectTorrentAsync(GetObjectTorrentRequest request, CancellationToken cancellationToken = default) {
            return chosenClient.GetObjectTorrentAsync(request, cancellationToken);
        }

        public string GetPreSignedURL(GetPreSignedUrlRequest request) {
            return chosenClient.GetPreSignedURL(request);
        }

        public Task<GetPublicAccessBlockResponse> GetPublicAccessBlockAsync(GetPublicAccessBlockRequest request, CancellationToken cancellationToken = default) {
            return chosenClient.GetPublicAccessBlockAsync(request, cancellationToken);
        }

        public Task<InitiateMultipartUploadResponse> InitiateMultipartUploadAsync(string bucketName, string key, CancellationToken cancellationToken = default) {
            return chosenClient.InitiateMultipartUploadAsync(bucketName, key, cancellationToken);
        }

        public Task<InitiateMultipartUploadResponse> InitiateMultipartUploadAsync(InitiateMultipartUploadRequest request, CancellationToken cancellationToken = default) {
            return chosenClient.InitiateMultipartUploadAsync(request, cancellationToken);
        }

        public Task<ListBucketAnalyticsConfigurationsResponse> ListBucketAnalyticsConfigurationsAsync(ListBucketAnalyticsConfigurationsRequest request, CancellationToken cancellationToken = default) {
            return chosenClient.ListBucketAnalyticsConfigurationsAsync(request, cancellationToken);
        }

        public Task<ListBucketInventoryConfigurationsResponse> ListBucketInventoryConfigurationsAsync(ListBucketInventoryConfigurationsRequest request, CancellationToken cancellationToken = default) {
            return chosenClient.ListBucketInventoryConfigurationsAsync(request, cancellationToken);
        }

        public Task<ListBucketMetricsConfigurationsResponse> ListBucketMetricsConfigurationsAsync(ListBucketMetricsConfigurationsRequest request, CancellationToken cancellationToken = default) {
            return chosenClient.ListBucketMetricsConfigurationsAsync(request, cancellationToken);
        }

        public Task<ListBucketsResponse> ListBucketsAsync(CancellationToken cancellationToken = default) {
            return chosenClient.ListBucketsAsync(cancellationToken);
        }

        public Task<ListBucketsResponse> ListBucketsAsync(ListBucketsRequest request, CancellationToken cancellationToken = default) {
            return chosenClient.ListBucketsAsync(request, cancellationToken);
        }

        public Task<ListMultipartUploadsResponse> ListMultipartUploadsAsync(string bucketName, CancellationToken cancellationToken = default) {
            return chosenClient.ListMultipartUploadsAsync(bucketName, cancellationToken);
        }

        public Task<ListMultipartUploadsResponse> ListMultipartUploadsAsync(string bucketName, string prefix, CancellationToken cancellationToken = default) {
            return chosenClient.ListMultipartUploadsAsync(bucketName, prefix, cancellationToken);
        }

        public Task<ListMultipartUploadsResponse> ListMultipartUploadsAsync(ListMultipartUploadsRequest request, CancellationToken cancellationToken = default) {
            return chosenClient.ListMultipartUploadsAsync(request, cancellationToken);
        }

        public Task<ListObjectsResponse> ListObjectsAsync(string bucketName, CancellationToken cancellationToken = default) {
            return chosenClient.ListObjectsAsync(bucketName, cancellationToken);
        }

        public Task<ListObjectsResponse> ListObjectsAsync(string bucketName, string prefix, CancellationToken cancellationToken = default) {
            return chosenClient.ListObjectsAsync(bucketName, prefix, cancellationToken);
        }

        public Task<ListObjectsResponse> ListObjectsAsync(ListObjectsRequest request, CancellationToken cancellationToken = default) {
            return chosenClient.ListObjectsAsync(request, cancellationToken);
        }

        public Task<ListObjectsV2Response> ListObjectsV2Async(ListObjectsV2Request request, CancellationToken cancellationToken = default) {
            return chosenClient.ListObjectsV2Async(request, cancellationToken);
        }

        public Task<ListPartsResponse> ListPartsAsync(string bucketName, string key, string uploadId, CancellationToken cancellationToken = default) {
            return chosenClient.ListPartsAsync(bucketName, key, uploadId, cancellationToken);
        }

        public Task<ListPartsResponse> ListPartsAsync(ListPartsRequest request, CancellationToken cancellationToken = default) {
            return chosenClient.ListPartsAsync(request, cancellationToken);
        }

        public Task<ListVersionsResponse> ListVersionsAsync(string bucketName, CancellationToken cancellationToken = default) {
            return chosenClient.ListVersionsAsync(bucketName, cancellationToken);
        }

        public Task<ListVersionsResponse> ListVersionsAsync(string bucketName, string prefix, CancellationToken cancellationToken = default) {
            return chosenClient.ListVersionsAsync(bucketName, prefix, cancellationToken);
        }

        public Task<ListVersionsResponse> ListVersionsAsync(ListVersionsRequest request, CancellationToken cancellationToken = default) {
            return chosenClient.ListVersionsAsync(request, cancellationToken);
        }

        public Task MakeObjectPublicAsync(string bucketName, string objectKey, bool enable) {
            return chosenClient.MakeObjectPublicAsync(bucketName, objectKey, enable);
        }

        public Task<PutACLResponse> PutACLAsync(PutACLRequest request, CancellationToken cancellationToken = default) {
            return chosenClient.PutACLAsync(request, cancellationToken);
        }

        public Task<PutBucketAccelerateConfigurationResponse> PutBucketAccelerateConfigurationAsync(PutBucketAccelerateConfigurationRequest request, CancellationToken cancellationToken = default) {
            return chosenClient.PutBucketAccelerateConfigurationAsync(request, cancellationToken);
        }

        public Task<PutBucketAnalyticsConfigurationResponse> PutBucketAnalyticsConfigurationAsync(PutBucketAnalyticsConfigurationRequest request, CancellationToken cancellationToken = default) {
            return chosenClient.PutBucketAnalyticsConfigurationAsync(request, cancellationToken);
        }

        public Task<PutBucketResponse> PutBucketAsync(string bucketName, CancellationToken cancellationToken = default) {
            return chosenClient.PutBucketAsync(bucketName, cancellationToken);
        }

        public Task<PutBucketResponse> PutBucketAsync(PutBucketRequest request, CancellationToken cancellationToken = default) {
            return chosenClient.PutBucketAsync(request, cancellationToken);
        }

        public Task<PutBucketEncryptionResponse> PutBucketEncryptionAsync(PutBucketEncryptionRequest request, CancellationToken cancellationToken = default) {
            return chosenClient.PutBucketEncryptionAsync(request, cancellationToken);
        }

        public Task<PutBucketInventoryConfigurationResponse> PutBucketInventoryConfigurationAsync(PutBucketInventoryConfigurationRequest request, CancellationToken cancellationToken = default) {
            return chosenClient.PutBucketInventoryConfigurationAsync(request, cancellationToken);
        }

        public Task<PutBucketLoggingResponse> PutBucketLoggingAsync(PutBucketLoggingRequest request, CancellationToken cancellationToken = default) {
            return chosenClient.PutBucketLoggingAsync(request, cancellationToken);
        }

        public Task<PutBucketMetricsConfigurationResponse> PutBucketMetricsConfigurationAsync(PutBucketMetricsConfigurationRequest request, CancellationToken cancellationToken = default) {
            return chosenClient.PutBucketMetricsConfigurationAsync(request, cancellationToken);
        }

        public Task<PutBucketNotificationResponse> PutBucketNotificationAsync(PutBucketNotificationRequest request, CancellationToken cancellationToken = default) {
            return chosenClient.PutBucketNotificationAsync(request, cancellationToken);
        }

        public Task<PutBucketPolicyResponse> PutBucketPolicyAsync(string bucketName, string policy, CancellationToken cancellationToken = default) {
            return chosenClient.PutBucketPolicyAsync(bucketName, policy, cancellationToken);
        }

        public Task<PutBucketPolicyResponse> PutBucketPolicyAsync(string bucketName, string policy, string contentMD5, CancellationToken cancellationToken = default) {
            return chosenClient.PutBucketPolicyAsync(bucketName, policy, contentMD5, cancellationToken);
        }

        public Task<PutBucketPolicyResponse> PutBucketPolicyAsync(PutBucketPolicyRequest request, CancellationToken cancellationToken = default) {
            return chosenClient.PutBucketPolicyAsync(request, cancellationToken);
        }

        public Task<PutBucketReplicationResponse> PutBucketReplicationAsync(PutBucketReplicationRequest request, CancellationToken cancellationToken = default) {
            return chosenClient.PutBucketReplicationAsync(request, cancellationToken);
        }

        public Task<PutBucketRequestPaymentResponse> PutBucketRequestPaymentAsync(string bucketName, RequestPaymentConfiguration requestPaymentConfiguration, CancellationToken cancellationToken = default) {
            return chosenClient.PutBucketRequestPaymentAsync(bucketName, requestPaymentConfiguration, cancellationToken);
        }

        public Task<PutBucketRequestPaymentResponse> PutBucketRequestPaymentAsync(PutBucketRequestPaymentRequest request, CancellationToken cancellationToken = default) {
            return chosenClient.PutBucketRequestPaymentAsync(request, cancellationToken);
        }

        public Task<PutBucketTaggingResponse> PutBucketTaggingAsync(string bucketName, List<Tag> tagSet, CancellationToken cancellationToken = default) {
            return chosenClient.PutBucketTaggingAsync(bucketName, tagSet, cancellationToken);
        }

        public Task<PutBucketTaggingResponse> PutBucketTaggingAsync(PutBucketTaggingRequest request, CancellationToken cancellationToken = default) {
            return chosenClient.PutBucketTaggingAsync(request, cancellationToken);
        }

        public Task<PutBucketVersioningResponse> PutBucketVersioningAsync(PutBucketVersioningRequest request, CancellationToken cancellationToken = default) {
            return chosenClient.PutBucketVersioningAsync(request, cancellationToken);
        }

        public Task<PutBucketWebsiteResponse> PutBucketWebsiteAsync(string bucketName, WebsiteConfiguration websiteConfiguration, CancellationToken cancellationToken = default) {
            return chosenClient.PutBucketWebsiteAsync(bucketName, websiteConfiguration, cancellationToken);
        }

        public Task<PutBucketWebsiteResponse> PutBucketWebsiteAsync(PutBucketWebsiteRequest request, CancellationToken cancellationToken = default) {
            return chosenClient.PutBucketWebsiteAsync(request, cancellationToken);
        }

        public Task<PutCORSConfigurationResponse> PutCORSConfigurationAsync(string bucketName, CORSConfiguration configuration, CancellationToken cancellationToken = default) {
            return chosenClient.PutCORSConfigurationAsync(bucketName, configuration, cancellationToken);
        }

        public Task<PutCORSConfigurationResponse> PutCORSConfigurationAsync(PutCORSConfigurationRequest request, CancellationToken cancellationToken = default) {
            return chosenClient.PutCORSConfigurationAsync(request, cancellationToken);
        }

        public Task<PutLifecycleConfigurationResponse> PutLifecycleConfigurationAsync(string bucketName, LifecycleConfiguration configuration, CancellationToken cancellationToken = default) {
            return chosenClient.PutLifecycleConfigurationAsync(bucketName, configuration, cancellationToken);
        }

        public Task<PutLifecycleConfigurationResponse> PutLifecycleConfigurationAsync(PutLifecycleConfigurationRequest request, CancellationToken cancellationToken = default) {
            return chosenClient.PutLifecycleConfigurationAsync(request, cancellationToken);
        }

        public Task<PutObjectResponse> PutObjectAsync(PutObjectRequest request, CancellationToken cancellationToken = default) {
            return chosenClient.PutObjectAsync(request, cancellationToken);
        }

        public Task<PutObjectLegalHoldResponse> PutObjectLegalHoldAsync(PutObjectLegalHoldRequest request, CancellationToken cancellationToken = default) {
            return chosenClient.PutObjectLegalHoldAsync(request, cancellationToken);
        }

        public Task<PutObjectLockConfigurationResponse> PutObjectLockConfigurationAsync(PutObjectLockConfigurationRequest request, CancellationToken cancellationToken = default) {
            return chosenClient.PutObjectLockConfigurationAsync(request, cancellationToken);
        }

        public Task<PutObjectRetentionResponse> PutObjectRetentionAsync(PutObjectRetentionRequest request, CancellationToken cancellationToken = default) {
            return chosenClient.PutObjectRetentionAsync(request, cancellationToken);
        }

        public Task<PutObjectTaggingResponse> PutObjectTaggingAsync(PutObjectTaggingRequest request, CancellationToken cancellationToken = default) {
            return chosenClient.PutObjectTaggingAsync(request, cancellationToken);
        }

        public Task<PutPublicAccessBlockResponse> PutPublicAccessBlockAsync(PutPublicAccessBlockRequest request, CancellationToken cancellationToken = default) {
            return chosenClient.PutPublicAccessBlockAsync(request, cancellationToken);
        }

        public Task<RestoreObjectResponse> RestoreObjectAsync(string bucketName, string key, CancellationToken cancellationToken = default) {
            return chosenClient.RestoreObjectAsync(bucketName, key, cancellationToken);
        }

        public Task<RestoreObjectResponse> RestoreObjectAsync(string bucketName, string key, int days, CancellationToken cancellationToken = default) {
            return chosenClient.RestoreObjectAsync(bucketName, key, days, cancellationToken);
        }

        public Task<RestoreObjectResponse> RestoreObjectAsync(string bucketName, string key, string versionId, CancellationToken cancellationToken = default) {
            return chosenClient.RestoreObjectAsync(bucketName, key, versionId, cancellationToken);
        }

        public Task<RestoreObjectResponse> RestoreObjectAsync(string bucketName, string key, string versionId, int days, CancellationToken cancellationToken = default) {
            return chosenClient.RestoreObjectAsync(bucketName, key, versionId, days, cancellationToken);
        }

        public Task<RestoreObjectResponse> RestoreObjectAsync(RestoreObjectRequest request, CancellationToken cancellationToken = default) {
            return chosenClient.RestoreObjectAsync(request, cancellationToken);
        }

        public Task<SelectObjectContentResponse> SelectObjectContentAsync(SelectObjectContentRequest request, CancellationToken cancellationToken = default) {
            return chosenClient.SelectObjectContentAsync(request, cancellationToken);
        }

        public Task UploadObjectFromFilePathAsync(string bucketName, string objectKey, string filepath, IDictionary<string, object> additionalProperties, CancellationToken cancellationToken = default) {
            return chosenClient.UploadObjectFromFilePathAsync(bucketName, objectKey, filepath, additionalProperties, cancellationToken);
        }

        public Task UploadObjectFromStreamAsync(string bucketName, string objectKey, Stream stream, IDictionary<string, object> additionalProperties, CancellationToken cancellationToken = default) {
            return chosenClient.UploadObjectFromStreamAsync(bucketName, objectKey, stream, additionalProperties, cancellationToken);
        }

        public Task<UploadPartResponse> UploadPartAsync(UploadPartRequest request, CancellationToken cancellationToken = default) {
            return chosenClient.UploadPartAsync(request, cancellationToken);
        }
    }
}
