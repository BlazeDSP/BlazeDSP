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
    using System.Security.Cryptography.X509Certificates;
    using System.Threading.Tasks;
    using System.Web.Http;

    using Blaze.DSP.Library.Interfaces.SimpleInjector;
    using Blaze.DSP.Library.Models.Database;
    using Blaze.DSP.Library.Models.Ember;

    using Hyak.Common;

    using Microsoft.Azure;
    using Microsoft.WindowsAzure.Management.Compute;
    using Microsoft.WindowsAzure.Management.Compute.Models;

    // TODO: Clean this code up (refactor)
    public class NodesController : ApiController
    {
        private readonly IDatabase _sql;

        public NodesController(IDatabase sql)
        {
            _sql = sql;
        }

        public async Task<IHttpActionResult> Get()
        {
            // Will throw if more than one record exists with 'Selected = 1' (reason: by design)
            var mancert = await _sql.QueryAsync<ManagementCertificate>("SELECT * FROM ManagementCertificates WHERE Selected = 1").ContinueWith(a => a.Result.SingleOrDefault());
            if (mancert == null)
            {
                // TODO: Inform user no certs exist
                return Ok(new
                {
                    Nodes = new List<Node>(),
                    NodeDeployments = new List<NodeDeployment>(),
                    NodeInstances = new List<NodeInstance>()
                });
            }

            var certbytes = Convert.FromBase64String(mancert.Certificate);
            var cert = new X509Certificate2(certbytes);

            var creds = new CertificateCloudCredentials(mancert.SubscriptionId, cert);
            var cmc = new ComputeManagementClient(creds);

            // ----

            var nodes = new List<Node>();
            var nodeDeployments = new List<NodeDeployment>();
            var nodeInstances = new List<NodeInstance>();

            var slots = Enum.GetValues(typeof(DeploymentSlot)).Cast<DeploymentSlot>().ToArray(); //.OrderByDescending(x => x).ToList();

            var hostedServices = await cmc.HostedServices.ListAsync();

            foreach (var hostedService in hostedServices.Where(a => a.ServiceName.ToLower().Contains("blazedsp-node")))
            {
                nodes.Add(new Node
                {
                    Id = hostedService.ServiceName,
                    Label = hostedService.Properties.Label,
                    Location = hostedService.Properties.Location,
                    Status = hostedService.Properties.Status,
                    NodeDeployments = new List<string>()
                });

                DeploymentGetResponse deployment = null;

                foreach (var slot in slots)
                {
                    try
                    {
                        deployment = await cmc.Deployments.GetBySlotAsync(hostedService.ServiceName, slot);
                    }
                    catch (CloudException ce)
                    {
                        if (!ce.Message.Contains("No deployments were found"))
                        {
                            throw new NotImplementedException(ce.Message, ce);
                        }
                    }
                    catch (Exception ex)
                    {
                        throw new NotImplementedException(ex.Message, ex);
                    }

                    if (deployment == null)
                    {
                        var guid = Guid.NewGuid().ToString();

                        nodeDeployments.Add(new NodeDeployment
                        {
                            Id = guid,
                            DeploymentSlot = slot
                        });

                        nodes.Single(a => a.Id == hostedService.ServiceName).NodeDeployments.Add(guid);

                        continue;
                    }

                    var guids = new List<string>();
                    foreach (var instance in deployment.RoleInstances)
                    {
                        var guid = Guid.NewGuid().ToString();
                        guids.Add(guid);
                        nodeInstances.Add(new NodeInstance
                        {
                            Id = guid,
                            Name = instance.InstanceName,
                            Size = instance.InstanceSize,
                            StateDetails = instance.InstanceStateDetails,
                            Status = instance.InstanceStatus,
                            PowerState = instance.PowerState,
                            RoleName = instance.RoleName
                        });
                    }

                    nodeDeployments.Add(new NodeDeployment
                    {
                        Id = deployment.PrivateId,
                        Name = deployment.Name,
                        Label = deployment.Label,
                        DeploymentSlot = deployment.DeploymentSlot,
                        SdkVersion = deployment.SdkVersion,
                        Status = deployment.Status,
                        CreatedTime = deployment.CreatedTime,
                        LastModifiedTime = deployment.LastModifiedTime,
                        NodeInstances = guids
                    });

                    nodes.Single(a => a.Id == hostedService.ServiceName).NodeDeployments.Add(deployment.PrivateId);
                }
            }

            return Ok(new
            {
                nodes,
                nodeDeployments,
                nodeInstances
            });
        }
    }
}