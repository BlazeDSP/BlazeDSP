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

namespace Frontend.Controllers.Api.Management
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net;
    using System.Net.Http;
    using System.Security.Cryptography.X509Certificates;
    using System.Threading.Tasks;
    using System.Web.Http;
    using System.Xml.Linq;

    using Blaze.DSP.Library.Interfaces.SimpleInjector;
    using Blaze.DSP.Library.Models.Database;

    public class ManagementCertificatesController : ApiController
    {
        // Strip the actual cert from response (we should never send the cert to the dashboard)
        private const string Columns = "Id, AddedDate, ModifiedDate, Selected, SchemaVersion, PublishMethod, ServiceManagementUrl, SubscriptionId, Name, CertificateThumbprint";
        private readonly IDatabase _sql;

        public ManagementCertificatesController(IDatabase sql)
        {
            _sql = sql;
        }

        public async Task<IHttpActionResult> Get()
        {
#if RELEASE_DEMO
            var certs = await _sql.QueryAsync<ManagementCertificate>(string.Format("SELECT {0} FROM ManagementCertificates", Columns)).ContinueWith(a =>
            {
                foreach (var cert in a.Result)
                {
                    cert.CertificateThumbprint = cert.CertificateThumbprint.Substring(0, 2) + "..." + cert.CertificateThumbprint.Substring(cert.CertificateThumbprint.Length - 2, 2);
                    cert.SubscriptionId = cert.SubscriptionId.Substring(0, 2) + "..." + cert.SubscriptionId.Substring(cert.SubscriptionId.Length - 2, 2);
                }
                return a.Result;
            });
            return Ok(certs);
#else
            return Ok(await _sql.QueryAsync<ManagementCertificate>(string.Format("SELECT {0} FROM ManagementCertificates", Columns)));
#endif
        }

        public async Task<IHttpActionResult> Get(int id)
        {
#if RELEASE_DEMO
            var certs = await _sql.QueryAsync<ManagementCertificate>(string.Format("SELECT {0} FROM ManagementCertificates WHERE Id = @Id", Columns), new { Id = id }).ContinueWith(a =>
            {
                var cert = a.Result.SingleOrDefault();

                if (cert == null)
                {
                    return null;
                }

                cert.CertificateThumbprint = cert.CertificateThumbprint.Substring(0, 2) + "..." + cert.CertificateThumbprint.Substring(cert.CertificateThumbprint.Length - 2, 2);
                cert.SubscriptionId = cert.SubscriptionId.Substring(0, 2) + "..." + cert.SubscriptionId.Substring(cert.SubscriptionId.Length - 2, 2);

                return cert;
            });
            return Ok(certs);
#else
            return Ok(await _sql.QueryAsync<ManagementCertificate>(string.Format("SELECT {0} FROM ManagementCertificates WHERE Id = @Id", Columns), new
            {
                Id = id
            }).ContinueWith(a => a.Result.SingleOrDefault()));
#endif
        }

        public async Task<IHttpActionResult> Post()
        {
            // TODO: Clean this up
            var files = await Request.Content
                                     .ReadAsMultipartAsync()
                                     .ContinueWith(a =>
                                     {
                                         if (a.IsFaulted || a.IsCanceled)
                                         {
                                             throw new HttpResponseException(HttpStatusCode.InternalServerError);
                                         }

                                         return a.Result.Contents;
                                     });
            if (files.Count < 1)
            {
                return Ok();
            }

            var certs = new List<ManagementCertificate>();

            foreach (var file in files)
            {
                var doc = XDocument.Load(await file.ReadAsStreamAsync());

                // It is possible to have multiple certs within the same file, currently only using first.
                var subscription = doc.Descendants("PublishProfile").Select(pub =>
                {
                    if (pub == null)
                    {
                        return null;
                    }

                    var sub = pub.Element("Subscription");
                    if (sub == null)
                    {
                        return null;
                    }

                    decimal schemaVersion;
                    if (!decimal.TryParse(pub.Attribute("SchemaVersion").Value, out schemaVersion))
                    {
                        return null;
                    }
                    var publishMethod = pub.Attribute("PublishMethod").Value;

                    var serviceManagementUrl = sub.Attribute("ServiceManagementUrl").Value;
                    var subscriptionId = sub.Attribute("Id").Value;
                    var name = sub.Attribute("Name").Value;
                    var certificate = sub.Attribute("ManagementCertificate").Value;

                    if (string.IsNullOrWhiteSpace(certificate))
                    {
                        return null;
                    }
                    var thumbprint = new X509Certificate2(Convert.FromBase64String(certificate)).Thumbprint;

                    return new ManagementCertificate
                    {
                        AddedDate = DateTimeOffset.UtcNow,
                        Selected = false,
                        SchemaVersion = schemaVersion,
                        PublishMethod = publishMethod,
                        ServiceManagementUrl = serviceManagementUrl,
                        SubscriptionId = subscriptionId,
                        Name = name,
                        Certificate = certificate,
                        CertificateThumbprint = thumbprint
                    };
                }).FirstOrDefault();

                if (subscription == null)
                {
                    continue;
                }

                var result = await _sql.CreateAsync(subscription);
                if (result < 1)
                {
                    throw new HttpResponseException(HttpStatusCode.InternalServerError);
                }

                // Strip the actual cert from response (we should never send the cert to the dashboard)
                subscription.Certificate = string.Empty;

                certs.Add(subscription);
            }

            return Ok(certs);
        }

        public async Task<IHttpActionResult> Put(int id, [FromBody] ManagementCertificate model)
        {
            if (!ModelState.IsValid)
            {
                Validate(model);
                return BadRequest(ModelState);
            }

            // Set ID because Ember doesn't send it
            model.Id = id;

            if (!model.Selected)
            {
                return Ok(model);
            }

            if (await _sql.ExecuteAsync("UPDATE ManagementCertificates SET Selected = 0") < 1)
            {
                return BadRequest();
            }

            // Ignore model values
            model.ModifiedDate = DateTimeOffset.UtcNow;

            var result = await _sql.ExecuteAsync("UPDATE ManagementCertificates SET ModifiedDate = @ModifiedDate, Selected = @Selected WHERE Id = @Id", new
            {
                model.Id, model.ModifiedDate, model.Selected
            });
            if (result < 1)
            {
                return BadRequest();
            }

            return Ok(model);
        }

        public async Task<IHttpActionResult> Delete(int id)
        {
            var result = await _sql.DeleteAsync(new ManagementCertificate
            {
                Id = id
            });
            if (!result)
            {
                return BadRequest();
            }

            return Ok(id);
        }
    }
}