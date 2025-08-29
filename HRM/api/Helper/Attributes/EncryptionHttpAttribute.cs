using System.Security.Cryptography;
using System.Text;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Options;
namespace API.Helper.Attributes
{
    public class SystemSettings
    {
        public string DataEncryptionKey = "Super secret key";
    }
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class EncryptionHttpAttribute : Attribute, IFilterFactory
    {
        public bool IsReusable => false;

        public IFilterMetadata CreateInstance(IServiceProvider serviceProvider)
        {
            var config = serviceProvider.GetRequiredService<IOptions<SystemSettings>>();
            return new EncryptionHttpFilter(config.Value);
        }
    }
    public class EncryptionHttpFilter : IAsyncResourceFilter
    {
        private readonly Aes _cryptoProvider;

        public EncryptionHttpFilter(SystemSettings settings)
        {
            _cryptoProvider = InitializeAes(settings.DataEncryptionKey);
        }
        private static Aes InitializeAes(string secretKey)
        {
            var paddedKey = secretKey.PadRight(32, '0');
            var aes = Aes.Create();
            aes.Key = Encoding.UTF8.GetBytes(paddedKey[..32]);
            aes.IV = Encoding.UTF8.GetBytes(paddedKey[..16]);
            aes.Mode = CipherMode.CBC;
            aes.Padding = PaddingMode.PKCS7;
            return aes;
        }
        public async Task OnResourceExecutionAsync(ResourceExecutingContext context, ResourceExecutionDelegate next)
        {
            context.HttpContext.Response.Body = EncryptOutputStream(context.HttpContext.Response.Body);
            context.HttpContext.Request.Body = DecryptInputStream(context.HttpContext.Request.Body);
            if (context.HttpContext.Request.QueryString.HasValue)
            {
                var paramStr = context.HttpContext.Request.QueryString.Value[1..];
                var decodedQuery = DecodeString(paramStr);
                context.HttpContext.Request.QueryString = new QueryString($"?{decodedQuery}");
            }
            await next();
            await context.HttpContext.Request.Body.DisposeAsync();
            await context.HttpContext.Response.Body.DisposeAsync();
        }

        private CryptoStream EncryptOutputStream(Stream outputStream)
        {
            var encryptor = _cryptoProvider.CreateEncryptor();
            var base64Transform = new ToBase64Transform();
            var encodedStream = new CryptoStream(outputStream, base64Transform, CryptoStreamMode.Write);
            return new CryptoStream(encodedStream, encryptor, CryptoStreamMode.Write);
        }

        private CryptoStream DecryptInputStream(Stream inputStream)
        {
            var decryptor = _cryptoProvider.CreateDecryptor();
            var base64Transform = new FromBase64Transform(FromBase64TransformMode.IgnoreWhiteSpaces);
            var decodedStream = new CryptoStream(inputStream, base64Transform, CryptoStreamMode.Read);
            return new CryptoStream(decodedStream, decryptor, CryptoStreamMode.Read);
        }

        private string DecodeString(string encryptedText)
        {
            using var memoryStream = new MemoryStream(Convert.FromBase64String(encryptedText));
            using var cryptoStream = new CryptoStream(memoryStream, _cryptoProvider.CreateDecryptor(), CryptoStreamMode.Read);
            using var reader = new StreamReader(cryptoStream);
            return reader.ReadToEnd();
        }
    }
}
