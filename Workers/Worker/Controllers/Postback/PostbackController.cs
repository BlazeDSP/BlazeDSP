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

namespace Worker.Controllers.Postback
{
    using System;
    using System.Data.SqlClient;
    using System.Net;
    using System.Net.Http;
    // ReSharper disable once RedundantUsingDirective
    using System.Net.Http.Headers;
    using System.Threading.Tasks;
    using System.Web.Http;

    using Blaze.DSP.Library.Constants;
    using Blaze.DSP.Library.Helpers.Http;
    using Blaze.DSP.Library.SignalR;

    using Dapper.Contrib.Extensions;

    using Microsoft.AspNet.SignalR;
    using Microsoft.Azure;

    using DatabaseConversion = Blaze.DSP.Library.Models.Database.Conversion;
    using EmberConversion = Blaze.DSP.Library.Models.Ember.Conversion;

    // TODO: Clean this up
    public class PostbackController : ApiController
    {
        private readonly string _conn = CloudConfigurationManager.GetSetting(DatabaseStrings.DatabaseConnectionStringName);

        private readonly IHubContext _conversionHubContext;

        public PostbackController()
        {
            // TODO: Refactor to own method.
            var connectionStringSignalR = CloudConfigurationManager.GetSetting(ServiceBusConnectionStrings.SignalRServiceBusConnection);
            if (string.IsNullOrWhiteSpace(connectionStringSignalR))
            {
                throw new NotImplementedException(string.Format("{0} can not be blank", ServiceBusConnectionStrings.SignalRServiceBusConnection));
            }

            GlobalHost.DependencyResolver.UseServiceBus(connectionStringSignalR, ServiceBusPathNames.SignalRServiceBus);

            _conversionHubContext = GlobalHost.ConnectionManager.GetHubContext<ConversionHub>();
        }

        public async Task<HttpResponseMessage> Get()
        {
            var fid = ControllerContext.Request.GetQueryString("fid");
            var com = ControllerContext.Request.GetQueryString("com");

            await Process(fid, com);//.ConfigureAwait(false);

            return Response();
        }

        public async Task<HttpResponseMessage> Post()
        {
            // TODO: This fails if form headers aren't set
            var fid = await ControllerContext.Request.GetFormString("fid");//.ConfigureAwait(true);
            // BUG: Can't access this value
            var com = await ControllerContext.Request.GetFormString("com");//.ConfigureAwait(true);

            await Process(fid, com);//.ConfigureAwait(false);

            return Response();
        }

        private async Task Process(string fid, string com)
        {
            // NOTE: Throw is null, only for now (should log error later is dest and flight are both null)
            if (string.IsNullOrWhiteSpace(fid))
            {
                throw new NotImplementedException();
            }

            int flightId;
            int.TryParse(fid, out flightId);

            if (flightId < 1)
            {
                throw new NotImplementedException();
            }

            decimal commission;
            decimal.TryParse(com, out commission);

            // Model
            var conversion = new DatabaseConversion
            {
                AddedDate = DateTimeOffset.Now,
                // TODO: Shit
                Flight = flightId < 1 ? (int?)null : flightId,
                Destination = null,
                // TODO: Shit
                Value = commission == decimal.Zero ? (decimal?)null : commission
            };

            using (var sql = new SqlConnection(_conn))
            {
                await sql.OpenAsync();//.ConfigureAwait(false);

                var response = await sql.InsertAsync(conversion);//.ConfigureAwait(false);
                if (response < 1)
                {
                    // TODO: Log error
                }

                sql.Close();
            }

            // SignalR
            // TODO: [Optimization] If there are no clients connected to SignalR, don't push event message.
            // TODO: Refactor to own method (singleton or simpleinjector).
            _conversionHubContext.Clients
                                 .All
                                 .newConversion(new EmberConversion
                                 {
                                     Id = conversion.Id,
                                     AddedDate = conversion.AddedDate,
                                     Flight = conversion.Flight,
                                     Destination = conversion.Destination,
                                     Value = conversion.Value
                                 });
        }

        // ReSharper disable once MemberCanBeMadeStatic.Local
        private HttpResponseMessage Response()
        {
#if DEBUG
            return Request.CreateResponse(HttpStatusCode.OK);
#else
            var response = Request.CreateResponse();

            response.StatusCode = HttpStatusCode.NoContent;

            if (response.Content == null)
            {
                response.Content = new StringContent(string.Empty);
            }

            response.Content.Headers.ContentType = new MediaTypeHeaderValue("text/html");

            return response;
#endif
        }
    }
}