using Microsoft.Maui.Controls.Shapes;
using System.Reflection;

namespace ChristmasMatch;

public partial class MainPage : ContentPage, IFmgLibHotReload
{
  

    public void Build()
    {

        this
         .Assign(out thispage)
        .BackgroundColor("#bee893".ToColor())
        .BindingContext(this)
        .Resources(
            new ResourceDictionary
            {
                new Style<Border>(e => e
                    .SizeRequest(100,140)
                    .Stroke(Color.FromArgb("#38ada9"))
                    .StrokeThickness(4)
                    .StrokeShape(new RoundRectangle().CornerRadius(16))
                ),

                new Style<Image>(e => e.Source("wreath.png"))
            }
        )
        .Content(
            new VerticalStackLayout()
            .Margin(0,20,0,0)
            .CenterVertical()
            .Children(
                new Grid()
                .Assign(out GameTable)
                .Spacing(10)
                .CenterHorizontal()
                .RowDefinitions(e => e.Star(count:6))
                .ColumnDefinitions(e => e.Star(count:3))
                .Children(
                    GetImageBorders()
                ),

                new Button()
                .Row(6)
                .WidthRequest(160)
                .CenterHorizontal()
                .Text("Reset")
                .FontSize(20)
                .OnClicked(Reset_Clicked),


                new Button()
                .WidthRequest(160)
                .Margin(0, 5)
                .CenterHorizontal()
                .Text("About")
                .FontSize(20)
                .OnClicked(async (sender, e) =>
                {
                    await Application.Current.MainPage.DisplayAlert("ABOUT", "This game was developed by Ferit Bulut. \n @fertbult", "OK");
                })





            )
        );
    }

   

    private IList<IView> GetImageBorders()
    {
        var result = new List<IView>();
        for (int i = 0; i < 4; i++)
            for (int j = 0; j < 3; j++)
                result.Add(new Border()
                    .Row(i)
                    .Column(j)
                    .GestureRecognizers(new TapGestureRecognizer().OnTapped(Card_Tapped))
                    .Content(new Image()));

        return result;
    }

   
}
