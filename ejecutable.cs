using System;
using System.Collections.Generic;
using System.Linq;
using Estructuras;
using Algoritmos;
namespace Ejecutable
{
    class Program
    {
        static void Main()
        {
            int reinas = 5;
            
            ListaCandidatos lista = new ColaDePrioridad();

            /// Solución inicial vacía
            List<(int, int)> solucion_inicial = new List<(int, int)>();

            
            // Instanciamos el algoritmo A* pasando la cola de prioridad
            AEstrella astar = new AEstrella(lista);

            // Realizamos la búsqueda
            (Solucion solucion, int revisados)? resultado = astar.busqueda(
                new Solucion(solucion_inicial, 0),
                s => criterio_parada(s, reinas),
                s => obtener_vecinos(s, reinas),
                calculo_coste,
                calculo_heuristica
            );

            if (resultado != null)
            {
                Console.WriteLine("Coordenadas: " + string.Join(", ", resultado.Value.Item1.Coords));
                Console.WriteLine("Nodos evaluados: " + resultado.Value.Item2);
            }
            else
            {
                Console.WriteLine("No se encontró solución.");
            }
        }

        // Función para calcular el coste entre soluciones
        static int calculo_coste(Solucion solucion, Solucion nueva_solucion)
        {
            return 1;
        }

        static int calculo_heuristica(Solucion solucion)
        {
            return 0;
        }

        // Función que devuelve los vecinos de una solución
        static List<(int, int)> obtener_vecinos(Solucion solucion, int reinas)
        {
            int row = solucion.Coords.Count == 0 ? -1 : solucion.Coords.Last().Item1;

            List<(int, int)> vecinos = new();

            if (row + 1 < reinas)
            {
                for (int j = 0; j < reinas; j++)
                {
                    vecinos.Add((row + 1, j));
                }
            }
            return vecinos;
        }

        // Criterio de parada: se considera solución válida si se han colocado todas las reinas y no se atacan
        static bool criterio_parada(Solucion solucion, int reinas)
        {
            if (solucion.Coords.Count < reinas) return false;
            for (int i = 0; i < solucion.Coords.Count; i++)
            {
                (int fila_i, int columna_i) nodo_i = solucion.Coords[i];
                for (int j = i + 1; j < solucion.Coords.Count; j++)
                {
                    (int fila_j, int columna_j) nodo_j = solucion.Coords[j];
                    if (nodo_j.Item2 == nodo_i.Item2 || Math.Abs(nodo_j.Item2 - nodo_i.Item2) == Math.Abs(j - i))
                    {
                        return false;
                    }
                }
            }
            return true;
        }
    }
}
