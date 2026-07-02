namespace AraonMC.ViewModels;

/// <summary>Base for top-level page view models. Exposes a header title.</summary>
public abstract class PageViewModelBase : ViewModelBase
{
    public string Title { get; init; } = string.Empty;
}
