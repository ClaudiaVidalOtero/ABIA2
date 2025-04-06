// Claudia Vidal Otero (claudia.votero@udc.es)
// Aldana Smyna Medina Lostaunau (aldana.medina@udc.es)
// Grupo 2 (Jueves)
class Program
{
    // Genera un tablero aleatorio pero verifica que sea solucionable
    static int[,] GenerarTableroAleatorioSolucionable()
    {
        int[,] tablero;
        Random rand = new Random();
        do
        {
            List<int> lista = new List<int> { 1, 2, 3, 4, 5, 6, 7, 8, 0 };
            // Se baraja la lista
            lista = lista.OrderBy(x => rand.Next()).ToList();
            tablero = new int[3, 3];
            int index = 0;
            for (int i = 0; i < 3; i++)
                for (int j = 0; j < 3; j++)
                    tablero[i, j] = lista[index++];
        } while (!EsSolucionable(tablero));
        return tablero;
    }

    // Verifica la paridad de las inversiones para determinar la solucionabilidad
    static bool EsSolucionable(int[,] tablero)
    {
        List<int> lista = new List<int>();
        for (int i = 0; i < 3; i++)
            for (int j = 0; j < 3; j++)
                if (tablero[i, j] != 0)
                    lista.Add(tablero[i, j]);

        int inversiones = 0;
        for (int i = 0; i < lista.Count; i++)
            for (int j = i + 1; j < lista.Count; j++)
                if (lista[i] > lista[j])
                    inversiones++;

        return inversiones % 2 == 0;
    }
    static void Main()
    {
        int[,] estadoInicialTablero = GenerarTableroAleatorioSolucionable();
        Estado estadoInicial = new Estado(estadoInicialTablero);

        int[,] estadoObjetivoTablero = new int[,]
        {
            { 1, 2, 3 },
            { 4, 5, 6 },
            { 7, 8, 0 }
        };
        Estado estadoObjetivo = new Estado(estadoObjetivoTablero);

        Console.WriteLine("Estado inicial:");
        estadoInicial.MostrarTablero();

        Planificador planificador = new Planificador();
        List<string> plan = planificador.EncontrarPlan(estadoInicial, estadoObjetivo);

        if (plan != null)
        {
            Console.WriteLine("Plan encontrado:");
            foreach (string accion in plan)
            {
                Console.WriteLine(accion);
            }

            Console.WriteLine("Ejecución del plan:");
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