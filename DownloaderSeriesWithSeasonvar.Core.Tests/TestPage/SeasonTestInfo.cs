using System;
using System.Collections.Generic;
using System.Text;

namespace DownloaderSeriesWithSeasonvar.Core.Tests.TestPage
{
    internal class SeasonTestInfo
    {
        public SeasonTestInfo(string webSource,
                              string jsonWebSourcen,
                              string jsonString,
                              string name,
                              string uri,
                              string[] episodeFileNames)
        {
            this.WebSource = webSource;
            this.JsonWebSource = jsonWebSourcen;
            this.JsonString = jsonString;
            this.Name = name;
            this.Uri = uri;
            this.EpisodeFileNames = episodeFileNames;
        }

        public int EpisodeCounts => EpisodeFileNames.Length;
        public string[] EpisodeFileNames { get; }
        public string JsonString { get; }
        public string JsonWebSource { get; }
        public string Name { get; }
        public string Uri { get; }
        public string WebSource { get; }
    }
}