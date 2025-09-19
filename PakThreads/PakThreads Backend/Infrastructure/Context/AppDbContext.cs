using Domain.Models.Entities.PostEntities;
using Domain.Models.Entities.User;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Context
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {

        }

        #region User

        public DbSet<User> User { get; set; }

        #endregion

        #region Post

        public DbSet<Post> Post { get; set; }
        public DbSet<PostComments> PostComments { get; set; }
        public DbSet<PostVotes> PostVotes { get; set; }
        public DbSet<PostTags> PostTags { get; set; }

        #endregion

    }
}
