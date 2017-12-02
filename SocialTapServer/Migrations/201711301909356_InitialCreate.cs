namespace SocialTapServer.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Bars",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        Name = c.String(),
                        Location_Latitude = c.Double(nullable: false),
                        Location_Longitude = c.Double(nullable: false),
                        Location_Altitude = c.Double(nullable: false),
                        Location_HorizontalAccuracy = c.Double(nullable: false),
                        Location_VerticalAccuracy = c.Double(nullable: false),
                        Location_Speed = c.Double(nullable: false),
                        Location_Course = c.Double(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Comments",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        Author = c.String(),
                        Content = c.String(),
                        Date = c.DateTime(nullable: false),
                        Bar_Id = c.Guid(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Bars", t => t.Bar_Id)
                .Index(t => t.Bar_Id);
            
            CreateTable(
                "dbo.Ratings",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        FillPercentage = c.Single(nullable: false),
                        MugSize = c.Single(nullable: false),
                        MugPrice = c.Single(nullable: false),
                        Date = c.DateTime(nullable: false),
                        Bar_Id = c.Guid(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Bars", t => t.Bar_Id)
                .Index(t => t.Bar_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Ratings", "Bar_Id", "dbo.Bars");
            DropForeignKey("dbo.Comments", "Bar_Id", "dbo.Bars");
            DropIndex("dbo.Ratings", new[] { "Bar_Id" });
            DropIndex("dbo.Comments", new[] { "Bar_Id" });
            DropTable("dbo.Ratings");
            DropTable("dbo.Comments");
            DropTable("dbo.Bars");
        }
    }
}
