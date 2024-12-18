using FmgLib.MauiMarkup;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChristmasMatch
{
    public partial class MainPage : ContentPage, IFmgLibHotReload
    {
        MainPage thispage;
        Grid GameTable;

        private bool _IsWaitForCheck;
        private Border _previouseBorder;
        private FruitCard _previouseData;

        public bool IsFliping { get; set; }
        public MainPage()
        {
            ShuffleCardsData();
            this.InitializeHotReload();
        }

        private static List<FruitCard> _cards = new List<FruitCard>
        {
            new FruitCard("candycane.png"),  new FruitCard("candycane.png"),
            new FruitCard("christmastree.png"), new FruitCard("christmastree.png"),
            new FruitCard("santaclaus.png"),   new FruitCard("santaclaus.png"),
            new FruitCard("sock.png"),   new FruitCard("sock.png"),
            new FruitCard("snowman.png"),   new FruitCard("snowman.png"),
            new FruitCard("snowglobe.png"),   new FruitCard("snowglobe.png")
        };



        private void ShuffleCardsData()
        {
            foreach (FruitCard fruitcard in _cards)
            {
                fruitcard.IsFaceUp = false;
            }

            Random r = new Random();
            _cards = _cards.OrderBy(x => r.Next()).ToList();
        }

        private async Task FlipCard(Border border, FruitCard card, bool isFirstFlip)
        {
            //flip both ui and data
            await border.RotateYTo(90, 250, Easing.CubicIn);
            card.IsFaceUp = isFirstFlip;
            (border.Content as Image).Source = isFirstFlip ? card.ImageSource : "wreath.jpg";
            await border.RotateYTo(isFirstFlip ? 180 : 0, 250, Easing.SpringOut);
        }

        private void Card_Tapped(object? sender, EventArgs e)
        {
            if (IsFliping)
            {
                return;
            }
            IsFliping = true;

            var border = sender as Border;
            var borderlist = GameTable.Children.Cast<Border>().ToList();

            var carddata = _cards[borderlist.IndexOf(border)];
            if (carddata.IsFaceUp)
            {
                IsFliping = false;
                return;
            }

             FlipCard(border, carddata, true);

            
            if (_IsWaitForCheck)
            {
                if (carddata.ImageSource != _previouseData.ImageSource)
                {
                    FlipCard(border, carddata, false);
                    FlipCard(_previouseBorder, _previouseData, false);
                }
            }
            else
            {
                _previouseBorder = border;
                _previouseData = carddata;
            }

            if (!_cards.Any(x => !x.IsFaceUp))
            {
                thispage.BackgroundColor = Color.FromArgb("#9dffb0");
                 Task.Delay(300);
                thispage.BackgroundColor = Color.FromArgb("#81f1f7");
                Task.Delay(300);
                thispage.BackgroundColor = Color.FromArgb("#fffffa");
                 Task.Delay(300);
                thispage.BackgroundColor = Color.FromArgb("#c48d3f");
                Task.Delay(300);
                thispage.BackgroundColor = Color.FromArgb("#fff563");
            }

            _IsWaitForCheck = !_IsWaitForCheck;
            IsFliping = false;

        }

        private void Reset_Clicked(object? sender, EventArgs e)
        {
            thispage.BackgroundColor = Color.FromHex("#bee893");

            if (IsFliping)
            {
                return;
            }
            IsFliping = true;

            var borders = GameTable.Children.Cast<Border>().ToList();
            borders.ForEach(async x =>
            {
                if (x.RotationY != 0)
                {
                    x.RotateYTo(0, 250, Easing.Linear);
                    await Task.Delay(125);
                    var img = x.Content as Image;
                    img.Source = "wreath";
                }

                x.RotationY = 0;
            });

             Task.Delay(500);

            new Animation {
            { 0, 0.125, new Animation (v => GameTable.TranslationX = v, 0, -13) },
            { 0.125, 0.250, new Animation (v => GameTable.TranslationX = v, -13, 13) },
            { 0.250, 0.375, new Animation (v => GameTable.TranslationX = v, 13, -11) },
            { 0.375, 0.5, new Animation (v => GameTable.TranslationX = v, -11, 11) },
            { 0.5, 0.625, new Animation (v => GameTable.TranslationX = v, 11, -7) },
            { 0.625, 0.75, new Animation (v => GameTable.TranslationX = v, -7, 7) },
            { 0.75, 0.875, new Animation (v => GameTable.TranslationX = v, 7, -5) },
            { 0.875, 1, new Animation (v => GameTable.TranslationX = v, -5, 0) }
        }.Commit(this, "ShuffleCards", length: 400, easing: Easing.Linear);

            ShuffleCardsData();
            _IsWaitForCheck = false;
            IsFliping = false;
        }
    }

    public class FruitCard
    {
        public FruitCard(string ımageSource)
        {
            ImageSource = ımageSource;
        }

        public string ImageSource { get; set; }
        public bool IsFaceUp { get; set; }
    }
}
