using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Serilog;

namespace PerformanceTester
{
    internal class Worker
    {
        public Worker(int workerId, string rootPath, List<Action> targets, string userName, string password)
        {
            _workerId = workerId;

            // NTLM Secured URL
            var uri = new Uri(rootPath);

            // Create a new Credential - note NTLM is not documented but works
            var credentialsCache = new CredentialCache();
            credentialsCache.Add(uri, "Negotiate", new NetworkCredential(userName, password));

            var handler = new HttpClientHandler() { Credentials = credentialsCache, PreAuthenticate = true };
            _http = new HttpClient(handler)
            {
                Timeout = new TimeSpan(0, 0, 600),
                BaseAddress = new Uri(rootPath)
            };

            _targets = targets;
        }

        public double ExecutionTime { get; set; } = 0;
        public int ErrorCount { get; set; } = 0;

        private readonly int _workerId;
        private readonly HttpClient _http;
        private readonly List<Action> _targets;

        internal async Task Run()
        {
            string target = "";
            var startAt = DateTime.Now;

            try
            {
                foreach (var t in _targets)
                {
                    target = t.Target;
                    Helper.WriteAction($"[{t.Method}] Worker={_workerId}; Target='{target}';");
                    HttpResponseMessage? response = null;

                    switch (t.Method)
                    {
                        case Helper.Methods.Get:
                            response = await _http.GetAsync(target);
                            break;
                        case Helper.Methods.Post:
                            //if (t.RequestDto == null)
                            //    break;
                            //response = await _http.PostAsJsonAsync(target, t.RequestDto);
                            break;
                        case Helper.Methods.Patch:
                            break;
                        case Helper.Methods.Put:
                            break;
                        case Helper.Methods.Delete:
                            break;
                        default:
                            break;
                    }
                    if (response == null)
                    {
                        Helper.WriteError($"[ERR] [{t.Method}] Worker={_workerId}; Target='{target}'; NO REPSO ");
                        ErrorCount++;
                    }
                    else if (!response.IsSuccessStatusCode)
                    {
                        Helper.WriteError($"[ERR] Worker={_workerId}; Target='{target}'; StatusCode='{response.StatusCode}'; Reason='{response.ReasonPhrase}'; Request='{response.RequestMessage}'");
                        ErrorCount++;
                    }
                    //else if (t.RequestDto != null && response != null && response.IsSuccessStatusCode)
                    //{
                    //    var incidents = await response.Content.ReadFromJsonAsync<IncidentListDto[]>();
                    //    if (incidents != null)
                    //        IncidentCount = incidents.Count();
                    //}
                }
            }
            catch (Exception ex)
            {
                Helper.WriteError($"[ERR] Worker={_workerId}; Target='{target}'; err='{ex.Message}'");
                ErrorCount++;
                Log.Error(ex, "Worker");
            }
            finally
            {
                _http.Dispose();
                var stopAt = DateTime.Now;
                ExecutionTime = stopAt.Subtract(startAt).TotalSeconds;
                Helper.WriteResults($"[TIME] Worker={_workerId}; ExecutionTime='{ExecutionTime}';");
            }
        }


    }
}
