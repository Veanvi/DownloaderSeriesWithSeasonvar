using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DownloaderSeriesWithSeasonvar.Core
{
    public interface IWebRequester : IDisposable
    {
        string GetWebPageSource(string address);

        Task<string> GetWebPageSourceAsync(string address);
    }
}