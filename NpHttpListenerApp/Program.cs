using System.Net;
using System.Text;

string serverUrl = "http://127.0.0.1:5000/";

using (HttpListener listener = new())
{
    listener.Prefixes.Add(serverUrl);

    listener.Start();

    var context = await listener.GetContextAsync();

    var request = context.Request;
    var response = context.Response;
    var user = context.User;

    Console.WriteLine($"Address app: {request.LocalEndPoint}");
    Console.WriteLine($"Address client: {request.RemoteEndPoint}");
    Console.WriteLine($"Raw Url: {request.RawUrl}");
    Console.WriteLine($"Url: {request.Url}");

    Console.WriteLine($"Headers of request:");
    foreach(string key in request.Headers.Keys)
    {
        Console.WriteLine($"{key}: {request.Headers[key]}");
    }
    Console.WriteLine();

    string htmlText = @"<!DOCTYPE html>
                        <html lang=""en"">
                        <head>
                            <meta charset=""UTF-8"">
                            <meta name=""viewport"" content=""width=device-width, initial-scale=1.0"">
                            <title>Response Document</title>
                        </head>
                        <body>
                            <h1>Hello!</h1>
                            <h2>Answer from listener!</h2>
                        </body>
                        </html>";

    byte[] buffer = Encoding.UTF8.GetBytes(htmlText);
    response.ContentLength64 = buffer.Length;
    using(Stream output = response.OutputStream)
    {
        await output.WriteAsync(buffer, 0, buffer.Length);
        await output.FlushAsync();
    }

    listener.Stop();
}

