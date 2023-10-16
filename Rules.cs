using Android.App;
using Android.Content.PM;
using Android.OS;
using Android.Widget;

namespace Netwalk
{
    [Activity(ScreenOrientation = ScreenOrientation.Portrait)]
    public class Rules : Activity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            SetContentView(Resource.Layout.rules);

            ImageView imageBack = FindViewById<ImageView>(Resource.Id.imageBack);

            imageBack.Click += (s, e) =>
            {
                Finish();
            };
        }
    }
}