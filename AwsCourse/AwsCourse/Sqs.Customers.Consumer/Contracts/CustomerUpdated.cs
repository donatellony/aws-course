using MediatR;

namespace Sqs.Customers.Consumer.Contracts;

public class CustomerUpdated : Customers.Contracts.Contracts.Messages.CustomerUpdated, IRequest;