using Plugin.Media;
using Plugin.Permissions;
using Plugin.Permissions.Abstractions;
using SwipeCardView.Sample.Droid;
using SwipeCardView.Sample.ViewModel;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;


namespace SwipeCardView.Sample.View
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Profile : ContentPage
    {
        [Obsolete]
        public Profile()
        {
            InitializeComponent();
            //Ask for permissions
            Device.BeginInvokeOnMainThread(async () => await AskForPermissions());
        }




        /// <summary>
        ///     Make sure Permissions are given to the users storage.
        /// </summary>
        /// <returns></returns>
        [Obsolete]
        private async Task<bool> AskForPermissions()
        {
            try
            {
                await CrossMedia.Current.Initialize();

                var storagePermissions = await CrossPermissions.Current.CheckPermissionStatusAsync(Permission.Storage);
                var photoPermissions = await CrossPermissions.Current.CheckPermissionStatusAsync(Permission.Photos);
                if (storagePermissions != PermissionStatus.Granted || photoPermissions != PermissionStatus.Granted)
                {
                    var results = await CrossPermissions.Current.RequestPermissionsAsync(new[] { Permission.Storage, Permission.Photos });
                    storagePermissions = results[Permission.Storage];
                    photoPermissions = results[Permission.Photos];
                }

                if (storagePermissions != PermissionStatus.Granted || photoPermissions != PermissionStatus.Granted)
                {
                    await DisplayAlert("Permissions Denied!", "Please go to your app settings and enable permissions.", "Ok");
                    return false;
                }
                else
                {
                    return true;
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("error. permissions not set. here is the stacktrace: \n" + ex.StackTrace);
                return false;
            }
        }

        [Obsolete]
        private async void SelectImagesButton_Clicked(object sender, EventArgs e)
        {
            //Check users permissions.
            var storagePermissions = await CrossPermissions.Current.CheckPermissionStatusAsync(Permission.Storage);
            var photoPermissions = await CrossPermissions.Current.CheckPermissionStatusAsync(Permission.Photos);
            if (storagePermissions == PermissionStatus.Granted && photoPermissions == PermissionStatus.Granted)
            {
                //If we are on iOS, call GMMultiImagePicker.
                /*if (Device.RuntimePlatform == Device.iOS)
                {
                    //If the image is modified (drawings, etc) by the users, you will need to change the delivery mode to HighQualityFormat.
                    bool imageModifiedWithDrawings = false;
                    if (imageModifiedWithDrawings)
                    {
                        await GMMultiImagePicker.Current.PickMultiImage(true);
                    }
                    else
                    {
                        await GMMultiImagePicker.Current.PickMultiImage();
                    }

                    MessagingCenter.Unsubscribe<App, List<string>>((App)Xamarin.Forms.Application.Current, "ImagesSelectediOS");
                    MessagingCenter.Subscribe<App, List<string>>((App)Xamarin.Forms.Application.Current, "ImagesSelectediOS", (s, images) =>
                    {
                        //If we have selected images, put them into the carousel view.
                        if (images.Count > 0)
                        {
                            ImgCarouselView.ItemsSource = images;
                            InfoText.IsVisible = true; //InfoText is optional
                        }
                    });

                }*/
                //If we are on Android, call IMediaService.
                if (Device.RuntimePlatform == Device.Android)
                {
                    DependencyService.Get<IMediaService>().OpenGallery();

                    MessagingCenter.Unsubscribe<App, List<string>>((App)Xamarin.Forms.Application.Current, "ImagesSelectedAndroid");
                    MessagingCenter.Subscribe<App, List<string>>((App)Xamarin.Forms.Application.Current, "ImagesSelectedAndroid", (s, images) =>
                    {
                        //If we have selected images, put them into the carousel view.
                        Console.WriteLine(images.Count);
                        if (images.Count > 0)
                        {
                            ImgCarouselView.ItemsSource = images;
                            InfoText.IsVisible = true; //InfoText is optional
                        }
                    });

                }
            }
            else
            {
                await DisplayAlert("Permission Denied!", "\nPlease go to your app settings and enable permissions.", "Ok");
            }




        }

        /// <summary>
        ///     Compress Android images before uploading them to Azure Blob Storage.
        /// </summary>
        /// <param name="totalImages"></param>
        /// <returns></returns>
        private List<string> CompressAllImages(List<string> totalImages)
        {
            int displayCount = 1;
            int totalCount = totalImages.Count;
            List<string> compressedImages = new List<string>();
            foreach (string path in totalImages)
            {
                compressedImages.Add(DependencyService.Get<ICompressImages>().CompressImage(path));
                displayCount++;
            }
            return compressedImages;
        }

        private async void UploadImagesButton_Clicked(object sender, EventArgs e)
        {
            // Get the list of images we have selected.
            List<string> imagePaths = ImgCarouselView.ItemsSource as List<string>;

            // If user is using Android, compress the images. (Optional)
            if (Device.RuntimePlatform == Device.Android)
            {
                imagePaths = CompressAllImages(imagePaths);
            }
        }

        /// <summary>
        ///     Unsubsribe from the MessagingCenter on disappearing.
        /// </summary>
        protected override void OnDisappearing()
        {
            base.OnDisappearing();
            MessagingCenter.Unsubscribe<App, List<string>>((App)Xamarin.Forms.Application.Current, "ImagesSelectedAndroid");
            MessagingCenter.Unsubscribe<App, List<string>>((App)Xamarin.Forms.Application.Current, "ImagesSelectediOS");
            GC.Collect();
        }
    }
}