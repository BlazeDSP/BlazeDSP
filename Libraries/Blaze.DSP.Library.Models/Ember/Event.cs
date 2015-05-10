// The MIT License (MIT)
// 
// Copyright (c) 2015 Daniel Franklin. http://blazedsp.com/
// 
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
// 
// The above copyright notice and this permission notice shall be included in
// all copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
// THE SOFTWARE.

namespace Blaze.DSP.Library.Models.Ember
{
    using System;

    using Newtonsoft.Json;

    public class Event
    {
        [JsonProperty(PropertyName = "id")]
        public int Id { get; set; }


        [JsonProperty(PropertyName = "time")]
        public DateTimeOffset Time { get; set; }


        [JsonProperty(PropertyName = "user_id")]
        public string UserId { get; set; }

        [JsonProperty(PropertyName = "user_agent")]
        public string UserAgent { get; set; }

        [JsonProperty(PropertyName = "user_language")]
        public string UserLanguage { get; set; }

        [JsonProperty(PropertyName = "user_host_address")]
        public string UserHostAddress { get; set; }
        
        [JsonProperty(PropertyName = "user_proxy_address")]
        public string UserProxyAddress { get; set; }



        [JsonProperty(PropertyName = "user_click_cost")]
        public decimal UserClickCost { get; set; }



        [JsonProperty(PropertyName = "referer")]
        public string Referer { get; set; }

        
        [JsonProperty(PropertyName = "flight")]
        public string Flight { get; set; }

        [JsonProperty(PropertyName = "destination")]
        public string Destination { get; set; }
    }
}