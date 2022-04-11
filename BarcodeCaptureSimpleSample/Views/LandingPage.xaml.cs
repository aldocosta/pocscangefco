using BarcodeCaptureSimpleSample.ViewModels;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace BarcodeCaptureSimpleSample.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class LandingPage : ContentPage
    {
        public LandingPage()
        {
            InitializeComponent();
        }



        async void OnbtnCloseScanClicked(object sender, EventArgs args)
        {
            var detailVM = new MainPageViewModel();

            var detailPage = new MainPage();

            detailPage.BindingContext = detailVM;

            await Application.Current.MainPage.Navigation.PushModalAsync(detailPage);
        }

        async void OnbtnAboutClicked(object sender, EventArgs args)
        {
            var detailVM = new DetailPageViewModel();

            var detailPage = new DetailPage();

            detailPage.BindingContext = detailVM;

            await Application.Current.MainPage.Navigation.PushAsync(detailPage);
        }

    }
}