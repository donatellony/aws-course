using Sqs.Customers.Contracts.Contracts.Messages;
using Sqs.Customers.Publisher.Lib.Domain;

namespace Sqs.Customers.Publisher.Lib.Mapping;

public static class DomainToMessageMapper
{
    public static CustomerCreated ToCustomerCreatedMessage(this Customer customer)
    {
        return new CustomerCreated
        {
            Id = customer.Id,
            GitHubUsername = customer.GitHubUsername,
            FullName = customer.FullName,
            Email = customer.Email,
            DateOfBirth = customer.DateOfBirth
        };
    }
    
    public static CustomerUpdated ToCustomerUpdatedMessage(this Customer customer)
    {
        return new CustomerUpdated
        {
            Id = customer.Id,
            GitHubUsername = customer.GitHubUsername,
            FullName = customer.FullName,
            Email = customer.Email,
            DateOfBirth = customer.DateOfBirth
        };
    }
    
    public static CustomerDeleted ToCustomerDeletedMessage(this Customer customer)
    {
        return new CustomerDeleted
        {
            Id = customer.Id,
            GitHubUsername = customer.GitHubUsername,
            FullName = customer.FullName,
            Email = customer.Email,
            DateOfBirth = customer.DateOfBirth
        };
    }
}