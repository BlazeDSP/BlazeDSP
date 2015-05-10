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

namespace Blaze.DSP.Library.Models.Database
{
    using System;
    using System.Collections.Generic;

    using Dapper.Contrib.Extensions;

    using Interfaces.Database;

    public class Campaign : ICampaign
    {
        public int Id { get; set; }

        public DateTimeOffset AddedDate { get; set; }
        public DateTimeOffset? ModifiedDate { get; set; }

        public string Name { get; set; }
        public string Description { get; set; }

        public DateTimeOffset? StartDate { get; set; }
        public DateTimeOffset? EndDate { get; set; }

        /// <summary>
        /// Ad Network
        /// </summary>
        public int? Network { get; set; }

        public string UserId { get; set; }

        // TODO: Implement in Controller
        [Computed]
        public IEnumerable<int> Flights { get; set; }
        //[Computed]
        //public IEnumerable<int> Actions { get; set; }
        [Computed]
        public IEnumerable<int> Tags { get; set; }
        [Computed]
        public IEnumerable<int> Notes { get; set; }
    }
}