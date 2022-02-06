const string RootUri = "http://localhost:5075";

using var httpClient = new HttpClient();
httpClient.BaseAddress = new Uri(RootUri);

var csvFileBytes = File.ReadAllBytes("Meter_Reading.csv");
using var byteArrayContent = new ByteArrayContent(csvFileBytes);

using var multipartContent = new MultipartFormDataContent();
multipartContent.Add(byteArrayContent, "meterReadingsFile", "meterReadingsFile");

using var response = await httpClient.PostAsync("meter-reading-uploads", multipartContent);
var meterReadingsResponse = await response.Content.ReadAsStringAsync();

Console.WriteLine($"Api returned status code {response.StatusCode}");
Console.WriteLine(meterReadingsResponse);
Console.ReadLine();
