using System.IO.IsolatedStorage;
using System.Threading.Tasks;

namespace Put.io.Core.Storage
{
    public class SettingsRepository : ISettingsRepository
    {
        private IsolatedStorageSettings Storage { get; set; }

        public SettingsRepository(IsolatedStorageSettings storage)
        {
            Storage = storage;
        }

        public void Save()
        {
            new TaskFactory().StartNew(() => Storage.Save());
        }

        public string ApiKey
        {
            get
            {
                if (Storage.Contains("ApiKey"))
                    return Storage["ApiKey"] as string;

                return string.Empty;
            }
            set
            {
                Storage["ApiKey"] = value;
                Save();
            }
        }
    }
}