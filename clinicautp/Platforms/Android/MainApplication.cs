using Android.App;
using Android.Runtime;
using PdfSharpCore.Fonts;

namespace clinicautp
{
    [Application]
    public class MainApplication : MauiApplication
    {
        public MainApplication(IntPtr handle, JniHandleOwnership ownership)
            : base(handle, ownership)
        {
            GlobalFontSettings.FontResolver = new CustomFontResolver();
        }

        protected override MauiApp CreateMauiApp() => MauiProgram.CreateMauiApp();
    }
}
