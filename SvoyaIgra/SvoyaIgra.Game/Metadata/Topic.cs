using System.Collections.Generic;
using SvoyaIgra.Game.ViewModels.Helpers;

namespace SvoyaIgra.Game.Metadata
{
    public class Topic:ViewModelBase
    {
        private List<Question> _questions = new List<Question>();
        public List<Question> Questions 
        { 
            get
            {
                return _questions;
            }
            set
            {
                if (_questions != value)
                {
                    _questions = value;
                    OnPropertyChanged(nameof(Questions));
                }
                
            }
        } 
        public string Name { get; set; }

        public Topic(List<Question> questions, string name)
        {
            Questions = questions;
            Name = name;
        }
      
    }

}
