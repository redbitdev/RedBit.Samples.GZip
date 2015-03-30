using System;
using System.IO;
using System.IO.Compression;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;

namespace SharpGIS.Http
{
    public class HttpGZipClientHandler : HttpClientHandler
    {
        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            request.Headers.AcceptEncoding.Add(new StringWithQualityHeaderValue("gzip"));
            var response = await base.SendAsync(request, cancellationToken);
            if (response.Content.Headers.ContentEncoding.Contains("gzip"))
            {
                await response.Content.LoadIntoBufferAsync();
                response.Content = new HttpGZipContent(await response.Content.ReadAsStreamAsync());
            }
            return response;
        }

        public override bool SupportsAutomaticDecompression
        {
            get
            {
                return true;
            }
        }
    }
    internal sealed class HttpGZipContent : HttpContent
    {
        private readonly GZipStream m_stream;
        public HttpGZipContent(Stream deflatedStream)
        {
            m_stream = new System.IO.Compression.GZipStream(deflatedStream, System.IO.Compression.CompressionMode.Decompress);
        }
        protected override Task SerializeToStreamAsync(System.IO.Stream stream, System.Net.TransportContext context)
        {
            return m_stream.CopyToAsync(stream);
        }
        protected override bool TryComputeLength(out long length)
        {
            // since GZipStream does not implement length, we just send backthe base stream length
            length = m_stream.BaseStream.Length;
            return true;
        }

        protected override void Dispose(bool disposing)
        {
            m_stream.Dispose();
            base.Dispose(disposing);
        }
    }
}