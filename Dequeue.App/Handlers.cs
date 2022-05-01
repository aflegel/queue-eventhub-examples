using System;
using System.Text;
using System.Threading.Tasks;
using Azure.Storage.Blobs;
using Azure.Messaging.EventHubs;
using Azure.Messaging.EventHubs.Consumer;
using Azure.Messaging.EventHubs.Processor;

namespace Dequeue.App;

internal static class QueueHandlers
{
	public static async Task ProcessEventHandler(ProcessEventArgs eventArgs)
	{
		// Write the body of the event to the console window
		Console.WriteLine("\tReceived event: {0}", Encoding.UTF8.GetString(eventArgs.Data.Body.ToArray()));

		// Update checkpoint in the blob storage so that the app receives only new events the next time it's run
		await eventArgs.UpdateCheckpointAsync(eventArgs.CancellationToken);
	}

	public static Task ProcessErrorHandler(ProcessErrorEventArgs eventArgs)
	{
		// Write details about the error to the console window
		Console.WriteLine($"\tPartition '{eventArgs.PartitionId}': an unhandled exception was encountered. This was not expected to happen.");
		Console.WriteLine(eventArgs.Exception.Message);
		return Task.CompletedTask;
	}
}