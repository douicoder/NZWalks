﻿using System.ComponentModel.DataAnnotations;

namespace NZWalks.API.Models.DTO.Walksdtos
{
    public class AddWalkRequestDto
    {
          [Required]
        [MaxLength(100)]
        public string Name { get; set; }

        [Required]
        [MaxLength(1000)]
        public string Description { get; set; }

        [Required]
        [Range(0, 50)]
        public string LengthInKm { get; set; }
        public string? TheWalkImageUrl { get; set; }
        [Required]
        public Guid DifficultyId { get; set; }
        [Required]
        public Guid RegionId { get; set; }
    }
}
