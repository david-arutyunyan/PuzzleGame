using System;
using System.Collections.Generic;
using Android.App;
using Android.Views;
using Android.Widget;

namespace Netwalk
{
    public class Puzzle
    {
        static Random rnd = new Random();

        //Решена ли головоломка или нет
        public static bool isSolved;

        //Значения клеток начальной головоломки(нерешённой)
        public static int[,] initialPuzzle;

        //Значения поворотов клеток начальной головоломки(нерешённой)
        public static int[,] rotation;

        static List<int> one = new List<int> { 1, 2, 4, 8 }; //Клетки с одной трубой
        static List<int> twoDif = new List<int> { 6, 12, 9, 3 }; //Клетки с двумя турбами
        static List<int> twoSame = new List<int> { 5, 10 }; //Клетки с двумя противолежащими турбами
        static List<int> three = new List<int> { 7, 14, 13, 11 }; //Клетки с тремя турбами

        //Словарь, задающий количество поворотов клетки в зависимости от её значения
        static Dictionary<int, int> numberOfTurns = new Dictionary<int, int>
            {
                { 1, 0 },
                { 2, 1 },
                { 3, 3 },
                { 4, 2 },
                { 5, 0 },
                { 6, 0 },
                { 7, 0 },
                { 8, 3 },
                { 9, 2 },
                { 10, 1 },
                { 11, 3 },
                { 12, 1 },
                { 13, 2 },
                { 14, 1 }
            };

        //Словарь, задающий картинку клетки в зависимости от её значения
        static Dictionary<int, int> imagesForCells = new Dictionary<int, int>
            {
                { 0, Resource.Drawable.pipe4 },
                { 1, Resource.Drawable.pipe4 },
                { 2, Resource.Drawable.pipe1 },
                { 3, Resource.Drawable.pipe4 },
                { 4, Resource.Drawable.pipe3 },
                { 5, Resource.Drawable.pipe1 },
                { 6, Resource.Drawable.pipe2 },
                { 7, Resource.Drawable.pipe4 },
                { 8, Resource.Drawable.pipe1 },
                { 9, Resource.Drawable.pipe3 },
                { 10, Resource.Drawable.pipe2 },
                { 11, Resource.Drawable.pipe1 },
                { 12, Resource.Drawable.pipe2 },
                { 13, Resource.Drawable.pipe2 }
            };

        //Словарь, задающий картинку сервера в зависимости от его значения
        static Dictionary<int, int> imagesForServer = new Dictionary<int, int>
            {
                { 0, Resource.Drawable.server4 },
                { 1, Resource.Drawable.server4 },
                { 2, Resource.Drawable.server1 },
                { 3, Resource.Drawable.server4 },
                { 4, Resource.Drawable.server3 },
                { 5, Resource.Drawable.server1 },
                { 6, Resource.Drawable.server2 },
                { 7, Resource.Drawable.server4 },
                { 8, Resource.Drawable.server1 },
                { 9, Resource.Drawable.server3 },
                { 10, Resource.Drawable.server2 },
                { 11, Resource.Drawable.server1 },
                { 12, Resource.Drawable.server2 },
                { 13, Resource.Drawable.server2 }
            };

        /// <summary>
        /// Рестарт текущей головоломки
        /// </summary>
        /// <param name="images">Лист картинок клеток</param>
        /// <param name="gameBoard">Лист значений клеток</param>
        /// <param name="size">Размер головоломки</param>
        public static void Restart(List<ImageView> images, int[,] gameBoard, int size)
        {
            NetwalkGeneration.Restart();

            int k = 0;
            for (int i = 0; i < size; i++)
            {
                for (int j = 0; j < size; j++)
                {
                    gameBoard[i, j] = initialPuzzle[i, j];
                    images[k].Rotation = rotation[i, j];
                    k++;
                }
            }

            List<int> ans = NetwalkGeneration.FindPaths(new List<int>());

            SetComputers(images, ans);
        }

        /// <summary>
        /// Показ решения
        /// </summary>
        /// <param name="images">Лист картинок клеток</param>
        /// <param name="size">Размер головоломки</param>
        public static void ShowSolution(List<ImageView> images, int size)
        {
            foreach (ImageView i in images)
            {
                i.Rotation = 0;
            }

            int k = 0;
            for (int i = 0; i < size; i++)
            {
                for (int j = 0; j < size; j++)
                {
                    int n = NetwalkGeneration.gameBoard[i, j].Value;
                    if (k == (size * size) / 2)
                        images[k].SetImageResource(imagesForServer[n - 1]);
                    else
                        if (n == 1 || n == 2 || n == 4 || n == 8) 
                            images[k].SetImageResource(Resource.Drawable.pipe4_connected);
                        else
                            images[k].SetImageResource(imagesForCells[n - 1]);

                    for (int l = 0; l < numberOfTurns[n]; l++)
                    {
                        images[k].Rotation = (images[k].Rotation + 90) % 360;
                    }
                    k++;
                }
            }
        }

        /// <summary>
        /// Проверка на то, решена ли головоломка
        /// </summary>
        /// <param name="gameBoard">Лист значений клеток</param>
        /// <param name="activity">Текущее activity</param>
        /// <param name="size">Размер головоломки</param>
        /// <returns></returns>
        public static bool Check(int[,] gameBoard, Activity activity, int size)
        {
            bool isSolved = true;
            for (int i = 0; i < size * size; i++)
            {
                foreach (int v in NetwalkGeneration.graph[i])
                {
                    bool flag = false;
                    if (v >= 0)
                    {
                        foreach (int j in NetwalkGeneration.graph[v])
                        {
                            if (j == i) flag = true;
                        }
                    }
                    if (!flag) isSolved = false;
                }
            }

            List<int> ans = NetwalkGeneration.FindPaths(new List<int>());

            if (ans.Count != NetwalkGeneration.computers.Count) isSolved = false;

            if (isSolved)
            {
                AlertDialog.Builder alert = new AlertDialog.Builder(activity);
                alert.SetTitle("Good job!");
                alert.SetMessage("You won! Congratulations!");
                alert.SetNegativeButton("ОК", (senderAlert, args) =>
                {

                });
                Dialog dialog = alert.Create();
                dialog.Show();
                return true;
            }
            return false;
        }

        /// <summary>
        /// Клик на клетку(поворот)
        /// </summary>
        /// <param name="gameBoard">Лист значений клеток</param>
        /// <param name="images">Лист картинок клеток</param>
        /// <param name="v">Нажатая клетка</param>
        /// <param name="size">Размер головоломки</param>
        public static void Click(int[,] gameBoard, List<ImageView> images, View v, int size)
        {
            int n = gameBoard[images.IndexOf((ImageView)v) / size, images.IndexOf((ImageView)v) % size];

            List<int> ans = NetwalkGeneration.Rotate(images.IndexOf((ImageView)v));

            SetComputers(images, ans);

            v.Rotation += 90;

            if (n == 1 || n == 2 || n == 4 || n == 8)
            {
                int newIndex = (one.IndexOf(n) + 1) % 4;
                gameBoard[images.IndexOf((ImageView)v) / size, images.IndexOf((ImageView)v) % size] = one[newIndex];
            }
            if (n == 6 || n == 12 || n == 9 || n == 3)
            {
                int newIndex = (twoDif.IndexOf(n) + 1) % 4;
                gameBoard[images.IndexOf((ImageView)v) / size, images.IndexOf((ImageView)v) % size] = twoDif[newIndex];
            }
            if (n == 5 || n == 10)
            {
                int newIndex = (twoSame.IndexOf(n) + 1) % 2;
                gameBoard[images.IndexOf((ImageView)v) / size, images.IndexOf((ImageView)v) % size] = twoSame[newIndex];
            }
            if (n == 7 || n == 14 || n == 13 || n == 11)
            {
                int newIndex = (three.IndexOf(n) + 1) % 4;
                gameBoard[images.IndexOf((ImageView)v) / size, images.IndexOf((ImageView)v) % size] = three[newIndex];
            }
        }

        // Рандомный поворот клетки (подготовка головоломки к отображению на экран)
        static void RandomRotation(int[,] gameBoard, List<ImageView> images, int r, int size, int i, int j, int k)
        {
            initialPuzzle[i, j] = gameBoard[i, j];
            for (int l = 0; l < r; l++)
            {
                images[k].Rotation = (images[k].Rotation + 90) % 360;
                NetwalkGeneration.Rotate(i * size + j);
                NetwalkGeneration.initialGraph[i * size + j] = NetwalkGeneration.graph[i * size + j];
            }
            rotation[i, j] = (int)(images[k].Rotation);
        }

        /// <summary>
        /// Генерируем головоломку
        /// </summary>
        /// <param name="gameBoard">Лист значений клеток</param>
        /// <param name="images">Лист картинок клеток</param>
        /// <param name="size">Размер головоломки</param>
        public static void GeneratePuzzle(int[,] gameBoard, List<ImageView> images, int size)
        {
            isSolved = false;

            initialPuzzle = new int[size, size];
            rotation = new int[size, size];

            NetwalkGeneration puzzle = new NetwalkGeneration(size);

            puzzle.Generate();

            for (int i = 0; i < size; i++)
            {
                for (int j = 0; j < size; j++)
                {
                    gameBoard[i, j] = 0;
                }
            }

            foreach (ImageView i in images)
            {
                i.Rotation = 0;
            }

            int k = 0;
            for (int i = 0; i < size; i++)
            {
                for (int j = 0; j < size; j++)
                {
                    int n = NetwalkGeneration.gameBoard[i, j].Value;
                    if (k == (size * size) / 2)
                        images[k].SetImageResource(imagesForServer[n - 1]);
                    else
                        images[k].SetImageResource(imagesForCells[n - 1]);

                    for (int l = 0; l < numberOfTurns[n]; l++)
                    {
                        images[k].Rotation = (images[k].Rotation + 90) % 360;
                    }

                    if (n == 1 || n == 2 || n == 4 || n == 8)
                    {
                        int r = rnd.Next(0, 4);
                        int newIndex = (one.IndexOf(n) + r) % 4;
                        gameBoard[i, j] = one[newIndex];
                        RandomRotation(gameBoard, images, r, size, i, j, k);
                    }
                    if (n == 6 || n == 12 || n == 9 || n == 3)
                    {
                        int r = rnd.Next(0, 4);
                        int newIndex = (twoDif.IndexOf(n) + r) % 4;
                        gameBoard[i, j] = twoDif[newIndex];
                        RandomRotation(gameBoard, images, r, size, i, j, k);
                    }
                    if (n == 5 || n == 10)
                    {
                        int r = rnd.Next(0, 2);
                        int newIndex = (twoSame.IndexOf(n) + r) % 2;
                        gameBoard[i, j] = twoSame[newIndex];
                        RandomRotation(gameBoard, images, r, size, i, j, k);
                    }
                    if (n == 7 || n == 14 || n == 13 || n == 11)
                    {
                        int r = rnd.Next(0, 4);
                        int newIndex = (three.IndexOf(n) + r) % 4;
                        gameBoard[i, j] = three[newIndex];
                        RandomRotation(gameBoard, images, r, size, i, j, k);
                    }
                    k++;
                }
            }
            List<int> ans = NetwalkGeneration.FindPaths(new List<int>());
            SetComputers(images, ans);
        }

        /// <summary>
        /// Устанавливаем картинки компьютерам
        /// </summary>
        /// <param name="images">Лист картинок клеток</param>
        /// <param name="ans">Подсоединённые к серверу компьютеры</param>
        static void SetComputers(List<ImageView> images, List<int> ans)
        {
            for (int i = 0; i < NetwalkGeneration.computers.Count; i++)
            {
                if (ans.Contains(NetwalkGeneration.computers[i]))
                {
                    images[NetwalkGeneration.computers[i]].SetImageResource(Resource.Drawable.pipe4_connected);
                }
                else
                {
                    images[NetwalkGeneration.computers[i]].SetImageResource(Resource.Drawable.pipe4);
                }
            }
        }
    }
}