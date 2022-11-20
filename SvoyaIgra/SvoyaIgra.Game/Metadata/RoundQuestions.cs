using System.Collections.Generic;

namespace SvoyaIgra.Game.Metadata
{
    public class RoundQuestions
    {
        public List<Topic> Topics { get; set; }
        //public int RoundNumber { get; set; }
        //public RoundQuestions(List<Topic> topics, int roundNumber)
        //{
        //    Topics = topics;
        //    RoundNumber = roundNumber;
        //}
        public RoundQuestions(List<Topic> topics)
        {
            Topics = topics;
        }

    }
}
