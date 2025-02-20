
**TRABAJO REALIZADO**
## SEMANA 1
En esta semana de trabajo, hemos completado las siguientes tareas:

### **1. División del código en tres ficheros**
El código `n_reinas` se ha dividido en tres ficheros:
- **Fichero 1**: Algoritmo de búsqueda
- **Fichero 2**: Código ejecutable
- **Fichero 3**: Estructuras de almacenamiento de datos

Permitiendo una mejor organización del código.

### **2. Conversión del código de Python a C#**
El código original en Python fue convertido a C#, asegurando que la funcionalidad sea exacatamente la misma, con el mismo nombre de métodos y clases.

- Durante la conversión, añadimos comentarios en las líneas que nos parecieron más importantes.

### **3. Uso de comentarios XML en C#**
Finalmente, para garantizar que el código sea comprensible y fácil de mantener, utilizamos el formato de comentarios XML en C#. Este formato permite generar documentación automáticamente y facilita la comprensión para otros programadores del propósito de cada clase y método.

## **Preguntas**

### 1. ¿Cuál es el estado inicial?
solucion_inicial = [] 
El estado inicial es una lista vacía que representa un tablero vacío, no hay reinas colocadas.

### 2. ¿Dado un estado, cómo se escogen los vecinos?
Los vecinos se eligen considerando las posiciones posibles para colocar la siguiente reina en la fila siguiente a la última reina colocada. La función `obtener_vecinos(solucion)` genera todas las posiciones válidas en la fila siguiente y las devuelve como una lista de posibles configuraciones para los vecinos.

### 3. ¿Cuándo finaliza el algoritmo?
Cuando todas las reinas se han colocado correctamente en el tablero sin que se amenacen entre sí, especificado en la función `criterio_parada`. 

### 4. ¿Qué utilidad tienen las variables incluídas en la clase ColaDePrioridad?
- `cp` (cola de prioridad): Esta cola asegura que siempre se extraiga la solucion con la menor prioridad.
- `buscador` (diccionario): Mapea las representaciones de las soluciones (en cadenas de texto) a sus entradas en la cola de prioridad.
- REMOVED: Eliminación correcta de soluciones de la cola de prioridad, asegurando que no se exploren soluciones ya vistas.
