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
            int calculoCoste(Solucion sol1, Solucion sol2)
            {
                return 1;
            }

            // Función heurística (aquí simplemente retorna 0)
            int calculoHeuristica(Solucion sol)
            {
                return 0;
            }

            // Función que devuelve los vecinos de una solución
            List<(int, int)> obtenerVecinos(Solucion sol)
            {
                int row = sol.Coords.Count == 0 ? -1 : sol.Coords.Last().Item1;
                List<(int, int)> vecinos = new List<(int, int)>();

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
            bool criterioParada(Solucion sol)
            {
                if (sol.Coords.Count < reinas) return false;
                for (int i = 0; i < sol.Coords.Count; i++)
                {
                    for (int j = i + 1; j < sol.Coords.Count; j++)
                    {
                        if (sol.Coords[i].Item2 == sol.Coords[j].Item2 ||
                            Math.Abs(sol.Coords[i].Item2 - sol.Coords[j].Item2) == Math.Abs(i - j))
                        {
                            return false;
                        }
                    }
                }
                return true;
            }

            // Creamos la estructura de candidatos (Cola de Prioridad)
            ColaDePrioridad cola = new ColaDePrioridad();

            // Instanciamos el algoritmo A* pasando la cola de prioridad
            AEstrella astar = new AEstrella(cola);

            // Solución inicial vacía
            Solucion inicial = new Solucion(new List<(int, int)>(), 0);

            // Realizamos la búsqueda
            var resultado = astar.Buscar(inicial, criterioParada, obtenerVecinos, calculoCoste, calculoHeuristica);

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
