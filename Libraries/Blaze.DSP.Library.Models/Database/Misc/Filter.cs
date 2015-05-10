//namespace Library.Models.Database
//{
//    /* FILTERS
//     * IP Address
//     * User Agent
//     * Accept Language
//     * Referrer
//     * MSISDN
//     * UA-CPU
//     * Internet Type
//     * Internet Speed
//     * Carrier
//     * OS
//     * Device
//     * GEO / Location
//     */
//    public class Filter
//    {
//        /// <summary>
//        /// VARBINARY(16)
//        /// </summary>
//        public byte[] IpAddress { get; set; }

//        /// <summary>
//        /// NVARCHAR(MAX)
//        /// </summary>
//        public string UserAgent { get; set; }

//        /// <summary>
//        /// NVARCHAR(20)
//        /// http://msdn.microsoft.com/en-us/library/cc233968.aspx
//        /// </summary>
//        public string AcceptLanguage { get; set; }

//        /// <summary>
//        /// BIGINT
//        /// </summary>
//        // ReSharper disable once InconsistentNaming
//        public System.Numerics.BigInteger MSISDN { get; set; }

//        /// <summary>
//        /// NVARCHAR(MAX)
//        /// </summary>
//        public string Referrer { get; set; }
//    }
//}