using System;
using System.Collections.Generic;
using System.Linq;
using Estructuras;
using Algoritmos;

namespace Ejecutable
{
    class Program
    {
        static void Main(string[] args)
        {
            int reinas = 6;

            // Función para calcular el coste entre soluciones
            public int calculo_coste(Solucion solucion, Solucion nueva_solucion)
            {
                return 1;
            }
            public int calculo_heuristica(Solucion solucion)
            {
                return 0;
            }

            // Función que devuelve los vecinos de una solución
            public List<(int, int)> obtener_vecinos(Solucion solucion, int reinas)
            {
                int row = solucion.Coords.Count == 0 ? -1 : solucion.Coords.Last().Item1;

                List<(int, int)> vecinos = new();

                if (row + 1 < reinas)
                {
                    for (int j = 0; j < reinas; j++)
                        vecinos.Add((row + 1, j));
                }
                return vecinos;
            }

            // Criterio de parada: se considera solución válida si se han colocado todas las reinas y no se atacan
            public  bool criterio_parada(Solucion solucion, int reinas)
            {
                if (solucion.Coords.Count < reinas) return false;
                for (int i = 0; i < solucion.Coords.Count; i++)
                {
                    (int fila_i, int columna_i) nodo_i = solucion.Coords[i];
                    for (int j = i + 1; j < solucion.Coords.Count; j++)
                    {
                        (int fila_j, int columna_j) nodo_j = solucion.Coords[j];
                        if (nodo_j.Item2 == nodo_i.Item2 || Math.Abs(nodo_j.Item2 - nodo_i.Item2) == Math.Abs(j - i))
                            return false;
                    }
                }
                return true;
            }

            // Creamos la estructura de candidatos (Cola de Prioridad)
            ListaCandidatos  lista = new ListaCandidatos();

            // Instanciamos el algoritmo A* pasando la cola de prioridad
            AEstrella astar = new AEstrella(lista);

            // Solución inicial vacía
            Solucion inicial = new Solucion(new List<(int, int)>(), 0);

            // Realizamos la búsqueda
            var resultado = astar.Buscar(inicial, criterio_parada, obtener_vecinos, calculo_coste, calculo_heuristica);

            if (resultado != null)
            {
                Console.WriteLine("Solución encontrada: " + resultado.Value.Item1);
                Console.WriteLine("Nodos evaluados: " + resultado.Value.Item2);
            }
            else
            {
                Console.WriteLine("No se encontró solución.");
            }
        }
    }
}
