using Microsoft.EntityFrameworkCore;
using System.Linq;

public class AppDbContext : DbContext {

    public DbSet<Entity.Student> Student { get; set; }
    public DbSet<Entity.Company> Company { get; set; }
    public DbSet<Entity.Advertisement> Advertisement { get; set; }
    public DbSet<Entity.Application> Application { get; set; }
    public DbSet<Entity.StudentSkills> StudentSkills { get; set; }
    public DbSet<Entity.Skill> Skill { get; set; }
    public DbSet<Entity.AdvertisementSkills> AdvertisementSkills { get; set; }
    public DbSet<Entity.Internship> Internship { get; set; }
    public DbSet<Entity.Feedback> Feedback { get; set; }

    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder) {
        base.OnModelCreating(modelBuilder);

        // Convert table names to snake_case
        foreach (var entity in modelBuilder.Model.GetEntityTypes()) {
            entity.SetTableName(ToSnakeCase(entity.GetTableName()));
            foreach (var property in entity.GetProperties()) {
                property.SetColumnName(ToSnakeCase(property.Name));
            }
        }


        // Unique constraints

        // Ensures that the Username field in the Student table is unique
        modelBuilder.Entity<Entity.Student>()
            .HasIndex(s => s.Username)
            .IsUnique();

        // Ensures that the Email field in the Student table is unique
        modelBuilder.Entity<Entity.Student>()
            .HasIndex(s => s.Email)
            .IsUnique();

        // Ensures that the Username field in the Company table is unique
        modelBuilder.Entity<Entity.Company>()
            .HasIndex(c => c.Username)
            .IsUnique();

        // Ensures that the Email field in the Company table is unique
        modelBuilder.Entity<Entity.Company>()
            .HasIndex(c => c.Email)
            .IsUnique();
        
        // Ensures that the Name field in the Skill table is unique
        modelBuilder.Entity<Entity.Skill>()
            .HasIndex(s => s.Name)
            .IsUnique();


        // Composite key

        // Configure the composite key for the StudentSkills table
        modelBuilder.Entity<Entity.StudentSkills>()
            .HasKey(ss => new { ss.StudentId, ss.SkillId });

        // Configure the composite key for AdvertisementSkills
        modelBuilder.Entity<Entity.AdvertisementSkills>()
            .HasKey(ads => new { ads.AdvertisementId, ads.SkillId });


        // Foreign key

        // Configure the foreign key relationship with Company
        modelBuilder.Entity<Entity.Advertisement>()
            .HasOne(a => a.Company)
            .WithMany(c => c.Advertisements)
            .HasForeignKey(a => a.CompanyId)
            .OnDelete(DeleteBehavior.Restrict); // Prevent cascading delete

        // Configure the foreign key relationship with the Student table
        modelBuilder.Entity<Entity.StudentSkills>()
            .HasOne(ss => ss.Student)
            .WithMany(s => s.StudentSkills)
            .HasForeignKey(ss => ss.StudentId)
            .OnDelete(DeleteBehavior.Restrict); // Prevent cascading delete

        // Configure the foreign key relationship with the Skill table
        modelBuilder.Entity<Entity.StudentSkills>()
            .HasOne(ss => ss.Skill)
            .WithMany(s => s.StudentSkills)
            .HasForeignKey(ss => ss.SkillId)
            .OnDelete(DeleteBehavior.Cascade); // Delete StudentSkill when the skill is deleted

        // Configure the foreign key relationship with Advertisement
        modelBuilder.Entity<Entity.AdvertisementSkills>()
            .HasOne(ads => ads.Advertisement)
            .WithMany(a => a.AdvertisementSkills)
            .HasForeignKey(ads => ads.AdvertisementId)
            .OnDelete(DeleteBehavior.Restrict); // Prevent cascading delete

        // Configure the foreign key relationship with Skill
        modelBuilder.Entity<Entity.AdvertisementSkills>()
            .HasOne(ads => ads.Skill)
            .WithMany(s => s.AdvertisementSkills)
            .HasForeignKey(ads => ads.SkillId)
            .OnDelete(DeleteBehavior.Cascade); // Delete AdvertisementSkill when the skill is deleted

        // Configure the foreign key relationship with Student
        modelBuilder.Entity<Entity.Application>()
            .HasOne(a => a.Student) // Each application belongs to one student
            .WithMany(s => s.Applications) // A student can make many applications
            .HasForeignKey(a => a.StudentId)
            .OnDelete(DeleteBehavior.Restrict); // Prevent cascading delete

        // Configure the foreign key relationship with Advertisement
        modelBuilder.Entity<Entity.Application>()
            .HasOne(a => a.Advertisement) // Each application targets one advertisement
            .WithMany(ad => ad.Applications) // An advertisement can have many applications
            .HasForeignKey(a => a.AdvertisementId)
            .OnDelete(DeleteBehavior.Restrict); // Prevent cascading delete

        // Configure the foreign key relationship with Student
        modelBuilder.Entity<Entity.Internship>()
            .HasOne(i => i.Student) // Each internship belongs to one student
            .WithOne(s => s.Internship) // A student can have one internships
            .HasForeignKey<Entity.Internship>(i => i.StudentId)
            .OnDelete(DeleteBehavior.Restrict); // Prevent cascading delete

        // Configure the foreign key relationship with Company
        modelBuilder.Entity<Entity.Internship>()
            .HasOne(i => i.Company) // Each internship is linked to one company
            .WithMany(c => c.Internships) // A company can have many internships
            .HasForeignKey(i => i.CompanyId)
            .OnDelete(DeleteBehavior.Restrict); // Prevent cascading delete

        // Configure the foreign key relationship with Advertisement
        modelBuilder.Entity<Entity.Internship>()
            .HasOne(i => i.Advertisement) // Each internship is linked to one advertisement
            .WithMany(a => a.Internships) // An advertisement can have many internships
            .HasForeignKey(i => i.AdvertisementId)
            .OnDelete(DeleteBehavior.Restrict); // Prevent cascading delete

        // Configure the foreign key relationship with Internship
        modelBuilder.Entity<Entity.Feedback>()
            .HasOne(f => f.Internship) // Each feedback is linked to one internship
            .WithOne(i => i.Feedback) // Each internship has one feedback
            .HasForeignKey<Entity.Feedback>(f => f.InternshipId)
            .OnDelete(DeleteBehavior.Cascade); // Delete feedback when the internship is deleted
        
        
        modelBuilder.Entity<Entity.CompanyNotifications>()
            .HasOne(e => e.Company)
            .WithMany(c => c.CompanyNotifications)
            .HasForeignKey(e => e.CompanyId)
            .OnDelete(DeleteBehavior.Cascade);
        
        modelBuilder.Entity<Entity.CompanyNotifications>()
            .HasOne(e => e.Student)
            .WithMany(s => s.CompanyNotifications)
            .HasForeignKey(e => e.StudentId)
            .OnDelete(DeleteBehavior.Cascade);
        
        modelBuilder.Entity<Entity.CompanyNotifications>()
            .HasOne(e => e.Advertisement)
            .WithMany(a => a.CompanyNotifications)
            .HasForeignKey(e => e.AdvertisementId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<Entity.StudentNotifications>()
            .HasOne(e => e.Advertisement)
            .WithMany(a => a.StudentNotifications)
            .HasForeignKey(e => e.AdvertisementId)
            .OnDelete(DeleteBehavior.Cascade);
        
        modelBuilder.Entity<Entity.StudentNotifications>()
            .HasOne(e => e.Student)
            .WithMany(s => s.StudentNotifications)
            .HasForeignKey(e => e.StudentId)
            .OnDelete(DeleteBehavior.Cascade);
    }

    private string ToSnakeCase(string name) {
        return string.Concat(
            name.Select((x, i) => i > 0 && char.IsUpper(x)
                ? "_" + char.ToLower(x)
                : char.ToLower(x).ToString()));
    }

}

