﻿using MLToolkit.Forms.SwipeCardView.Core;
using System.Collections.ObjectModel;
using System.Windows.Input;
using Xamarin.Forms;

namespace SwipeCardView.Sample.ViewModel
{
    public class MainViewModel : BasePageViewModel
    {
        private ObservableCollection<string> _cardItems;

        private string _message;

        public MainViewModel()
        {
            _cardItems = new ObservableCollection<string>();
            for (var i = 1; i <= 5; i++)
            {
                _cardItems.Add($"Card {i}");
            }

            SwipedCommand = new Command<SwipedCardEventArgs>(OnSwipedCommand);

            //ClearItemsCommand = new Command(OnClearItemsCommand);
            //AddItemsCommand = new Command(OnAddItemsCommand);
        }

        public ObservableCollection<string> CardItems
        {
            get => _cardItems;
            set
            {
                _cardItems = value;
                RaisePropertyChanged();
            }
        }

        public string Message
        {
            get => _message;
            set
            {
                _message = value;
                RaisePropertyChanged();
            }
        }

        public ICommand SwipedCommand { get; }

        public ICommand ClearItemsCommand { get; }

        public ICommand AddItemsCommand { get; }

        private void OnSwipedCommand(SwipedCardEventArgs eventArgs)
        {
            var item = eventArgs.Item as string;
            Message = $"{item} swiped {eventArgs.Direction}";
        }

        /*private void OnClearItemsCommand()
        {
            CardItems.Clear();
            Message = string.Empty;
        }

        private void OnAddItemsCommand()
        {
            for (var i = 1; i <= 5; i++)
            {
                CardItems.Add($"Card {i}");
            }
        }*/
    }
}