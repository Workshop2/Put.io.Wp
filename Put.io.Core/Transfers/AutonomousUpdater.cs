using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Threading;
using System.Threading.Tasks;
using Put.io.Core.Models;
using Put.io.Core.Storage;
using Put.io.Core.ViewModels;
using System.Linq;

namespace Put.io.Core.Transfers
{
    public class AutonomousUpdater : IDisposable
    {
        private ObservableCollection<TransferViewModel> Collection { get; set; }
        private Dictionary<int, Task> Tasks { get; set; }
        private ISettingsRepository Settings { get; set; }
        private readonly TimeSpan Sleep = new TimeSpan(0, 0, 5);

        public AutonomousUpdater(ObservableCollection<TransferViewModel> transferCollection, ISettingsRepository settings)
        {
            Settings = settings;
            transferCollection.CollectionChanged += CollectionChanged;
            Collection = transferCollection;
            Tasks = new Dictionary<int, Task>();

            MergeTasks();
        }

        private void MergeTasks()
        {
            var matching = Tasks.Where(x => Collection.Any(y => y.Transfer.TransferID == x.Key)).ToList();
            var removed = Tasks.Where(x => !Collection.Any(y => y.Transfer.TransferID == x.Key)).ToList();
            var newTasks = Collection.Where(x => !Tasks.Any(y => y.Key == x.Transfer.TransferID)).ToList();

            foreach (var task in removed)
            {
                Tasks.Remove(task.Key);
            }

            var factory = new TaskFactory();
            foreach (var task in newTasks)
            {
                if (task.Transfer.Status != StatusType.Downloading && task.Transfer.Status != StatusType.InQueue)
                    continue;

                var toProcess = task;
                var newTask = factory.StartNew(() => ProcessSomething(toProcess));
                Tasks.Add(toProcess.Transfer.TransferID, newTask);
            }

        }

        private void ProcessSomething(TransferViewModel task)
        {
            if (task.Transfer.Status != StatusType.Downloading)
                return;

            if (Tasks.All(x => x.Key != task.Transfer.TransferID))
                return;

            var rester = new Api.Rest.Transfers(Settings.ApiKey);
            rester.GetTransfer(task.Transfer.TransferID, response =>
            {
                task.Transfer.PercentComplete = response.Data.transfer.percent_done;
                Thread.Sleep(Sleep);
                ProcessSomething(task);
            });
        }

        private void CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            switch (e.Action)
            {
                case NotifyCollectionChangedAction.Reset:
                    Tasks.Clear();
                    break;
                default:
                    MergeTasks();
                    break;
            }
        }

        #region Disposable
        private bool _disposed;

        public void Dispose()
        {
            Dispose(true);
        }

        private void Dispose(bool disposing)
        {
            if (_disposed) return;

            if (disposing)
            {
                Collection.CollectionChanged -= CollectionChanged;
                Tasks.Clear();
            }

            _disposed = true;
        }
        #endregion
    }
}