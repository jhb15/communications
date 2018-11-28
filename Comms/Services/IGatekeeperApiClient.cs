using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace Comms.Services
{
    public interface IGatekeeperApiClient
    {
        Task<HttpResponseMessage> GetAsync(string path);
    }
}
