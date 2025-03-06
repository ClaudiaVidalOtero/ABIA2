**TRABAJO REALIZADO**
## SEMANA 3: BÚSQUEDA INFORMADA

1. BUSQUEDA AVARA

La clase `BusquedaAvara` implementa la búsqueda avara, un algoritmo que selecciona las soluciones basándose únicamente en una función heurística. Al calcular la prioridad de un nodo, solo considera el valor heurístico proporcionado, ignorando el costo real acumulado. Esto hace que la búsqueda se enfoque en la dirección que parece más prometedora a corto plazo, aunque no siempre garantice encontrar la solución óptima.

2. COSTE UNIFORME

La clase `CosteUniforme` implementa un algoritmo de búsqueda basado en el costo uniforme, que prioriza la exploración de soluciones en función de su costo acumulado. Utiliza una cola de prioridad para garantizar que los nodos con menor costo sean evaluados primero, asegurando así la exploración óptima del camino más "barato" hasta el momento. Su método `calculo_de_prioridad` simplemente devuelve el costo de la solución sin considerar ninguna heurística adicional.  

3. MODIFICACIÓN DE LA FUNCIÓN DE COSTE

Modificamos la función de coste para que en vez de devolver siempre coste constante 1 devuelva el coste acumulado hasta la nueva solución. El coste acumulado corresponde a la cantidad de nodos atravesados hasta el momento, sumando uno cada nodo nuevo.

4. MODIFICACIÓN DE LA FUNCIÓN DE HEURÍSTICA

Modificamos la función de heurística para que en vez de devolver siempre heurística constante 0, evalue la cantidad de conflictos entre reinas en una solución del problema de las N reinas, contando cuántas parejas de reinas están en la misma fila, columna o diagonal. Si devuelve 0, la solución es válida; si es mayor, indica conflictos. 

5. RESULTADOS:



# AESTRELLA: 
Resolviendo para 4 reinas...
Coordenadas de las reinas: (0, 1), (1, 3), (2, 0), (3, 2)
Nodos evaluados: 86

Resolviendo para 5 reinas...
Coordenadas de las reinas: (0, 0), (1, 3), (2, 1), (3, 4), (4, 2)
Nodos evaluados: 782

Resolviendo para 6 reinas...
Coordenadas de las reinas: (0, 4), (1, 2), (2, 0), (3, 5), (4, 3), (5, 1)
Nodos evaluados: 9332

# BUSQUEDA AVARA:
Resolviendo para 4 reinas...
Coordenadas de las reinas: (0, 1), (1, 3), (2, 0), (3, 2)
Nodos evaluados: 16

Resolviendo para 5 reinas...
Coordenadas de las reinas: (0, 1), (1, 4), (2, 2), (3, 0), (4, 3)
Nodos evaluados: 19

Resolviendo para 6 reinas...
Coordenadas de las reinas: (0, 1), (1, 3), (2, 5), (3, 0), (4, 2), (5, 4)
Nodos evaluados: 93

Resolviendo para 7 reinas...
Coordenadas de las reinas: (0, 0), (1, 3), (2, 6), (3, 2), (4, 5), (5, 1), (6, 4)
Nodos evaluados: 117

Resolviendo para 8 reinas...
Coordenadas de las reinas: (0, 2), (1, 0), (2, 6), (3, 4), (4, 7), (5, 1), (6, 3), (7, 5)
Nodos evaluados: 310

Resolviendo para 9 reinas...
Coordenadas de las reinas: (0, 6), (1, 0), (2, 5), (3, 1), (4, 4), (5, 7), (6, 3), (7, 8), (8, 2)
Nodos evaluados: 830

Resolviendo para 10 reinas...
Coordenadas de las reinas: (0, 5), (1, 1), (2, 6), (3, 4), (4, 0), (5, 8), (6, 3), (7, 9), (8, 7), (9, 2)
Nodos evaluados: 585

Resolviendo para 11 reinas...
Coordenadas de las reinas: (0, 1), (1, 8), (2, 5), (3, 2), (4, 9), (5, 3), (6, 10), (7, 7), (8, 4), (9, 6), (10, 0)
Nodos evaluados: 8476

# COSTE UNIFORME:
Resolviendo para 4 reinas...
Coordenadas de las reinas: (0, 1), (1, 3), (2, 0), (3, 2)
Nodos evaluados: 134

Resolviendo para 5 reinas...
Coordenadas de las reinas: (0, 2), (1, 4), (2, 1), (3, 3), (4, 0)
Nodos evaluados: 1065

Resolviendo para 6 reinas...
Coordenadas de las reinas: (0, 4), (1, 2), (2, 0), (3, 5), (4, 3), (5, 1)
Nodos evaluados: 14038