namespace MusicAppEF.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.MusicalGroups",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Songs",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        Name = c.String(),
                        Text = c.String(),
                        Rating = c.Int(nullable: false),
                        MusicalGroupId = c.Guid(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.MusicalGroups", t => t.MusicalGroupId, cascadeDelete: true)
                .Index(t => t.MusicalGroupId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Songs", "MusicalGroupId", "dbo.MusicalGroups");
            DropIndex("dbo.Songs", new[] { "MusicalGroupId" });
            DropTable("dbo.Songs");
            DropTable("dbo.MusicalGroups");
        }
    }
}
