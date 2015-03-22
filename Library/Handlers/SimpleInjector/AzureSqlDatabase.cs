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

using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Threading.Tasks;

using Dapper;
using Dapper.Contrib.Extensions;

using Library.Interfaces.SimpleInjector;

namespace Library.Handlers.SimpleInjector
{
    public class AzureSqlDatabase : IDatabase, IDisposable
    {
        private readonly SqlConnection _sql;

        public AzureSqlDatabase(string conn)
        {
            _sql = new SqlConnection(conn);
        }

        public void Dispose()
        {
            _sql.Dispose();
        }

        public Task<IEnumerable<T>> QueryAsync<T>(string sql, dynamic param = null)
        {
            return SqlMapper.QueryAsync<T>(_sql, sql, param);
        }

        public Task<int> CreateAsync<T>(T entry) where T : class
        {
            return _sql.InsertAsync<T>(entry);
        }

        public Task<T> ReadAsync<T>(dynamic id) where T : class
        {
            return SqlMapperExtensions.GetAsync<T>(_sql, id);
        }

        public Task<bool> UpdateAsync<T>(T entry) where T : class
        {
            return _sql.UpdateAsync<T>(entry);
        }

        public Task<bool> DeleteAsync<T>(T entry) where T : class
        {
            return _sql.DeleteAsync<T>(entry);
        }
    }
}