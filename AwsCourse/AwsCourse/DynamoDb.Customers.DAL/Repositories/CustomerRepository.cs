using System.Net;
using System.Text.Json;
using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DocumentModel;
using Amazon.DynamoDBv2.Model;
using Amazon.Runtime;
using DynamoDb.Customers.DAL.Contracts.Data;

namespace DynamoDb.Customers.DAL.Repositories;

public class CustomerRepository : ICustomerRepository
{
    private readonly IAmazonDynamoDB _dynamoDb;
    private readonly string _tableName = "customers"; // TODO - replace with config

    public CustomerRepository(IAmazonDynamoDB dynamoDb)
    {
        _dynamoDb = dynamoDb;
    }

    public async Task<bool> CreateAsync(CustomerDto customer)
    {
        customer.UpdatedAt = DateTime.UtcNow;
        var attributes = MapToItem(customer);
        var createItemRequest = new PutItemRequest
        {
            TableName = _tableName,
            Item = attributes
        };

        var response = await _dynamoDb.PutItemAsync(createItemRequest);
        return response.HttpStatusCode == HttpStatusCode.OK;
    }

    public async Task<CustomerDto?> GetAsync(Guid id)
    {
        var getItemRequest = new GetItemRequest
        {
            TableName = _tableName,
            Key = new Dictionary<string, AttributeValue>
            {
                { "pk", new AttributeValue { S = id.ToString() } },
                { "sk", new AttributeValue { S = id.ToString() } }
            }
        };

        var response = await _dynamoDb.GetItemAsync(getItemRequest);

        return DeserializeGetItemResponse<CustomerDto>(response);
    }

    public async Task<IEnumerable<CustomerDto>> GetAllAsync()
    {
        throw new NotImplementedException();
    }

    public async Task<bool> UpdateAsync(CustomerDto customer)
    {
        customer.UpdatedAt = DateTime.UtcNow;
        var attributes = MapToItem(customer);
        var updateItemRequest = new PutItemRequest
        {
            TableName = _tableName,
            Item = attributes
        };

        var response = await _dynamoDb.PutItemAsync(updateItemRequest);
        return IsPositiveResponse(response);
    }

    public async Task<bool> DeleteAsync(Guid id)
    {
        var deleteItemRequest = new DeleteItemRequest
        {
            TableName = _tableName,
            Key = new Dictionary<string, AttributeValue>
            {
                { "pk", new AttributeValue { S = id.ToString() } },
                { "sk", new AttributeValue { S = id.ToString() } }
            }
        };

        var response = await _dynamoDb.DeleteItemAsync(deleteItemRequest);

        return IsPositiveResponse(response);
    }

    private static Dictionary<string, AttributeValue> MapToItem<T>(T obj)
    {
        var objectAsJson = JsonSerializer.Serialize(obj);
        var objectAsAttributes = Document.FromJson(objectAsJson).ToAttributeMap();
        return objectAsAttributes;
    }

    private static T? DeserializeGetItemResponse<T>(GetItemResponse response) where T : class
    {
        if (response.Item.Count == 0)
        {
            return null;
        }

        var itemAsDocument = Document.FromAttributeMap(response.Item);
        return JsonSerializer.Deserialize<T>(itemAsDocument.ToJson());
    }
    
    private static bool IsPositiveResponse(AmazonWebServiceResponse response)
    {
        return response.HttpStatusCode == HttpStatusCode.OK;
    }
}