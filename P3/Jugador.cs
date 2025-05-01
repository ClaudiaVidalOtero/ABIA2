
/// <summary>
/// Clase base abstracta para jugadores, con una propiedad para la marca (X o O)
/// y un método abstracto para obtener el movimiento del jugador.
/// </summary>
public abstract class Jugador
{
    /// <summary>
    /// Propiedad que representa la marca del jugador (1 para X, -1 para O).
    /// </summary>
    public int MarcaJugador { get; private set; }

    /// <summary>
    /// Constructor de la clase base, inicializa la marca del jugador.
    /// </summary>
    /// <param name="marca">Marca del jugador (1 para X, -1 para O).</param>
    protected Jugador(int marca)
    {
        MarcaJugador = marca;
    }

    /// <summary>
    /// Método abstracto que debe ser implementado por clases derivadas para obtener el movimiento
    /// del jugador, dada la situación del tablero.
    /// </summary>
    /// <param name="tablero">El estado actual del tablero.</param>
    /// <returns>Un objeto Movimiento que indica la jugada del jugador.</returns>
    public abstract Movimiento ObtenerMovimiento(Tablero tablero);
}

/// <summary>
/// Clase que representa a un jugador humano, quien realiza su movimiento introduciendo un número
/// correspondiente a una casilla del tablero.
/// </summary>
public class JugadorHumano : Jugador
{
    /// <summary>
    /// Constructor del jugador humano que inicializa la marca del jugador
    /// a través del constructor de la clase base.
    /// </summary>
    /// <param name="marca">Marca del jugador (1 para X, -1 para O).</param>
    public JugadorHumano(int marca) : base(marca) { }

    /// <summary>
    /// Obtiene el movimiento del jugador humano, quien debe seleccionar un número de casilla
    /// entre 1 y 9, que se asigna a una fila y columna del tablero.
    /// </summary>
    /// <param name="tablero">El estado actual del tablero.</param>
    /// <returns>Un objeto Movimiento que indica la jugada seleccionada por el jugador.</returns>
    public override Movimiento ObtenerMovimiento(Tablero tablero)
    {
        int numeroSeleccionado; // Número introducido por el jugador.
        Movimiento movimiento = null; // Movimiento a realizar por el jugador.

        // Bucle que se repite hasta que el jugador ingrese un número válido y la casilla esté disponible.
        do
        {
            // Se solicita al jugador que ingrese un número entre 1 y 9.
            Console.WriteLine("Introduce un número del 1 al 9 correspondiente a la casilla:");
            string linea = Console.ReadLine();

            // Se intenta convertir la entrada del usuario a un número entero.
            int.TryParse(linea, out numeroSeleccionado);

            // Se calcula la fila y la columna basándose en el número seleccionado por el jugador.
            int fila = (numeroSeleccionado - 1) / 3;
            int columna = (numeroSeleccionado - 1) % 3;

            // Se crea un objeto Movimiento con la fila y columna calculadas.
            movimiento = new Movimiento(fila, columna);

        }
        // El bucle continúa si el número no es válido (fuera de rango) o si la casilla está ocupada.
        while (numeroSeleccionado < 1 || numeroSeleccionado > 9 ||
            !tablero.ObtenerMovimientosLegales().Exists(m => m.Fila == movimiento.Fila && m.Columna == movimiento.Columna));

        // Se devuelve el movimiento válido que el jugador seleccionó.
        return movimiento;
    }
}

/// <summary>
/// El agente con el rol de jugador contrincante que decide sus movimientos usando el algoritmo Minimax.
/// </summary>
public class JugadorAgente : Jugador
{
    private Random generador;

    /// <summary>
    /// Constructor del agente que inicializa su marca y su generador de números aleatorios.
    /// </summary>
    /// <param name="marca">Marca del jugador (1 para X, -1 para O).</param>
    public JugadorAgente(int marca) : base(marca)
    {
        generador = new Random();
    }

    /// <summary>
    /// Obtiene el movimiento óptimo del agente usando Minimax dependiendo de la marca.
    /// </summary>
    /// <param name="tablero">Estado actual del tablero.</param>
    /// <returns>Movimiento elegido.</returns>
    public override Movimiento ObtenerMovimiento(Tablero tablero)
    {
        // Elige la función Minimax correspondiente según la marca del jugador
        return MarcaJugador == 1 ? MinimaxDecisionMax(tablero) : MinimaxDecisionMin(tablero);
    }

    /// <summary>
    /// Realiza la decisión Minimax cuando el agente es el jugador MAX (X).
    /// </summary>
    /// <param name="estado">Estado actual del tablero.</param>
    /// <returns>Mejor movimiento encontrado.</returns>
    private Movimiento MinimaxDecisionMax(Tablero estado)
    {
        List<Movimiento> mejores = new List<Movimiento>();
        int mejorValor = int.MinValue;

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

    /// <summary>
    /// Realiza la decisión Minimax cuando el agente es el jugador MIN (O).
    /// </summary>
    /// <param name="estado">Estado actual del tablero.</param>
    /// <returns>Mejor movimiento encontrado.</returns>
    private Movimiento MinimaxDecisionMin(Tablero estado)
    {
        List<Movimiento> mejores = new List<Movimiento>();
        int mejorValor = int.MaxValue;

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

    /// <summary>
    /// Función Max del algoritmo Minimax. Representa al jugador X.
    /// </summary>
    private int MaxValue(Tablero estado)
    {
        if (estado.EsTerminal()) return estado.Utilidad();

        int v = int.MinValue;
        foreach (Movimiento m in estado.ObtenerMovimientosLegales())
        {
            Tablero resultado = estado.AplicarMovimiento(m, 1);
            int valor = MinValue(resultado);
            v = Math.Max(v, valor);
        }
        return v;
    }

    /// <summary>
    /// Función Min del algoritmo Minimax. Representa al jugador O.
    /// </summary>
    private int MinValue(Tablero estado)
    {
        if (estado.EsTerminal()) return estado.Utilidad();

        int v = int.MaxValue;
        foreach (Movimiento m in estado.ObtenerMovimientosLegales())
        {
            Tablero resultado = estado.AplicarMovimiento(m, -1);
            int valor = MaxValue(resultado);
            v = Math.Min(v, valor);
        }
        return v;
    }
}
