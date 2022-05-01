using System;
using System.Text;
using System.Threading.Tasks;
using Azure.Storage.Blobs;
using Azure.Messaging.EventHubs;
using Azure.Messaging.EventHubs.Consumer;
using Azure.Messaging.EventHubs.Processor;
using Dequeue.App;

const string ehubNamespaceConnectionString = "<EVENT HUBS NAMESPACE - CONNECTION STRING>";
const string eventHubName = "<EVENT HUB NAME>";
const string blobStorageConnectionString = "<AZURE STORAGE CONNECTION STRING>";
const string blobContainerName = "<BLOB CONTAINER NAME>";

// The Event Hubs client types are safe to cache and use as a singleton for the lifetime
// of the application, which is best practice when events are being published or read regularly.        

// Read from the default consumer group: $Default
string consumerGroup = EventHubConsumerClient.DefaultConsumerGroupName;

// Create a blob container client that the event processor will use 
var storageClient = new BlobContainerClient(blobStorageConnectionString, blobContainerName);

// Create an event processor client to process events in the event hub
var processor = new EventProcessorClient(storageClient, consumerGroup, ehubNamespaceConnectionString, eventHubName);

// Register handlers for processing events and handling errors
processor.ProcessEventAsync += QueueHandlers.ProcessEventHandler;
processor.ProcessErrorAsync += QueueHandlers.ProcessErrorHandler;

// Start the processing
await processor.StartProcessingAsync();

// Wait for 30 seconds for the events to be processed
await Task.Delay(TimeSpan.FromSeconds(30));

// Stop the processing
await processor.StopProcessingAsync();
