using System.Text;
using Azure.Messaging.EventHubs;
using Azure.Messaging.EventHubs.Producer;

// connection string to the Event Hubs namespace
const string connectionString = "<EVENT HUBS NAMESPACE - CONNECTION STRING>";

// name of the event hub
const string eventHubName = "<EVENT HUB NAME>";

// number of events to be sent to the event hub
const int numOfEvents = 3;

// Create a producer client that you can use to send events to an event hub
var producerClient = new EventHubProducerClient(connectionString, eventHubName);

// Create a batch of events 
using var eventBatch = await producerClient.CreateBatchAsync();

for (int i = 1; i <= numOfEvents; i++)
{
	if (!eventBatch.TryAdd(new EventData(Encoding.UTF8.GetBytes($"Event {i}"))))
	{
		// if it is too large for the batch
		throw new Exception($"Event {i} is too large for the batch and cannot be sent.");
	}
}

try
{
	// Use the producer client to send the batch of events to the event hub
	await producerClient.SendAsync(eventBatch);
	Console.WriteLine($"A batch of {numOfEvents} events has been published.");
}
finally
{
	await producerClient.DisposeAsync();
}