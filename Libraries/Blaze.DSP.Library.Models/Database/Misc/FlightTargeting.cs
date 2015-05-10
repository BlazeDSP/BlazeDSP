//using Library.Models.Enums;

//namespace Library.Models.Database
//{
//    public class FlightTargeting
//    {
//        public int Id { get; set; }

//        /// <summary>
//        ///     Facebook - LA, Male, Single, 18-28, $80,000+
//        /// </summary>
//        public string Name { get; set; }

//        /// <summary>
//        ///     Males who are aged between 18 and 28 who are single, live in Los Angeles and earn more than $80,000 per year.
//        /// </summary>
//        public string Description { get; set; }

//        /// <summary>
//        ///     Enum for extensability reasons (an ad network may decide 'transgender' is a gender)
//        /// </summary>
//        public GenderType Gender { get; set; }

//        public byte Age { get; set; }

//        public string Country { get; set; }
//        public string State { get; set; }
//        public string City { get; set; }
//        /// <summary>
//        ///     Area within city (suburb, county, etc)
//        /// </summary>
//        public string Area { get; set; }

//        // TODO: The system should decide which Property (Postcode or Zipcode) to use based on user input (string => Postcode, number => Zipcode).
//        // WARNING: Ember is going to spit, needs custom handlering.
//        /// <summary>
//        ///     Use if zipcode/postcode contains numbers and letters (UK).
//        /// </summary>
//        public string Postcode { get; set; }

//        // NOTE: Make search easier
//        /// <summary>
//        ///     Use if zipcode/postcode only contains numbers (US, AU).
//        /// </summary>
//        public uint Zipcode { get; set; }

//        public decimal Longitude { get; set; }
//        public decimal Latitude { get; set; }

//        public decimal Income { get; set; }

//        // TODO: [Optimization] Could be a string(?) for extensability purposes (check ad networks usage).
//        public int MaritalStatus { get; set; }
//    }
//}