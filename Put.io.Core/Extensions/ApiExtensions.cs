using System;
using System.Collections.Generic;
using Put.io.Api.ResponseObjects.Files;
using Put.io.Api.ResponseObjects.Transfers;
using Put.io.Core.InvokeSynchronising;
using Put.io.Core.Models;
using System.Linq;
using Put.io.Core.ViewModels;
using File = Put.io.Core.Models.File;
using Transfer = Put.io.Core.Models.Transfer;

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

        public static List<FileViewModel> ToModelList(this FileList @this, IPropertyChangedInvoke invoker)
        {
            if (@this == null || @this.files == null)
                return new List<FileViewModel>();

            return @this.files.Select(x => new FileViewModel { File = x.ToModel(), Invoker = invoker }).ToList();
        }

        public static Transfer ToModel(this Api.ResponseObjects.Transfers.Transfer @this, IPropertyChangedInvoke invoker)
        {
            return new Transfer
            {
                Name = @this.name,
                Size = @this.size,
                PercentComplete = @this.percent_done,
                TransferID = @this.id,
                Status = @this.status.ToStatusType(),
                Invoker = invoker
            };
        }

        public static List<TransferViewModel> ToModelList(this TransferList @this, IPropertyChangedInvoke invoker)
        {
            if (@this == null || @this.Transfers == null)
                return new List<TransferViewModel>();

            return @this.Transfers.Select(x => new TransferViewModel { Transfer = x.ToModel(invoker), Invoker = invoker }).ToList();
        }

        public static StatusType ToStatusType(this string @this)
        {
            StatusType output;

            return Enum.TryParse(@this.Replace("_", string.Empty), true, out output) ? output : StatusType.Other;
        }
    }
}