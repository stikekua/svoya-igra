using System;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using SvoyaIgra.MultimediaProvider.Services;
using System.Collections.Generic;
using System.Linq;
using System.Diagnostics;
using System.IO;
using System.Windows;
using SvoyaIgra.Dal.Dto;
using SvoyaIgra.Dal.Services;

namespace SvoyaIgra.MultimediaViewer.ViewModel;

[INotifyPropertyChanged]
public partial class EditorViewModel
{
    private readonly IMultimediaService _multimediaService;
    private readonly ITopicService _topicService;
    private readonly IQuestionService _questionService;

    [ObservableProperty]
    [NotifyCanExecuteChangedFor(nameof(OpenFolderCommand))]
    [NotifyCanExecuteChangedFor(nameof(CopyToClipboardCommand))]
    [NotifyCanExecuteChangedFor(nameof(SetMultimediaIdCommand))]
    private string _selected_multimedia;

    [ObservableProperty]
    private IEnumerable<string> _multimedias = new List<string>();

    [ObservableProperty]
    [NotifyCanExecuteChangedFor(nameof(LoadQuestionsCommand))]
    private TopicDto _selectedTopic;
    [ObservableProperty]
    private int _selectedTopicIndex;

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(SelectedTopicIndex))]
    private IEnumerable<TopicDto> _topics;

    [ObservableProperty]
    [NotifyCanExecuteChangedFor(nameof(SetMultimediaIdCommand))]
    private QuestionDto _selectedQuestion;

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(SelectedQuestion))]
    private IEnumerable<QuestionDto> _questions;

    public EditorViewModel(IMultimediaService multimediaService, ITopicService topicService, IQuestionService questionService)
    {
        _multimediaService = multimediaService;
        _topicService = topicService;
        _questionService = questionService;

        Multimedias = _multimediaService.GetMultimedias();
    }

    [RelayCommand]
    private void CreateMultimedia(object obj)
    {
        var ms = Multimedias.ToList();
        var newMultimedia = _multimediaService.CreateMultimedia();
        ms.Add(newMultimedia);
        Multimedias = ms;        
    }

    [RelayCommand(CanExecute = nameof(CanOpenFolder))]
    private void OpenFolder(object obj)
    {
        var mutimediaCfg = _multimediaService.GetMultimediaConfig(Selected_multimedia);
        var path = mutimediaCfg.FolderPath;
        if (Directory.Exists(path))
        {
            ProcessStartInfo startInfo = new ProcessStartInfo
            {
                Arguments = path,
                FileName = "explorer.exe"
            };

            Process.Start(startInfo);
        }
        else
        {
            //MessageBox.Show(string.Format("{0} Directory does not exist!", folderPath));
        }
    }
    public bool CanOpenFolder(object obj)
    {
        return !string.IsNullOrWhiteSpace(Selected_multimedia);
    }

    [RelayCommand(CanExecute = nameof(CanOpenFolder))]
    private void CopyToClipboard(object obj)
    {
        Clipboard.SetText(Selected_multimedia);
    }

    [RelayCommand]
    private void LoadTopics(object obj)
    {
        Topics = _topicService.GetAllTopics();
        SelectedTopicIndex = 0;
    }

    public bool CanLoadQuestions(object obj)
    {
        return SelectedTopic != null;
    }

    [RelayCommand(CanExecute = nameof(CanLoadQuestions))]
    private async void LoadQuestions(object obj)
    {
        var qs = await _questionService.GetQuestionsByTopicAsync(SelectedTopic.Id) ?? new List<QuestionDto>();
        Questions = qs.ToList();
        SelectedQuestion = Questions.FirstOrDefault();
    }

    public bool CanSetMultimedia(object obj)
    {
        return !string.IsNullOrWhiteSpace(Selected_multimedia) && SelectedQuestion != null;
    }

    [RelayCommand(CanExecute = nameof(CanSetMultimedia))]
    private async void SetMultimediaId(object obj)
    {
        if (!string.IsNullOrWhiteSpace(SelectedQuestion.MultimediaId)) //not empty
        {
            if (MessageBox.Show("Question already has MultimediaId do you want to overwrite this assignment?",
                    "Set Question MultimediaId",
                    MessageBoxButton.YesNo,
                    MessageBoxImage.Question) == MessageBoxResult.No)
            {
                return;
            }
        }
        SelectedQuestion = await _questionService.SetQuestionMultimediaIdAsync(SelectedQuestion.Id, Selected_multimedia);
    }
    
}