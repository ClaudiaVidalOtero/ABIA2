// Claudia Vidal Otero
// Aldana Smyna Medina Lostaunau
using System;
using System.Collections.Generic;
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

        public bool __eq__(Solucion other)
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

        public int __lt__(Solucion other)
        {
            if (other == null) return 1;
            return Coste < other.Coste ? -1 : (Coste > other.Coste ? 1 : 0);
        }
        /// <summary>
        /// Devuelve una representación en cadena de la solución (sus coordenadas unidas por "-").
        /// </summary>
        public string __str__()
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
        private const int REMOVED = -1; // -1, const solo acepta int
        
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
            string str_solucion = solucion.__str__(); // Obtiene la representación en string de la solución
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
            string str_solucion = solucion.__str__();
            
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
                    buscador.Remove(solucion.__str__()); // Se elimina del diccionario
                    return solucion;
                }
            }
            throw new InvalidOperationException("No hay siguiente en una cola de prioridad vacía");
        }
        /// <summary> Obtiene la cantidad de elementos en la cola de prioridad. </summary>
        public override int __len__ => buscador.Count;
    }

}
