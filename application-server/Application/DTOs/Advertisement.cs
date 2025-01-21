using System;

namespace DTO {

    public class Advertisement {
        
        public int AdvertisementId { get; set; }
        public DateTime CreatedAt { get; set; }
        public int CompanyId { get; set; }
        public string Description { get; set; }
        public int Duration { get; set; }
        public int Spots { get; set; }
        public int Available { get; set; }
        public bool Open { get; set; }
        public String Questionnaire { get; set; }
    }
}