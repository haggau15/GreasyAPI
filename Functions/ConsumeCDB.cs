using Azure; // for Page<T> if you use AsPages
using Azure.Cosmos;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;
using System.Net;

namespace CosmosFunctions.Functions;

public class ConsumeCDB
{
    private readonly ILogger<ConsumeCDB> _logger;
    private readonly CosmosClient _cosmosClient;

    public ConsumeCDB(ILogger<ConsumeCDB> logger, CosmosClient cosmosClient)
    {
        _logger = logger;
        _cosmosClient = cosmosClient;
    }

    [Function("ConsumeCDB")]
    public async Task<HttpResponseData> Run(
        [HttpTrigger(AuthorizationLevel.Function, "get", Route = "partition/{greasereviews}")]
        HttpRequestData req,
        string greasereviews,
        FunctionContext ctx)
    {
        var container = _cosmosClient.GetContainer("greasecontainerdb", "greasecontainer");

        var items = new List<PlaceDocument>();
        var query = new QueryDefinition("SELECT * FROM c");
        var options = new QueryRequestOptions { PartitionKey = new PartitionKey(greasereviews) };

        var results = container.GetItemQueryIterator<PlaceDocument>(query, requestOptions: options);

        // Item-by-item:
        await foreach (var doc in results)
        {
            items.Add(doc);
        }

        // If you prefer paging:
        // await foreach (Page<PlaceDocument> page in results.AsPages(pageSizeHint: 100))
        // {
        //     items.AddRange(page.Values);
        //     // string? continuation = page.ContinuationToken;
        // }

        var res = req.CreateResponse(HttpStatusCode.OK);
        await res.WriteAsJsonAsync(items);
        return res;
    }
}
