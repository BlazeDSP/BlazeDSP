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

namespace Blaze.DSP.Library.Helpers.Assembly
{
    using System;
    using System.IO;

    public static class ApplicationInformation
    {
        private static System.Reflection.Assembly _executingAssembly;

        private static Version _executingAssemblyVersion;

        private static DateTime? _compileDate;

        public static System.Reflection.Assembly ExecutingAssembly
        {
            private get { return _executingAssembly ?? (_executingAssembly = System.Reflection.Assembly.GetExecutingAssembly()); }
            set { _executingAssembly = value; }
        }

        public static Version ExecutingAssemblyVersion
        {
            get { return _executingAssemblyVersion ?? (_executingAssemblyVersion = ExecutingAssembly.GetName().Version); }
        }

        public static DateTime CompileDate
        {
            get
            {
                if (!_compileDate.HasValue)
                {
                    _compileDate = RetrieveLinkerTimestamp(ExecutingAssembly.Location);
                }

                return _compileDate ?? new DateTime();
            }
        }

        private static DateTime RetrieveLinkerTimestamp(string filePath)
        {
            const int peHeaderOffset = 60;

            const int linkerTimestampOffset = 8;

            var b = new byte[2048];

            FileStream s = null;

            try
            {
                s = new FileStream(filePath, FileMode.Open, FileAccess.Read);

                s.Read(b, 0, 2048);
            }
            finally
            {
                if (s != null)
                {
                    s.Close();
                }
            }

            var i = BitConverter.ToInt32(b, peHeaderOffset);

            var secondsSince1970 = BitConverter.ToInt32(b, i + linkerTimestampOffset);

            var dt = new DateTime(1970, 1, 1, 0, 0, 0).AddSeconds(secondsSince1970);

            return DateTime.SpecifyKind(dt, DateTimeKind.Utc);
        }
    }
}