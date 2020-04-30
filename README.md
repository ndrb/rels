

# Relies heavily on SwipeCardView Control for Xamarin.Forms
[![NuGet](https://img.shields.io/nuget/v/MLToolkit.Forms.SwipeCardView.svg?label=NuGet)](https://www.nuget.org/packages/MLToolkit.Forms.SwipeCardView/)



### Simple Page

The intention of this sample is to show how simple it is to start using SwipeCardView in your MVVM app. All you need is a collection of items and a command handler.

![SwipeCardView Android Simple Page](docs/images/SwipeCardView_Android_SimplePage.png)

```XML
<swipeCardView:SwipeCardView
    ItemsSource="{Binding CardItems}"
    SwipedCommand="{Binding SwipedCommand}"
    VerticalOptions="FillAndExpand">
    <swipeCardView:SwipeCardView.ItemTemplate>
        <DataTemplate>
            <Label Text="{Binding .}" FontSize="Large" HorizontalTextAlignment="Center" VerticalTextAlignment="Center" BackgroundColor="Beige"/>
        </DataTemplate>
    </swipeCardView:SwipeCardView.ItemTemplate>
</swipeCardView:SwipeCardView>
```

- Data source is CardItems, a list of strings defined in the bound ViewModel
- Card appearance is defined by a simple DataTemplate, which contains only a Label
- Various SwipeCardView properties are not being set, so the control is using default values
- SwipedCommand will be triggered when the card is swiped over threshold in any direction

