using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace AuthMVC.Models.Entity
{
    [Table("tblGalleryPhotos")]
    public class PhotoGallery
    {
        [Key]
        public int Id { get; set; } 
        [Required]
        public string MainPhoto { get; set; }
        [Required]
        public string PhotoPreview { get; set; }
    }
}