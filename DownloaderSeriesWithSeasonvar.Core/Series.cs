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

        public int FileSize { get; }
        public Uri FileUri { get; }
        public string Name { get; }
        public byte Number { get; }

        public override bool Equals(object obj)
        {
            if (obj == null)
                return false;

            var other = obj as Series;

            if (Name == other.Name &&
                Number == other.Number &&
                FileUri.Equals(other.FileUri) &&
                FileSize == other.FileSize)
                return true;

            return false;
        }
    }
}