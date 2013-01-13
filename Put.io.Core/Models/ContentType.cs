using System;
using System.Linq;

namespace Put.io.Core.Models
{
    public enum ContentType
    {
        Directory,
        Video,
        Music,
        Image,
        Other
    }

    public static class ContentTypeParser
    {
        private static readonly string[] DirectoryTypes = new string[] { "application/x-directory" };

        public static ContentType ParseString(string type)
        {
            if (DirectoryTypes.Any(x => x.Equals(type, StringComparison.InvariantCultureIgnoreCase)))
                return ContentType.Directory;

            var typeParts = type.Split('/');
            if (!typeParts.Any())
                return ContentType.Other;

            if (typeParts[0].Equals("video", StringComparison.InvariantCultureIgnoreCase))
                return ContentType.Video;

            if (typeParts[0].Equals("audio", StringComparison.InvariantCultureIgnoreCase))
                return ContentType.Music;

            if (typeParts[0].Equals("image", StringComparison.InvariantCultureIgnoreCase))
                return ContentType.Image;
            
            return ContentType.Other;
        }


    }
}