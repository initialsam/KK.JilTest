using System;
using System.Net.Http.Formatting;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Jil;
using System.Text;
using System.IO;
using System.Net.Http;
using System.Net;

namespace KK.JilTest.Formatters
{
    // Ref: https://github.com/bmbsqd/jil-mediaformatter
    // Ref: http://blog.developers.ba/replace-json-net-jil-json-serializer-asp-net-web-api/
    public class JilFormatter : MediaTypeFormatter
    {
        private static readonly MediaTypeHeaderValue applicationJsonMediaType = new MediaTypeHeaderValue("application/json");
        private static readonly MediaTypeHeaderValue textJsonMediaType = new MediaTypeHeaderValue("text/json");
        private static readonly Task<bool> done = Task.FromResult(true);

        private readonly Options options;

        public JilFormatter(Options options)
        {
            this.options = options;
            SupportedMediaTypes.Add(applicationJsonMediaType);
            SupportedMediaTypes.Add(textJsonMediaType);

            SupportedEncodings.Add(new UTF8Encoding(false, true));
            SupportedEncodings.Add(new UnicodeEncoding(false, true, true));
        }

        public JilFormatter() : this( GetDefaultOptions() ) { }

        private static Options GetDefaultOptions()
        {
            return new Options(dateFormat: DateTimeFormat.ISO8601);
        }

        public override bool CanReadType(Type type)
        {
            if (type == null)
            {
                throw new ArgumentNullException("type");
            }
            return true;
        }

        public override bool CanWriteType(Type type)
        {
            if (type == null)
            {
                throw new ArgumentNullException("type");
            }
            return true;
        }

        public override Task<object> ReadFromStreamAsync(Type type, Stream readStream, HttpContent content, IFormatterLogger formatterLogger)
        {
            var reader = new StreamReader(readStream);
            var deserialize = TypedDeserializers.GetTyped(type);
            var result = deserialize(reader, options);
            return Task.FromResult(result);
        }

        public override Task WriteToStreamAsync(Type type, object value, Stream writeStream, HttpContent content, TransportContext transportContext)
        {
            var writer = new StreamWriter(writeStream);
            JSON.Serialize(value, writer, options);
            writer.Flush();
            return done;
        }
    }
}