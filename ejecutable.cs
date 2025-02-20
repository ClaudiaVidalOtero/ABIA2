// Claudia Vidal Otero
// Aldana Smyna Medina Lostaunau
using System;
using System.Collections.Generic;
using System.Linq;
using Estructuras;
using Algoritmos;

namespace Ejecutable
{
    class Program
    /// <summary>
    /// Programa principal que ejecuta el algoritmo A* para resolver el problema.
    /// </summary>
    {
        static void Main()
        {
            int reinas = 5;
            
            ListaCandidatos lista = new ColaDePrioridad(); // Se crea una cola de prioridad (lista de candidatos)

            List<(int, int)> solucion_inicial = new List<(int, int)>(); // Solución inicial vacía

            AEstrella astar = new AEstrella(lista); // Instanciamos el algoritmo A* pasando la cola de prioridad

            // Se realiza la búsqueda con A*, pasando la solución inicial y las funciones de parada, vecinos, coste y heurística
            (Solucion solucion, int revisados)? resultado = astar.busqueda(
                new Solucion(solucion_inicial, 0),
                s => criterio_parada(s, reinas),
                s => obtener_vecinos(s, reinas),
                calculo_coste,
                calculo_heuristica
            );
            
            // Si se encuentra una solución válida, mostramos los resultados
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

        /// <summary>
        /// Función para calcular el coste entre soluciones.
        /// En este caso, el coste es siempre 1.
        /// </summary>
        /// <param name="solucion">La solución actual.</param>
        /// <param name="nueva_solucion">La nueva solución a evaluar.</param>
        /// <returns>El coste entre las soluciones</returns>
        static int calculo_coste(Solucion solucion, Solucion nueva_solucion)
        {
            return 1;
        }

        /// <summary>
        /// Función para calcular la heurística de una solución.
        /// En este caso, siempre devuelve 0 (no se está utilizando una heurística específica).
        /// </summary>
        /// <param name="solucion"> La solución a evaluar. </param>
        /// <returns> El valor de la heurística </returns>
        static int calculo_heuristica(Solucion solucion)
        {
            return 0;
        }
        /// <summary>
        /// Función que devuelve los vecinos de una solución dada, es decir, las posiciones de las reinas que pueden ser colocadas.
        /// Genera todas las posibles ubicaciones para la siguiente reina en la siguiente fila.
        /// </summary>
        /// <param name="solucion"> La solución actual, que contiene las coordenadas de las reinas colocadas hasta el momento. </param>
        /// <param name="reinas"> El numero total de reinas que deben ser colocadas. </param>
        /// <returns> Una lista de coordenadas (tuplas) posibles para la siguiente reina. </returns>
   
        static List<(int, int)> obtener_vecinos(Solucion solucion, int reinas)
        {
            // Si la solución está vacía, comenzamos desde la fila -1
            int row = solucion.Coords.Count == 0 ? -1 : solucion.Coords.Last().Item1;

            List<(int, int)> vecinos = new();

            // Si aún hay filas disponibles para colocar una reina
            if (row + 1 < reinas)
            {
                // Generamos todas las posibles posiciones en la siguiente fila
                for (int j = 0; j < reinas; j++)
                {
                    // Agregamos las posibles posiciones en la fila siguiente (row + 1)
                    vecinos.Add((row + 1, j));
                }
            }
            return vecinos;
        }

        /// <summary>
        /// Criterio de parada para el algoritmo A*.
        /// Consideramos que la solución es válida si se han colocado todas las reinas en el tablero
        /// y ninguna de ellas se está atacando entre sí.
        /// </summary>
        /// <param name="solucion"> La solución actual. </param>
        /// <param name="reinas"> El número total de reinas que deben ser colocadas. </param>
        /// <returns>True si la solución es válida, es decir, todas las reinas están colocadas correctamente. False si no lo es.</returns>
       
        static bool criterio_parada(Solucion solucion, int reinas)
        {
            // Si aun no se han colocado todas las reinas, no es una solución
            if (solucion.Coords.Count < reinas) return false;

            // Recorremos todas las reinas colocadas en la solución
            for (int i = 0; i < solucion.Coords.Count; i++)
            {
                (int fila_i, int columna_i) nodo_i = solucion.Coords[i];
                
                // Se compara la reina actual con las demás para asegurarnos de que no se atacan
                for (int j = i + 1; j < solucion.Coords.Count; j++)
                {
                    (int fila_j, int columna_j) nodo_j = solucion.Coords[j];

                    // Se verifica si las reinas están en la misma columna o si se atacan diagonalmente
                    if (nodo_j.Item2 == nodo_i.Item2 || Math.Abs(nodo_j.Item2 - nodo_i.Item2) == Math.Abs(j - i))
                    {
                        return false;
                    }
                }
            }
            return true; // Todas las reinas están colocadas correctamente
        }
    }
}
