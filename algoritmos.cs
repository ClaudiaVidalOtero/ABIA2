
using System;
using System.Collections.Generic;
using System.Linq;
using Estructuras;
using Ejecutable;

namespace Algoritmos
{

    public class AlgoritmoDeBusqueda
    {
        public ListaCandidatos ListaCandidatos { get; set; }

        public AlgoritmoDeBusqueda(ListaCandidatos lista)
        {
            ListaCandidatos = lista;
        }

        // Calcula la prioridad; si se proporciona una función heurística, se usa
        public virtual int CalcularPrioridad(Solucion solucion, Func<Solucion, int>? calculoHeuristica = null)
        {
            if (calculoHeuristica != null)
                return solucion.Coste + calculoHeuristica(solucion);
            return solucion.Coste;
        }

        // Método de búsqueda
        public (Solucion, int)? Buscar(
            Solucion solucionInicial,
            Func<Solucion, bool> criterioParada,
            Func<Solucion, List<(int, int)>> obtenerVecinos,
            Func<Solucion, Solucion, int> calculoCoste,
            Func<Solucion, int>? calculoHeuristica = null)
        {
            ListaCandidatos.anhadir(new Solucion(solucionInicial.Coords, 0));

            Dictionary<string, int> vistos = new Dictionary<string, int>();
            int nodosEvaluados = 0;
            Solucion? solucionActual = null;
            bool encontrado = false;

            while (ListaCandidatos.__len__ > 0 && !encontrado)
            {
                solucionActual = ListaCandidatos.obtener_siguiente();
                vistos[solucionActual.ToString()] = solucionActual.Coste;
                nodosEvaluados++;

                if (criterioParada(solucionActual))
                {
                    encontrado = true;
                    break;
                }

                List<(int, int)> vecinos = obtenerVecinos(solucionActual);
                foreach ((int, int) vecino in vecinos)
                {
                    // Crear una copia de las coordenadas y agregar el vecino
                    List<(int, int)> nuevasCoords = solucionActual.Coords.ToList();
                    nuevasCoords.Add(vecino);
                    Solucion nueva_solucion = new Solucion(nuevasCoords, 0);

                    if (!vistos.ContainsKey(nueva_solucion.ToString()))
                    {
                        nueva_solucion.Coste = solucionActual.Coste + calculoCoste(solucionActual, nueva_solucion);
                        int prioridad = CalcularPrioridad(nueva_solucion, calculoHeuristica);
                        ListaCandidatos.anhadir(nueva_solucion, prioridad);
                    }
                }
            }

            if (!encontrado)
                return null;

            return (solucionActual!, nodosEvaluados);
        }
    }

    public class AEstrella : AlgoritmoDeBusqueda
    {
        public AEstrella(ListaCandidatos lista) : base(lista) { }

        // Se puede sobrescribir el cálculo de prioridad si se desea
        public override int CalcularPrioridad(Solucion solucion, Func<Solucion, int>? calculoHeuristica = null)
        {
            if (calculoHeuristica != null)
                return solucion.Coste + calculoHeuristica(solucion);
            return solucion.Coste;
        }
    }
}
