namespace Blaze.DSP.Library.Authenticator
{
    using System;
    using System.Globalization;
    using System.Net;
    using System.Security.Cryptography;

    public class GoogleAuthenticator
    {
        // Based on the code at http://stackoverflow.com/a/12398317/465404
        private const int IntervalLength = 30;

        private const int PinLength = 6;

        private static readonly int PinModulo = (int)Math.Pow(10, PinLength);

        private static readonly DateTime UnixEpoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);

        /// <summary>
        ///     Number of intervals that have elapsed.
        /// </summary>
        public static long CurrentInterval
        {
            get
            {
                var elapsedSeconds = (long)Math.Floor((DateTime.UtcNow - UnixEpoch).TotalSeconds);

                return elapsedSeconds / IntervalLength;
            }
        }

        public static byte[] GenerateSecretKey()
        {
            var secretKey = new byte[10];

            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(secretKey);
            }

            return secretKey;
        }

        /// <summary>
        ///     Generates a QR code bitmap for provisioning.
        /// </summary>
        public byte[] GenerateProvisioningImage(string identifier, byte[] key, int width, int height)
        {
            var keyString = Base32Encoder.ToBase32String(key);
            var provisionUrl = WebUtility.UrlEncode(string.Format("otpauth://totp/{0}?secret={1}", identifier, keyString));

            var chartUrl = string.Format("https://chart.apis.google.com/chart?cht=qr&chs={0}x{1}&chl={2}", width, height, provisionUrl);
            using (var client = new WebClient())
            {
                return client.DownloadData(chartUrl);
            }
        }

        /// <summary>
        ///     Generates a pin for the given key.
        /// </summary>
        public string GeneratePin(byte[] key)
        {
            return GeneratePin(key, CurrentInterval);
        }

        /// <summary>
        ///     Generates a pin by hashing a key and counter.
        /// </summary>
        public static string GeneratePin(byte[] key, long counter)
        {
            const int sizeOfInt32 = 4;

            var counterBytes = BitConverter.GetBytes(counter);

            if (BitConverter.IsLittleEndian)
            {
                //spec requires bytes in big-endian order
                Array.Reverse(counterBytes);
            }

            var hash = new HMACSHA1(key).ComputeHash(counterBytes);
            var offset = hash[hash.Length - 1] & 0xF;

            var selectedBytes = new byte[sizeOfInt32];
            Buffer.BlockCopy(hash, offset, selectedBytes, 0, sizeOfInt32);

            if (BitConverter.IsLittleEndian)
            {
                //spec interprets bytes in big-endian order
                Array.Reverse(selectedBytes);
            }

            var selectedInteger = BitConverter.ToInt32(selectedBytes, 0);

            //remove the most significant bit for interoperability per spec
            var truncatedHash = selectedInteger & 0x7FFFFFFF;

            //generate number of digits for given pin length
            var pin = truncatedHash % PinModulo;

            return pin.ToString(CultureInfo.InvariantCulture).PadLeft(PinLength, '0');
        }
    }
}