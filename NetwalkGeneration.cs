using System;
using System.Collections.Generic;

namespace Netwalk
{
    public class NetwalkGeneration
    {
        const int INF = 1000000000;

        public static Cell[,] gameBoard; //Игровое поле sizeOfField на sizeOfField
        public static List<int> computers; //Лист компьютеров
        public static List<List<int>> graph; //Граф
        public static List<List<int>> initialGraph; //Граф заданной головоломки(не меняется)
        static Random rnd = new Random(); //Рандом
        static int sizeOfField; //Размер поля
        static List<Cell> greenCells; //Лист клеток, от которых может расти дерево
        static int[] shortcut; //Для хранения кратчайших путей в графе от исходной
        static int[] paths; //Для хранения предков
        static bool[] flags; //Для хранения булевских флагов посещения вершин
        static List<int> rotate; //Значения соседних клеток при вращении любой клетки

        public NetwalkGeneration(int size)
        {
            sizeOfField = size;
            shortcut = new int[size * size];
            paths = new int[size * size];
            flags = new bool[size * size];
            gameBoard = new Cell[size, size];
            rotate = new List<int> { -1, -size, 1, size };
            graph = new List<List<int>>();
            initialGraph = new List<List<int>>();
            computers = new List<int>();
            greenCells = new List<Cell>();
        }

        /// <summary>
        /// Рестартим игру(приводим граф в начальное состояние)
        /// </summary>
        public static void Restart()
        {
            graph.Clear();
            for (int i = 0; i < sizeOfField * sizeOfField; i++)
            {
                graph.Add(new List<int>());
                foreach (int j in initialGraph[i])
                {
                    graph[i].Add(j);
                }
            }
        }

        /// <summary>
        /// Вращение нажатой клетки
        /// </summary>
        /// <param name="cell">Клетка</param>
        /// <returns>Новые пути от сервера до компьютеров</returns>
        public static List<int> Rotate(int cell)
        {
            List<int> answer = new List<int>();
            List<int> new_ = new List<int>();

            for (int i = 0; i < graph[cell].Count; i++)
            {
                new_.Add(graph[cell][i]);
            }

            for (int i = 0; i < graph[cell].Count; i++)
            {
                int value = rotate[(rotate.IndexOf(graph[cell][i] - cell) + 1) % 4];

                if (graph[cell][i] == -1) value = -sizeOfField;
                else if (graph[cell][i] == -2) value = 1;
                else if (graph[cell][i] == -3) value = sizeOfField;
                else if (graph[cell][i] == -4) value = -1;


                new_.Remove(graph[cell][i]);
                if ((cell + value + 1) % sizeOfField == 0 && value == -1)
                    new_.Add(-1);
                else if (cell + value < 0)
                    new_.Add(-2);
                else if ((cell + value) % sizeOfField == 0 && value == 1)
                    new_.Add(-3);
                else if (cell + value >= sizeOfField * sizeOfField)
                    new_.Add(-4);
                else new_.Add(cell + value);
            }

            graph[cell] = new_;

            return FindPaths(answer);
        }

        /// <summary>
        /// Поиск путей от сервера до всех компьютеров
        /// </summary>
        /// <param name="answer">Лист компьютеров, до которых существуют пути от сервера</param>
        /// <returns>Лист компьютеров, до которых есть пути от сервера</returns>
        public static List<int> FindPaths(List<int> answer)
        {
            Dijkstra();

            foreach (int t in computers)
            {
                if (GetPath(t))
                {
                    answer.Add(t);
                }
            }

            return answer;
        }

        /// <summary>
        /// Очистка инфы о графе
        /// </summary>
        static void GraphInit()
        {
            for (int i = 0; i < sizeOfField * sizeOfField; i++)
            {
                shortcut[i] = INF;
                paths[i] = -1;
                flags[i] = false;
            }
        }

        /// <summary>
        /// Получение пути от сервера до компьютеров(если он есть)
        /// </summary>
        /// <param name="t">Компьютер, до которого ищется путь</param>
        /// <returns>Результат поиска</returns>
        static bool GetPath(int comp)
        {
            List<int> path = new List<int>();

            for (int v = comp; v != (sizeOfField * sizeOfField) / 2; v = paths[v])
            {
                if (v == -1)
                {
                    return false;
                }
                path.Add(v);
            }
            path.Add((sizeOfField * sizeOfField) / 2);

            return true;
        }

        /// <summary>
        /// Дейкстрой ищем все пути от ЦК до всех компьютеров
        /// </summary>
        static void Dijkstra()
        {
            GraphInit();

            shortcut[(sizeOfField * sizeOfField) / 2] = 0;
            for (int i = 0; i < sizeOfField * sizeOfField; ++i)
            {
                int v = -1;
                for (int j = 0; j < sizeOfField * sizeOfField; ++j)
                    if (!flags[j] && (v == -1 || shortcut[j] < shortcut[v]))
                        v = j;
                if (shortcut[v] == INF)
                    break;

                flags[v] = true;

                for (int j = 0; j < graph[v].Count; ++j)
                {
                    int to = graph[v][j];
                    if (to >= 0)
                    {
                        bool b = false;
                        for (int l = 0; l < graph[graph[v][j]].Count; l++)
                        {
                            if (graph[graph[v][j]][l] == v) b = true;
                        }
                        if (shortcut[v] < shortcut[to] && b)
                        {
                            shortcut[to] = shortcut[v];
                            paths[to] = v;
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Очистка игрового поля и листа с "зеленым клетками" (рестарт)
        /// </summary>
        void Clear()
        {
            initialGraph.Clear();
            graph.Clear();
            computers.Clear();
            greenCells.Clear();

            for (int i = 0; i < sizeOfField; i++)
            {
                for (int j = 0; j < sizeOfField; j++)
                {
                    initialGraph.Add(new List<int>());
                    graph.Add(new List<int>());
                    gameBoard[i, j] = new Cell(i, j, 0);
                }
            }
        }

        /// <summary>
        /// Ищем в листе с клетками, от которых можно расти(зеленые клетки), клетки, от которых уже некуда расти и удаляем их из этого листа
        /// </summary>
        void Check()
        {
            List<Cell> deletes = new List<Cell>(); //Лист с зелеными клеткам, которые уже не зеленые и которые нужно удалить
            for (int i = 0; i < greenCells.Count; i++)
            {
                Cell cur = greenCells[i];

                //Если текущая "зеленая клетка" окружена стенами или трубами(то есть ей некуда расти), то удаляем её
                if ((cur.X - 1 < 0 || gameBoard[cur.X - 1, cur.Y].Value != 0) &&
                    (cur.X + 1 > sizeOfField - 1 || gameBoard[cur.X + 1, cur.Y].Value != 0) &&
                    (cur.Y - 1 < 0 || gameBoard[cur.X, cur.Y - 1].Value != 0) &&
                    (cur.Y + 1 > sizeOfField - 1 || gameBoard[cur.X, cur.Y + 1].Value != 0))
                {
                    deletes.Add(cur);
                }
            }

            for (int i = 0; i < deletes.Count; i++) //Удаляем все не зеленые клетки из листа с зелеными клетками
            {
                greenCells.Remove(deletes[i]);
            }
        }

        /// <summary>
        /// Готовим стартовую позицию(центральный комп и начальные ответвления)
        /// </summary>
        void PreStart()
        {
            //Листы с соседями центральной клетки, где расположен центральный компьютер(ЦК)
            List<Cell> neibs = new List<Cell>() { new Cell(sizeOfField / 2 - 1, sizeOfField / 2, 8), new Cell(sizeOfField / 2, sizeOfField / 2 + 1, 1), new Cell(sizeOfField / 2 + 1, sizeOfField / 2, 2), new Cell(sizeOfField / 2, sizeOfField / 2 - 1, 4) };

            int count = rnd.Next(1, 4); //Выбираем случайное количество начальных ответвлений от ЦК(от 1 до 3)

            //Выбираем случайные направления от ЦК(от 1 до 3) и запихиваем их в "зеленые клетки"
            for (int i = 0; i < count; i++)
            {
                Cell k = neibs[rnd.Next(0, neibs.Count)];

                graph[(sizeOfField * sizeOfField) / 2].Add(k.X * sizeOfField + k.Y);
                graph[k.X * sizeOfField + k.Y].Add((sizeOfField * sizeOfField) / 2);

                initialGraph[(sizeOfField * sizeOfField) / 2].Add(k.X * sizeOfField + k.Y);
                initialGraph[k.X * sizeOfField + k.Y].Add((sizeOfField * sizeOfField) / 2);

                if (k.Value == 8)
                {
                    gameBoard[sizeOfField / 2, sizeOfField / 2].Value += 2;
                    gameBoard[sizeOfField / 2 - 1, sizeOfField / 2].Value = 8;
                }
                else if (k.Value == 1)
                {
                    gameBoard[sizeOfField / 2, sizeOfField / 2].Value += 4;
                    gameBoard[sizeOfField / 2, sizeOfField / 2 + 1].Value = 1;
                }
                else if (k.Value == 2)
                {
                    gameBoard[sizeOfField / 2, sizeOfField / 2].Value += 8;
                    gameBoard[sizeOfField / 2 + 1, sizeOfField / 2].Value = 2;
                }
                else if (k.Value == 4)
                {
                    gameBoard[sizeOfField / 2, sizeOfField / 2].Value += 1;
                    gameBoard[sizeOfField / 2, sizeOfField / 2 - 1].Value = 4;
                }

                greenCells.Add(k);
                neibs.Remove(k);
            }
        }

        /// <summary>
        /// Ищем соседей текущей клетки, от который растем
        /// </summary>
        /// <param name="growingCell">Текущая клетка</param>
        /// <param name="neib">Лист этих соседей</param>
        void FindNeibs(Cell growingCell, List<Cell> neib)
        {
            //Ищем соседей, которые не стена и пустые
            if (growingCell.Y - 1 >= 0 && gameBoard[growingCell.X, growingCell.Y - 1].Value == 0)
            {
                neib.Add(gameBoard[growingCell.X, growingCell.Y - 1]);
            }
            if (growingCell.X - 1 >= 0 && gameBoard[growingCell.X - 1, growingCell.Y].Value == 0)
            {
                neib.Add(gameBoard[growingCell.X - 1, growingCell.Y]);
            }
            if (growingCell.Y + 1 <= sizeOfField - 1 && gameBoard[growingCell.X, growingCell.Y + 1].Value == 0)
            {
                neib.Add(gameBoard[growingCell.X, growingCell.Y + 1]);
            }
            if (growingCell.X + 1 <= sizeOfField - 1 && gameBoard[growingCell.X + 1, growingCell.Y].Value == 0)
            {
                neib.Add(gameBoard[growingCell.X + 1, growingCell.Y]);
            }
        }

        /// <summary>
        /// Меняем значение клетки, от которой растем и значение клетки, куда растем
        /// </summary>
        /// <param name="growingCell">Клетка, от которой растем</param>
        /// <param name="toGrow">Клетки, куда растем</param>
        void Changes(Cell growingCell, Cell toGrow)
        {
            if (toGrow.X == growingCell.X && toGrow.Y == growingCell.Y - 1)
            {
                gameBoard[growingCell.X, growingCell.Y].Value += 1;
                gameBoard[growingCell.X, growingCell.Y - 1].Value += 4;
            }
            else if (toGrow.X == growingCell.X - 1 && toGrow.Y == growingCell.Y)
            {
                gameBoard[growingCell.X, growingCell.Y].Value += 2;
                gameBoard[growingCell.X - 1, growingCell.Y].Value += 8;
            }
            else if (toGrow.X == growingCell.X && toGrow.Y == growingCell.Y + 1)
            {
                gameBoard[growingCell.X, growingCell.Y].Value += 4;
                gameBoard[growingCell.X, growingCell.Y + 1].Value += 1;
            }
            else if (toGrow.X == growingCell.X + 1 && toGrow.Y == growingCell.Y)
            {
                gameBoard[growingCell.X, growingCell.Y].Value += 8;
                gameBoard[growingCell.X + 1, growingCell.Y].Value += 2;
            }

            //Если у клети уже есть три ответвления, то удаляем её из листа
            //с зелеными клетками(чтоб не было клеток с ответвлениями в четыре стороны)
            if (gameBoard[growingCell.X, growingCell.Y].Value == 7 ||
                gameBoard[growingCell.X, growingCell.Y].Value == 11 ||
                gameBoard[growingCell.X, growingCell.Y].Value == 13 ||
                gameBoard[growingCell.X, growingCell.Y].Value == 14)
                greenCells.Remove(growingCell);
        }

        /// <summary>
        /// Добавляем в лист компьютеров компьютеры, которые образовались в результате генерации
        /// </summary>
        static void AddComputers()
        {
            for (int i = 0; i < sizeOfField; i++)
            {
                for (int j = 0; j < sizeOfField; j++)
                {
                    if (!(i == sizeOfField / 2 && j == sizeOfField / 2) && (gameBoard[i, j].Value == 1 || gameBoard[i, j].Value == 2 || gameBoard[i, j].Value == 4 || gameBoard[i, j].Value == 8))
                    {
                        computers.Add(gameBoard[i, j].X * sizeOfField + gameBoard[i, j].Y);
                    }
                }
            }
        }

        public void Generate()
        {
            Clear(); //Очистка всего

            PreStart(); //Подготовка центрального компьютера

            while (greenCells.Count > 0) //Пока есть клетки, от которых можно расти
            {
                Cell growingCell = greenCells[rnd.Next(0, greenCells.Count)]; //Выбираем рандомную из зеленых клеток

                List<Cell> neib = new List<Cell>(); //Лист с пустыми соседями выбранной клетки(в кого можно расти)

                FindNeibs(growingCell, neib);

                Cell toGrow = neib[rnd.Next(0, neib.Count)]; //Выбираем случайного соседа

                greenCells.Add(toGrow); //Добавляем соседа в число зеленых клеток, от которых можно расти

                graph[growingCell.X * sizeOfField + growingCell.Y].Add(toGrow.X * sizeOfField + toGrow.Y);
                graph[toGrow.X * sizeOfField + toGrow.Y].Add(growingCell.X * sizeOfField + growingCell.Y);

                initialGraph[growingCell.X * sizeOfField + growingCell.Y].Add(toGrow.X * sizeOfField + toGrow.Y);
                initialGraph[toGrow.X * sizeOfField + toGrow.Y].Add(growingCell.X * sizeOfField + growingCell.Y);

                Changes(growingCell, toGrow);

                Check();
            }

            AddComputers();
        }
    }
}