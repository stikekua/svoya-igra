using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using SvoyaIgra.Game.ViewModels.Helpers;

namespace SvoyaIgra.Game.Metadata
{
    public class Topic
    {
        public List<Question> Questions { get; set; } = new List<Question>();
        public string Name { get; set; }

    public Topic(List<Question> questions, string name)
        {
            Questions = questions;
            Name = name;
        }
      
    }

}
