using Microsoft.EntityFrameworkCore;
using SvoyaIgra.Dal.Bo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SvoyaIgra.DbMigrations.Data
{
    internal class InitialData
    {
        public static void CreateInitialData(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Theme>().HasData(
                new Theme
                {
                    Id = 1,
                    Name = "Tema1",
                    Difficulty = ThemeDifficulty.Round1
                },
                new Theme
                {
                    Id = 2,
                    Name = "Tema2",
                    Difficulty = ThemeDifficulty.Round2
                }
            );

            modelBuilder.Entity<Question>().HasData(
                new Question
                {
                    Id = 1,
                    Type = QuestionType.Text,
                    Difficulty = QuestionDifficulty.Level1,
                    ThemeId = 1,
                    Text = "Question?",
                    MultimediaId = "00000000-0000-0000-0000-000000000000",
                    Answer = "Answer!"
                }
            );
        }
    }
}
