using System;

namespace DownloaderSeriesWithSeasonvar.Core
{
    public class Series
    {
        internal Series(string name, Uri fileUri, int fileSize, byte number)
        {
            Name = name;
            Number = number;
            FileUri = fileUri;
            FileSize = fileSize;
        }

        public string Name { get; }
        public byte Number { get; }
        public Uri FileUri { get; }
        public int FileSize { get; }
    }
}