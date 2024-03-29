﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DownloaderSeriesWithSeasonvar.Core
{
    public interface IInfoDownloader
    {
        List<Uri> GetInfoList(Uri address);

        Task<List<Uri>> GetInfoListAsync(Uri address);

        string GetOriginalName(Uri address);

        Task<string> GetOriginalNameAsync(Uri address);
    }
}