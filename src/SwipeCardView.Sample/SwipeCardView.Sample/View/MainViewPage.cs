﻿using SwipeCardView.Sample.ViewModel;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace SwipeCardView.Sample.View
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MainViewPage : ContentPage
    {
        public MainViewPage()
        {
            InitializeComponent();
            BindingContext = new SimplePageViewModel();
        }
    }
}