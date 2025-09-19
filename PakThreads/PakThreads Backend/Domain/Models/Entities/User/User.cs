using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Models.BaseEntities;

namespace Domain.Models.Entities.User
{
    public class User : BaseEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }
        [Column(TypeName = "NVARCHAR")]
        [MaxLength(100)]
        public string FirstName { get; set; } = "";
        [Column(TypeName = "NVARCHAR")]
        [MaxLength(100)]
        public string LastName { get; set; } = "";
        [Column(TypeName = "NVARCHAR")]
        [MaxLength(100)]
        public string UserName { get; set; } = "";
        [Column(TypeName = "NVARCHAR")]
        [MaxLength(100)]
        public string UserEmail { get; set; } = "";
        [Column(TypeName = "NVARCHAR")]
        [MaxLength(100)]
        public string Country { get; set; } = "";
        [Column(TypeName = "NVARCHAR")]
        [MaxLength(1500)]
        public string UserBio { get; set; } = "";
        [Column(TypeName = "NVARCHAR")]
        [MaxLength(200)]
        public string Password { get; set; } = "";
        [Column(TypeName = "NVARCHAR")]
        [MaxLength(100)]
        public string UserType { get; set; } = "";
        [Column(TypeName = "NVARCHAR")]
        [MaxLength(100)]
        public string Gender { get; set; } = "";
        [Column(TypeName = "NVARCHAR")]
        [MaxLength(100)]
        public string PhoneNumber { get; set; } = "";
        [DataType(DataType.DateTime)]
        public DateTime DateOfBirth { get; set; }
        [Column(TypeName = "NVARCHAR")]
        [MaxLength(1500)]
        public string BlockedReason { get; set; } = "";

        [Column(TypeName = "NVARCHAR")]
        [MaxLength(300)]
        public string ProfileImageUrl { get; set; } = "";
        public bool IsEmailVerified { get; set; } = false;
        public bool IsBlocked { get; set; } = false;
        public bool IsSiteUser { get; set; } = true;
    }
}
