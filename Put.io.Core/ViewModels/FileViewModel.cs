using System;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Put.io.Core.Common;
using Put.io.Core.Extensions;
using Put.io.Core.InvokeSynchronising;
using Put.io.Core.Models;
using System.Linq;
using Put.io.Core.ProgressTracking;
using Put.io.Core.Storage;

namespace Put.io.Core.ViewModels
{
    public class FileViewModel : ViewModelBase
    {
        protected ISettingsRepository Settings { get; set; }
        protected ProgressTracker ProgressTracker { get; set; }

        public FileViewModel()
        {
            if (IsInDesignMode)
            {
                File = new File { ScreenShot = @"http://i.imgur.com/pq7lih.jpg", Name = "This is a very long file name, oh my very long like most torrents" };
                ScreenShot = new BitmapImage(new Uri(File.ScreenShot, UriKind.Absolute));
                SizeInformation = "2.4 GB";
                CreatedDate = DateTime.Now.ToShortDateString();
            }
        }


        public FileViewModel(ISettingsRepository settings, ProgressTracker tracker, IPropertyChangedInvoke invoker)
            : this()
        {
            Settings = settings;
            ProgressTracker = tracker;
            Invoker = invoker;
        }

        #region Methods
        
        private void UpdateDynamicFields()
        {
            SizeInformation = File.Size.ToFileSize();
            CreatedDate = File.CreatedDate.ToString(CultureInfo.CurrentCulture);

            const int maxFileName = 50;
            if (string.IsNullOrEmpty(File.Name) || File.Name.Length <= maxFileName)
            {
                NameTrimmed = File.Name;
            }
            else
            {
                NameTrimmed = File.Name.Substring(0, maxFileName - 3) + "...";
            }
        }

        #endregion

        #region Properties
        private File _file;
        public File File
        {
            get { return _file; }
            set
            {
                if (_file == value) return;

                _file = value;
                OnPropertyChanged();

                if (_file != null)
                    UpdateDynamicFields();
            }
        }

        private ObservableCollection<FileViewModel> _children;
        public ObservableCollection<FileViewModel> Children
        {
            get { return _children; }
            set
            {
                if (_children == value) return;

                if (value != null)
                {
                    foreach (var fileViewModel in value)
                    {
                        fileViewModel.Parent = this;
                    }
                }

                _children = value;
                OnPropertyChanged();
            }
        }

        public FileViewModel Parent { get; set; }

        public bool IsExpandable
        {
            get
            {
                return File.ContentType == ContentType.Directory;
            }
        }

        public bool IsOpenable
        {
            get { return File.ContentType == ContentType.Video; } //TODO: Work out betterer
        }

        public string Path()
        {
            if (Parent == null)
                return " / ";

            var path = Parent.Path();
            path += string.Format("{0} / ", ShrinkFileName(Parent.File.Name));

            return path;
        }

        private const int MaxFileName = 20;
        private string ShrinkFileName(string name)
        {
            if (string.IsNullOrEmpty(name))
                name = string.Empty;

            if (name.Length > MaxFileName + 3)
            {
                name = name.Substring(0, name.Length - (name.Length - MaxFileName)) + "...";
            }

            return name;
        }

        private ImageSource _screenShot;
        public ImageSource ScreenShot
        {
            get
            {
                if (_screenShot != null)
                    return _screenShot;

                if (string.IsNullOrEmpty(File.ScreenShot))
                    return null;

                //Download image with progress indication
                var transaction = ProgressTracker.StartNewTransaction();

                var imageSource = new BitmapImage();
                imageSource.ImageOpened += (sender, args) => ProgressTracker.CompleteTransaction(transaction);
                imageSource.ImageFailed += (sender, args) => ProgressTracker.CompleteTransaction(transaction);

                //Start the download
                imageSource.UriSource = new Uri(File.ScreenShot, UriKind.Absolute);

                _screenShot = imageSource;
                return _screenShot;
            }
            set
            {
                if (_screenShot == value) return;

                _screenShot = value;
                OnPropertyChanged();
            }
        }

        private string _sizeInformation;
        public string SizeInformation
        {
            get { return _sizeInformation; }
            set
            {
                if (_sizeInformation == value) return;

                _sizeInformation = value;
                OnPropertyChanged();
            }
        }

        private string _createdDate;
        public string CreatedDate
        {
            get { return _createdDate; }
            set
            {
                if (_createdDate == value) return;

                _createdDate = value;
                OnPropertyChanged();
            }
        }

        private string _nameTrimmed;
        public string NameTrimmed
        {
            get { return _nameTrimmed; }
            set
            {
                if (_nameTrimmed == value) return;

                _nameTrimmed = value;
                OnPropertyChanged();
            }
        }
        #endregion
    }
}