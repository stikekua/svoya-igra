using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using SvoyaIgra.MultimediaProvider.Services;
using SvoyaIgra.Shared.Entities;

namespace SvoyaIgra.MultimediaViewer.ViewModel;

[INotifyPropertyChanged]
public partial class ViewerViewModel
{
    private readonly IMultimediaService _multimediaService;

    private MediaPlayer _mediaPlayer;

    [ObservableProperty] 
    private ImageSource _image;

    [ObservableProperty]
    private string _id;

    [ObservableProperty]
    private MultimediaForEnum _multimedia_for;

    [ObservableProperty]
    [NotifyCanExecuteChangedFor(nameof(LoadPictureCommand))]
    [NotifyCanExecuteChangedFor(nameof(GetPathCommand))]
    private string _selected;

    [ObservableProperty]
    private IEnumerable<string> _qfiles;

    [ObservableProperty]
    private IEnumerable<string> _afiles;

    [ObservableProperty]
    private string _file_path;

    public ViewerViewModel(IMultimediaService multimediaService)
    {
        _multimediaService = multimediaService;
        _mediaPlayer = new MediaPlayer();

        Id = "29FDBD16-9DA1-4495-B67A-87BCF0942881";
        Multimedia_for = MultimediaForEnum.Question;
    }

    [RelayCommand]
    private void SetMultimediaFor(object obj)
    {
        Multimedia_for = Enum.Parse<MultimediaForEnum>((string)obj);
    }

    [RelayCommand]
    private void LoadMutimedia(object obj)
    {
        var mutimediaCfg = _multimediaService.GetMultimediaConfig(Id);

        Qfiles = mutimediaCfg.QuestionFiles;
        Afiles = mutimediaCfg.AnswerFiles;
    }

    [RelayCommand(CanExecute = nameof(CanLoadPicture))]
    private void LoadPicture(object obj)
    {
        if (Qfiles.Contains(Selected))
        {
            Multimedia_for = MultimediaForEnum.Question;
        }
        else if (Afiles.Contains(Selected))
        {
            Multimedia_for = MultimediaForEnum.Answer;
        }

        var mutimediaStream =
            _multimediaService.GetMultimedia(Id, Multimedia_for, Selected);

        if (mutimediaStream.mediaType == MediaType.Image)
        {
            var ms = ConverToMemoryStream(mutimediaStream.stream);
            SetImageSource(ms);
        }
        else if (mutimediaStream.mediaType == MediaType.Audio)
        {
            var ms = ConverToMemoryStream(mutimediaStream.stream);
            _mediaPlayer.Open(new Uri(@"C:\SvoyaIgra\MultimediaStore\29FDBD16-9DA1-4495-B67A-87BCF0942881\Answer\Sinitana - No Rules.mp3"));
            _mediaPlayer.Play();
        }
        else if (mutimediaStream.mediaType == MediaType.Video)
        {
            
        }


        
    }

    public bool CanLoadPicture(object obj)
    {
        return !string.IsNullOrWhiteSpace(Selected);
    }

    [RelayCommand(CanExecute = nameof(CanGetPath))]
    private void GetPath(object obj)
    {
        if (Qfiles.Contains(Selected))
        {
            Multimedia_for = MultimediaForEnum.Question;
        }
        else if (Afiles.Contains(Selected))
        {
            Multimedia_for = MultimediaForEnum.Answer;
        }

        var mutimedia = _multimediaService.GetMultimediaPath(Id, Multimedia_for, Selected);
        File_path = mutimedia.path ?? "";
    }

    public bool CanGetPath(object obj)
    {
        return !string.IsNullOrWhiteSpace(Selected);
    }
    
    private void SetImageSource(MemoryStream ms)
    {
        var imgsrc = new BitmapImage();
        imgsrc.BeginInit();
        imgsrc.StreamSource = ms;
        imgsrc.EndInit();
        Image = imgsrc;
    }

    private MemoryStream ConverToMemoryStream(Stream stream)
    {
        MemoryStream ms = new MemoryStream();
        stream.CopyTo(ms);
        ms.Seek(0, SeekOrigin.Begin);
        stream.Close();

        return ms;
    }

}