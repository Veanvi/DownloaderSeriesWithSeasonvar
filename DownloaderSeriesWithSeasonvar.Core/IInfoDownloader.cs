using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DownloaderSeriesWithSeasonvar.Core
{
    public interface IInfoDownloader<T>
    {
        bool IsEnableTorProxy { get; }
        bool IsHeadless { get; }

        List<T> GetInfoList(Uri address);

        Task<List<T>> GetInfoListAsync(Uri address);

        string GetOriginalName(Uri address);

        Task<string> GetOriginalNameAsync(Uri address);
    }
}