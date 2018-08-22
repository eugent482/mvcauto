namespace AuthMVC.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DateTime : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.tblUsers", "BirthDay", c => c.DateTime(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.tblUsers", "BirthDay");
        }
    }
}
