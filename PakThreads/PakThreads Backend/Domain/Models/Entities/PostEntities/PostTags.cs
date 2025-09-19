using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Models.BaseEntities;

namespace Domain.Models.Entities.PostEntities
{
    public class PostTags : BaseEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }
        [ForeignKey("Post")]
        public long PostId { get; set; }
        public Post? Post { get; set; }
        [Column(TypeName = "NVARCHAR")]
        [MaxLength(100)]
        public string TagName { get; set; } = "";
    }
}
