﻿// See https://aka.ms/new-console-template for more information
using IdentityModel.Client;
using System.Text.Json;

// discover endpoints from metadata
var client = new HttpClient();
var disco = await client.GetDiscoveryDocumentAsync("https://localhost:5001");
if (disco.IsError)
{
    Console.WriteLine(disco.Error);
    return;
}

// request token
var tokenResponse = await client.RequestClientCredentialsTokenAsync(new ClientCredentialsTokenRequest
{
    Address = disco.TokenEndpoint,

    ClientId = "client",
    ClientSecret = "secret",
    Scope = "api1 api2"
});

if (tokenResponse.IsError)
{
    Console.WriteLine(tokenResponse.Error);
    return;
}

Console.WriteLine(tokenResponse.AccessToken);

// call api
var apiClient = new HttpClient();
apiClient.SetBearerToken(tokenResponse.AccessToken);

Console.ForegroundColor = ConsoleColor.Yellow;
Console.WriteLine("Running against NET7 API");
Console.ResetColor();

var coreResponse = await apiClient.GetAsync("https://localhost:6001/identity");

if (!coreResponse.IsSuccessStatusCode)
{
    Console.WriteLine($"Core response: {coreResponse.StatusCode}");
}
else
{    
     var doc = JsonDocument.Parse(await coreResponse.Content.ReadAsStringAsync()).RootElement;
     Console.WriteLine(JsonSerializer.Serialize(doc, new JsonSerializerOptions { WriteIndented = true }));
}

Console.ForegroundColor = ConsoleColor.Yellow;
Console.WriteLine("Running against NET Framework 4.8 API");
Console.ResetColor();

var response = await apiClient.GetAsync("https://localhost:44380/api/identity?type=json");

if (!response.IsSuccessStatusCode)
{
    Console.WriteLine(response.StatusCode);
}
else
{    
    var doc48 = JsonDocument.Parse(await response.Content.ReadAsStringAsync()).RootElement;
    Console.WriteLine(JsonSerializer.Serialize(doc48, new JsonSerializerOptions { WriteIndented = true }));
}

Console.ReadLine();
