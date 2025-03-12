// Claudia Vidal Otero
// Aldana Smyna Medina Lostaunau

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
            List<int> listaReinas = new List<int> {4, 5, 6, 7, 8, 9, 10}; // Diferentes cantidades de reinas a probar
            List<(int, int)> reinas_prefijadas = new List<(int, int)> { (0, 3), (2, 4) };
            reinas_prefijadas.Sort();

            foreach (int reinas in listaReinas)
            {
                Console.WriteLine($"\nResolviendo para {reinas} reinas...");

                // Se establece el estado inicial con las reinas prefijadas
                Solucion nodo_inicial = new Solucion(reinas_prefijadas, 0);

                ColaDePrioridad cola = new ColaDePrioridad();     // Estructura de datos para ejemplo

                List<(int, int)> solucionInicial = new List<(int, int)>(); // Solución inicial vacía

                BusquedaAvara busqueda1 = new BusquedaAvara(cola);  // Insatancia de la clase de búsqueda para el ejemplo

                var resultado = busqueda1.busqueda(
                    new Solucion(solucionInicial, 0),
                    s => criterio_parada(s, reinas),
                    s => obtener_vecinos(s, reinas, reinas_prefijadas),
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
        
        static int contar_conflictos(Solucion solucion)
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
            return conflictos; // Devolvemos la cantidad de conflictos 
        }
        static int calculo_coste(Solucion solucion, Solucion nueva_solucion)
        { 
            return contar_conflictos(nueva_solucion);
        }
        /// <summary>
        /// Calcula la heurística de una solución en el problema de las N reinas.
        /// La heurística se basa en contar el número de conflictos entre reinas en el tablero.
        /// </summary>
        /// <param name="solucion">La solución actual con las coordenadas de las reinas.</param>
        /// <returns>El número de conflictos entre reinas en la solución dada.</returns>
        static int calculo_heuristica(Solucion solucion)
        {
            int reinas_colocadas = solucion.Coords.Count;

            // Creamos una lista para almacenar el número de ataques por reina
            List<int> ataquesPorReina = new List<int>();

            // Contamos los ataques para cada reina
            for (int i = 0; i < reinas_colocadas; i++)
            {
                var reina = solucion.Coords[i];
                int fila = reina.Item1;
                int col = reina.Item2;
                int ataques = 0;

                // Contamos los ataques con otras reinas
                for (int j = 0; j < reinas_colocadas; j++)
                {
                    if (i == j) continue; // No compararnos con la misma reina
                    var otraReina = solucion.Coords[j];
                    int filaOtra = otraReina.Item1;
                    int colOtra = otraReina.Item2;

                    // Verificamos si se atacan: misma fila, columna o diagonal
                    if (fila == filaOtra || col == colOtra || Math.Abs(fila - filaOtra) == Math.Abs(col - colOtra))
                    {
                        ataques++;
                    }
                }

                // Guardamos el número de ataques para esta reina
                ataquesPorReina.Add(ataques);
            }

            // Ahora buscamos la reina con más ataques
            int reinaConMasAtaques = ataquesPorReina.IndexOf(ataquesPorReina.Max());
            var reinaElegida = solucion.Coords[reinaConMasAtaques];
            int filaReina = reinaElegida.Item1;
            int columnaReina = reinaElegida.Item2;

            // Buscamos en la misma columna, pero en las filas donde hay menos ataques
            int mejorColumna = columnaReina;
            int mejorAtaques = int.MaxValue;
            int mejorFila = -1;

            // Recorremos todas las posibles posiciones en la misma columna
            for (int filaPosible = 0; filaPosible < reinas_colocadas; filaPosible++)
            {
                // Si la posición no tiene ataques, la consideramos
                int ataquesPosicion = 0;

                for (int j = 0; j < reinas_colocadas; j++)
                {
                    if (j == reinaConMasAtaques) continue;
                    var otraReina = solucion.Coords[j];
                    int filaOtra = otraReina.Item1;
                    int colOtra = otraReina.Item2;

                    if (filaPosible == filaOtra || mejorColumna == colOtra || Math.Abs(filaPosible - filaOtra) == Math.Abs(mejorColumna - colOtra))
                    {
                        ataquesPosicion++;
                    }
                }

                // Si encontramos una posición con menos ataques, la elegimos
                if (ataquesPosicion < mejorAtaques)
                {
                    mejorAtaques = ataquesPosicion;
                    mejorFila = filaPosible;
                }
            }

            // Si encontramos una nueva posición válida, la movemos
            if (mejorFila != -1 && mejorAtaques < ataquesPorReina[reinaConMasAtaques])
            {
                // Actualizamos la solución si encontramos una mejora
                solucion.Coords[reinaConMasAtaques] = (mejorFila, mejorColumna);
                return mejorAtaques; // La heurística ahora es el número de ataques restantes
            }

            // Si no encontramos ninguna mejora, mantenemos la heurística original
            return ataquesPorReina.Sum();
        }


        /// <summary>
        /// Genera los vecinos de una solución dada, asegurándose de que solo se agreguen posiciones válidas.
        /// Un vecino es una posible ubicación de la siguiente reina en la fila siguiente, sin generar conflictos con las ya colocadas.
        /// </summary>
        /// <param name="solucion">La solución actual con las reinas colocadas hasta el momento.</param>
        /// <param name="reinas">El número total de reinas a colocar.</param>
        /// <returns>Una lista de coordenadas (fila, columna) donde se puede colocar la siguiente reina sin generar conflictos.</returns>
        static List<(int, int)> obtener_vecinos(Solucion solucion, int reinas, List<(int, int)> reinas_prefijadas)
        {
            int row = solucion.Coords.Count == 0 ? -1 : solucion.Coords.Last().Item1;
            List<(int, int)> vecinos = new List<(int, int)>();

            if (row + 1 < reinas)
            {
                for (int j = 0; j < reinas; j++)
                {
                    if (!reinas_prefijadas.Contains((row + 1, j)) && es_consistente(solucion.Coords, row + 1, j))
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

        /// <summary>
        /// Verifica si colocar una nueva reina en una posición dada es consistente con la solución actual.
        /// Una solución es consistente si ninguna de las reinas colocadas hasta el momento se ataca entre sí.
        /// </summary>
        /// <param name="coords">Lista de coordenadas de las reinas colocadas hasta el momento.</param>
        /// <param name="nueva_fila">Fila donde se quiere colocar la nueva reina.</param>
        /// <param name="nueva_columna">Columna donde se quiere colocar la nueva reina.</param>
        /// <returns>True si la posición es válida y no genera conflictos, False en caso contrario.</returns>

        static bool es_consistente(List<(int, int)> coords, int nueva_fila, int nueva_columna)
        {
            for (int i = 0; i < coords.Count; i++)
            {
                int fila_existente = coords[i].Item1;
                int columna_existente = coords[i].Item2;

                // Verifica conflicto en la misma columna o en la misma diagonal
                if (columna_existente == nueva_columna ||
                    Math.Abs(fila_existente - nueva_fila) == Math.Abs(columna_existente - nueva_columna))
                {
                    return false;
                }
            }
            return true;
        }
    }
}
