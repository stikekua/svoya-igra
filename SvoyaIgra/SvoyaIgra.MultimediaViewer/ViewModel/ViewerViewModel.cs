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

    [ObservableProperty] 
    private ImageSource _image;

    [ObservableProperty]
    private string _id;

    [ObservableProperty]
    private MultimediaForEnum _multimedia_for;

    [ObservableProperty]
    private string _selected;

    [ObservableProperty]
    private IEnumerable<string> _qfiles;

    [ObservableProperty]
    private IEnumerable<string> _afiles;

    public ViewerViewModel(IMultimediaService multimediaService)
    {
        _multimediaService = multimediaService;

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

    [RelayCommand]
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

        SetImageSource(mutimediaStream.stream);
    }


    private void SetImageSource(Stream stream)
    {
        MemoryStream ms = new MemoryStream();
        stream.CopyTo(ms);
        ms.Seek(0, SeekOrigin.Begin);
        stream.Close();

        var imgsrc = new BitmapImage();
        imgsrc.BeginInit();
        imgsrc.StreamSource = ms;
        imgsrc.EndInit();
        Image = imgsrc;
    }


}