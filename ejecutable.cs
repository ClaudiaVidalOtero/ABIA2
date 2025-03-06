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
    /// Programa principal que ejecuta el algoritmo para resolver el problema.
    /// </summary>
    {
        static void Main()
        {
            List<int> listaReinas = new List<int> {4, 5, 6, 7, 8, 9, 10, 11}; // Diferentes cantidades de reinas a probar

            foreach (int reinas in listaReinas)
            {
                Console.WriteLine($"\nResolviendo para {reinas} reinas...");

                ColaDePrioridad lista = new ColaDePrioridad();     // Estructura de datos para ejemplo

                List<(int, int)> solucionInicial = new List<(int, int)>(); // Solución inicial vacía

                CosteUniforme busqueda1 = new CosteUniforme(lista);  // Insatancia de la clase de búsqueda para el ejemplo

                var resultado = busqueda1.busqueda(
                    new Solucion(solucionInicial, 0),
                    s => criterio_parada(s, reinas),
                    s => obtener_vecinos(s, reinas),
                    calculo_coste,
                    calculo_heuristica
                );

                if (resultado != null)
                {
                    Console.WriteLine("Coordenadas de las reinas: " + string.Join(", ", resultado.Value.Item1.Coords));
                    Console.WriteLine("Nodos evaluados: " + resultado.Value.Item2);
                }
                else
                {
                    Console.WriteLine("No se encontró solución.");
                }
            }
        }
        /// <summary>
        /// Calcula el coste de moverse de la solución actual a la nueva solución.
        /// En este contexto, el coste representa el número de pasos dados hasta el momento.
        /// </summary>
        /// <param name="solucion">La solución actual.</param>
        /// <param name="nueva_solucion">La nueva solución generada a partir de la actual.</param>
        /// <returns>El coste acumulado hasta la nueva solución.</returns>
        static int calculo_coste(Solucion solucion, Solucion nueva_solucion)
        { 
            return solucion.Coste + 1;
        }

        static int calculo_heuristica(Solucion solucion)
        {
            int conflictos = 0;
            int n = solucion.Coords.Count;
            
            // Recorremos todas las coordenadas de las reinas
            for (int i = 0; i < n; i++)
            {
                for (int j = i + 1; j < n; j++)
                {
                    // Verificamos si las reinas están en la misma fila, columna o diagonal
                    if (solucion.Coords[i].Item1 == solucion.Coords[j].Item1 || // Mismo fila
                        solucion.Coords[i].Item2 == solucion.Coords[j].Item2 || // Mismo columna
                        Math.Abs(solucion.Coords[i].Item1 - solucion.Coords[j].Item1) == Math.Abs(solucion.Coords[i].Item2 - solucion.Coords[j].Item2)) // Misma diagonal
                    {
                        conflictos++;
                    }
                }
            }

            return conflictos; // Devolvemos la cantidad de conflictos como heurística
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
