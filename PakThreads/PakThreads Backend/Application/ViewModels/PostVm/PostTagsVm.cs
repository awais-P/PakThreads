using Domain.Models.Entities.PostEntities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.ViewModels.PostVm
{
    public class PostTagsVm
    {
        public long Id { get; set; }
        public long PostId { get; set; }
        public string TagName { get; set; } = "";
    }
}
