![alt Umi logo](https://raw.githubusercontent.com/CasperCBroeren/Umi/master/Umi.Core/assets/logoUmi.png)

Because in application development your collection of urls can feel like a sea of Uri's which are not easily testable, I invented an endpoint manager called Umi.

Umi is a lightweight approach for using urls in your application but register them collectively so they can be tested and retrieved with ease. 
Umi has an status page which serves html or JSON. This is handy for manual checks or alerting purposes.

Umi is a Japanese girl name, meaning sea ;)

![alt Umi status page](https://raw.githubusercontent.com/CasperCBroeren/Umi/master/screenshot1.png)

### Use
First register the endpoint where the option LocatorUrl is optional incase you don't want the route at /umi

```csharp
app.UseUmi(options =>
            {
                options.LocatorUrl = "/umi"; 
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
- Nuget package
- Authentication on the status page 
- .Net Standard variant
