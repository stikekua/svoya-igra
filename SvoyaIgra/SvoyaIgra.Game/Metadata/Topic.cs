using System.Collections.Generic;

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
