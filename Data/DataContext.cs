using Microsoft.EntityFrameworkCore;

namespace Teams.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {

        }

        public DbSet<User> Users { get; set; }
        public DbSet<Group> Groups { get; set; }
        public DbSet<Target> Targets { get; set; }
        public DbSet<Leader> Leaders { get; set; }
        public DbSet<Member> Members { get; set; }
        public DbSet<Executor> Executors { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>()
            .HasKey(u => u.Id).HasName("User_Id");

            modelBuilder.Entity<Group>()
            .HasKey(g => g.Id).HasName("Group_Id");

            modelBuilder.Entity<Target>()
            .HasKey(t => t.Id).HasName("Target_Id");
            modelBuilder.Entity<Target>()
            .HasOne(t => t.Group)
            .WithMany(t => t.Targets)
            .HasForeignKey(t => t.Groupid);

            modelBuilder.Entity<Leader>()
            .HasKey(l => new { l.UserId, l.GroupId });
            modelBuilder.Entity<Leader>()
            .HasOne(l => l.User)
            .WithMany(l => l.Leaders)
            .HasForeignKey(l => l.UserId);
            modelBuilder.Entity<Leader>()
            .HasOne(l => l.Group)
            .WithMany(l => l.Leaders)
            .HasForeignKey(l => l.GroupId);

            modelBuilder.Entity<Member>()
            .HasKey(m => new { m.UserId, m.GroupId });
            modelBuilder.Entity<Member>()
            .HasOne(m => m.User)
            .WithMany(m => m.Members)
            .HasForeignKey(m => m.UserId);
            modelBuilder.Entity<Member>()
            .HasOne(m => m.Group)
            .WithMany(m => m.Members)
            .HasForeignKey(m => m.GroupId);

            modelBuilder.Entity<Executor>()
            .HasKey(e => new { e.UserId, e.TargetId });
            modelBuilder.Entity<Executor>()
            .HasOne(e => e.User)
            .WithMany(e => e.Executors)
            .HasForeignKey(e => e.UserId);
            modelBuilder.Entity<Executor>()
            .HasOne(e => e.Target)
            .WithMany(e => e.Executors)
            .HasForeignKey(e => e.TargetId);
        }

    }
}