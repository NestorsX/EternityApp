using System.IO;
using System.Threading.Tasks;

namespace EternityApp.Interfaces
{
    public interface IPhotoPickerService
    {
        Task<Stream> GetImageStreamAsync();
    }
}
