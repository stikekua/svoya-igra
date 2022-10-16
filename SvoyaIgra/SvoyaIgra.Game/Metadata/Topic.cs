using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace SvoyaIgra.Game.Metadata
{
    public class Topic:INotifyPropertyChanged
    {
        public List<Question> Questions { get; set; } = new List<Question>();
        public string Name { get; set; }

    public Topic(List<Question> questions, string name)
        {
            Questions = questions;
            Name = name;
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void NotifyPropertyChanged([CallerMemberName] String propertyName = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }
    }

}
