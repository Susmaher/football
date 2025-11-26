using backend.Models;
using Microsoft.EntityFrameworkCore;

namespace backend.Context
{
    public class FootballDbContext : DbContext
    {
        public FootballDbContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<Team> Teams { get; set; }
        public DbSet<Division> Divisions { get; set; }
        public DbSet<TeamPlayer> TeamPlayers { get; set; }
        public DbSet<Player> Players { get; set; }
        public DbSet<Match> Matches { get; set; }
        public DbSet<Referee> Referees { get; set; }
        public DbSet<Field> Fields { get; set; }
        public DbSet<MatchEvent> MatchEvents { get; set; }
        public DbSet<Position> Positions { get; set; }
        public DbSet<User> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            //ezzel tudja az entity framework, hogy a match táblában lévő két team id hova tartozik.  
            //Elméletben nem lehet törölni team-et, ha vannak meccsei
            modelBuilder.Entity<Match>()
                .HasOne(m => m.HomeTeam)
                .WithMany(t => t.HomeMatch)
                .HasForeignKey(m => m.HomeTeamId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Match>()
                .HasOne(m => m.AwayTeam)
                .WithMany(t => t.AwayMatch)
                .HasForeignKey(m => m.AwayTeamId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Match>()
                .Property(m => m.Status)
                .HasConversion<int>();

            modelBuilder.Entity<MatchEvent>()
                .Property(me => me.EventType)
                .HasConversion<int>();
        }
    }
}
