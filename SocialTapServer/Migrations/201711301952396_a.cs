namespace SocialTapServer.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class a : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.Bars", "Location_Altitude");
            DropColumn("dbo.Bars", "Location_HorizontalAccuracy");
            DropColumn("dbo.Bars", "Location_VerticalAccuracy");
            DropColumn("dbo.Bars", "Location_Speed");
            DropColumn("dbo.Bars", "Location_Course");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Bars", "Location_Course", c => c.Double(nullable: false));
            AddColumn("dbo.Bars", "Location_Speed", c => c.Double(nullable: false));
            AddColumn("dbo.Bars", "Location_VerticalAccuracy", c => c.Double(nullable: false));
            AddColumn("dbo.Bars", "Location_HorizontalAccuracy", c => c.Double(nullable: false));
            AddColumn("dbo.Bars", "Location_Altitude", c => c.Double(nullable: false));
        }
    }
}
