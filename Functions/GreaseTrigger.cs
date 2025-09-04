using System.IO;
using System.Text.Json;
using System.Threading.Tasks;
using Azure.Cosmos;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;

namespace CosmosFunctions;

public class GreaseTrigger
{
    private readonly ILogger<GreaseTrigger> _logger;
    private readonly CosmosClient _cosmosClient;

    public GreaseTrigger(ILogger<GreaseTrigger> logger, CosmosClient cosmosClient)
    {
        _logger = logger;
        _cosmosClient = cosmosClient;
    }

    [Function("GreaseTrigger")]
    public async Task<HttpResponseData> Run(
    [HttpTrigger(AuthorizationLevel.Function, "post")] HttpRequestData req,
    FunctionContext executionContext)
    {
        var requestBody = await new StreamReader(req.Body).ReadToEndAsync();
        var item = JsonSerializer.Deserialize<TestItem>(requestBody);

        var container = _cosmosClient.GetContainer("greasecontainerdb", "greasecontainer");
        await container.CreateItemAsync(item, new PartitionKey(item.greasereviews));

        var response = req.CreateResponse(System.Net.HttpStatusCode.OK);
        await response.WriteAsJsonAsync(item);
        return response;
    }
}

public class TestItem
{
    public string id { get; set; } = Guid.NewGuid().ToString();
    public string greasereviews { get; set; } = "";
    public int Age { get; set; }
}
