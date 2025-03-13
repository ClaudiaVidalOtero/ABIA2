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
            List<(int, int)> nodo_inicial = new List<(int, int)> { (0, 3), (2, 4) };
            nodo_inicial.Sort();

            foreach (int reinas in listaReinas)
            {
                Console.WriteLine($"\nResolviendo para {reinas} reinas...");

                // Se establece el estado inicial con las reinas prefijadas

                ColaDePrioridad cola = new ColaDePrioridad();     // Estructura de datos para ejemplo

                List<(int, int)> solucionInicial = new List<(int, int)>(); // Solución inicial vacía

                AEstrella busqueda1 = new AEstrella(cola);  // Instancia de la clase de búsqueda para el ejemplo

                var resultado = busqueda1.busqueda(
                    new Solucion(solucionInicial, 0),
                    s => criterio_parada(s, reinas),
                    s => obtener_vecinos(s, reinas, nodo_inicial),
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
            int conflictos = 0;
            int n = nueva_solucion.Coords.Count;
            
            // Recorremos todas las coordenadas de las reinas
            for (int i = 0; i < n; i++)
            {
                for (int j = i + 1; j < n; j++)
                {
                    // Verificamos si las reinas están en la misma fila, columna o diagonal
                    if (nueva_solucion.Coords[i].Item1 == nueva_solucion.Coords[j].Item1 || // Mismo fila
                        nueva_solucion.Coords[i].Item2 == nueva_solucion.Coords[j].Item2 || // Mismo columna
                        Math.Abs(nueva_solucion.Coords[i].Item1 - nueva_solucion.Coords[j].Item1) == Math.Abs(nueva_solucion.Coords[i].Item2 - nueva_solucion.Coords[j].Item2)) // Misma diagonal
                    {
                        conflictos++;
                    }
                }
            }
            return conflictos; // Devolvemos la cantidad de conflictos 
        }

        /// <summary>
        /// Calcula la heurística de una solución en el problema de las N reinas.
        /// La heurística se basa en contar el número de conflictos entre reinas en el tablero.
        /// </summary>
        /// <param name="solucion">La solución actual con las coordenadas de las reinas.</param>
        /// <returns>El número de conflictos entre reinas en la solución dada.</returns>
        static int calculo_heuristica(Solucion solucion)
        {
            // Obtenemos el número de reinas colocadas en la solución
            int reinas_colocadas = solucion.Coords.Count;

            // Lista que almacenará el número de ataques para cada reina
            List<int> ataquesPorReina = new List<int>();

            // Contamos los ataques para cada reina
            for (int i = 0; i < reinas_colocadas; i++)
            {
                // Obtenemos las coordenadas de la reina en la posición i (fila y columna)
                int filaReina = solucion.Coords[i].Item1;
                int colReina = solucion.Coords[i].Item2;
                int ataques = 0;

                // Comparamos la reina i con todas las demás reinas (excepto consigo misma)
                for (int j = 0; j < reinas_colocadas; j++)
                {
                    if (i == j) continue; // No comparamos la reina consigo misma

                    // Obtenemos las coordenadas de la otra reina (j)
                    int filaOtraReina = solucion.Coords[j].Item1;
                    int colOtraReina = solucion.Coords[j].Item2;

                    // Verificamos si las reinas se atacan (misma fila, columna o diagonal)
                    if (filaReina == filaOtraReina || colReina == colOtraReina || Math.Abs(filaReina - filaOtraReina) == Math.Abs(colReina - colOtraReina))
                    {
                        ataques++; // Incrementamos el número de ataques
                    }
                }

                // Guardamos el número de ataques de la reina i
                ataquesPorReina.Add(ataques);
            }

            // Seleccionamos la reina con más ataques (la que tiene más conflictos)
            int reinaConMasAtaques = ataquesPorReina.IndexOf(ataquesPorReina.Max());
            // Obtenemos las coordenadas de la reina con más ataques
            int reinaFila = solucion.Coords[reinaConMasAtaques].Item1;
            int reinaColumna = solucion.Coords[reinaConMasAtaques].Item2;

            // Inicializamos los valores para la mejor posición ( donde haya menos ataques)
            int mejorAtaques = int.MaxValue;
            int mejorFila = -1;

            // Recorremos todas las posibles posiciones en las filas para encontrar la mejor para la reina seleccionada
            for (int filaPosible = 0; filaPosible < reinas_colocadas; filaPosible++)
            {
                if (filaPosible == reinaFila) continue; // No moverla a la misma fila

                // Variable para contar los ataques en la posición actual
                int ataquesPosicion = 0;

                // Comparamos esta posible posición con todas las demás reinas
                for (int j = 0; j < reinas_colocadas; j++)
                {
                    if (j == reinaConMasAtaques) continue; // No comparamos con la reina seleccionada

                    // Obtenemos las coordenadas de la otra reina (j)
                    int filaOtraReina = solucion.Coords[j].Item1;
                    int colOtraReina = solucion.Coords[j].Item2;

                    // Verificamos si las reinas se atacan en la nueva posición
                    if (filaPosible == filaOtraReina || reinaColumna == colOtraReina || Math.Abs(filaPosible - filaOtraReina) == Math.Abs(reinaColumna - colOtraReina))
                    {
                        ataquesPosicion++; // Incrementamos el número de ataques
                    }
                }

                // Si encontramos una posición con menos ataques, la consideramos
                if (ataquesPosicion < mejorAtaques)
                {
                    mejorAtaques = ataquesPosicion; // Actualizamos el número de ataques
                    mejorFila = filaPosible; // Actualizamos la mejor fila
                }
            }

            // Si encontramos una mejor posición (con menos ataques), movemos la reina
            if (mejorFila != -1 && mejorAtaques < ataquesPorReina[reinaConMasAtaques])
            {
                solucion.Coords[reinaConMasAtaques] = new ValueTuple<int, int>(mejorFila, reinaColumna); // Movemos la reina
                return mejorAtaques; // Devolvemos los ataques en la nueva posición
            }

            // Si no encontramos una mejora, devolvemos la suma total de ataques de todas las reinas
            return ataquesPorReina.Sum();
        }





        /// <summary>
        /// Genera los vecinos de una solución dada, asegurándose de que solo se agreguen posiciones válidas.
        /// Un vecino es una posible ubicación de la siguiente reina en la fila siguiente, sin generar conflictos con las ya colocadas.
        /// </summary>
        /// <param name="solucion">La solución actual con las reinas colocadas hasta el momento.</param>
        /// <param name="reinas">El número total de reinas a colocar.</param>
        /// <returns>Una lista de coordenadas (fila, columna) donde se puede colocar la siguiente reina sin generar conflictos.</returns>
        static List<(int, int)> obtener_vecinos(Solucion solucion, int reinas, List<(int, int)> nodo_inicial)
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
                    if (nodo_inicial.Contains((row + 1, j))) continue;

                    // Creando una nueva solución provisional con las coordenadas actuales
                    List<(int, int)> nuevaCoords = [.. solucion.Coords]; // Copia de las coordenadas actuales
                    nuevaCoords.Add((row + 1, j)); // Agregamos la nueva coordenada a la fila siguiente

                    // Creamos una nueva solución provisional con las coordenadas modificadas
                    Solucion nuevaSolucion = new Solucion(nuevaCoords, 0); // Suponiendo que Solucion tiene un constructor para las coordenadas


                    // Verificar si la nueva solución provisional es consistente
                    if (es_consistente(nuevaSolucion))
                    {
                        vecinos.Add((row + 1, j)); // Si es consistente, agregarla a los vecinos
                    }
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

        static bool es_consistente(Solucion solucion)
        {
            for (int i = 0; i < solucion.Coords.Count; i++)
            {
                for (int j = i + 1; j < solucion.Coords.Count; j++)
                {
                    int fila1 = solucion.Coords[i].Item1;
                    int columna1 = solucion.Coords[i].Item2;
                    int fila2 = solucion.Coords[j].Item1;
                    int columna2 = solucion.Coords[j].Item2;

                    // Verificar si las reinas están en la misma fila, columna o diagonal
                    if (fila1 == fila2 || columna1 == columna2 || Math.Abs(fila1 - fila2) == Math.Abs(columna1 - columna2))
                    {
                        return false; // La solución es inconsistente
                    }
                }
            }
            return true; // La solución es consistente
        }

    }
}
