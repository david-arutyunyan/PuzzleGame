using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.OS;
using Android.Widget;

namespace Netwalk
{
    [Activity(ScreenOrientation = ScreenOrientation.Portrait)]
    public class Settings : Activity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            SetContentView(Resource.Layout.settings);

            ImageView imageBack = FindViewById<ImageView>(Resource.Id.imageBack);
            Button button25 = FindViewById<Button>(Resource.Id.button25);
            Button button49 = FindViewById<Button>(Resource.Id.button49);

            imageBack.Click += (s, e) =>
            {
                Intent intent = new Intent();
                SetResult(Result.Canceled, intent);
                Finish();
            };

            button49.Click += (s, e) =>
            {
                Intent intent = new Intent();
                SetResult(Result.FirstUser, intent);
                Finish();
            };

            button25.Click += (s, e) =>
            {
                Intent intent = new Intent();
                SetResult(Result.Ok, intent);
                Finish();
            };
        }
    }
}