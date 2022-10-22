using Microsoft.EntityFrameworkCore;
using SvoyaIgra.Dal.Bo;
using SvoyaIgra.Shared.Entities;

namespace SvoyaIgra.DbMigrations.Data
{
    internal class InitialData
    {
        public static void CreateInitialData(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Author>().HasData(
                new Author
                {
                    Id = 1,
                    Name = "Test",
                }
            );

            modelBuilder.Entity<Topic>().HasData(
                new Topic
                {
                    Id = 1,
                    Name = "Tema1",
                    Difficulty = TopicDifficulty.Round
                }
            );

            modelBuilder.Entity<Question>().HasData(
                new Question
                {
                    Id = 1,
                    Type = QuestionType.Text,
                    Difficulty = QuestionDifficulty.Level1,
                    TopicId = 1,
                    AuthorId = 1,
                    Text = "Question1?",
                    MultimediaId = "00000000-0000-0000-0000-000000000000",
                    Answer = "Answer1!"
                },
                new Question
                {
                    Id = 2,
                    Type = QuestionType.Text,
                    Difficulty = QuestionDifficulty.Level2,
                    TopicId = 1,
                    AuthorId = 1,
                    Text = "Question2?",
                    MultimediaId = "00000000-0000-0000-0000-000000000000",
                    Answer = "Answer2!"
                },
                new Question
                {
                    Id = 3,
                    Type = QuestionType.Text,
                    Difficulty = QuestionDifficulty.Level3,
                    TopicId = 1,
                    AuthorId = 1,
                    Text = "Question3?",
                    MultimediaId = "00000000-0000-0000-0000-000000000000",
                    Answer = "Answer3!"
                }, new Question
                {
                    Id = 4,
                    Type = QuestionType.Text,
                    Difficulty = QuestionDifficulty.Level4,
                    TopicId = 1,
                    AuthorId = 1,
                    Text = "Question4?",
                    MultimediaId = "00000000-0000-0000-0000-000000000000",
                    Answer = "Answer4!"
                }, new Question
                {
                    Id = 5,
                    Type = QuestionType.Text,
                    Difficulty = QuestionDifficulty.Level5,
                    TopicId = 1,
                    AuthorId = 1,
                    Text = "Question5?",
                    MultimediaId = "00000000-0000-0000-0000-000000000000",
                    Answer = "Answer5!"
                }
            );
        }
    }
}
