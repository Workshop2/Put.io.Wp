using System;
using System.Collections.ObjectModel;
using System.Threading;
using System.Threading.Tasks;
using Put.io.Api.ResponseObjects.Transfers;
using Put.io.Core.InvokeSynchronising;
using Put.io.Core.Storage;
using Put.io.Core.ViewModels;
using Put.io.Core.Extensions;
using System.Linq;

namespace Put.io.Core.Transfers
{
    public class AutonomousUpdater : IDisposable
    {
        private ObservableCollection<TransferViewModel> Collection { get; set; }
        private readonly TimeSpan _sleep = TimeSpan.FromSeconds(5);
        private readonly TimeSpan _startupDelay = TimeSpan.FromSeconds(10);
        private IPropertyChangedInvoke Invoker { get; set; }
        private ISettingsRepository Settings { get; set; }

        public AutonomousUpdater(ObservableCollection<TransferViewModel> transferCollection, ISettingsRepository settings, IPropertyChangedInvoke invoker)
        {
            Settings = settings;
            Collection = transferCollection;
            Invoker = invoker;

            new TaskFactory().StartNew(Startup);
        }

        private void Startup()
        {
            Thread.Sleep(_startupDelay);
            Tick();
        }

        private void Tick()
        {
            Thread.Sleep(_sleep);

            if (_disposed)
                return;

            var rest = new Api.Rest.Transfers(Settings.ApiKey);
            rest.ListTransfers(response =>
            {
                if (response.Data == null) return;

                MergeTransfers(response.Data);
                Tick();
            });
        }

        private void MergeTransfers(TransferList data)
        {
            foreach (var transfer in data.Transfers)
            {
                var source = transfer.ToModel(Invoker);
                var target = Collection.FirstOrDefault(x => x.Transfer.TransferID == source.TransferID);

                if (target == null)
                {
                    Invoker.HandleCall(() => Collection.Add(new TransferViewModel { Invoker = Invoker, Transfer = source }));
                    continue;
                }

                GeneralExtensions.Copy(source, target.Transfer);
            }

            var toDelete = Collection.Where(x => !data.Transfers.Any(y => y.id == x.Transfer.TransferID)).ToList();
            foreach (var model in toDelete)
            {
                var model1 = model;
                Invoker.HandleCall(() => Collection.Remove(model1));
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
                //TODO: Dispose
            }

            _disposed = true;
        }
        #endregion
    }
}