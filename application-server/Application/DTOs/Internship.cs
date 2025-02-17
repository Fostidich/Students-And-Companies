using System;

namespace DTO
{

    public class Internship
    {

        public int InternshipId { get; set; }
        public DateTime CreatedAt { get; set; }
        public int StudentId { get; set; }
        public int CompanyId { get; set; }
        public int AdvertisementId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

    }
}