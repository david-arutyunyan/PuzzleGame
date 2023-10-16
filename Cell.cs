namespace Netwalk
{
    public class Cell
    {
        public int X { get; } //Координата клетки по X
        public int Y { get; } //Координата клетки по Y
        public int Value { get; set; } //Значение клетки

        public Cell(int x, int y, int value)
        {
            X = x;
            Y = y;
            Value = value;
        }
    }
}