using AppFotosMovil.ViewModels;

namespace AppFotosMovil;

public partial class MainPage : ContentPage
{
    public MainPage()
    {
        InitializeComponent();
        BindingContext = new FotoViewModel();
    }
}