using EternityApp.Droid;
using EternityApp.Interfaces;
using System.IO;
using System.Threading.Tasks;
using Android.Content;
using Xamarin.Forms;

[assembly: Dependency(typeof(PhotoPickerService))]
namespace EternityApp.Droid
{
    public class PhotoPickerService : IPhotoPickerService
    {
        public Task<Stream> GetImageStreamAsync()
        {
            Intent intent = new Intent();
            intent.SetType("image/*");
            intent.SetAction(Intent.ActionGetContent);
            MainActivity.Instance.StartActivityForResult(Intent.CreateChooser(intent, "Выбрать фото"), MainActivity.PickImageId);
            MainActivity.Instance.PickImageTaskCompletionSource = new TaskCompletionSource<Stream>();
            return MainActivity.Instance.PickImageTaskCompletionSource.Task;
        }
    }
}