using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.OS;
using Android.Widget;

namespace Netwalk
{
    [Activity(MainLauncher = true, ScreenOrientation = ScreenOrientation.Portrait)]
    public class StartMenu : Activity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            Xamarin.Essentials.Platform.Init(this, savedInstanceState);

            SetContentView(Resource.Layout.startmenu);

            ImageView imageStart = FindViewById<ImageView>(Resource.Id.imageStart);

            imageStart.Click += (s, e) =>
            {
                Intent game = new Intent(this, typeof(Field25));
                StartActivity(game);
            };
        }
    }
}