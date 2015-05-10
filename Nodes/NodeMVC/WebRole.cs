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

namespace NodeMVC
{
    using System.Diagnostics;
    using System.Linq;
    using System.Threading;

    using Microsoft.WindowsAzure.Diagnostics;
    using Microsoft.WindowsAzure.ServiceRuntime;

    // ReSharper disable once UnusedMember.Global
    public class WebRole : RoleEntryPoint
    {
        private readonly ManualResetEvent _completedEvent = new ManualResetEvent(false);

        //private static readonly string FilePath = string.Format("{0}{1}", RoleEnvironment.GetLocalResource(LocalStorage.StorageName).RootPath, LocalStorage.LockName);

        //private readonly CloudBlockBlob _blockBlob;

        //public WebRole()
        //{
        //    var storageAccount = CloudStorageAccount.Parse(CloudConfigurationManager.GetSetting("Node.Storage.ConnectionString"));
        //    var blobClient = storageAccount.CreateCloudBlobClient();
        //    var container = blobClient.GetContainerReference("node");
        //    container.CreateIfNotExists();
        //    _blockBlob = container.GetBlockBlobReference(string.Format("offline.{0}.lock", RoleEnvironment.CurrentRoleInstance.Id.GetHash()));
        //}

        public override void Run()
        {
            Trace.WriteLine("Running Web Instance");

            _completedEvent.WaitOne();
        }

        public override bool OnStart()
        {
            Trace.Listeners.Add(new DiagnosticMonitorTraceListener());

            Trace.WriteLine("Starting Web Instance");



            // Set Node offline
            //_blockBlob.UploadText(string.Empty);
            //File.WriteAllText(FilePath, string.Empty);



            // Role Config Change Events
            RoleEnvironment.Changing += RoleEnvironmentChanging;
            RoleEnvironment.Changed += RoleEnvironmentChanged;
            RoleEnvironment.StatusCheck += RoleEnvironment_StatusCheck;


            // Stop the App Pool from shutting down
            //using (var server = new ServerManager())
            //{
            //    // BUG: Causes the role to fail
            //    foreach (var appPool in server.ApplicationPools) // BUG: <-----
            //    {
            //        appPool["startMode"] = "AlwaysRunning";
            //    }

            //    server.ApplicationPoolDefaults.AutoStart = true;
            //    server.ApplicationPoolDefaults.ProcessModel.IdleTimeout = TimeSpan.Zero;
            //    server.ApplicationPoolDefaults.Recycling.PeriodicRestart.Time = TimeSpan.Zero;
            //    server.CommitChanges();
            //}


            // GoGoGadget
            return base.OnStart();
        }

        public override void OnStop()
        {
            Trace.WriteLine("Stopping Web Instance");

            // Wait until all requests are finished procssing before shutting down
            var rcCounter = new PerformanceCounter("ASP.NET", "Requests Current", "");
            while (rcCounter.NextValue() > 0)
            {
                //Trace.TraceInformation("ASP.NET Requests Current = " + rcCounter.NextValue());
                Thread.Sleep(1000);
            }

            // Release blocked thread
            _completedEvent.Set();

            base.OnStop();
        }

        private void RoleEnvironment_StatusCheck(object sender, RoleInstanceStatusCheckEventArgs e)
        {
            // NOTE: Used to signal the Node failed to load Destinations
            // BUG: This fails for all but the first insance when run locally and explodes in the cloud
            //if (File.Exists(FilePath))
            ////if (_blockBlob.Exists())
            //{
            //    Trace.WriteLine(string.Format("[{0}] RoleEnvironment_StatusCheck SetBusy", DateTime.Now));

            //    e.SetBusy();
            //}
        }

        private void RoleEnvironmentChanging(object sender, RoleEnvironmentChangingEventArgs e)
        {
            Trace.TraceInformation("RoleEnvironmentChanging Event Fired");

            var changes = e.Changes
                           .OfType<RoleEnvironmentConfigurationSettingChange>()
                           .ToList();

            if (!changes.Any())
            {
                return;
            }

            e.Cancel = true; // Instruct server to reboot on change (if not canceled below)

            //// TODO: Implement (for now just repboot server)
            //// Service Bus Connection Strings Updated
            //if (changes.Any(c => c.ConfigurationSettingName == ""))
            //{
            //    e.Cancel = false; // Cancel reboot of server
            //}
        }

        private void RoleEnvironmentChanged(object sender, RoleEnvironmentChangedEventArgs e)
        {
            Trace.TraceInformation("RoleEnvironmentChanged Event Fired");

            //// TODO: Implement
            //var changes = e.Changes
            //               .OfType<RoleEnvironmentConfigurationSettingChange>()
            //               .ToList();

            //// Service Bus Connection Strings Updated
            //if (changes.Any(c => c.ConfigurationSettingName == ""))
            //{
            //    // TODO: Close Service Bus connection and reopen with new connection string
            //}
        }
    }
}