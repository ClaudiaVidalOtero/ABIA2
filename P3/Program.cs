using System;
using System.Threading;
using System.Collections.Generic;

class TresEnRaya
{
    static char[,] tablero = {
        { '1', '2', '3' },
        { '4', '5', '6' },
        { '7', '8', '9' }
    };

    static bool jugadorXEsHumano = true;
    static bool jugadorOEsHumano = false;

    static void Main()
    {
        Console.WriteLine("¿Cómo quieres jugar?");
        Console.WriteLine("1. Humano (X) vs Agente (O)");
        Console.WriteLine("2. Agente (X) vs Agente (O)");
        Console.Write("Selecciona opción (1 o 2): ");
        string opcion = Console.ReadLine();
        if (opcion == "2")
        {
            jugadorXEsHumano = false;
        }

        bool turnoHumano = true;
        while (true)
        {
            ImprimirTablero();
            if ((turnoHumano && jugadorXEsHumano) || (!turnoHumano && jugadorOEsHumano))
            {
                TurnoHumano(turnoHumano ? 'X' : 'O');
            }
            else
            {
                Console.WriteLine($"Turno del agente ({(turnoHumano ? 'X' : 'O')}):");
                MovimientoAgente(turnoHumano ? 'X' : 'O');
                Thread.Sleep(500); // Pausa visual para seguir jugadas
            }

            char resultado = ComprobarGanador();
            if (resultado == 'X' || resultado == 'O')
            {
                ImprimirTablero();
                Console.WriteLine("Ganador: " + resultado);
                break;
            }
            else if (Empate())
            {
                ImprimirTablero();
                Console.WriteLine("¡Empate!");
                break;
            }

            turnoHumano = !turnoHumano;
        }
    }

    static void ImprimirTablero()
    {
        Console.Clear();
        for (int i = 0; i < 3; i++)
        {
            Console.WriteLine(" {0} | {1} | {2} ", tablero[i, 0], tablero[i, 1], tablero[i, 2]);
            if (i < 2) Console.WriteLine("---|---|---");
        }
    }

    static void TurnoHumano(char jugador)
    {
        int posicion;
        while (true)
        {
            Console.Write($"Introduce posición (1-9) para {jugador}: ");
            string input = Console.ReadLine();
            if (int.TryParse(input, out posicion) && posicion >= 1 && posicion <= 9)
            {
                int fila = (posicion - 1) / 3;
                int col = (posicion - 1) % 3;
                if (tablero[fila, col] != 'X' && tablero[fila, col] != 'O')
                {
                    tablero[fila, col] = jugador;
                    break;
                }
            }
            Console.WriteLine("Movimiento inválido.");
        }
    }

    static void MovimientoAgente(char jugador)
    {
        int mejorValor = jugador == 'O' ? int.MinValue : int.MaxValue;
        List<(int fila, int col)> mejoresMovimientos = new List<(int, int)>();

        for (int i = 0; i < 3; i++)
        {
            for (int j = 0; j < 3; j++)
            {
                if (tablero[i, j] != 'X' && tablero[i, j] != 'O')
                {
                    char temp = tablero[i, j];
                    tablero[i, j] = jugador;
                    int valor = Minimax(jugador == 'X');
                    tablero[i, j] = temp;

                    if ((jugador == 'O' && valor > mejorValor) || (jugador == 'X' && valor < mejorValor))
                    {
                        mejorValor = valor;
                        mejoresMovimientos.Clear();
                        mejoresMovimientos.Add((i, j));
                    }
                    else if (valor == mejorValor)
                    {
                        mejoresMovimientos.Add((i, j));
                    }
                }
            }
        }

        // Elegir movimiento aleatorio entre los mejores
        var random = new Random();
        var (mejorFila, mejorCol) = mejoresMovimientos[random.Next(mejoresMovimientos.Count)];
        tablero[mejorFila, mejorCol] = jugador;
    }


    static int Minimax(bool esMaximizador)
    {
        char resultado = ComprobarGanador();
        if (resultado == 'O') return 1;
        if (resultado == 'X') return -1;
        if (Empate()) return 0;

        if (esMaximizador)
        {
            int mejorValor = int.MinValue;
            for (int i = 0; i < 3; i++)
                for (int j = 0; j < 3; j++)
                    if (tablero[i, j] != 'X' && tablero[i, j] != 'O')
                    {
                        char temp = tablero[i, j];
                        tablero[i, j] = 'O';
                        mejorValor = Math.Max(mejorValor, Minimax(false));
                        tablero[i, j] = temp;
                    }
            return mejorValor;
        }
        else
        {
            int mejorValor = int.MaxValue;
            for (int i = 0; i < 3; i++)
                for (int j = 0; j < 3; j++)
                    if (tablero[i, j] != 'X' && tablero[i, j] != 'O')
                    {
                        char temp = tablero[i, j];
                        tablero[i, j] = 'X';
                        mejorValor = Math.Min(mejorValor, Minimax(true));
                        tablero[i, j] = temp;
                    }
            return mejorValor;
        }
    }

    static char ComprobarGanador()
    {
        for (int i = 0; i < 3; i++)
        {
            if (tablero[i, 0] == tablero[i, 1] && tablero[i, 1] == tablero[i, 2])
                return tablero[i, 0];
            if (tablero[0, i] == tablero[1, i] && tablero[1, i] == tablero[2, i])
                return tablero[0, i];
        }

        if (tablero[0, 0] == tablero[1, 1] && tablero[1, 1] == tablero[2, 2])
            return tablero[0, 0];
        if (tablero[0, 2] == tablero[1, 1] && tablero[1, 1] == tablero[2, 0])
            return tablero[0, 2];

        return '-';
    }

    static bool Empate()
    {
        foreach (char c in tablero)
            if (c != 'X' && c != 'O') return false;
        return true;
    }
}

