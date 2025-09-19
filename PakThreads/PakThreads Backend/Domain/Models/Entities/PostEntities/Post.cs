using Domain.Models.BaseEntities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models.Entities.PostEntities
{
    public class Post : BaseEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }
        [Column(TypeName = "NVARCHAR")]
        [MaxLength(100)]
        public string Title { get; set; } = "";
        [Column(TypeName = "NVARCHAR(MAX)")]
        public string ContentUrl { get; set; } = "";

        [Column(TypeName = "NVARCHAR")]
        [MaxLength(100)]
        public string PostType { get; set; } = "";

        public int Upvotes { get; set; }  
        public int Downvotes { get; set; }
    }
}
