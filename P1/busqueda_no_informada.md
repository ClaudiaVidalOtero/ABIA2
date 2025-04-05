**TRABAJO REALIZADO**
## SEMANA 2:
BUSQUEDA POR ANCHURA:

Para implementar la búsqueda por anchura tenemos dos soluciones posibles:

- La primera solución sería ejecutar el algoritmo AEstrella, asegurándonos de que 
la función de cálculo de coste devuelva 1 y  la función de cálculo de heurística devuelva siempre 0.

- La segunda solución consiste en implementar en el código una subclase BusquedaPorAnchura() 
que herede de la clase AlgoritmoDeBusqueda y en vez de usar la clase ListaCandidatos, o sea, 
una lista para guardar sus candidatos, utilice una nueva subclase de ListaCandidatos llamada ColaCandidatos, o sea, una cola.

Esta opción funciona gracias a la forma de guardar los datos de una cola, en la que el primero en entra es el primero en salir, ordenando los nodos de menos a más reciente.

BUSQUEDA POR PROFUNDIDAD:
Para implementar la búsqueda por profundidad tenemos también dos posibles soluciones:

- La primera solución sería ejecutar el algoritmo AEstrella, asegurándonos de que la función de cálculo de coste devuelva -1 y  la función de cálculo de heurística devuelva siempre 0.

- La segunda solución consiste en implementar en el código una subclase BusquedaProfundidad() que herede de la clase AlgoritmoDeBusqueda y en vez de usar la clase ListaCandidatos, o sea, una lista para guardar sus candidatos, utilice una nueva subclase de ListaCandidatos llamada PilaCandidatos, es decir una pila.
    
Esta segunda opción funciona debido a la forma de almacenamiento de datos de una pila, en la cual el primero en entrar es el primero en salir y los nodos se ordenan de más a menos reciente.
    
De los dos métodos, (cambiar simplemente el coste o implementar cola y pila para los algoritmos) deberíamos implementar la segunda opción, ya que proporcionan una visión más clara de la búsqueda en sí, y son más sencillas de entender.

En el caso de BusquedaProfundidad utilizar una pila para almacenar los candidatos se ajusta mejor al comportamiento del algoritmo, donde se explora primero todos los nodos a una profundidad dada antes de moverse a la siguiente profundidad.

En el caso de BusquedaPorAnchura, utilizar una cola para almacenar los candidatos refleja mejor el comportamiento de la búsqueda por anchura, donde se exploran primero todos los nodos en un nivel antes de moverse al siguiente nivel.


## NODOS EVALUADOS SEGÚN NÚMERO DE REINAS Y ALGORITMO

BUSQUEDA POR ANCHURA:

Resolviendo para 4 reinas...
Coordenadas de las reinas: (0, 1), (1, 3), (2, 0), (3, 2)
Nodos evaluados: 200

Resolviendo para 5 reinas...
Coordenadas de las reinas: (0, 0), (1, 2), (2, 4), (3, 1), (4, 3)
Nodos evaluados: 1140

Resolviendo para 6 reinas...
Coordenadas de las reinas: (0, 1), (1, 3), (2, 5), (3, 0), (4, 2), (5, 4)
Nodos evaluados: 22092

Resolviendo para 7 reinas...
Coordenadas de las reinas: (0, 0), (1, 2), (2, 4), (3, 6), (4, 1), (5, 3), (6, 5)
Nodos evaluados: 182609

Resolviendo para 8 reinas...
Coordenadas de las reinas: (0, 0), (1, 4), (2, 7), (3, 5), (4, 2), (5, 6), (6, 1), (7, 3)
Nodos evaluados: 3696597


BUSQUEDA POR PROFUNDIDAD:

Resolviendo para 4 reinas...
Coordenadas de las reinas: (0, 2), (1, 0), (2, 3), (3, 1)
Nodos evaluados: 155

Resolviendo para 5 reinas...
Coordenadas de las reinas: (0, 4), (1, 2), (2, 0), (3, 3), (4, 1)
Nodos evaluados: 451

Resolviendo para 6 reinas...
Coordenadas de las reinas: (0, 4), (1, 2), (2, 0), (3, 5), (4, 3), (5, 1)
Nodos evaluados: 15316

Resolviendo para 7 reinas...
Coordenadas de las reinas: (0, 6), (1, 4), (2, 2), (3, 0), (4, 5), (5, 3), (6, 1)
Nodos evaluados: 52914

Resolviendo para 8 reinas...
Coordenadas de las reinas: (0, 7), (1, 3), (2, 0), (3, 2), (4, 5), (5, 1), (6, 6), (7, 4)
Nodos evaluados: 1485549

A la vista de estos datos se puede observar que el algoritmo de profundidad es el más optimo ya que llega a la solucion óptima explorando menos nodos que por anchura. Esto se debe al backtracking que implementa este algoritmo desapilando todos los nodos visitados siendo más eficiente que el otro.