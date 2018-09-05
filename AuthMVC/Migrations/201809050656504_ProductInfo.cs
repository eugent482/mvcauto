namespace AuthMVC.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ProductInfo : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.tblProductInfo",
                c => new
                    {
                        Id = c.Int(nullable: false),
                        Photo = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.tblProducts", t => t.Id)
                .Index(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.tblProductInfo", "Id", "dbo.tblProducts");
            DropIndex("dbo.tblProductInfo", new[] { "Id" });
            DropTable("dbo.tblProductInfo");
        }
    }
}
