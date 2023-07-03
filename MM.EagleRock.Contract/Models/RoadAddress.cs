using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MM.EagleRock.Contract.Models
{
    /// <summary>
    /// Models attributes that describe the road inspected.
    /// </summary>
    public class RoadAddress
    {
        /// <summary>
        /// Describes a segment of the road inspected.
        /// </summary>
        /// <remarks>
        /// Could be a list of geo-locations which scope the segment, 
        /// or 2 geo-locations representing the segment's entry and exit points.
        /// </remarks>
        /// <example>
        /// TODO: provide segment data example, GEOJSON?
        /// </example>
        public String Segment { get; set; }

        public String StreetName { get; set; }

        public String City { get; set; }

        public String State { get; set; }

        public String Country { get; set; }

        public String PostalCode { get; set; }
    }
}
