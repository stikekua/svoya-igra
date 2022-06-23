using Microsoft.EntityFrameworkCore;
using SvoyaIgra.Dal.Bo;
using SvoyaIgra.Shared.Constants;

namespace SvoyaIgra.Dal
{
    public static class ModelBuilderExtensions
    {
        public static ModelBuilder AddQBankModel(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Author>().ToTable("Author", DbConstants.SchemaQBank).HasKey(x => x.Id);
            modelBuilder.Entity<Author>().Property(x => x.Name).IsRequired().HasMaxLength(DbConstants.StringLength);

            modelBuilder.Entity<Topic>().ToTable("Topic", DbConstants.SchemaQBank).HasKey(x => x.Id);
            modelBuilder.Entity<Topic>().Property(x=>x.Name).IsRequired().HasMaxLength(DbConstants.StringLength);

            modelBuilder.Entity<Question>().ToTable("Question", DbConstants.SchemaQBank).HasKey(x => x.Id);
            modelBuilder.Entity<Question>().HasOne(q => q.Topic).WithMany(t => t.Questions).HasForeignKey(q => q.TopicId);
            modelBuilder.Entity<Question>().HasOne(q => q.Author).WithMany(a => a.Questions).HasForeignKey(q => q.AuthorId);
            modelBuilder.Entity<Question>().Property(q => q.MultimediaId).IsRequired().HasMaxLength(DbConstants.GuidLength).IsFixedLength();

            return modelBuilder;
        }
    }
}
