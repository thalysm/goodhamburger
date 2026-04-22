namespace GoodHamburger.Frontend.Services;

public class SearchService
{
    private string _searchQuery = string.Empty;

    public string SearchQuery
    {
        get => _searchQuery;
        set
        {
            if (_searchQuery != value)
            {
                _searchQuery = value;
                OnSearchChanged?.Invoke(_searchQuery);
            }
        }
    }

    public event Action<string>? OnSearchChanged;
}
