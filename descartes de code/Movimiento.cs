namespace TicTacToe
{
    public class Movimiento
    {
        public int Fila { get; private set; }
        public int Columna { get; private set; }

        public Movimiento(int fila, int columna)
        {
            Fila = fila;
            Columna = columna;
        }

        public override string ToString()
        {
            return string.Format("({0}, {1})", Fila, Columna);
        }
    }
}
