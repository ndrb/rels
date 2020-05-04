using Android.App;
using Android.Content.PM;
using Android.OS;
using Firebase;
using Android.Runtime;
using Android.Database;
using System;
using Android.Provider;
using Android.Widget;
using Android.Content;
using System.Collections.Generic;
using Xamarin.Forms;
using Android.App;
using Android.Content.PM;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using Plugin.CurrentActivity;
using Android.Content;
using System.Collections.Generic;
using Xamarin.Forms;
using Android.Database;
using Android.Provider;
using CarouselView.FormsPlugin.Android;


namespace SwipeCardView.Sample.Droid
{
    [Activity(Label = "SwipeCardView.Sample", Icon = "@mipmap/icon", Theme = "@style/MainTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation, ScreenOrientation = ScreenOrientation.Portrait)]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
        protected override void OnCreate(Bundle bundle)
        {
            /*
             * Help:
             * https://medium.com/firebase-developers/firebase-auth-on-xamarin-forms-171432aa3d76
             * https://forums.xamarin.com/discussion/33019/the-type-or-namespace-name-app-does-not-exist-in-the-namespace
             * https://evgenyzborovsky.com/2018/03/26/firebase-authentication-in-xamarin-forms/
             * https://forums.xamarin.com/discussion/175159/i-didnt-find-googleservices-json-in-the-build-action
             * https://stackoverflow.com/questions/49452001/default-firebaseapp-is-not-initialized-in-this-process-make-sure-to-call-fireba?fbclid=IwAR1rc8Bx6dRctfb2lYAg_NrtZKF6X4ib0jloul6ic4dbcr3hCc7z9M18AQE
             * https://github.com/xamarin/GooglePlayServicesComponents/issues/223
             * 
             * 
             * https://medium.com/swlh/select-multiple-images-from-the-gallery-in-xamarin-forms-df2e037be572
             */
            TabLayoutResource = Resource.Layout.Tabbar;
            ToolbarResource = Resource.Layout.Toolbar;

            //NuGet Initializations
            CrossCurrentActivity.Current.Init(this, bundle);
            CarouselViewRenderer.Init();
            FFImageLoading.Forms.Platform.CachedImageRenderer.Init(enableFastRenderer: false);

            base.OnCreate(bundle);
            Xamarin.Essentials.Platform.Init(this, bundle);
            global::Xamarin.Forms.Forms.Init(this, bundle);

            var width = Resources.DisplayMetrics.WidthPixels;
            var height = Resources.DisplayMetrics.HeightPixels;
            var density = Resources.DisplayMetrics.Density;

            App.ScreenWidth = (width - 0.5f) / density;
            App.ScreenHeight = (height - 0.5f) / density;


            FirebaseApp.InitializeApp(this);
            LoadApplication(new App());
        }

        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }

        #region Image Picker Implementation
        public static int OPENGALLERYCODE = 100;
        protected override void OnActivityResult(int requestCode, Result resultCode, Intent data)
        {
            base.OnActivityResult(requestCode, resultCode, data);

            //If we are calling multiple image selection, enter into here and return photos and their filepaths.
            if (requestCode == OPENGALLERYCODE && resultCode == Result.Ok)
            {
                List<string> images = new List<string>();

                if (data != null)
                {
                    //Separate all photos and get the path from them all individually.
                    ClipData clipData = data.ClipData;
                    if (clipData != null)
                    {
                        for (int i = 0; i < clipData.ItemCount; i++)
                        {
                            ClipData.Item item = clipData.GetItemAt(i);
                            Android.Net.Uri uri = item.Uri;
                            var path = GetRealPathFromURI(uri);


                            if (path != null)
                            {
                                images.Add(path);
                            }
                        }
                    }
                    else
                    {
                        Android.Net.Uri uri = data.Data;
                        var path = GetRealPathFromURI(uri);

                        if (path != null)
                        {
                            images.Add(path);
                        }
                    }

                    //Send our images to the carousel view.
                    MessagingCenter.Send<App, List<string>>((App)Xamarin.Forms.Application.Current, "ImagesSelectedAndroid", images);
                }
            }
        }

        /// <summary>
        ///     Get the real path for the current image passed.
        /// </summary>
        [Obsolete]
        public String GetRealPathFromURI(Android.Net.Uri contentURI)
        {
            try
            {
                ICursor imageCursor = null;
                string fullPathToImage = "";

                imageCursor = ContentResolver.Query(contentURI, null, null, null, null);
                imageCursor.MoveToFirst();
                int idx = imageCursor.GetColumnIndex(MediaStore.Images.ImageColumns.Data);

                if (idx != -1)
                {
                    fullPathToImage = imageCursor.GetString(idx);
                }
                else
                {
                    ICursor cursor = null;
                    var docID = DocumentsContract.GetDocumentId(contentURI);
                    var id = docID.Split(':')[1];
                    var whereSelect = MediaStore.Images.ImageColumns.Id + "=?";
                    var projections = new string[] { MediaStore.Images.ImageColumns.Data };

                    cursor = ContentResolver.Query(MediaStore.Images.Media.InternalContentUri, projections, whereSelect, new string[] { id }, null);
                    if (cursor.Count == 0)
                    {
                        cursor = ContentResolver.Query(MediaStore.Images.Media.ExternalContentUri, projections, whereSelect, new string[] { id }, null);
                    }
                    var colData = cursor.GetColumnIndexOrThrow(MediaStore.Images.ImageColumns.Data);
                    cursor.MoveToFirst();
                    fullPathToImage = cursor.GetString(colData);
                }
                return fullPathToImage;
            }
            catch (Exception ex)
            {
                Toast.MakeText(Xamarin.Forms.Forms.Context, "Unable to get path", ToastLength.Long).Show();
            }
            return null;
        }
        #endregion
    }
}