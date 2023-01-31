using System.Text.Json;
using Documents.Core.Interfaces.Logic;
using Documents.Core.Interfaces.Services;
using MassTransit;
using SharedModels;

namespace Documents.Api.Consumers;

public class AppointmentResultCreatedConsumer : IConsumer<AppointmentResultCreated>
{
    private readonly IPdfGenerator _pdfGenerator;
    private readonly IAzureService _azureService;

    public AppointmentResultCreatedConsumer(IPdfGenerator pdfGenerator, IAzureService azureService)
    {
        _pdfGenerator = pdfGenerator;
        _azureService = azureService;
    }

    public async Task Consume(ConsumeContext<AppointmentResultCreated> context)
    {
        var bytes =await _pdfGenerator.CreatePdfAsync(context.Message.Complaints, context.Message.Conclusion,
            context.Message.Recommendations);
        await _azureService.UploadAppointmentResultDocumentAsync(bytes, context.Message.ResultId);

    }
}