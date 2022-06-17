using Microsoft.EntityFrameworkCore;
using SvoyaIgra.Dal.Bo;

namespace SvoyaIgra.DbMigrations.Data
{
    internal class InitialData
    {
        public static void CreateInitialData(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Topic>().HasData(
                new Topic
                {
                    Id = 1,
                    Name = "Tema1",
                    Difficulty = TopicDifficulty.Round1
                },
                new Topic
                {
                    Id = 2,
                    Name = "Tema2",
                    Difficulty = TopicDifficulty.Round2
                }
            );

            modelBuilder.Entity<Question>().HasData(
                new Question
                {
                    Id = 1,
                    Type = QuestionType.Text,
                    Difficulty = QuestionDifficulty.Level1,
                    TopicId = 1,
                    Text = "Question?",
                    MultimediaId = "00000000-0000-0000-0000-000000000000",
                    Answer = "Answer!"
                }
            );
        }
    }
}
