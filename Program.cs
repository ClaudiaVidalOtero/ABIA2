using System;
using System.Collections.Generic;

/// <summary>
/// Representa las posibles marcas en el tablero.
/// </summary>
public enum Marca
{
    Vacía,
    X,
    O
}

/// <summary>
/// Representa una jugada en el tablero: fila y columna.
/// </summary>
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

/// <summary>
/// Representa el tablero de Tres en Raya y operaciones sobre él.
/// </summary>
public class Tablero
{
    private Marca[,] casillas;

    /// <summary>
    /// Constructor que inicializa un tablero vacío.
    /// </summary>
    public Tablero()
    {
        casillas = new Marca[3, 3];
        for (int i = 0; i < 3; i++)
        {
            for (int j = 0; j < 3; j++)
            {
                casillas[i, j] = Marca.Vacía;
            }
        }
    }

    /// <summary>
    /// Obtiene la lista de movimientos legales (casillas vacías).
    /// </summary>
    public List<Movimiento> ObtenerMovimientosLegales()
    {
        List<Movimiento> movimientos = new List<Movimiento>();

        for (int i = 0; i < 3; i++)
        {
            for (int j = 0; j < 3; j++)
            {
                if (casillas[i, j] == Marca.Vacía)
                {
                    movimientos.Add(new Movimiento(i, j));
                }
            }
        }

        return movimientos;
    }

    /// <summary>
    /// Aplica un movimiento devolviendo un nuevo tablero.
    /// </summary>
    public Tablero AplicarMovimiento(Movimiento mov, Marca jugador)
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

    /// <summary>
    /// Comprueba si el estado actual es terminal (victoria o empate).
    /// </summary>
    public bool EsTerminal()
    {
        if (ObtenerGanador() != Marca.Vacía)
        {
            return true;
        }

        List<Movimiento> disponibles = ObtenerMovimientosLegales();
        return disponibles.Count == 0;
    }

    /// <summary>
    /// Devuelve la utilidad del estado: +1 si gana X, -1 si gana O, 0 en empate o no terminal.
    /// </summary>
    public int Utilidad()
    {
        Marca ganador = ObtenerGanador();

        if (ganador == Marca.X)
        {
            return +1;
        }

        if (ganador == Marca.O)
        {
            return -1;
        }

        return 0;
    }

    /// <summary>
    /// Determina el ganador: X, O o Vacía si no hay.
    /// </summary>
    public Marca ObtenerGanador()
    {
        // Filas y columnas
        for (int i = 0; i < 3; i++)
        {
            if (casillas[i, 0] != Marca.Vacía &&
                casillas[i, 0] == casillas[i, 1] &&
                casillas[i, 1] == casillas[i, 2])
            {
                return casillas[i, 0];
            }

            if (casillas[0, i] != Marca.Vacía &&
                casillas[0, i] == casillas[1, i] &&
                casillas[1, i] == casillas[2, i])
            {
                return casillas[0, i];
            }
        }

        // Diagonales
        if (casillas[0, 0] != Marca.Vacía &&
            casillas[0, 0] == casillas[1, 1] &&
            casillas[1, 1] == casillas[2, 2])
        {
            return casillas[0, 0];
        }

        if (casillas[0, 2] != Marca.Vacía &&
            casillas[0, 2] == casillas[1, 1] &&
            casillas[1, 1] == casillas[2, 0])
        {
            return casillas[0, 2];
        }

        return Marca.Vacía;
    }

    /// <summary>
    /// Muestra el tablero por consola.
    /// </summary>
    public void Mostrar()
    {
        Console.WriteLine("-------------");
        for (int i = 0; i < 3; i++)
        {
            Console.Write("| ");
            for (int j = 0; j < 3; j++)
            {
                int posicion = i * 3 + j + 1;
                string símbolo = casillas[i, j] == Marca.Vacía ? posicion.ToString() : casillas[i, j].ToString();
                Console.Write(símbolo + " | ");
            }

            Console.WriteLine();
            Console.WriteLine("-------------");
        }
    }

}

/// <summary>
/// Jugador abstracto con método para elegir un movimiento.
/// </summary>
public abstract class Jugador
{
    public Marca MarcaJugador { get; private set; }

    protected Jugador(Marca marca)
    {
        MarcaJugador = marca;
    }

    public abstract Movimiento ObtenerMovimiento(Tablero tablero);
}

/// <summary>
/// Jugador humano: solicita coordenadas por consola.
/// </summary>
public class JugadorHumano : Jugador
{
    public JugadorHumano(Marca marca)
        : base(marca)
    {
    }

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

/// <summary>
/// Jugador agente que utiliza el algoritmo Minimax con desempate aleatorio.
/// </summary>
public class JugadorAgente : Jugador
{
    private Random generador;

    public JugadorAgente(Marca marca)
        : base(marca)
    {
        generador = new Random();
    }

    public override Movimiento ObtenerMovimiento(Tablero tablero)
    {
        if (MarcaJugador == Marca.X)
        {
            return MinimaxDecisionMax(tablero);
        }

        return MinimaxDecisionMin(tablero);
    }

    private Movimiento MinimaxDecisionMax(Tablero estado)
    {
        List<Movimiento> mejores = new List<Movimiento>();
        int mejorValor = Int32.MinValue;

        List<Movimiento> acciones = estado.ObtenerMovimientosLegales();

        foreach (Movimiento m in acciones)
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

        int indice = generador.Next(mejores.Count);
        return mejores[indice];
    }

    private Movimiento MinimaxDecisionMin(Tablero estado)
    {
        List<Movimiento> mejores = new List<Movimiento>();
        int mejorValor = Int32.MaxValue;

        List<Movimiento> acciones = estado.ObtenerMovimientosLegales();

        foreach (Movimiento m in acciones)
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

        int indice = generador.Next(mejores.Count);
        return mejores[indice];
    }

    private int MaxValue(Tablero estado)
    {
        if (estado.EsTerminal())
        {
            return estado.Utilidad();
        }

        int v = Int32.MinValue;
        List<Movimiento> acciones = estado.ObtenerMovimientosLegales();

        foreach (Movimiento m in acciones)
        {
            Tablero resultado = estado.AplicarMovimiento(m, Marca.X);
            int valor = MinValue(resultado);
            v = Math.Max(v, valor);
        }

        return v;
    }

    private int MinValue(Tablero estado)
    {
        if (estado.EsTerminal())
        {
            return estado.Utilidad();
        }

        int v = Int32.MaxValue;
        List<Movimiento> acciones = estado.ObtenerMovimientosLegales();

        foreach (Movimiento m in acciones)
        {
            Tablero resultado = estado.AplicarMovimiento(m, Marca.O);
            int valor = MaxValue(resultado);
            v = Math.Min(v, valor);
        }

        return v;
    }
}

/// <summary>
/// Clase principal del juego.
/// </summary>
public class Juego
{
    /// <summary>
    /// Punto de entrada.
    /// </summary>
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

        Jugador jugadorX;
        Jugador jugadorO;

        if (opcion == 1)
        {
            jugadorX = new JugadorHumano(Marca.X);
            jugadorO = new JugadorAgente(Marca.O);
        }
        else
        {
            jugadorX = new JugadorAgente(Marca.X);
            jugadorO = new JugadorAgente(Marca.O);
        }

        Tablero tablero = new Tablero();
        Marca turno = Marca.X;

        while (!tablero.EsTerminal())
        {
            tablero.Mostrar();

            Movimiento jugada;
            if (turno == Marca.X)
            {
                jugada = jugadorX.ObtenerMovimiento(tablero);
            }
            else
            {
                jugada = jugadorO.ObtenerMovimiento(tablero);
            }

            tablero = tablero.AplicarMovimiento(jugada, turno);
            turno = (turno == Marca.X) ? Marca.O : Marca.X;
        }

        tablero.Mostrar();
        Marca ganador = tablero.ObtenerGanador();

        if (ganador == Marca.Vacía)
        {
            Console.WriteLine("Empate.");
        }
        else
        {
            Console.WriteLine("Gana: " + ganador.ToString());
        }
    }
}
