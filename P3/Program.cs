using System;
using System.Collections.Generic;

namespace TresEnRaya
{
    public class Juego
    {
        public static void Main()
        {
            Console.WriteLine("Seleccione modo de juego:");
            Console.WriteLine("1. Humano vs Agente");
            Console.WriteLine("2. Agente vs Agente");
            int opcion = -1;

            while (opcion != 1 && opcion != 2)
            {
                Console.WriteLine("Ingrese 1 o 2:");
                Int32.TryParse(Console.ReadLine(), out opcion);
            }

            Jugador jugadorX = opcion == 1 ? new JugadorHumano(1) : new JugadorAgente(1);
            Jugador jugadorO = new JugadorAgente(-1);

            Tablero tablero = new Tablero();
            int turno = 1;

            while (!tablero.EsTerminal())
            {
                tablero.Mostrar();
                Movimiento jugada = turno == 1 ? jugadorX.ObtenerMovimiento(tablero) : jugadorO.ObtenerMovimiento(tablero);
                tablero = tablero.AplicarMovimiento(jugada, turno);
                turno = -turno;
            }

            tablero.Mostrar();
            int ganador = tablero.ObtenerGanador();

            if (ganador == 0)
                Console.WriteLine("Empate.");
            else
                Console.WriteLine("Gana: " + (ganador == 1 ? "X" : "O"));
        }
    }
}