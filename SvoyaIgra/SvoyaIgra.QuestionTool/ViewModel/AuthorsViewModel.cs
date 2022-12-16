using System.Collections.ObjectModel;
using System.Linq;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using SvoyaIgra.Dal.Dto;
using SvoyaIgra.Dal.Services;

namespace SvoyaIgra.QuestionTool.ViewModel;

[INotifyPropertyChanged]
public partial class AuthorsViewModel
{
    [ObservableProperty] 
    public ObservableCollection<AuthorDto> _authors = new ObservableCollection<AuthorDto>();

    [ObservableProperty]
    private string _newAuthorName;

    private IAuthorService _authorService;

    public AuthorsViewModel(IAuthorService authorService)
    {
        _authorService = authorService;
    }

    [RelayCommand]
    private async void LoadAuthors(object obj)
    {
        var authors = await _authorService.GetAllAuthorsAsync();
        Authors = new ObservableCollection<AuthorDto>(authors);
    }

    [RelayCommand]
    private async void CreateAuthor(object obj)
    {
        if (string.IsNullOrWhiteSpace(NewAuthorName)) return;
        if (Authors.Count == 0) return;
        if (Authors.FirstOrDefault(x => x.Name == NewAuthorName) != null) return;
        
        var author = await _authorService.CreateAuthorAsync(NewAuthorName);
        Authors.Add(author);
        NewAuthorName = "";
    }
}