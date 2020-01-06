namespace VS.Abstractions.Storage.Paging {

    using System;
    using System.Net.Mime;
    
    public class IndexItem {

        public IndexItem(Uri location, ContentDisposition contentDisposition, ContentType contentType) {
            Location = location;
            ContentDisposition = contentDisposition;
            ContentType = contentType;
        }

        [UriMustBeRelative]
        public Uri Location { get; }

        public ContentDisposition ContentDisposition { get; }

        public ContentType ContentType { get; }
    }
}
