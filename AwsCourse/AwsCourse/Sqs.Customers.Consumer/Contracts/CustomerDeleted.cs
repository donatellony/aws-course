using MediatR;

namespace Sqs.Customers.Consumer.Contracts;

public class CustomerDeleted : Customers.Contracts.Contracts.Messages.CustomerUpdated, IRequest;