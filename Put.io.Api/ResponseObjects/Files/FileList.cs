using System.Collections.Generic;

namespace Put.io.Api.ResponseObjects.Files
{
    public class FileList : BaseObject
    {
        public List<File> files { get; set; }
        public File parent { get; set; }
    }
}