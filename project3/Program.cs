using System;
using System.Collections.Generic;
using System.Linq;



class Program
{
    static HashSet<Predicado> GenerarPredicadosDesdeTablero(int[,] tablero)
    {
        var predicados = new HashSet<Predicado>();

        for (int fila = 0; fila < 3; fila++)
        {
            for (int col = 0; col < 3; col++)
            {
                int val = tablero[fila, col];
                if (val == 0)
                    predicados.Add(new Predicado("Vacia", fila, col));
                else
                    predicados.Add(new Predicado("En", val, fila, col));
            }
        }

        return predicados;
    }

    static int[,] GenerarTableroAleatorioSolucionable()
    {
        int[,] tablero;
        Random rand = new Random();
        do
        {
            var lista = new List<int> { 1, 2, 3, 4, 5, 6, 7, 8, 0 };
            lista = lista.OrderBy(x => rand.Next()).ToList();

            tablero = new int[3, 3];
            int idx = 0;
            for (int i = 0; i < 3; i++)
                for (int j = 0; j < 3; j++)
                    tablero[i, j] = lista[idx++];
        }
        while (!EsSolucionable(tablero));
        return tablero;
    }

    static bool EsSolucionable(int[,] tablero)
    {
        var lista = new List<int>();
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
        int[,] tableroInicial = GenerarTableroAleatorioSolucionable();
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
                Console.WriteLine(accion);

            Console.WriteLine("\nEjecución del plan:");
            foreach (var accion in plan)
            {
                Console.WriteLine($"Ejecutando: {accion}");
                estadoInicial = estadoInicial.Aplicar(accion);
                estadoInicial.Mostrar();
            }
        }
        else
        {
            Console.WriteLine("No se encontró un plan.");
        }
    }
}
