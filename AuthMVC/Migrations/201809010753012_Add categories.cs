namespace AuthMVC.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Addcategories : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.tblCategories",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 50),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.tblCategories");
        }
    }
}
