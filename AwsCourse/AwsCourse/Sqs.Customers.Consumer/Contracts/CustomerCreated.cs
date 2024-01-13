using MediatR;

namespace Sqs.Customers.Consumer.Contracts;

public class CustomerCreated : Customers.Contracts.Contracts.Messages.CustomerCreated, IRequest;