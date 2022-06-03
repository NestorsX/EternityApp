using Android.Widget;
using EternityApp.Droid;

[assembly: Xamarin.Forms.Dependency(typeof(ToastService))]
namespace EternityApp.Droid
{
    public class ToastService : IToast
    {
        public void Show(string message)
        {
            Toast.MakeText(Android.App.Application.Context, message, ToastLength.Long).Show();
        }
    }
}