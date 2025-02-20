/// Claudia Vidal Otero
/// Aldana Smyna Medina Lostaunau
using System;
using System.Collections.Generic;
using System.Linq;
using Estructuras;
using Ejecutable;

namespace Algoritmos
{
    /// <summary>
    /// 
    /// </summary>

    public class AlgoritmoDeBusqueda
    {
        /// <summary>
        /// Lista de candidatos que almacena las soluciones generadas.
        /// </summary>
        public  ListaCandidatos listaCandidatos;

        /// <summary>
        /// Constructor que inicializa el algoritmo con una lista de candidatos.
        /// </summary>
        /// <param name="lista">Lista de candidatos utilizada en la busqueda.</param>
        public AlgoritmoDeBusqueda(ListaCandidatos lista)     
        {
            listaCandidatos = lista;
        }

        /// <summary>
        /// Calcula la prioridad de una solución dada, usando una heurística opcional.
        /// </summary>
        /// <param name="solucion"> Solución a evaluar. </param>
        /// <param name="calculoHeuristica"> Función heurística opcional. </param>
        /// <returns> Un valor entero que representa la prioridad. </returns>
        public int calculo_de_prioridad(Solucion solucion, Func<Solucion, int>? calculoHeuristica = null)
        {
            return 0; 
        }

        /// <summary>
        /// Método principal que ejecuta la busqueda de la solucion optima.
        /// </summary>
        public virtual (Solucion, int)? busqueda(
            Solucion solucion_inicial,
            Func<Solucion, bool> criterio_parada,
            Func<Solucion, List<(int, int)>> obtener_vecinos,
            Func<Solucion, Solucion, int> calculo_coste,
            Func<Solucion, int>? calculo_heuristica = null)
        {
            // Se inicializa la lista de candidatos con la solución inicial
            ListaCandidatos candidatos = listaCandidatos;
            candidatos.anhadir(new Solucion(solucion_inicial.Coords, 0));          
            // Diccionario que almacena soluciones ya exploradas y su coste
            Dictionary<string, int> vistos = new Dictionary<string, int>();        
            bool finalizado = false;
            int revisados = 0;
            Solucion? solucion = null;  // Se declara fuera del bucle para poder devolverla despues

            while (candidatos.__len__ > 0 && !finalizado)
            {
                solucion = candidatos.obtener_siguiente();
                vistos[solucion.__str__()] = solucion.Coste; // Se marca como visitada la solución actual                     
                revisados++;

                // Verifica si la solución cumple el criterio de parada
                if (criterio_parada(solucion))
                {
                    finalizado = true;
                    break;
                }

                // Obtiene los vecinos de la solución actual
                List<(int, int)> vecinos = obtener_vecinos(solucion);
                foreach ((int, int) vecino in vecinos)
                {
                    List<(int, int)> nuevas_coordenadas = solucion.Coords.ToList(); // Se copia la lista
                    nuevas_coordenadas.Add(vecino); // Se agrega la nueva coordenada a la copia  

                    Solucion nueva_solucion = new Solucion(nuevas_coordenadas, 0);

                    // Si la nueva solución no ha sido explorada, se evalúa y añade a la lista
                    if (!vistos.ContainsKey(nueva_solucion.__str__()))
                    {
                        nueva_solucion.Coste = solucion.Coste + calculo_coste(solucion, nueva_solucion);
                        int prioridad = calculo_de_prioridad(nueva_solucion, calculo_heuristica);
                        candidatos.anhadir(nueva_solucion, prioridad);
                    }
                }
            }
            // Si no se encontró una solución válida, devuelve null
            if (!finalizado || solucion == null)
            {
                return null;
            }

            return (solucion!, revisados);   
        }
    }

    /// <summary>
    /// Implementación del algortimo A*
    /// </summary>
    class AEstrella : AlgoritmoDeBusqueda
    {
        /// <summary>
        /// Constructor de A* que inicializa la lista de candidatos.
        /// </summary>
        /// <param name="lista">Lista de candidatos utilizada.</param>
        public AEstrella(ListaCandidatos lista) : base(lista) {}

        /// <summary>
        /// Calcula la prioridad de una solución considerando su coste y heuristica.
        /// </summary>
        /// <param name="solucion"> Solución a evaluar. </param>
        /// <param name="calculo_heuristica"> Funcion heurística opcional. </param>
        /// <returns> Prioridad basada en coste más heurística. </returns>
        public new int calculo_de_prioridad(Solucion solucion, Func<Solucion, int>? calculo_heuristica=null)
        {
            return solucion.Coste + (calculo_heuristica != null ? calculo_heuristica(solucion) : 0);
        }

        

    }

    public class BusquedaProfundidad : AlgoritmoDeBusqueda
    {
        public BusquedaProfundidad(PilaCandidatos pila) : base(new PilaCandidatos()) { }
    }

    public class BusquedaPorAnchura : AlgoritmoDeBusqueda
    {
        public BusquedaPorAnchura(ColaCandidatos cola) : base(new ColaCandidatos()) { }
    }

}
