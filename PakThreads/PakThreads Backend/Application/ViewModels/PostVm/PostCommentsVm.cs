using Domain.Models.Entities.PostEntities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.ViewModels.BaseVm;

namespace Application.ViewModels.PostVm
{
    public class PostCommentsVm : PaginationVm
    {
        public int Id { get; set; }
        public long PostId { get; set; }
        public Post? Post { get; set; }
        public string Comment { get; set; } = "";
        public int Upvotes { get; set; }
        public int Downvotes { get; set; }
        public string AddedBy { get; set; } = "";
        public DateTime AddedDate { get; set; }
        public string UpdatedBy { get; set; } = "";
        public DateTime UpdatedDate { get; set; }
    }
}
