namespace AuthMVC.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Gallerytable : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.tblGalleryPhotos",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        MainPhoto = c.String(nullable: false),
                        PhotoPreview = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.tblGalleryPhotos");
        }
    }
}
