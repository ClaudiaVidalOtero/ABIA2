using System;

namespace TicTacToe
{
    public class Juego
    {
        public static void Main()
        {
            Console.WriteLine("Seleccione modo de juego:");
            Console.WriteLine("1. Humano vs Humano");
            int opcion = -1;

            while (opcion != 1)
            {
                Console.WriteLine("Ingrese 1 para jugar:");
                Int32.TryParse(Console.ReadLine(), out opcion);
            }

            Jugador jugador1 = new JugadorHumano(1);
            Jugador jugador2 = new JugadorHumano(2);

            Tablero tablero = new Tablero();
            int turno = 1; // 1 para jugador 1, 2 para jugador 2

            while (!tablero.EsTerminal())
            {
                tablero.Mostrar();
                Movimiento jugada;

                if (turno == 1)
                {
                    jugada = jugador1.ObtenerMovimiento(tablero);
                    tablero = tablero.AplicarMovimiento(jugada, 1); // Jugador 1
                    turno = 2;
                }
                else
                {
                    jugada = jugador2.ObtenerMovimiento(tablero);
                    tablero = tablero.AplicarMovimiento(jugada, 2); // Jugador 2
                    turno = 1;
                }
            }

            tablero.Mostrar();
            int ganador = tablero.ObtenerGanador();

            if (ganador == 0)
            {
                Console.WriteLine("Empate.");
            }
            else
            {
                Console.WriteLine("Gana el Jugador {0}!", ganador);
            }
        }
    }
}
