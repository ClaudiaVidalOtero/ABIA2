/// Claudia Vidal Otero
/// Aldana Smyna Medina Lostaunau
using System;
using System.Collections.Generic;
using System.Linq;
using Estructuras;
using Ejecutable;

namespace Algoritmos
{

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
        /// <param name="calculo_heuristica"> Función heurística opcional. </param>
        /// <returns> Un valor entero que representa la prioridad. </returns>
        public virtual int calculo_de_prioridad(Solucion solucion, Func<Solucion, int>? calculo_heuristica = null)
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
                vistos[solucion.ToString()] = solucion.Coste; // Se marca como visitada la solución actual                     
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
                    if (!vistos.ContainsKey(nueva_solucion.ToString()))
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
        public AEstrella(ColaDePrioridad lista) : base(lista) {}
        
        /// <summary>
        /// Calcula la prioridad de una solución considerando su coste y heuristica.
        /// </summary>
        /// <param name="solucion"> Solución a evaluar. </param>
        /// <param name="calculo_heuristica"> Funcion heurística opcional. </param>
        /// <returns> Prioridad basada en coste más heurística. </returns>
        public override int calculo_de_prioridad(Solucion solucion, Func<Solucion, int>? calculo_heuristica=null)
        {
            return solucion.Coste + (calculo_heuristica != null ? calculo_heuristica(solucion) : 0);
        }
    }
    /// <summary>
    /// Implementación del algortimo de Búsqueda por profundidad
    /// </summary>
    public class BusquedaProfundidad : AlgoritmoDeBusqueda
    {
        /// <summary>
        /// Constructor de BusquedaProfundidad que inicializa la pila de candidatos.
        /// </summary>
        /// <param name="pila">Pila de candidatos utilizada.</param>
        public BusquedaProfundidad(PilaCandidatos pila) : base(new PilaCandidatos()) { }
    }
    /// <summary>
    /// Implementación del algortimo de Búsqueda por anchura
    /// </summary>
    public class BusquedaPorAnchura : AlgoritmoDeBusqueda
    {
        /// <summary>
        /// Constructor de BusquedaPorAnchura que inicializa la cola de candidatos.
        /// </summary>
        /// <param name="cola">Cola de candidatos utilizada.</param>
        public BusquedaPorAnchura(ColaCandidatos cola) : base(new ColaCandidatos()) { }
    }
        // Clase CosteUniforme
    public class CosteUniforme : AlgoritmoDeBusqueda
    {
        /// <summary>
        /// Constructor de CosteUniforme que inicializa el algoritmo con una cola de prioridad.
        /// </summary>
        public CosteUniforme(ColaDePrioridad lista) : base(lista) {}
        /// <summary>
        /// Calcula la prioridad de una solución en Coste Uniforme, que es únicamente su coste.
        /// </summary>
        /// <param name="solucion">Solución a evaluar.</param>
        /// <param name="calculo_heuristica">Función heurística opcional (no utilizada en este caso).</param>
        /// <returns>El coste de la solución.</returns>
        public override int calculo_de_prioridad(Solucion solucion, Func<Solucion, int>? calculo_heuristica = null)
        {
            solucion.Coste = 1; // Coste uniforme siempre es 1
            return solucion.Coste;
        }
    }

    // Clase BusquedaAvara
    class BusquedaAvara : AlgoritmoDeBusqueda
    {
        /// <summary>
        /// Constructor de BusquedaAvara que inicializa el algoritmo con una cola de prioridad.
        /// </summary>
        public BusquedaAvara(ColaDePrioridad lista) : base(lista) {}
        /// <summary>
        /// Calcula la prioridad de una solución en Búsqueda Avara, que es únicamente la heurística.
        /// </summary>
        /// <param name="solucion">Solución a evaluar.</param>
        /// <param name="calculo_heuristica">Función heurística proporcionada.</param>
        /// <returns>El valor heurístico de la solución.</returns>
        public override int calculo_de_prioridad(Solucion solucion,Func<Solucion, int>? calculo_heuristica = null)
        {
            return calculo_heuristica != null ? calculo_heuristica(solucion) : 0;
        }
    }
}
