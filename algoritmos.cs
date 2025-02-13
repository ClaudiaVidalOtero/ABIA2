# This is a sample Python script.

# Press ⌃R to execute it or replace it with your code.
# Press Double ⇧ to search everywhere for classes, files, tool windows, actions, and settings.
import heapq
using System;

REMOVED = '<removed-task>'  # placeholder for a removed task

//EN C #
class Solucion
{
    private List<string> Coords;
    private int Coste;

    public Solucion(List<string> coords, int coste)
    {
        Coords = coords;
        Coste = coste; 
    }
    
// HASTA AQUI, NI IDEA DE COMO PASAR ESTAS
    def __eq__(self, other):
        return str(self.coords) == str(other.coords)

    def __lt__(self, other):
        return str(self.coste) < str(other.coste)

    def __str__(self):
        return '-'.join(str(x) for x in self.coords)

}


class AlgoritmoDeBusqueda
{
    private ListaCandidatos Lista;  // NO VA PQ HAY QUE HACER UN NAMESPACE PARA PODER LLAMAR A LISTA DE CANDIDATOS

    public AlgoritmoDeBusqueda(lista)
    {
        Lista = lista;
    }

    // ESTAS FUNCIONES NO LAS SÉ PASAR A C#
    def calculo_de_prioridad(self, nodo_info, calculo_heuristica=None):
        return 0

    def busqueda(self, solucion_inicial, criterio_parada, obtener_vecinos, calculo_coste, calculo_heuristica=None):
        candidatos = self.lista()
        candidatos.anhadir(Solucion(solucion_inicial, 0))
        vistos = dict()
        finalizado = False
        revisados = 0
        while len(candidatos) > 0 and not finalizado:
            solucion = candidatos.obtener_siguiente()
            vistos[str(solucion)] = solucion.coste
            revisados += 1
            if criterio_parada(solucion):
                finalizado = True
                break
            vecinos = obtener_vecinos(solucion)
            for vecino in vecinos:
                nueva_solucion = Solucion(
                    coords=solucion.coords + [vecino]
                )
                if str(nueva_solucion) not in vistos:
                    nueva_solucion.coste = solucion.coste+calculo_coste(solucion, nueva_solucion)
                    candidatos.anhadir(nueva_solucion, prioridad=self.calculo_de_prioridad(nueva_solucion, calculo_heuristica))
        if not finalizado:
            return None
        return solucion, revisados

}

// ESTO ESTÁ EN C#
class AEstrella:AlgoritmoDeBusqueda
{
    public AEstrella() : base() {}

    //ESTO NO ESTÁ EN C#
    def calculo_de_prioridad(self, solucion, calculo_heuristica=None):
        return solucion.coste + calculo_heuristica(solucion)
}


