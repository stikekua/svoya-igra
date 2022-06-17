using Microsoft.EntityFrameworkCore;
using SvoyaIgra.Dal.Bo;
using SvoyaIgra.Shared.Constants;

namespace SvoyaIgra.Dal
{
    public static class ModelBuilderExtensions
    {
        public static ModelBuilder AddSvoyaIgraModel(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Topic>().ToTable("Topic", DbConstants.SchemaSvoyaIgra).HasKey(x => x.Id);
            modelBuilder.Entity<Topic>().Property(x=>x.Name).HasMaxLength(DbConstants.StringLength);

            modelBuilder.Entity<Question>().ToTable("Question", DbConstants.SchemaSvoyaIgra).HasKey(x => x.Id);
            modelBuilder.Entity<Question>().HasOne(q => q.Topic).WithMany(t => t.Questions).HasForeignKey(q => q.TopicId);
            modelBuilder.Entity<Question>().Property(q => q.MultimediaId).IsRequired().HasMaxLength(DbConstants.GuidLength).IsFixedLength();

            return modelBuilder;
        }
    }
}
