namespace Offices.Infrastructure.Models;

public class OfficeDatabaseSettings
{
    public string ConnectionString { get; set; } = null!;

    public string DatabaseName { get; set; } = null!;

    public string OfficesCollectionName { get; set; } = null!;
}