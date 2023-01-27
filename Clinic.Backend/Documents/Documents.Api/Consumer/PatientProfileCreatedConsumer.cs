using System.Text.Json;
using MassTransit;
using SharedModels;

namespace Documents.Api.Consumer;

public class PatientProfileCreatedConsumer : IConsumer<PatientProfileCreated>
{
    public async Task Consume(ConsumeContext<PatientProfileCreated> context)
    {
        var jsonMessage = JsonSerializer.Serialize(context.Message);
        Console.WriteLine($"PatientProfileCreated message: {jsonMessage}");
    }
}