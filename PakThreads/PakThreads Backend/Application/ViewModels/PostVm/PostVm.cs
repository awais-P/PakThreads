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
    public class PostVm : PaginationVm
    {
        public long Id { get; set; }
        public string Title { get; set; } = "";
        public string ContentUrl { get; set; } = "";
        public string PostType { get; set; } = "";
        public int Upvotes { get; set; }
        public int Downvotes { get; set; }
        public List<PostTagsVm> PostTagsVms { get; set; } = new List<PostTagsVm>();
        public string AddedBy { get; set; } = "";
        public DateTime AddedDate { get; set; }
        public string UpdatedBy { get; set; } = "";
        public DateTime UpdatedDate { get; set; }
    }
}
