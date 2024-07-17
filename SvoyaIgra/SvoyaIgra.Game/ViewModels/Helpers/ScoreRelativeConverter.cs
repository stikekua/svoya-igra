using System;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Windows.Data;
using SvoyaIgra.Game.Metadata;

namespace SvoyaIgra.Game.ViewModels.Helpers
{
    public class ScoreRelativeConverter : IMultiValueConverter
    {
        public object Convert( object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            var score = (int)values[0];

            var players = (ObservableCollection<Player>)values[1];
            var maxScore = players.Max(x => x.Score);

            if (maxScore == 0) return 0.0;

            var maxRelative = int.Parse(parameter.ToString()!) ;
            
            var result = score*maxRelative/maxScore;
            return System.Convert.ToDouble(result);
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            return null;
            //throw new NotImplementedException();
        }
    }
}

