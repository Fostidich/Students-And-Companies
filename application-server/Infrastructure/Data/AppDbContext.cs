using Microsoft.EntityFrameworkCore;
using System.Linq;

public class AppDbContext : DbContext {

    public DbSet<Entity.User> Users { get; set; }
    public DbSet<Entity.Student> Students { get; set; }

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
    }

    private string ToSnakeCase(string name) {
        return string.Concat(
            name.Select((x, i) => i > 0 && char.IsUpper(x)
                ? "_" + char.ToLower(x)
                : char.ToLower(x).ToString()));
    }

}

