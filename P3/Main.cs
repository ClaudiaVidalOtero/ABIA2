public class Juego
{
    /// <summary>
    /// Método principal que ejecuta el juego de Tres en Raya, permitiendo jugar entre un humano y un agente,
    /// o entre dos agentes. Gestiona los turnos y muestra el resultado final del juego.
    /// </summary>
    public static void Main()
    {
        // Se muestra el menú para que el usuario elija el modo de juego.
        Console.WriteLine("Seleccione modo de juego:");
        Console.WriteLine("1. Humano vs Agente");
        Console.WriteLine("2. Agente vs Agente");

        int opcion = -1; // Variable para almacenar la opción elegida por el jugador

        // Bucle que se repite hasta que el usuario introduce una opción válida (1 o 2)
        while (opcion != 1 && opcion != 2)
        {
            Console.WriteLine("Ingrese 1 o 2:");
            int.TryParse(Console.ReadLine(), out opcion); // Intenta convertir la entrada del usuario en un número entero
        }

        // Se crea el jugador X (marca 1) dependiendo de la opción elegida:
        // Si la opción es 1, el jugador X es un humano, de lo contrario, es un agente.
        Jugador jugadorX = opcion == 1 ? new JugadorHumano(1) : new JugadorAgente(1);

        // El jugador O (marca -1) siempre será un agente, independientemente de la opción elegida.
        Jugador jugadorO = new JugadorAgente(-1);

        // Se inicializa el tablero vacío para comenzar el juego
        Tablero tablero = new Tablero();

        // Variable que indica el turno del jugador actual (1 para X, -1 para O)
        int turno = 1;

        // Bucle principal del juego: se ejecuta mientras el estado del tablero no sea terminal (gana alguien o empate)
        while (!tablero.EsTerminal())
        {
            tablero.Mostrar();

            // El jugador actual (según el turno) obtiene su movimiento del tablero
            Movimiento jugada = turno == 1 ? jugadorX.ObtenerMovimiento(tablero) : jugadorO.ObtenerMovimiento(tablero);

            // Se aplica el movimiento al tablero y se obtiene el nuevo estado del tablero
            tablero = tablero.AplicarMovimiento(jugada, turno);

            // Se cambia de turno: de 1 a -1 o de -1 a 1, alternando entre X y O
            turno = -turno;
        }

        // Se muestra el tablero final después de que el juego haya terminado
        tablero.Mostrar();

        // Se determina el ganador: 1 si gana X, -1 si gana O, 0 si hay empate
        int ganador = tablero.ObtenerGanador();

        // Muestra el resultado final del juego
        if (ganador == 0)
            Console.WriteLine("Empate.");
        else
            Console.WriteLine("Gana: " + (ganador == 1 ? "X (humano)" : "O (agente)"));
    }
}
