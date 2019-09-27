using System;
using System.Text;

namespace DownloaderSeriesWithSeasonvar.Core
{
    public class Episode
    {
        internal Episode(string name, Uri fileUri, int fileSize, int number)
        {
            Name = name;
            Number = number;
            FileUri = fileUri;
            FileSize = fileSize;
        }

        public int FileSize { get; }
        public Uri FileUri { get; }
        public string Name { get; }
        public int Number { get; }

        public override bool Equals(object obj)
        {
            return ToString().GetHashCode() == obj.GetHashCode();
        }

        public override int GetHashCode()
        {
            return ToString().GetHashCode();
        }

        public override string ToString()
        {
            string result = $"[Name:{Name}, Number:{Number}, FileSize:{FileSize}, FileUri:{FileUri.GetHashCode()}]";
            return result;
        }
    }
}