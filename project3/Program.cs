// Claudia Vidal Otero (claudia.votero@udc.es)
// Aldana Smyna Medina Lostaunau (aldana.medina@udc.es)
// Grupo 2 (Jueves)

class Program
{
    /// <summary>
    /// Genera un tablero aleatorio de 8-puzzle que sea solucionable.
    /// </summary>
    static int[,] GenerarTableroAleatorioSolucionable()
    {
        int[,] tablero;
        Random rand = new Random();
        do
        {
            // Crear una lista con los números del 0 al 8
            List<int> lista = new List<int> { 1, 2, 3, 4, 5, 6, 7, 8, 0 };

            // Mezclar la lista aleatoriamente
            lista = lista.OrderBy(x => rand.Next()).ToList();

            // Convertir la lista en una matriz 3x3
            tablero = new int[3, 3];
            int index = 0;

            for (int fila = 0; fila < 3; fila++)
                for (int col = 0; col < 3; col++)
                    tablero[fila, col] = lista[index++];

        } while (!EsSolucionable(tablero)); // Verificar si el tablero es solucionable

        return tablero;
    }

    /// <summary>
    /// Determina si un tablero es solucionable usando el número de inversiones.
    /// </summary>
    static bool EsSolucionable(int[,] tablero)
    {
        List<int> lista = new List<int>();
        
        // Convertir la matriz en lista, ignorando el cero (espacio vacío)
        for (int fila = 0; fila < 3; fila++)
            for (int col = 0; col < 3; col++)
                if (tablero[fila, col] != 0)
                    lista.Add(tablero[fila, col]);

        // Contar el número de inversiones (pares fuera de orden)
        int inversiones = 0;
        for (int i = 0; i < lista.Count; i++)
            for (int j = i + 1; j < lista.Count; j++)
                if (lista[i] > lista[j])
                    inversiones++;

        // Un tablero es solucionable si el número de inversiones es par
        return inversiones % 2 == 0;
    }

    /// <summary>
    /// Método principal: genera estado inicial, encuentra un plan y lo ejecuta paso a paso.
    /// </summary>
    static void Main()
    {
        // Crear estado inicial aleatorio y solucionable
        int[,] tableroInicial = GenerarTableroAleatorioSolucionable();
        Estado estadoInicial = new Estado(tableroInicial);

        // Definir el estado objetivo 
        int[,] tableroObjetivo = new int[,]
        {
            { 1, 2, 3 },
            { 4, 5, 6 },
            { 7, 8, 0 }
        };
        Estado estadoObjetivo = new Estado(tableroObjetivo);

        // Mostrar el tablero inicial
        Console.WriteLine("Estado inicial:");
        estadoInicial.MostrarTablero();

        // Crear el planificador y buscar un plan para alcanzar el objetivo
        Planificador planificador = new Planificador();
        List<string> plan = planificador.EncontrarPlan(estadoInicial, estadoObjetivo);

        if (plan != null)
        {
            // Mostrar la lista de acciones encontradas
            Console.WriteLine("Plan encontrado:");
            foreach (string accion in plan)
            {
                Console.WriteLine(accion);
            }

            // Ejecutar el plan paso a paso mostrando cada estado
            Console.WriteLine("\nEjecución del plan:");
            foreach (string accion in plan)
            {
                Console.WriteLine($"Ejecutando: {accion}");
                estadoInicial = estadoInicial.AplicarAccion(new Accion(accion));
                estadoInicial.MostrarTablero();
            }
        }
        else
        {
            Console.WriteLine("No se encontró solución.");
        }
    }
}
