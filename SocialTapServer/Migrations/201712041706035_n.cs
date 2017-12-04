namespace SocialTapServer.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class n : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Bars", "CommentsCount", c => c.Int(nullable: false));
            AddColumn("dbo.Bars", "RatingsCount", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Bars", "RatingsCount");
            DropColumn("dbo.Bars", "CommentsCount");
        }
    }
}
