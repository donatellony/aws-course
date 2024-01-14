using DynamoDb.Customers.DAL.Contracts.Data;

namespace DynamoDb.Customers.DAL.Repositories;

public interface ICustomerRepository
{
    Task<bool> CreateAsync(CustomerDto customer);

    Task<CustomerDto?> GetAsync(Guid id);

    /// <summary>
    /// SHOULD NOT BE USED IN MOST CASES!!! Retrieves all customers from the DynamoDB table asynchronously.
    /// </summary>
    /// <returns>A task that represents the asynchronous operation. The task result contains a collection of CustomerDto objects.</returns>
    Task<IEnumerable<CustomerDto>> GetAllAsync();

    Task<bool> UpdateAsync(CustomerDto customer, DateTime requestStarted);

    Task<bool> DeleteAsync(Guid id);
}