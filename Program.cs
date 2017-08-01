using System;
using System.Net.Http;
using Newtonsoft.Json.Linq;

namespace code_sample
{
    class Program
    {
        static void Main(string[] args)
        {
            var fileName = @"C:\temp\test.jpg";
            var taggunApiKey = "<--ENTER YOUR API KEY HERE -->";
            var taggunApiUrl = "https://api.taggun.io/api/receipt/v1/simple/file";

            byte[] fileData = System.IO.File.ReadAllBytes(fileName);

            var timeStart = DateTime.Now;

            using (var httpClient = new HttpClient { Timeout = new TimeSpan(0, 0, 0, 60, 0) })
            {
                HttpResponseMessage response = null;

                httpClient.DefaultRequestHeaders.TryAddWithoutValidation("apikey", taggunApiKey);

                var parentContent = new MultipartFormDataContent("----WebKitFormBoundaryfzdR3Imh7urK8qw");

                var documentContent = new ByteArrayContent(fileData);
                documentContent.Headers.Remove("Content-Type");
                documentContent.Headers.Remove("Content-Disposition");
                documentContent.Headers.TryAddWithoutValidation("Content-Type", "image/jpeg");
                documentContent.Headers.TryAddWithoutValidation("Content-Disposition",
                string.Format(@"form-data; name=""file""; filename=""{0}""", "testfilename.jpg"));
                parentContent.Add(documentContent);

                var refreshContent = new StringContent("false");
                refreshContent.Headers.Remove("Content-Type");
                refreshContent.Headers.Remove("Content-Disposition");
                refreshContent.Headers.TryAddWithoutValidation("Content-Disposition", @"form-data; name=""refresh""");
                parentContent.Add(refreshContent);

                response = httpClient.PostAsync(taggunApiUrl, parentContent).Result;
                response.EnsureSuccessStatusCode();
                var result = response.Content.ReadAsStringAsync().Result;
                Console.WriteLine(JObject.Parse(result).ToString());
                    
              
            }
        }
    }
}
