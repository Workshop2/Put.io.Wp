using System;
using System.Linq;

namespace Put.io.Core.Models
{
    public enum ContentType
    {
        Directory,
        Video,
        Music,
        Other
    }

    public static class ContentTypeParser
    {
        private static readonly string[] DirectoryTypes = new string[] { "application/x-directory" };
        private static readonly string[] VideoTypes = new string[] { "" };
        private static readonly string[] MusicTypes = new string[] { "" };

        public static ContentType ParseString(string type)
        {
            if (DirectoryTypes.Any(x => x.Equals(type, StringComparison.InvariantCultureIgnoreCase)))
                return ContentType.Directory;

            if (VideoTypes.Any(x => x.Equals(type, StringComparison.InvariantCultureIgnoreCase)))
                return ContentType.Video;

            if (MusicTypes.Any(x => x.Equals(type, StringComparison.InvariantCultureIgnoreCase)))
                return ContentType.Music;
            
            return ContentType.Other;
        }


    }
}