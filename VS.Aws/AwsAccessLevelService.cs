
namespace VS.Aws {
    using Amazon.S3;
    using System;
    using VS.Abstractions.Storage;

    public class AwsAccessLevelService : IAccessLevelService<S3CannedACL> {
        public S3CannedACL GetLevel(AccessLevel AccessLevel) {
            switch (AccessLevel) {
                default:
                case AccessLevel.None: return S3CannedACL.NoACL;
                case AccessLevel.Read: return S3CannedACL.PublicRead;
                case AccessLevel.Write: return S3CannedACL.PublicReadWrite;
                case AccessLevel.Delete: return S3CannedACL.PublicReadWrite;
                case AccessLevel.List: return S3CannedACL.PublicRead;
                case AccessLevel.Create: return S3CannedACL.PublicReadWrite;

            };
        }
    }
}
