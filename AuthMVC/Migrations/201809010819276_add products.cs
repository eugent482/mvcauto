namespace AuthMVC.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addproducts : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.tblProducts",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 50),
                        Description = c.String(maxLength: 200),
                        Price = c.Decimal(nullable: false, precision: 18, scale: 2),
                        CategoryId = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.tblCategories", t => t.CategoryId)
                .Index(t => t.CategoryId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.tblProducts", "CategoryId", "dbo.tblCategories");
            DropIndex("dbo.tblProducts", new[] { "CategoryId" });
            DropTable("dbo.tblProducts");
        }
    }
}
