
using System;
using System.Collections.Generic;
using System.Linq;
using Estructuras;
using Ejecutable;

namespace Algoritmos
{

    class AlgoritmoDeBusqueda
    {
        public required ListaCandidatos listaCandidatos;

        public AlgoritmoDeBusqueda(ListaCandidatos lista)     //!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!! get p set
        {
            listaCandidatos = lista;
        }

        public int calculo_de_prioridad(Solucion solucion, Func<Solucion, int>? calculoHeuristica = null)
        {
            return 0; 
        }

        public (Solucion, int)? Busqueda(
            Solucion solucion_inicial,
            Func<Solucion, bool> criterio_parada,
            Func<Solucion, List<(int, int)>> obtener_vecinos,
            Func<Solucion, Solucion, int> calculo_coste,
            Func<Solucion, int>? calculo_heuristica = null)
        {
            ListaCandidatos candidatos = listaCandidatos;
            candidatos.anhadir(new Estructuras.Solucion(solucion_inicial.Coords, 0));               ///revisar si es col_ini.coords o solo sol_ini

            Dictionary<string, int> vistos = new Dictionary<string, int>();         //TIPOS CORRECTOS?
            bool finalizado = false;
            int revisados = 0;
            Solucion? solucion = null;  //  Declaro la variable fuera del bucle para poder usarla en el return

            while (candidatos.__len__ > 0 && !finalizado)
            {
                solucion = candidatos.obtener_siguiente();
                vistos[solucion.__str__()] = solucion.Coste;                        // MIRAR ESTO DE STR, EN PYTHON SE USA LA FUNCION EXISTENTE NO LA CREADA
                revisados++;

                if (criterio_parada(solucion))
                {
                    finalizado = true;
                    break;
                }

                List<(int, int)> vecinos = obtener_vecinos(solucion);
                foreach ((int, int) vecino in vecinos)
                {
                    List<(int, int)> nuevas_coordenadas = solucion.Coords.ToList();     // Se copia la lista
                    nuevas_coordenadas.Add(vecino);                                     // Se modifica solo la copia    REVISAR ESTA LÓGICA

                    Solucion nueva_solucion = new Solucion(nuevas_coordenadas, 0);
                
                    if (!vistos.ContainsKey(nueva_solucion.__str__()))
                    {
                        nueva_solucion.Coste = solucion.Coste + calculo_coste(solucion, nueva_solucion);
                        int prioridad = calculo_de_prioridad(nueva_solucion, calculo_heuristica);
                        candidatos.anhadir(nueva_solucion, prioridad);
                    }
                }
            }

            if (!finalizado)
            {
                return null;
            }

            return (solucion!, revisados);
            
        }

        
    }

    // ESTA ESTÁ BIEN
    class AEstrella : AlgoritmoDeBusqueda
    {
        public AEstrella(ListaCandidatos lista) : base(lista) {}

        // Método que calcula la prioridad usando la función externa
        public int calculo_de_prioridad(Solucion solucion)
        {
            return solucion.Coste + calculo_heuristica(solucion);
        }
    }

}
