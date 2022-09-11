using Serilog;

namespace PerformanceTester
{
    public class WorkerManager
    {
        public WorkerManager(string rootPath, string target, int workerCount, string userName = "", string password = "")
        {

            _target = target;
            _rootPath = rootPath;
            _workerCount = workerCount;
            _userName = userName;
            _password = password;
        }

        private readonly string _target;
        private readonly string _rootPath;
        private readonly int _workerCount;
        private readonly string _userName;
        private readonly string _password;

        public async Task RunAsync()
        {
            var startAt = DateTime.Now;
            List<Action> targets = new();
            double executionTime = 0;
            int errorCount = 0;
            try
            {
                switch (_target)
                {
                    case "0":
                        targets = Helper.Targets0();
                        break;
                    case "1":
                        targets = Helper.Targets1();
                        break;
                    case "2":
                        targets = Helper.Targets2();
                        break;
                    case "3":
                        targets = Helper.Targets3();
                        break;
                    case "4":
                        targets = Helper.Targets4();
                        break;
                    case "5":
                        targets = Helper.Targets5();
                        break;
                    case "6":
                        targets = Helper.Targets6();
                        break;
                    case "7":
                        targets = Helper.Targets7();
                        break;
                    default:
                        Helper.WriteError("NO TARGET SPECIFIED!");
                        return;
                }

                List<Worker> workers = new List<Worker>();
                for (int i = 0; i < _workerCount; i++)
                {
                    workers.Add(new Worker(i, _rootPath, targets, _userName, _password));
                }

                var options = new ParallelOptions()
                {
                    MaxDegreeOfParallelism = _workerCount < 2 ? 10 : _workerCount //(_workerCount / 2)
                };
                await Parallel.ForEachAsync(workers, options, async (worker, ct) => {
                    await worker.Run();
                });

                foreach (var t in workers)
                {
                    executionTime = executionTime  + t.ExecutionTime;
                    errorCount = errorCount + t.ErrorCount;
                }
            }
            catch (Exception ex)
            {
                Helper.WriteAction($"[ERR] All Workers; Error='{ex.Message}';");
                Log.Error(ex, "WorkerManager");
            }
            finally
            {
                var stopAt = DateTime.Now;
                var totalSecs = stopAt.Subtract(startAt).TotalSeconds;
                double transactions = (_workerCount * targets.Count);
                double trnPerSecond = transactions / totalSecs;
                double avgExectionTime = executionTime / _workerCount;
                double errRate = errorCount / transactions * 100;

                Log.Information($"[TIME] All Workers; Ended on='{stopAt:yyyy-MM-dd HH:mm:ss}';");

                Helper.WriteResults($"-----------------------------------------");
                Helper.WriteResults($"[TIME] All Workers;  ");
                Helper.WriteResults($"  Ended on='{stopAt:yyyy-MM-dd HH:mm:ss}';");
                Helper.WriteResults($"  ExecutionTime='{totalSecs}';");
                Helper.WriteResults($"  Transactions per second='{trnPerSecond}';");
                Helper.WriteResults($"  Workers='{_workerCount}'; ");
                Helper.WriteResults($"  Transactions='{transactions}';");
                Helper.WriteResults($"  ErrorCount='{errorCount}'; ");
                Helper.WriteResults($"  Error Rate='{errRate}';");
                Helper.WriteResults($"-----------------------------------------");
            }

        }
    }
}