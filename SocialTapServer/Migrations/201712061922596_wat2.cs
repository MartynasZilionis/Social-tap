namespace SocialTapServer.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class wat2 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Comments", "Stars", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Comments", "Stars");
        }
    }
}
