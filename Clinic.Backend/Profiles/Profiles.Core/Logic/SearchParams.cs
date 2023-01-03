namespace Profiles.Core.Logic;

public class SearchParams
{
    private const int MaxPageSize = 50;
    public int PageNumber { get; set; } = 1;
    private int _pageSize = 3;

    public int PageSize
    {
        get => _pageSize;
        set => _pageSize = (value > MaxPageSize) ? MaxPageSize : value;
    }

    public string FullName { get; set; } = string.Empty;
    public string OrderByExperience { get; set; } = "Upcoming"; // для списка запрашиваемым админом не нужен но есть ли смысл убирать
}