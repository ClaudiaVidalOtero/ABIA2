using System;
using System.Collections.Generic;

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

public class Tablero
{
    private int[,] casillas;

    public Tablero()
    {
        casillas = new int[3, 3]; // 0 = vacío, 1 = X, -1 = O
    }

    public List<Movimiento> ObtenerMovimientosLegales()
    {
        List<Movimiento> movimientos = new List<Movimiento>();
        for (int i = 0; i < 3; i++)
        {
            for (int j = 0; j < 3; j++)
            {
                if (casillas[i, j] == 0)
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
        Array.Copy(casillas, nuevo.casillas, casillas.Length);
        nuevo.casillas[mov.Fila, mov.Columna] = jugador;
        return nuevo;
    }

    public bool EsTerminal()
    {
        return ObtenerGanador() != 0 || ObtenerMovimientosLegales().Count == 0;
    }

    public int Utilidad()
    {
        return ObtenerGanador();
    }

    public int ObtenerGanador()
    {
        for (int i = 0; i < 3; i++)
        {
            if (casillas[i, 0] != 0 && casillas[i, 0] == casillas[i, 1] && casillas[i, 1] == casillas[i, 2])
                return casillas[i, 0];
            if (casillas[0, i] != 0 && casillas[0, i] == casillas[1, i] && casillas[1, i] == casillas[2, i])
                return casillas[0, i];
        }

        if (casillas[0, 0] != 0 && casillas[0, 0] == casillas[1, 1] && casillas[1, 1] == casillas[2, 2])
            return casillas[0, 0];
        if (casillas[0, 2] != 0 && casillas[0, 2] == casillas[1, 1] && casillas[1, 1] == casillas[2, 0])
            return casillas[0, 2];

        return 0;
    }

    public void Mostrar()
    {
        Console.WriteLine("-------------");
        for (int i = 0; i < 3; i++)
        {
            Console.Write("| ");
            for (int j = 0; j < 3; j++)
            {
                int valor = casillas[i, j];
                int posicion = i * 3 + j + 1;
                string simbolo = valor == 0 ? posicion.ToString() : (valor == 1 ? "X" : "O");
                Console.Write(simbolo + " | ");
            }
            Console.WriteLine();
            Console.WriteLine("-------------");
        }
    }
}

public abstract class Jugador
{
    public int MarcaJugador { get; private set; }

    protected Jugador(int marca)
    {
        MarcaJugador = marca;
    }

    public abstract Movimiento ObtenerMovimiento(Tablero tablero);
}

public class JugadorHumano : Jugador
{
    public JugadorHumano(int marca) : base(marca) { }

    public override Movimiento ObtenerMovimiento(Tablero tablero)
    {
        int numeroSeleccionado;
        Movimiento movimiento = null;

        do
        {
            Console.WriteLine("Introduce un número del 1 al 9 correspondiente a la casilla:");
            string linea = Console.ReadLine();
            Int32.TryParse(linea, out numeroSeleccionado);

            int fila = (numeroSeleccionado - 1) / 3;
            int columna = (numeroSeleccionado - 1) % 3;
            movimiento = new Movimiento(fila, columna);
        }
        while (numeroSeleccionado < 1 || numeroSeleccionado > 9 ||
            !tablero.ObtenerMovimientosLegales().Exists(m => m.Fila == movimiento.Fila && m.Columna == movimiento.Columna));

        return movimiento;
    }
}

public class JugadorAgente : Jugador
{
    private Random generador;

    public JugadorAgente(int marca) : base(marca)
    {
        generador = new Random();
    }

    public override Movimiento ObtenerMovimiento(Tablero tablero)
    {
        return MarcaJugador == 1 ? MinimaxDecisionMax(tablero) : MinimaxDecisionMin(tablero);
    }

    private Movimiento MinimaxDecisionMax(Tablero estado)
    {
        List<Movimiento> mejores = new List<Movimiento>();
        int mejorValor = Int32.MinValue;

        foreach (Movimiento m in estado.ObtenerMovimientosLegales())
        {
            Tablero resultado = estado.AplicarMovimiento(m, MarcaJugador);
            int valor = MinValue(resultado);
            if (valor > mejorValor)
            {
                mejores.Clear();
                mejores.Add(m);
                mejorValor = valor;
            }
            else if (valor == mejorValor)
            {
                mejores.Add(m);
            }
        }

        return mejores[generador.Next(mejores.Count)];
    }

    private Movimiento MinimaxDecisionMin(Tablero estado)
    {
        List<Movimiento> mejores = new List<Movimiento>();
        int mejorValor = Int32.MaxValue;

        foreach (Movimiento m in estado.ObtenerMovimientosLegales())
        {
            Tablero resultado = estado.AplicarMovimiento(m, MarcaJugador);
            int valor = MaxValue(resultado);
            if (valor < mejorValor)
            {
                mejores.Clear();
                mejores.Add(m);
                mejorValor = valor;
            }
            else if (valor == mejorValor)
            {
                mejores.Add(m);
            }
        }

        return mejores[generador.Next(mejores.Count)];
    }

    private int MaxValue(Tablero estado)
    {
        if (estado.EsTerminal()) return estado.Utilidad();

        int v = Int32.MinValue;
        foreach (Movimiento m in estado.ObtenerMovimientosLegales())
        {
            Tablero resultado = estado.AplicarMovimiento(m, 1);
            int valor = MinValue(resultado);
            v = Math.Max(v, valor);
        }
        return v;
    }

    private int MinValue(Tablero estado)
    {
        if (estado.EsTerminal()) return estado.Utilidad();

        int v = Int32.MaxValue;
        foreach (Movimiento m in estado.ObtenerMovimientosLegales())
        {
            Tablero resultado = estado.AplicarMovimiento(m, -1);
            int valor = MaxValue(resultado);
            v = Math.Min(v, valor);
        }
        return v;
    }
}

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
