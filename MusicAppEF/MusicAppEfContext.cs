namespace MusicAppEF
{
    using System;
    using System.Data.Entity;
    using System.Linq;

    public class MusicAppEfContext : DbContext
    {
        public MusicAppEfContext()
           : base("name=MusicAppEfContext")
        {
        }

        public DbSet<MusicalGroup> MusicalGroups { get; set; }
        public DbSet<Song> Songs { get; set; }
    }
}