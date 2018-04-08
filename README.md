![alt Umi logo](https://raw.githubusercontent.com/CasperCBroeren/Umi/master/Umi.Core/assets/logoUmi.png)

[![NuGet](https://img.shields.io/badge/Umi.Core-v1.0.2-green.svg)](https://www.nuget.org/packages/Umi.Core/1.0.2)

Because in application development your collection of outgoing urls can feel like a sea of uri's. These Urls aren't easily testable, so then Umi got invented, which is an endpoint manager. Or to put it different; it's like swagger but then for the url's that the application uses. 

Umi is a lightweight approach for using urls in your application but register them collectively so they can be tested and retrieved with ease. Umi has an status page which serves html or JSON. This is handy for manual checks or alerting purposes.  Your (dev)ops will love it.

Umi is a Japanese girl name, meaning sea ;)

![alt Umi status page](https://raw.githubusercontent.com/CasperCBroeren/Umi/master/screenshot2.png)

### Use
First register the endpoint where the option LocatorUrl is optional incase you don't want the route at /umi. Also the authentication is optional but please make use of it anyway you like.

```csharp
app.UseUmi(options =>
            {
                options.LocatorUrl = "/umi"; 
                options.Authentication =  new BasicAuthentication("aladin", "opensesame");
            });
```

Second register your strings or Uri's which are endpoint like this

 ```csharp
 var endpoint1 = new Uri("https://restcountries.eu/rest/v2/currency/eur").RegisterAsEndpoint();
```
More eleborate example where we adjust the expected error code and give a category to our endpoint
 ```csharp
 var endpoint2 = "https://www.googleapis.com/youtube/v3/activities".RegisterAsEndpoint(config =>
                {
                    config.TestAsSuccessStatusCode = HttpStatusCode.BadRequest;
                    config.Category = "YouTube";
                });
 ```
Full example where we adjust also the request before sending and alter the OK after test
 ```csharp
    var endpoint3 = "https://www.googleapis.com/youtube/v2/activities".RegisterAsEndpoint(config =>
            {
                config.TestAsSuccessStatusCode = HttpStatusCode.BadRequest;
                config.Category = "YouTube";
                config.PreTest = (request) => {
                    request.Method = HttpMethod.Post;
                };
                config.PostTest = (response, result) =>
                {
                    result.Ok = response.StatusCode == HttpStatusCode.Ambiguous || response.StatusCode == HttpStatusCode.Continue;
                };
            });
 ```
### Roadmap
- ✓ Get overview page to work in Core (html and json)
- ✓ Test method
- ✓ Nuget package
- ✓ Authentication on the status page 
- .Net Standard variant
