using System;
using System.Collections.Generic;

namespace TicTacToe
{
    public class Tablero
    {
        private int[,] casillas;

        public Tablero()
        {
            casillas = new int[3, 3];
            int contador = 1;
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    casillas[i, j] = contador++;
                }
            }
        }

        public List<Movimiento> ObtenerMovimientosLegales()
        {
            List<Movimiento> movimientos = new List<Movimiento>();

            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    if (casillas[i, j] > 0 && casillas[i, j] <= 9) // Casilla no ocupada
                    {
                        movimientos.Add(new Movimiento(i, j));
                    }
                }
            }

            return movimientos;
        }

        public Tablero AplicarMovimiento(Movimiento mov, int jugador)
        {
            Tablero nuevo = new Tablero();
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    nuevo.casillas[i, j] = casillas[i, j];
                }
            }

            nuevo.casillas[mov.Fila, mov.Columna] = jugador;
            return nuevo;
        }

        public bool EsTerminal()
        {
            if (ObtenerGanador() != 0) // No 0 significa que hay un ganador
            {
                return true;
            }

            List<Movimiento> disponibles = ObtenerMovimientosLegales();
            return disponibles.Count == 0;
        }

        public int Utilidad()
        {
            int ganador = ObtenerGanador();

            if (ganador == 1) // Jugador 1 (X)
            {
                return +1;
            }

            if (ganador == 2) // Jugador 2 (O)
            {
                return -1;
            }

            return 0;
        }

        public int ObtenerGanador()
        {
            // Filas y columnas
            for (int i = 0; i < 3; i++)
            {
                if (casillas[i, 0] == casillas[i, 1] && casillas[i, 1] == casillas[i, 2])
                {
                    return casillas[i, 0];
                }

                if (casillas[0, i] == casillas[1, i] && casillas[1, i] == casillas[2, i])
                {
                    return casillas[0, i];
                }
            }

            // Diagonales
            if (casillas[0, 0] == casillas[1, 1] && casillas[1, 1] == casillas[2, 2])
            {
                return casillas[0, 0];
            }

            if (casillas[0, 2] == casillas[1, 1] && casillas[1, 1] == casillas[2, 0])
            {
                return casillas[0, 2];
            }

            return 0; // No hay ganador
        }

        public void Mostrar()
        {
            Console.WriteLine("-------------");
            for (int i = 0; i < 3; i++)
            {
                Console.Write("| ");
                for (int j = 0; j < 3; j++)
                {
                    Console.Write(casillas[i, j] + " | ");
                }

                Console.WriteLine();
                Console.WriteLine("-------------");
            }
        }
    }
}
