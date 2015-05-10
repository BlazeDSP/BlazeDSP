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

namespace NodeMVC.Utils
{
    using System;
    using System.Runtime.Caching;

    using Blaze.DSP.Library.Models.Update;

    using Ether.WeightedSelector;

    public static class DestinationsCache
    {
        public static WeightedSelector<DestinationsUpdate> Get(int? id)
        {
            return Get(id.ToString());
        }

        public static WeightedSelector<DestinationsUpdate> Get(string id)
        {
            return MemoryCache.Default.Get(id) as WeightedSelector<DestinationsUpdate>;
        }

        public static void Set(int? id, DestinationsUpdate data)
        {
            Set(id.ToString(), data);
        }

        public static void Set(string id, DestinationsUpdate data)
        {
            WeightedSelector<DestinationsUpdate> ws;
            if (MemoryCache.Default.Contains(id))
            {
                ws = Get(id);
                if (ws == null)
                {
                    throw new NotImplementedException("Cache 'list' is NULL");
                }

                // TODO: Fix bug where items can get added multiple times
                ws.Add(data, 1);
            }
            else
            {
                ws = new WeightedSelector<DestinationsUpdate>();
                ws.Add(data, 1);
            }

            MemoryCache.Default.Set(new CacheItem(id, ws), new CacheItemPolicy
            {
                AbsoluteExpiration = DateTimeOffset.MaxValue,
                Priority = CacheItemPriority.NotRemovable
            });
        }
    }
}