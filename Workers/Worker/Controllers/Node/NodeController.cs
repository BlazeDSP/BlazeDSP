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

namespace Worker.Controllers.Node
{
    using System.Collections.Generic;
    using System.Data.SqlClient;
    using System.Linq;
    using System.Threading.Tasks;
    using System.Web.Http;

    using Blaze.DSP.Library.Constants;
    using Blaze.DSP.Library.Models.Update;

    using Dapper;

    using Microsoft.Azure;

    // TODO: This should push other configuration data as well as DestinationsUpdate
    public class NodeController : ApiController
    {
        private readonly string _conn = CloudConfigurationManager.GetSetting(DatabaseStrings.DatabaseConnectionStringName);
        private readonly string _auth = CloudConfigurationManager.GetSetting(Endpoint.AuthenticationStringName);

        public async Task<IHttpActionResult> Get()
        {
            if (!Request.Headers.Contains(Endpoint.AuthenticationHeader) || !Request.Headers.GetValues(Endpoint.AuthenticationHeader).Contains(_auth))
            {
                return Unauthorized();
            }

            IEnumerable<dynamic> datas;

            using (var sql = new SqlConnection(_conn))
            {
                await sql.OpenAsync();//.ConfigureAwait(false);

                // TODO: Clean up query
                datas = await sql.QueryAsync<DestinationsUpdate>("SELECT bfd.Flight, bfd.Destination, dest.Url FROM BridgeFlightsDestinations bfd JOIN Destinations dest ON dest.Id = bfd.Destination");//.ConfigureAwait(false);

                sql.Close();
            }

            return Ok(datas);
        }

        public async Task<IHttpActionResult> Get(int id)
        {
            if (!Request.Headers.Contains(Endpoint.AuthenticationHeader) || !Request.Headers.GetValues(Endpoint.AuthenticationHeader).Contains(_auth))
            {
                return Unauthorized();
            }

            IEnumerable<dynamic> datas;

            using (var sql = new SqlConnection(_conn))
            {
                await sql.OpenAsync();//.ConfigureAwait(false);

                // TODO: Clean up query
                datas = await sql.QueryAsync<DestinationsUpdate>("SELECT bfd.Flight, bfd.Destination, dest.Url FROM BridgeFlightsDestinations bfd JOIN Destinations dest ON dest.Id = bfd.Destination WHERE bfd.Destination = @Id", new { Id = id });//.ConfigureAwait(false);

                sql.Close();
            }

            return Ok(datas);
        }
    }
}