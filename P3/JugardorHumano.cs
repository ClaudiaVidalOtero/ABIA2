using System;

namespace TicTacToe
{
    public class JugadorHumano : Jugador
    {
        public JugadorHumano(int numeroJugador) : base(numeroJugador)
        {
        }

        public override Movimiento ObtenerMovimiento(Tablero tablero)
        {
            int movimientoSeleccionado;

            do
            {
                Console.WriteLine("Jugador {0}, selecciona un número entre 1 y 9:", NumeroJugador);
                string linea = Console.ReadLine();
                Int32.TryParse(linea, out movimientoSeleccionado);
            }
            while (movimientoSeleccionado < 1 || movimientoSeleccionado > 9 || !CasillaValida(movimientoSeleccionado, tablero));

            return ObtenerCoordenadas(movimientoSeleccionado);
        }

        private bool CasillaValida(int numero, Tablero tablero)
        {
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    if (tablero.casillas[i, j] == numero)
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        private Movimiento ObtenerCoordenadas(int numero)
        {
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    if (numero == casillas[i, j])
                    {
                        return new Movimiento(i, j);
                    }
                }
            }

            return null; // Si no encuentra el movimiento
        }
    }
}
