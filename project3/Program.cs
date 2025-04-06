// Claudia Vidal Otero (claudia.votero@udc.es)
// Aldana Smyna Medina Lostaunau (aldana.medina@udc.es)
// Grupo 2 (Jueves)

/// <summary>
/// Clase principal que contiene el punto de entrada del programa y funciones auxiliares
/// para generar estados y validar tableros del 8-puzzle usando lógica STRIPS.
/// </summary>
class Program
{
    /// <summary>
    /// Convierte un tablero de enteros (3x3) en un conjunto de predicados STRIPS.
    /// 0 representa el hueco vacío. El resto de números son fichas numeradas.
    /// </summary>
    /// <param name="tablero">Matriz de enteros representando el tablero</param>
    /// <returns>Conjunto de predicados que representan el estado</returns>
    static HashSet<Predicado> GenerarPredicadosDesdeTablero(int[,] tablero)
    {
        var predicados = new HashSet<Predicado>();

        for (int fila = 0; fila < 3; fila++)
        {
            for (int col = 0; col < 3; col++)
            {
                int val = tablero[fila, col];
                if (val == 0)
                    predicados.Add(new Predicado("Vacia", fila, col)); // Representa el hueco vacío
                else
                    predicados.Add(new Predicado("En", val, fila, col)); // Representa una ficha en una posición
            }
        }

        return predicados;
    }

    /// <summary>
    /// Genera un tablero aleatorio del 8-puzzle que sea solucionable.
    /// </summary>
    /// <returns>Matriz 3x3 con los números del 8-puzzle en orden aleatorio pero válido</returns>
    static int[,] GenerarTableroAleatorioSolucionable()
    {
        int[,] tablero;
        Random rand = new Random();
        do
        {
            var lista = new List<int> { 1, 2, 3, 4, 5, 6, 7, 8, 0 }; // Incluye el hueco (0)
            lista = lista.OrderBy(x => rand.Next()).ToList(); // Mezcla aleatoria

            tablero = new int[3, 3];
            int idx = 0;
            for (int i = 0; i < 3; i++)
                for (int j = 0; j < 3; j++)
                    tablero[i, j] = lista[idx++]; // Rellena el tablero con los números mezclados
        }
        while (!EsSolucionable(tablero)); // Solo acepta configuraciones válidas
        return tablero;
    }

    /// <summary>
    /// Verifica si un tablero del 8-puzzle es solucionable, contando las inversiones.
    /// </summary>
    /// <param name="tablero">Matriz 3x3</param>
    /// <returns>True si es solucionable, False si no</returns>
    static bool EsSolucionable(int[,] tablero)
    {
        var lista = new List<int>();
        for (int i = 0; i < 3; i++)
            for (int j = 0; j < 3; j++)
                if (tablero[i, j] != 0)
                    lista.Add(tablero[i, j]); // Se ignora el hueco

        int inversiones = 0;
        for (int i = 0; i < lista.Count; i++)
            for (int j = i + 1; j < lista.Count; j++)
                if (lista[i] > lista[j])
                    inversiones++; // Cuenta pares en orden incorrecto

        return inversiones % 2 == 0; // Es solucionable si las inversiones son pares
    }

    /// <summary>
    /// Punto de entrada del programa.
    /// Genera un problema aleatorio del 8-puzzle, encuentra un plan con STRIPS y lo ejecuta.
    /// </summary>
    static void Main()
    {
        // Generación del estado inicial aleatorio
        int[,] tableroInicial = GenerarTableroAleatorioSolucionable();

        // Estado objetivo fijo: 1 2 3 / 4 5 6 / 7 8 0
        int[,] tableroObjetivo = new int[,]
        {
            {1, 2, 3},
            {4, 5, 6},
            {7, 8, 0}
        };

        var estadoInicial = new Estado(GenerarPredicadosDesdeTablero(tableroInicial));
        var estadoObjetivo = new Estado(GenerarPredicadosDesdeTablero(tableroObjetivo));

        Console.WriteLine("Estado inicial:");
        estadoInicial.Mostrar();

        var planificador = new Planificador();
        var plan = planificador.EncontrarPlan(estadoInicial, estadoObjetivo);

        if (plan != null)
        {
            Console.WriteLine("Plan encontrado:");
            foreach (var accion in plan)
                Console.WriteLine(accion); // Muestra la secuencia de acciones

            Console.WriteLine("\nEjecución del plan:");
            foreach (var accion in plan)
            {
                Console.WriteLine($"Ejecutando: {accion}");
                estadoInicial = estadoInicial.Aplicar(accion); // Aplica la acción al estado actual
                estadoInicial.Mostrar();
            }
        }
        else
        {
            Console.WriteLine("No se encontró un plan.");
        }
    }
}
