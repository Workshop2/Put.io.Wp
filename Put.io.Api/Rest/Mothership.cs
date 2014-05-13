using System;
using System.Diagnostics;
using System.Globalization;
using RestSharp;

namespace Put.io.Api.Rest
{
    public class Mothership
    {
        public void Fire(Data data)
        {
            var client = new RestClient("http://x-volt.com");
            string path = string.Format("putio/index.php?no-cache={0}", DateTime.Now.Ticks.ToString(CultureInfo.InvariantCulture));
            var request = new RestRequest(path, Method.POST);
            request.Parameters.Clear();

            string dataString = request.JsonSerializer.Serialize(data);
            request.AddParameter("data", dataString, ParameterType.GetOrPost);

            client.ExecuteAsync(request, (response, handle) =>
            {
                if (response.Content != "1")
                {
                    Debug.WriteLine("Error calling home");
                }
            });
        }

        public class Data
        {
            public string DeviceUniqueId { get; set; }
            public string DeviceFirmwareVersion { get; set; }
            public string DeviceManufacturer { get; set; }
            public string DeviceName { get; set; }
            public string DeviceTotalMemory { get; set; }
            public string PhysicalScreenResolution { get; set; }
        }
    }
}