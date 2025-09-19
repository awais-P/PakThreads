using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.ViewModels.PostVm
{
    public class GetPostsVm
    {
        public long Id { get; set; }
        public string Title { get; set; } = "";
        public string ContentUrl { get; set; } = "";
        public string PostType { get; set; } = "";
        public int Upvotes { get; set; }
        public int Downvotes { get; set; }
        public string UserName { get; set; } = "";
        public string ProfileImageUrl { get; set; } = "";
        public List<PostTagsVm> PostTagsVms { get; set; } = new List<PostTagsVm>();
        public string AddedBy { get; set; } = "";
        public DateTime AddedDate { get; set; }
        public string UpdatedBy { get; set; } = "";
        public DateTime UpdatedDate { get; set; }
    }
}
