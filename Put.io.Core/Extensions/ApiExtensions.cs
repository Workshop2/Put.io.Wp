using System.Collections.Generic;
using Put.io.Core.Models;
using System.Linq;
using Put.io.Core.ViewModels;

namespace Put.io.Core.Extensions
{
    public static class ApiExtensions
    {
        public static File ToModel(this Api.ResponseObjects.Files.File @this)
        {
            return new File
            {
                FileID = @this.id,
                Name = @this.name,
                ContentType = ContentTypeParser.ParseString(@this.content_type),
                Mp4Available = @this.is_mp4_available,
                ParentID = @this.parent_id,
                ScreenShot = @this.screenshot,
                Size = @this.size
            };
        }

        public static List<FileViewModel> ToModelList(this Api.ResponseObjects.Files.FileList @this)
        {
            if (@this == null || @this.files == null)
                return new List<FileViewModel>();

            return @this.files.Select(x => new FileViewModel{File = x.ToModel()}).ToList();
        }

        public static Transfer ToModel(this Api.ResponseObjects.Transfers.Transfer @this)
        {
            return new Transfer
            {
                Name = @this.name,
                Size = @this.size,
                PercentComplete = @this.percent_done,
                TransferID = @this.id
            };
        }

        public static List<TransferViewModel> ToModelList(this Api.ResponseObjects.Transfers.TransferList @this)
        {
            if(@this == null || @this.Transfers == null)
                return new List<TransferViewModel>();

            return @this.Transfers.Select(x => new TransferViewModel {Transfer = x.ToModel()}).ToList();
        }
    }
}