using System.ComponentModel.DataAnnotations;
namespace MM.EagleRock.Contract.Models
{
    /// <summary>
    /// Models a geographic location's coordinates.
    /// </summary>
    public class GeoLocation
    {
        [Required]
        public double Latitude { get; set; }

        [Required]
        public double Longitude { get; set; }
    }
}
