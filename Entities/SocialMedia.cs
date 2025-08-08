using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Migfer_Kariyer.Entities
{
    public class SocialMedia
    {
        [Key]
        public int Id { get; set; }

        [Column(TypeName = "NVARCHAR")]
        [StringLength(200)]
        public string FacebookUrl { get; set; } = string.Empty;

        [Column(TypeName = "NVARCHAR")]
        [StringLength(200)]
        public string TwitterUrl { get; set; } = string.Empty;

        [Column(TypeName = "NVARCHAR")]
        [StringLength(200)]
        public string InstagramUrl { get; set; } = string.Empty;

        [Column(TypeName = "NVARCHAR")]
        [StringLength(200)]
        public string LinkedinUrl { get; set; } = string.Empty;

        [Column(TypeName = "NVARCHAR")]
        [StringLength(200)]
        public string YoutubeUrl { get; set; } = string.Empty;

        [Column(TypeName = "NVARCHAR")]
        [StringLength(200)]
        public string DiscordUrl { get; set; } = string.Empty;
    }
}