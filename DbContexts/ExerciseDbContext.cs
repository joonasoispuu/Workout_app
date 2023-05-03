using Microsoft.EntityFrameworkCore;

namespace WorkoutApp
{
    public class ExerciseDbContext : DbContext
    {
        public DbSet<ExerciseModel> Exercises { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var dbPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "workout.db");

            // Creates database if it doesnt already exist
            if (!File.Exists(dbPath))
            {
                File.Create(dbPath).Dispose();
            }

            optionsBuilder.UseSqlite($"Data Source={dbPath}");
        }
    }
}