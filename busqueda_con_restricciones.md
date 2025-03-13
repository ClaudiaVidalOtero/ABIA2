**TRABAJO REALIZADO**
## SEMANA 3: BÚSQUEDA CON RESTRICCIONES

1. FUNCIÓN DE CONSISTENCIA:
La función `es_consistente` verifica si una solución del problema de las N reinas es válida. Recorre todas las coordenadas de las reinas en la solución y compara cada pareja para asegurarse de que no estén en la misma fila, columna o diagonal. Si encuentra dos reinas que se atacan mutuamente según estas reglas, devuelve false, indicando una solución inconsistente. Si ninguna reina se ataca, devuelve true, confirmando que la solución es válida.

La ejecución del algoritmo AEStrella con esta función es la siguiente:

Resolviendo para 4 reinas...
Coordenadas de las reinas: (0, 1), (1, 3), (2, 0), (3, 2)
Nodos evaluados: 16

Resolviendo para 5 reinas...
Coordenadas de las reinas: (0, 0), (1, 2), (2, 4), (3, 1), (4, 3)
Nodos evaluados: 45

Resolviendo para 6 reinas...
Coordenadas de las reinas: (0, 1), (1, 3), (2, 5), (3, 0), (4, 2), (5, 4)
Nodos evaluados: 81

Resolviendo para 7 reinas...
Coordenadas de las reinas: (0, 2), (1, 6), (2, 1), (3, 3), (4, 5), (5, 0), (6, 4)
Nodos evaluados: 90

Resolviendo para 8 reinas...
Coordenadas de las reinas: (0, 6), (1, 3), (2, 1), (3, 7), (4, 5), (5, 0), (6, 2), (7, 4)
Nodos evaluados: 547

Resolviendo para 9 reinas...
Coordenadas de las reinas: (0, 0), (1, 6), (2, 3), (3, 7), (4, 2), (5, 4), (6, 8), (7, 1), (8, 5)
Nodos evaluados: 1004

Resolviendo para 10 reinas...
Coordenadas de las reinas: (0, 6), (1, 4), (2, 0), (3, 7), (4, 5), (5, 2), (6, 8), (7, 1), (8, 3), (9, 9)
Nodos evaluados: 1634 
   


2. MODIFICACIÓN DE LA FUNCIÓN OBTENER VECINOS:

La función `obtener_vecinos` genera posibles posiciones para colocar la siguiente reina en el problema de las N reinas, evitando aceptar vecinos que creen soluciones inconsistentes. Para cada columna en la fila siguiente, crea una solución provisional con la nueva posición y usa la función `es_consistente` para verificar si la nueva posición es válida. Si la solución provisional no genera conflictos entre reinas, se agrega a la lista de vecinos, si no, se descarta, asegurando que solo se consideren movimientos válidos.

La ejecución del algoritmo AEStrella con esta función mejorada es la siguiente:

Resolviendo para 4 reinas...
Coordenadas de las reinas: (0, 1), (1, 3), (2, 0), (3, 2)
Nodos evaluados: 16

Resolviendo para 5 reinas...
Coordenadas de las reinas: (0, 0), (1, 2), (2, 4), (3, 1), (4, 3)
Nodos evaluados: 45

Resolviendo para 6 reinas...
Coordenadas de las reinas: (0, 1), (1, 3), (2, 5), (3, 0), (4, 2), (5, 4)
Nodos evaluados: 81

Resolviendo para 7 reinas...
Coordenadas de las reinas: (0, 2), (1, 6), (2, 1), (3, 3), (4, 5), (5, 0), (6, 4)
Nodos evaluados: 90

Resolviendo para 8 reinas...
Coordenadas de las reinas: (0, 6), (1, 3), (2, 1), (3, 7), (4, 5), (5, 0), (6, 2), (7, 4)
Nodos evaluados: 547

Resolviendo para 9 reinas...
Coordenadas de las reinas: (0, 0), (1, 6), (2, 3), (3, 7), (4, 2), (5, 4), (6, 8), (7, 1), (8, 5)
Nodos evaluados: 1004

Resolviendo para 10 reinas...
Coordenadas de las reinas: (0, 6), (1, 4), (2, 0), (3, 7), (4, 5), (5, 2), (6, 8), (7, 1), (8, 3), (9, 9)
Nodos evaluados: 1634 

3. EJECUCIÓN CON REINAS PREFIJADAS:

Para lograr esta ejecución, en lugar de iniciar con una solución vacía, inicializamos `nodo_inicial` con las coordenadas de las reinas prefijadas. Luego, ejecutamos el código comenzando con reinas = 4 e incrementamos el valor hasta que el número de nodos evaluados supere 1500, registrando dicho número para cada caso. 

La ejecución del algoritmo AEstrella con la funcion consistencia y reinas prefijadas es la siguiente: 

Resolviendo para 4 reinas...
Coordenadas de las reinas: (0, 1), (1, 3), (2, 0), (3, 2)
Nodos evaluados: 12

Resolviendo para 5 reinas...
Coordenadas de las reinas: (0, 0), (1, 3), (2, 1), (3, 4), (4, 2)
Nodos evaluados: 34

Resolviendo para 6 reinas...
Coordenadas de las reinas: (0, 4), (1, 2), (2, 0), (3, 5), (4, 3), (5, 1)
Nodos evaluados: 69

Resolviendo para 7 reinas...
Coordenadas de las reinas: (0, 4), (1, 1), (2, 5), (3, 2), (4, 6), (5, 3), (6, 0)
Nodos evaluados: 104

Resolviendo para 8 reinas...
Coordenadas de las reinas: (0, 4), (1, 6), (2, 1), (3, 3), (4, 7), (5, 0), (6, 2), (7, 5)
Nodos evaluados: 189

Resolviendo para 9 reinas...
Coordenadas de las reinas: (0, 2), (1, 0), (2, 7), (3, 3), (4, 8), (5, 6), (6, 4), (7, 1), (8, 5)
Nodos evaluados: 551

Resolviendo para 10 reinas...
Coordenadas de las reinas: (0, 0), (1, 6), (2, 8), (3, 1), (4, 7), (5, 4), (6, 2), (7, 9), (8, 5), (9, 3)
Nodos evaluados: 3278









