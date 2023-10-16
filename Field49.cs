using System.Collections.Generic;
using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace Netwalk
{
    [Activity(ScreenOrientation = ScreenOrientation.Portrait)]
    public class Field49 : Activity
    {
        //Поле головоломки
        static int[,] gameBoard = new int[7, 7];

        //Кнопки на экране
        static ImageView imageRules, imageSettings, imageShowSolution, imageRestart;
        static Button buttonNewGame;

        //49 клеток в GridView
        static List<ImageView> images;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            SetContentView(Resource.Layout.field49);

            images = new List<ImageView>(49);

            images.Add(FindViewById<ImageView>(Resource.Id.cell1));
            images.Add(FindViewById<ImageView>(Resource.Id.cell2));
            images.Add(FindViewById<ImageView>(Resource.Id.cell3));
            images.Add(FindViewById<ImageView>(Resource.Id.cell4));
            images.Add(FindViewById<ImageView>(Resource.Id.cell5));
            images.Add(FindViewById<ImageView>(Resource.Id.cell6));
            images.Add(FindViewById<ImageView>(Resource.Id.cell7));
            images.Add(FindViewById<ImageView>(Resource.Id.cell8));
            images.Add(FindViewById<ImageView>(Resource.Id.cell9));
            images.Add(FindViewById<ImageView>(Resource.Id.cell10));
            images.Add(FindViewById<ImageView>(Resource.Id.cell11));
            images.Add(FindViewById<ImageView>(Resource.Id.cell12));
            images.Add(FindViewById<ImageView>(Resource.Id.cell13));
            images.Add(FindViewById<ImageView>(Resource.Id.cell14));
            images.Add(FindViewById<ImageView>(Resource.Id.cell15));
            images.Add(FindViewById<ImageView>(Resource.Id.cell16));
            images.Add(FindViewById<ImageView>(Resource.Id.cell17));
            images.Add(FindViewById<ImageView>(Resource.Id.cell18));
            images.Add(FindViewById<ImageView>(Resource.Id.cell19));
            images.Add(FindViewById<ImageView>(Resource.Id.cell20));
            images.Add(FindViewById<ImageView>(Resource.Id.cell21));
            images.Add(FindViewById<ImageView>(Resource.Id.cell22));
            images.Add(FindViewById<ImageView>(Resource.Id.cell23));
            images.Add(FindViewById<ImageView>(Resource.Id.cell24));
            images.Add(FindViewById<ImageView>(Resource.Id.cell25));
            images.Add(FindViewById<ImageView>(Resource.Id.cell26));
            images.Add(FindViewById<ImageView>(Resource.Id.cell27));
            images.Add(FindViewById<ImageView>(Resource.Id.cell28));
            images.Add(FindViewById<ImageView>(Resource.Id.cell29));
            images.Add(FindViewById<ImageView>(Resource.Id.cell30));
            images.Add(FindViewById<ImageView>(Resource.Id.cell31));
            images.Add(FindViewById<ImageView>(Resource.Id.cell32));
            images.Add(FindViewById<ImageView>(Resource.Id.cell33));
            images.Add(FindViewById<ImageView>(Resource.Id.cell34));
            images.Add(FindViewById<ImageView>(Resource.Id.cell35));
            images.Add(FindViewById<ImageView>(Resource.Id.cell36));
            images.Add(FindViewById<ImageView>(Resource.Id.cell37));
            images.Add(FindViewById<ImageView>(Resource.Id.cell38));
            images.Add(FindViewById<ImageView>(Resource.Id.cell39));
            images.Add(FindViewById<ImageView>(Resource.Id.cell40));
            images.Add(FindViewById<ImageView>(Resource.Id.cell41));
            images.Add(FindViewById<ImageView>(Resource.Id.cell42));
            images.Add(FindViewById<ImageView>(Resource.Id.cell43));
            images.Add(FindViewById<ImageView>(Resource.Id.cell44));
            images.Add(FindViewById<ImageView>(Resource.Id.cell45));
            images.Add(FindViewById<ImageView>(Resource.Id.cell46));
            images.Add(FindViewById<ImageView>(Resource.Id.cell47));
            images.Add(FindViewById<ImageView>(Resource.Id.cell48));
            images.Add(FindViewById<ImageView>(Resource.Id.cell49));

            imageRules = FindViewById<ImageView>(Resource.Id.imageRules);
            imageSettings = FindViewById<ImageView>(Resource.Id.imageSettings);
            imageShowSolution = FindViewById<ImageView>(Resource.Id.imageShowSolution);
            imageRestart = FindViewById<ImageView>(Resource.Id.imageRestart);

            buttonNewGame = FindViewById<Button>(Resource.Id.buttonNewGame49);

            buttonNewGame.Click += (s, e) =>
            {
                buttonNewGame.Visibility = ViewStates.Invisible;
                Puzzle.GeneratePuzzle(gameBoard, images, 7);
            };

            imageSettings.Click += (s, e) =>
            {
                Intent settings = new Intent(this, typeof(Settings));
                StartActivityForResult(settings, requestCode: 1);
            };

            imageRules.Click += (s, e) =>
            {
                Intent rules = new Intent(this, typeof(Rules));
                StartActivityForResult(rules, requestCode: 1);
            };

            imageShowSolution.Click += (s, e) =>
            {
                if (!Puzzle.isSolved)
                {
                    AlertDialog.Builder alert = new AlertDialog.Builder(this);
                    alert.SetTitle("Show solution");
                    alert.SetMessage("Are you sure you want to see the solution?");
                    alert.SetPositiveButton("NO", (senderAlert, args) =>
                    {

                    });
                    alert.SetNegativeButton("YES", (senderAlert, args) =>
                    {
                        Puzzle.isSolved = true;
                        buttonNewGame.Visibility = ViewStates.Visible;
                        Puzzle.ShowSolution(images, 7);
                    });
                    Dialog dialog = alert.Create();
                    dialog.Show();
                }
            };

            imageRestart.Click += (s, e) =>
            {
                if (!Puzzle.isSolved)
                {
                    AlertDialog.Builder alert = new AlertDialog.Builder(this);
                    alert.SetTitle("Restart");
                    alert.SetMessage("Are you sure you want to start the current game again?");
                    alert.SetPositiveButton("NO", (senderAlert, args) =>
                    {

                    });
                    alert.SetNegativeButton("YES", (senderAlert, args) =>
                    {
                        Puzzle.Restart(images, gameBoard, 7);
                    });
                    Dialog dialog = alert.Create();
                    dialog.Show();
                }
            };

            Puzzle.GeneratePuzzle(gameBoard, images, 7);
        }

        /// <summary>
        /// Переопределённый метод нажатия кнопки "Назад" на смартфоне
        /// </summary>
        public override void OnBackPressed()
        {
            AlertDialog.Builder alert = new AlertDialog.Builder(this);
            alert.SetTitle("Exit");
            alert.SetMessage("Are you sure you want to go to the menu? The current game will be lost!");
            alert.SetPositiveButton("NO", (senderAlert, args) =>
            {

            });
            alert.SetNegativeButton("YES", (senderAlert, args) =>
            {
                base.OnBackPressed();
            });
            Dialog dialog = alert.Create();
            dialog.Show();
        }

        protected override void OnActivityResult(int requestCode, [GeneratedEnum] Result resultCode, Intent data)
        {
            if (resultCode == Result.Ok)
            {
                Finish();
                Intent field25 = new Intent(this, typeof(Field25));
                StartActivity(field25);
            }
            if (resultCode == Result.FirstUser)
            {
                Finish();
                Intent field49 = new Intent(this, typeof(Field49));
                StartActivity(field49);
            }
        }

        [Java.Interop.Export("Click")]
        public void Click(View v)
        {
            if (!Puzzle.isSolved)
            {
                Puzzle.Click(gameBoard, images, v, 7);
                Puzzle.isSolved = Puzzle.Check(gameBoard, this, 7);
                if (Puzzle.isSolved) buttonNewGame.Visibility = ViewStates.Visible;
            }
        }
    }
}