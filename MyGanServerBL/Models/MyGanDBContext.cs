using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

#nullable disable

namespace MyGanServerBL.Models
{
    public partial class MyGanDBContext : DbContext
    {
        public MyGanDBContext()
        {
        }

        public MyGanDBContext(DbContextOptions<MyGanDBContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Allergy> Allergies { get; set; }
        public virtual DbSet<Event> Events { get; set; }
        public virtual DbSet<Grade> Grades { get; set; }
        public virtual DbSet<Group> Groups { get; set; }
        public virtual DbSet<Kindergarten> Kindergartens { get; set; }
        public virtual DbSet<KindergartenManager> KindergartenManagers { get; set; }
        public virtual DbSet<Message> Messages { get; set; }
        public virtual DbSet<PendingTeacher> PendingTeachers { get; set; }
        public virtual DbSet<Photo> Photos { get; set; }
        public virtual DbSet<RelationToStudent> RelationToStudents { get; set; }
        public virtual DbSet<StatusType> StatusTypes { get; set; }
        public virtual DbSet<Student> Students { get; set; }
        public virtual DbSet<StudentAllergy> StudentAllergies { get; set; }
        public virtual DbSet<StudentOfUser> StudentOfUsers { get; set; }
        public virtual DbSet<User> Users { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseSqlServer("Server=localhost\\sqlexpress;Database=MyGanDB;Trusted_Connection=True;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("Relational:Collation", "SQL_Latin1_General_CP1_CI_AS");

            modelBuilder.Entity<Allergy>(entity =>
            {
                entity.Property(e => e.AllergyId).HasColumnName("allergyID");

                entity.Property(e => e.AllergyName)
                    .IsRequired()
                    .HasMaxLength(255)
                    .HasColumnName("allergyName");
            });

            modelBuilder.Entity<Event>(entity =>
            {
                entity.Property(e => e.EventId).HasColumnName("EventID");

                entity.Property(e => e.EventDate)
                    .IsRequired()
                    .HasMaxLength(250)
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.EventName)
                    .IsRequired()
                    .HasMaxLength(250);

                entity.Property(e => e.GroupId).HasColumnName("GroupID");

                entity.HasOne(d => d.Group)
                    .WithMany(p => p.Events)
                    .HasForeignKey(d => d.GroupId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_GroupEvent");
            });

            modelBuilder.Entity<Grade>(entity =>
            {
                entity.ToTable("Grade");

                entity.Property(e => e.GradeId).HasColumnName("GradeID");

                entity.Property(e => e.GradeName)
                    .IsRequired()
                    .HasMaxLength(255);
            });

            modelBuilder.Entity<Group>(entity =>
            {
                entity.Property(e => e.GroupId).HasColumnName("GroupID");

                entity.Property(e => e.GroupName)
                    .IsRequired()
                    .HasMaxLength(255);

                entity.Property(e => e.KindergartenId).HasColumnName("KindergartenID");

                entity.Property(e => e.TeacherId).HasColumnName("TeacherID");

                entity.HasOne(d => d.Kindergarten)
                    .WithMany(p => p.Groups)
                    .HasForeignKey(d => d.KindergartenId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_KinderGartenGroup");

                entity.HasOne(d => d.Teacher)
                    .WithMany(p => p.Groups)
                    .HasForeignKey(d => d.TeacherId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_GroupTeacher");
            });

            modelBuilder.Entity<Kindergarten>(entity =>
            {
                entity.ToTable("Kindergarten");

                entity.Property(e => e.KindergartenId).HasColumnName("KindergartenID");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(255);
            });

            modelBuilder.Entity<KindergartenManager>(entity =>
            {
                entity.HasKey(e => new { e.UserId, e.KindergartenId })
                    .HasName("PK_KindergartenManager");

                entity.Property(e => e.UserId).HasColumnName("UserID");

                entity.Property(e => e.KindergartenId).HasColumnName("KindergartenID");

                entity.HasOne(d => d.Kindergarten)
                    .WithMany(p => p.KindergartenManagers)
                    .HasForeignKey(d => d.KindergartenId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_KindergartenManagersKindergarten");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.KindergartenManagers)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_KindergartenUsers");
            });

            modelBuilder.Entity<Message>(entity =>
            {
                entity.Property(e => e.MessageId).HasColumnName("MessageID");

                entity.Property(e => e.Content)
                    .IsRequired()
                    .HasMaxLength(255);

                entity.Property(e => e.GroupId).HasColumnName("GroupID");

                entity.Property(e => e.MessageDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.UserId).HasColumnName("UserID");

                entity.HasOne(d => d.Group)
                    .WithMany(p => p.Messages)
                    .HasForeignKey(d => d.GroupId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_GroupMessage");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.Messages)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_UserMessage");
            });

            modelBuilder.Entity<PendingTeacher>(entity =>
            {
                entity.HasKey(e => new { e.UserId, e.GroupId });

                entity.Property(e => e.UserId).HasColumnName("UserID");

                entity.Property(e => e.GroupId).HasColumnName("GroupID");

                entity.Property(e => e.StatusId).HasColumnName("StatusID");

                entity.HasOne(d => d.Group)
                    .WithMany(p => p.PendingTeachers)
                    .HasForeignKey(d => d.GroupId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_PendingTeacherGroup");

                entity.HasOne(d => d.Status)
                    .WithMany(p => p.PendingTeachers)
                    .HasForeignKey(d => d.StatusId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_PendingTeacherStatus");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.PendingTeachers)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_PendingTeacher");
            });

            modelBuilder.Entity<Photo>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Description)
                    .IsRequired()
                    .HasMaxLength(255);

                entity.Property(e => e.EventId).HasColumnName("EventID");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(255);

                entity.Property(e => e.UserId).HasColumnName("UserID");

                entity.HasOne(d => d.Event)
                    .WithMany(p => p.Photos)
                    .HasForeignKey(d => d.EventId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_EventPhotos");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.Photos)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_UserPhoto");
            });

            modelBuilder.Entity<RelationToStudent>(entity =>
            {
                entity.ToTable("RelationToStudent");

                entity.Property(e => e.RelationToStudentId).HasColumnName("RelationToStudentID");

                entity.Property(e => e.RelationType)
                    .IsRequired()
                    .HasMaxLength(255);
            });

            modelBuilder.Entity<StatusType>(entity =>
            {
                entity.HasKey(e => e.StatusId)
                    .HasName("PK__StatusTy__C8EE204338F6D74C");

                entity.ToTable("StatusType");

                entity.Property(e => e.StatusId).HasColumnName("StatusID");

                entity.Property(e => e.Description)
                    .IsRequired()
                    .HasMaxLength(255);
            });

            modelBuilder.Entity<Student>(entity =>
            {
                entity.Property(e => e.StudentId)
                    .HasMaxLength(255)
                    .HasColumnName("StudentID");

                entity.Property(e => e.BirthDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.FirstName)
                    .IsRequired()
                    .HasMaxLength(255);

                entity.Property(e => e.Gender)
                    .IsRequired()
                    .HasMaxLength(255);

                entity.Property(e => e.GradeId).HasColumnName("GradeID");

                entity.Property(e => e.GroupId).HasColumnName("GroupID");

                entity.Property(e => e.LastName)
                    .IsRequired()
                    .HasMaxLength(255);

                entity.HasOne(d => d.Grade)
                    .WithMany(p => p.Students)
                    .HasForeignKey(d => d.GradeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_StudentGrade");

                entity.HasOne(d => d.Group)
                    .WithMany(p => p.Students)
                    .HasForeignKey(d => d.GroupId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_StudentGroup");
            });

            modelBuilder.Entity<StudentAllergy>(entity =>
            {
                entity.HasKey(e => new { e.StudentId, e.AllergyId })
                    .HasName("PK_StudentAllergy");

                entity.Property(e => e.StudentId)
                    .HasMaxLength(255)
                    .HasColumnName("StudentID");

                entity.Property(e => e.AllergyId).HasColumnName("AllergyID");

                entity.HasOne(d => d.Allergy)
                    .WithMany(p => p.StudentAllergies)
                    .HasForeignKey(d => d.AllergyId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_StudentAllergyName");

                entity.HasOne(d => d.Student)
                    .WithMany(p => p.StudentAllergies)
                    .HasForeignKey(d => d.StudentId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_StudentAllergiesStudents");
            });

            modelBuilder.Entity<StudentOfUser>(entity =>
            {
                entity.HasKey(e => new { e.StudentId, e.UserId });

                entity.Property(e => e.StudentId)
                    .HasMaxLength(255)
                    .HasColumnName("StudentID");

                entity.Property(e => e.UserId).HasColumnName("UserID");

                entity.Property(e => e.RelationToStudentId).HasColumnName("RelationToStudentID");

                entity.Property(e => e.StatusId).HasColumnName("StatusID");

                entity.HasOne(d => d.RelationToStudent)
                    .WithMany(p => p.StudentOfUsers)
                    .HasForeignKey(d => d.RelationToStudentId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_RelationToStudent");

                entity.HasOne(d => d.Status)
                    .WithMany(p => p.StudentOfUsers)
                    .HasForeignKey(d => d.StatusId)
                    .HasConstraintName("FK_UserStatus");

                entity.HasOne(d => d.Student)
                    .WithMany(p => p.StudentOfUsers)
                    .HasForeignKey(d => d.StudentId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_StudentOfParent");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.StudentOfUsers)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ParentOfStudent");
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.HasIndex(e => e.Email, "UQ__Users__A9D1053498953839")
                    .IsUnique();

                entity.Property(e => e.UserId).HasColumnName("UserID");

                entity.Property(e => e.Email)
                    .IsRequired()
                    .HasMaxLength(255);

                entity.Property(e => e.Fname)
                    .IsRequired()
                    .HasMaxLength(255)
                    .HasColumnName("FName");

                entity.Property(e => e.LastName)
                    .IsRequired()
                    .HasMaxLength(255);

                entity.Property(e => e.Password)
                    .IsRequired()
                    .HasMaxLength(255);

                entity.Property(e => e.PhoneNumber)
                    .IsRequired()
                    .HasMaxLength(255);
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
