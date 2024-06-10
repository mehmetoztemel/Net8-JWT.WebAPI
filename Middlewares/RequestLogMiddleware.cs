
using System.Net;
using System.Text;

namespace Net8_JWT.WebAPI.Middlewares
{
    public class RequestLogMiddleware
    {

        private readonly RequestDelegate _next;

        private string FolderName = "Logs";
        private string FileName = string.Empty;
        public RequestLogMiddleware(RequestDelegate next)
        {
            _next = next;
            FileName = $"{DateTime.Now.ToString("yyyy-MM-dd")}-Log.txt";
        }

        public async Task InvokeAsync(HttpContext context)
        {
            IPAddress remoteIp = context.Connection.RemoteIpAddress!;
            try
            {

                if (!Directory.Exists(Path.Combine(FolderName)))
                {
                    Directory.CreateDirectory(Path.Combine(FolderName));
                }

                //File.AppendText StreamWriter ile aynı işi yapıyor
                using (StreamWriter writer = File.AppendText(Path.Combine(FolderName, FileName)))
                {

                    writer.Write("Request ");
                    writer.Write("\rDate Time: ");
                    writer.WriteLine($"{DateTime.Now.ToLongTimeString()}, {DateTime.Now.ToLongDateString()}");
                    writer.WriteLine($"IP Address: {remoteIp.ToString()}");
                    writer.WriteLine($"TraceId: {context.TraceIdentifier}");
                    writer.WriteLine($"Path: {context.Request.Path}");
                    writer.WriteLine($"Method: {context.Request.Method}");
                    writer.WriteLine($"QueryString: {context.Request.QueryString}");
                    writer.WriteLine($"Body: {await FormatRequest(context.Request)}");
                    writer.WriteLine("---------------------------------------------------------------------------");
                }
                await _next(context);
            }
            catch (Exception ex)
            {
                using (StreamWriter writer = File.AppendText(Path.Combine(FolderName, FileName)))
                {
                    writer.Write("Exception ");
                    writer.Write("\rDate Time: ");
                    writer.WriteLine($"{DateTime.Now.ToLongTimeString()}, {DateTime.Now.ToLongDateString()}");
                    writer.WriteLine($"IP Address: {remoteIp.ToString()}");
                    writer.WriteLine($"TraceId: {context.TraceIdentifier}");
                    writer.WriteLine($"Path: {context.Request.Path}");
                    writer.WriteLine($"Method: {context.Request.Method}");
                    writer.WriteLine($"QueryString: {context.Request.QueryString}");
                    writer.WriteLine($"ErrorMessage: {ex.Message}");
                    writer.WriteLine("---------------------------------------------------------------------------");
                }
            }
        }
        private async Task<string> FormatRequest(HttpRequest request)
        {
            // request'i okuyabilmek için bu özelliği aktif ediyoruz.
            request.EnableBuffering();
            // request body'nin yedeğini alıyoruz.
            var backupBody = request.Body;
            // burada streami okuyoruz.
            var buffer = new byte[Convert.ToInt32(request.ContentLength)];
            // okumuş olduğumuz requesti buffera kopyalıyoruz.
            await request.Body.ReadAsync(buffer, 0, buffer.Length);
            //okumuş olduğumuz requesti burada string'e çeviriyoruz.
            var requestBody = Encoding.UTF8.GetString(buffer);
            backupBody.Seek(0, SeekOrigin.Begin);
            // almış olduğumuz yedeği burada set ediyoruz.
            request.Body = backupBody;
            return requestBody;
        }
    }
}
