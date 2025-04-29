namespace TicTacToe
{
    public abstract class Jugador
    {
        protected int NumeroJugador;

        protected Jugador(int numeroJugador)
        {
            NumeroJugador = numeroJugador;
        }

        public abstract Movimiento ObtenerMovimiento(Tablero tablero);
    }
}

