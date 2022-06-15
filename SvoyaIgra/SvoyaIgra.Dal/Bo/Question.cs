using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SvoyaIgra.Dal.Bo
{
    public class Question
    {
        public int Id { get; set; }
        public QuestionType Type { get; set; }
        public QuestionDifficulty Difficulty { get; set; }
        public int ThemeId { get; set; }
        public Theme Theme { get; set; }
        public string Text { get; set; }
        public string MultimediaId { get; set; }
        public string Answer { get; set; }
    }
}
