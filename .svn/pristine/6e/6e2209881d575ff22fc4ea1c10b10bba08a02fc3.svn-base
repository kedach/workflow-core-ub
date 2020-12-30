using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ConcurrentCollections;
using Microsoft.Extensions.Logging;
using WorkflowCore.Interface;
using WorkflowCore.Models;

namespace WorkflowCore.Services.BackgroundTasks
{
    internal abstract class QueueConsumer : IBackgroundTask
    {
        protected abstract QueueType Queue { get; }
        //消费的最大并发数量
        protected virtual int MaxConcurrentItems => Math.Max(Environment.ProcessorCount, 2);
        protected virtual bool EnableSecondPasses => false;

        protected readonly IQueueProvider QueueProvider;
        protected readonly ILogger Logger;
        protected readonly WorkflowOptions Options;
        protected Task DispatchTask;        
        private CancellationTokenSource _cancellationTokenSource;
        //承载消费的数组
        private Dictionary<string, EventWaitHandle> _activeTasks;
        private ConcurrentHashSet<string> _secondPasses;

        protected QueueConsumer(IQueueProvider queueProvider, ILoggerFactory loggerFactory, WorkflowOptions options)
        {
            QueueProvider = queueProvider;
            Options = options;
            Logger = loggerFactory.CreateLogger(GetType());

            _activeTasks = new Dictionary<string, EventWaitHandle>();
            _secondPasses = new ConcurrentHashSet<string>();
        }

        protected abstract Task ProcessItem(string itemId, CancellationToken cancellationToken);

        public virtual void Start()
        {
            if (DispatchTask != null)
            {
                throw new InvalidOperationException();
            }
            //https://docs.microsoft.com/zh-cn/dotnet/standard/parallel-programming/task-cancellation 
            //并行编程
            _cancellationTokenSource = new CancellationTokenSource();

            DispatchTask = Task.Factory.StartNew(Execute, TaskCreationOptions.LongRunning);
        }

        public virtual void Stop()
        {
            _cancellationTokenSource.Cancel();
            DispatchTask.Wait();
            DispatchTask = null;
        }

        private async Task Execute()
        {
            var cancelToken = _cancellationTokenSource.Token;            
            //判定队列是否活动
            while (!cancelToken.IsCancellationRequested)
            {
                try
                {
                    var activeCount = 0;
                    lock (_activeTasks)
                    {
                        activeCount = _activeTasks.Count;
                    }
                    if (activeCount >= MaxConcurrentItems)
                    {
                        await Task.Delay(Options.IdleTime);
                        continue;
                    }

                    var item = await QueueProvider.DequeueWork(Queue, cancelToken);

                    if (item == null)
                    {
                        if (!QueueProvider.IsDequeueBlocking)
                            await Task.Delay(Options.IdleTime, cancelToken);
                        continue;
                    }

                    var hasTask = false;
                    lock (_activeTasks)
                    {
                        hasTask = _activeTasks.ContainsKey(item);
                    }
                    if (hasTask)
                    {
                        _secondPasses.Add(item);
                        //默认回推给队列，即不允许同一实例执行多次
                        if (!EnableSecondPasses)
                            await QueueProvider.QueueWork(item, Queue);
                        continue;
                    }                   

                    _secondPasses.TryRemove(item);

                    var waitHandle = new ManualResetEvent(false);
                    lock (_activeTasks)
                    {
                        _activeTasks.Add(item, waitHandle);
                    }
                    var task = ExecuteItem(item, waitHandle);
                }
                catch (OperationCanceledException)
                {
                }
                catch (Exception ex)
                {
                    Logger.LogError(ex, ex.Message);
                }
            }

            List<EventWaitHandle> toComplete;
            lock (_activeTasks)
            {
                toComplete = _activeTasks.Values.ToList();
            }

            foreach (var handle in toComplete)
                handle.WaitOne();
        }

        private async Task ExecuteItem(string itemId, EventWaitHandle waitHandle)
        {
            try
            {
                await ProcessItem(itemId, _cancellationTokenSource.Token);
                while (EnableSecondPasses && _secondPasses.Contains(itemId))
                {
                    _secondPasses.TryRemove(itemId);
                    await ProcessItem(itemId, _cancellationTokenSource.Token);
                }
            }
            catch (OperationCanceledException)
            {
                Logger.LogInformation($"Operation cancelled while processing {itemId}");
            }
            catch (Exception ex)
            {
                Logger.LogError(default(EventId), ex, $"Error executing item {itemId} - {ex.Message}");
            }
            finally
            {
                waitHandle.Set();
                lock (_activeTasks)
                {
                    _activeTasks.Remove(itemId);
                }
            }
        }
    }
}
