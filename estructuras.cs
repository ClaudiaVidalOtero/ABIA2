// Claudia Vidal Otero
// Aldana Smyna Medina Lostaunau
using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using Ejecutable;
using Algoritmos;


namespace Estructuras
{
    /// <summary>
    /// Representa una solución con coordenadas y un costo asociado.     
    /// </summary>
    public class Solucion
    
    {
        /// <summary> Lista de coordenadas que componen la solución. </summary>
        public List<(int, int)> Coords { get; private set; }
        /// <summary> Coste de la solución. </summary>
        public int Coste { get; set; }

        /// <summary> Constructor de la solución. </summary>
        /// <param name="coords"> Lista de coordenadas. </param>
        /// <param name="coste"> Coste de la solución. </param>
        public Solucion(List<(int, int)> coords, int coste)
        {
            Coords = coords;
            Coste = coste;
        }
        /// <summary>
        /// Compara si dos soluciones son iguales en base a sus coordenadas.
        /// </summary>
        /// <returns>
        /// True si ambas soluciones tienen las mismas coordenadas. De lo contrario, false.
        /// </returns>
        /// <param name="other">La otra solución a comparar.</param>

        public bool Equals(Solucion other)
        {
            if (other == null) return false;
            return string.Join("-", Coords) == string.Join("-", other.Coords);
        }
        /// <summary>
        /// Compara dos soluciones según su costo.
        /// </summary>
        /// <returns>
        /// -1 si esta solución es mejor (menor costo), 1 si es peor, 0 si son iguales.
        /// </returns>
        /// <param name="other">La otra solución a comparar.</param>

        public int CompareTo(Solucion other)
        {
            if (other == null) return 1;
            return Coste < other.Coste ? -1 : (Coste > other.Coste ? 1 : 0);
        }
        /// <summary>
        /// Devuelve una representación en cadena de la solución (sus coordenadas unidas por "-").
        /// </summary>
        public override string ToString()
        {
            return string.Join("-", Coords);
        }
    }

    /// <summary>
    /// Clase base para estructuras de candidatos, utilizada para almacenar soluciones.
    /// </summary>
    public class ListaCandidatos
    {
        public virtual void anhadir(Solucion solucion, int prioridad = 0) {}
        public virtual void borrar(Solucion solucion) {}
        public virtual Solucion? obtener_siguiente() {throw new NotImplementedException();}
        public virtual int __len__ => 0;
    }

    /// <summary>
    /// Implementación de una cola de prioridad para gestionar soluciones ordenadas por prioridad.
    /// </summary>
    public class ColaDePrioridad : ListaCandidatos
    {
        private readonly List<(int prioridad, Solucion solucion)> cp;
        private readonly Dictionary<string, (int prioridad, Solucion solucion)> buscador;
        private const int REMOVED = 9999999;   // coste solo acepta int
        
        /// <summary> Constructor de la cola de prioridad. </summary>
        public ColaDePrioridad()
        {
            cp = new List<(int prioridad, Solucion solucion)>();
            buscador = new Dictionary<string, (int prioridad, Solucion solucion)>();
        }

        /// <summary>
        /// Agrega una solución a la cola de prioridad.
        /// Si la solución ya existe con menor prioridad, se actualiza.
        /// </summary>
        /// <param name="solucion">Solución a agregar.</param>
        /// <param name="prioridad">Prioridad de la solución.</param>
        public override void anhadir(Solucion solucion, int prioridad = 0)
        {
            string str_solucion = solucion.ToString(); // Obtiene la representación en string de la solución
            // Si la solución ya existe con una prioridad igual o menor, no se añade
            if (buscador.ContainsKey(str_solucion))
            {
                (int prioridad, Solucion solucion) solucionBuscador = buscador[str_solucion];
                if (solucionBuscador.prioridad <= prioridad)
                    return;
                borrar(solucion); // Si la nueva prioridad es menor, se borra la antigua
            }
            // Se añade la nueva solución con su prioridad
            (int prioridad, Solucion solucion) entrada = (prioridad, solucion);
            buscador[str_solucion] = entrada;
            cp.Add(entrada);
            // Se ordena la lista de la cola por prioridad ascendente
            cp.Sort((a, b) => a.prioridad.CompareTo(b.prioridad));              
        }
        
        /// <summary>
        /// Marca una solución como eliminada de la cola de prioridad.
        /// </summary>
        /// <param name="solucion"> Solución a eliminar. </param>
        public override void borrar(Solucion solucion)
        {
            string str_solucion = solucion.ToString();
            
            if (buscador.ContainsKey(str_solucion))
            {
                (int prioridad, Solucion solucion) entrada = buscador[str_solucion];
                buscador.Remove(str_solucion);  

                entrada.Item2.Coste = REMOVED;                           
            }
        }

        /// <summary>
        /// Obtiene la siguiente solución con mayor prioridad.
        /// </summary>
        /// <returns>Solución con mayor prioridad.</returns>
        /// <exception cref="InvalidOperationException"> Se lanza si la cola está vacía. </exception>
        public override Solucion obtener_siguiente()
        {
            while (cp.Count > 0)
            {
                (int prioridad, Solucion solucion) = cp[0]; // Se obtiene el primer elemento de la cola     
                cp.RemoveAt(0); // Se elimina de la lista
                // Si la solución no ha sido marcada como eliminada, se devuelve
                if (solucion.Coste != REMOVED)
                {
                    buscador.Remove(solucion.ToString()); // Se elimina del diccionario
                    return solucion;
                }
            }
            throw new InvalidOperationException("No hay siguiente en una cola de prioridad vacía");
        }
        /// <summary> Obtiene la cantidad de elementos en la cola de prioridad. </summary>
        public override int __len__ => buscador.Count;
    }

    /// <summary>
    /// Implementación de una pila (LIFO) para gestionar soluciones.
    /// </summary>
    public class PilaCandidatos : ListaCandidatos
    {
        private List<Solucion> pila; // Lista interna que actúa como pila.

        /// <summary> Constructor de la pila de candidatos. </summary>
        public PilaCandidatos()
        {
            pila = new List<Solucion>();
        }
        /// <summary>
        /// Agrega una solución a la pila.
        /// La solución más reciente se encuentra en la parte superior.
        /// </summary>
        /// <param name="solucion">Solución a agregar.</param>
        /// <param name="prioridad">Este parámetro no se usa en la pila.</param>
        public override void anhadir(Solucion solucion, int prioridad = 0)
        {
            pila.Add(solucion);  // Se agrega al final de la lista, manteniendo el comportamiento de pila (LIFO)
        }

        /// <summary> Elimina el último elemento de la pila. </summary>
        /// <param name="solucion">Solución a eliminar.</param>
        public override void borrar(Solucion solucion)
        {
            // Se recorre la lista desde el final (últimos elementos primero)
            for (int i = pila.Count - 1; i >= 0; i--)
            {
                if (pila[i].Equals(solucion))
                {
                    pila.RemoveAt(i);
                    break; // Solo elimina la última instancia encontrada (LIFO)
                }
            }
        }


        /// <summary> Extrae y devuelve la última solución añadida (tope de la pila) </summary>
        /// <returns> La solución en la cima de la pila </returns>
        /// <exception cref="InvalidOperationException">Se lanza si la pila está vacía.</exception>
        public override Solucion obtener_siguiente()
        {
            if (!esta_vacia())
            {
                Solucion solucion = pila[^1]; // Último elemento (top)
                pila.RemoveAt(pila.Count - 1);
                return solucion;
            }
            throw new InvalidOperationException("La pila está vacía.");
        }

        /// <summary> Verifica si la pila está vacía </summary>
        public bool esta_vacia()
        {
            return pila.Count == 0;
        }

        /// <summary> Obtiene la cantidad de elementos en la pila. </summary>
        public override int __len__ => pila.Count;
    }

    /// <summary> Implementación de una cola (FIFO) para gestionar soluciones </summary>
    public class ColaCandidatos : ListaCandidatos
    {
        private Queue<Solucion> cola; // Cola interna basada en la estructura Queue

        /// <summary> Constructor de la cola de candidatos. </summary>
        public ColaCandidatos()
        {
            cola = new Queue<Solucion>();
        }

        /// <summary>
        /// Agrega una solución a la cola.
        /// La primera solución añadida será la primera en salir.
        /// </summary>
        /// <param name="solucion">Solución a agregar </param>
        /// <param name="prioridad">Este parámetro no se usa en la cola.</param>
        public override void anhadir(Solucion solucion, int prioridad = 0)
        {
            cola.Enqueue(solucion);  // Solo agregamos la solución
        }

        /// <summary>
        /// Elimina todas las apariciones de una solución en la cola.
        /// </summary>
        /// <param name="solucion">Solución a eliminar.</param>
        public override void borrar(Solucion solucion)
        {
            // Se filtran las soluciones diferentes a la que queremos borrar.
            cola = new Queue<Solucion>(cola.Where(s => !s.Equals(solucion)));
        }

        /// <summary>
        /// Extrae y devuelve la primera solución añadida a la cola.
        /// </summary>
        /// <returns>La solución al frente de la cola </returns>
        /// <exception cref="InvalidOperationException">Se lanza si la cola está vacía.</exception>
        public override Solucion obtener_siguiente()
        {
            if (!esta_vacia())
            {
                return cola.Dequeue(); 
            }
            throw new InvalidOperationException("La cola está vacía.");
        }

        /// <summary> Verifica si la cola está vacía </summary>
        public bool esta_vacia()
        {
            return cola.Count == 0;
        }

        /// <summary> Obtiene la cantidad de elementos en la cola. </summary>
        public override int __len__ => cola.Count;
    }

}
