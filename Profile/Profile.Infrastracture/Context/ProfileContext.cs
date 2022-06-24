using FurtherEducation.Postgre.Extension;
using Microsoft.EntityFrameworkCore;
using Profile.Domain.Diary;
using Profile.Domain.Profile;

#nullable disable

namespace Profile.Infrastracture.Context
{
    public partial class ProfileContext : DbContext
    {
        public ProfileContext()
        {
        }

        public ProfileContext(DbContextOptions<ProfileContext> options)
            : base(options)
        {
        }

        public virtual DbSet<ProfileEntity> ProfileEntities { get; set; }
        public virtual DbSet<TeacherDiaryEntity> TeacherDiaryEntities { get; set; }
        public virtual DbSet<StudentDiaryEntity> StudentDiaryEntities { get; set; }
        public virtual DbSet<DiaryRecordEntity> DiaryRecords { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseNpgsql("Host=localhost;Port=5432;Database=postgres;Username=keycloak;Password=password", server => server.MigrationsAssembly("Profile.Infrastracture"));
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("Relational:Collation", "en_US.utf8");

            modelBuilder.Entity<ProfileEntity>(entity =>
            {
                entity.ToTable("profile_entity");

                entity.Property(e => e.UserId)
                 .HasColumnName("user_id");

                entity.Property(e => e.FirstName).HasColumnName("first_name");
                entity.Property(e => e.LastName).HasColumnName("last_name");

                entity.HasMany(profile => profile.StudentDiaries)
                   .WithOne(diary => diary.UserProfile)
                   .HasForeignKey(d => d.UserId);

                entity.HasMany(profile => profile.TeacherDiaries)
                   .WithOne(diary => diary.UserProfile)
                   .HasForeignKey(d => d.UserId);
            });

            modelBuilder.Entity<TeacherDiaryEntity>(entity =>
            {
                entity.ToTable("profile_teacher_diary_entity");

                //USER + COURSE = UNIQUE DIARY
                entity.HasKey(e => new { e.UserId, e.CourseId });

                entity.Property(e => e.UserId).HasColumnName("user_id");

                entity.Property(e => e.CourseId).HasColumnName("course_id");
                entity.Property(e => e.CourseName).HasColumnName("course_name");
                entity.Property(e => e.CreatedAt).HasColumnName("start_time").UsesUtc();
                entity.Property(e => e.Status).HasColumnName("status");
            });

            modelBuilder.Entity<StudentDiaryEntity>(entity =>
            {
                entity.ToTable("profile_student_diary_entity");

                //USER + COURSE = UNIQUE DIARY
                entity.HasKey(e => new { e.UserId, e.CourseId});
               
                entity.HasMany(diary => diary.Records)
                    .WithOne(record => record.Diary)
                    .HasForeignKey(e => new { e.UserId, e.CourseId });

                entity.Property(e => e.UserId).HasColumnName("user_id");

                entity.Property(e => e.CourseId).HasColumnName("course_id");
                entity.Property(e => e.CourseName).HasColumnName("course_name");
                entity.Property(e => e.StartedAt).HasColumnName("start_time").UsesUtc();
                entity.Property(e => e.Status).HasColumnName("status");
            });

            modelBuilder.Entity<DiaryRecordEntity>(entity =>
            {
                entity.ToTable("profile_diary_recod_entity");

                entity.HasKey(e => new { e.UserId, e.CourseId, e.TaskId });

                entity.Property(e => e.UserId).HasColumnName("user_id");

                entity.Property(e => e.CourseId).HasColumnName("course_id");
                entity.Property(e => e.TaskId).HasColumnName("task_id");
                entity.Property(e => e.ModuleId).HasColumnName("module_id");

                entity.Property(e => e.AnswerGivenAt).HasColumnName("answer_given_at").UsesUtc();
                entity.Property(e => e.Score).HasColumnName("score");
            });

            base.OnModelCreating(modelBuilder);
        }


    }
}
