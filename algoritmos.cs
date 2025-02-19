using System;
using System.Collections.Generic;

//REMOVED = '<removed-task>'  // placeholder for a removed task


// ESTA ESTÁ BIEN 
class Solucion
{
    public List<(int, int)> Coords { get; private set; }
    public int Coste { get; set; }

    public Solucion(List<(int, int)> coords, int coste)
    {
        Coords = coords;
        Coste = coste;
    }

    // Python equivalente __eq__
    public bool __eq__(Solucion otra)
    {
        if (otra == null) return false;
        return string.Join("-", Coords) == string.Join("-", otra.Coords);
    }

    // Python equivalente __lt__ 
    public int __lt__(Solucion otra)
    {
        if (otra == null) return 1;  // Si el otro objeto es null, este objeto es mayor
        return Coste < otra.Coste ? -1 : (Coste > otra.Coste ? 1 : 0);
    }

    // Python equivalente __str__
    public string __str__()
    {
        return string.Join("-", Coords);
    }
}


// SIN ACABAR
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

    public (Solucion, int)? Busqueda(Solucion solucionInicial, Func<Solucion, bool> criterioParada, 
                                      Func<Solucion, List<string>> obtenerVecinos, 
                                      Func<Solucion, Solucion, int> calculoCoste, 
                                      Func<Solucion, int>? calculoHeuristica = null) // Aquí cambiamos a Func<Solucion, int>?
    {
        var candidatos = listaCandidatos; 
        candidatos.anhadir(new Solucion(solucionInicial.Coords, 0)); 

        var vistos = new Dictionary<string, int>();
        bool finalizado = false;
        int revisados = 0;

        while (candidatos.__len__ > 0 && !finalizado)
        {
            var solucion = candidatos.obtener_siguiente();  
            vistos[string.Join("-", solucion.Coords)] = solucion.Coste;
            revisados++;

            if (criterioParada(solucion))
            {
                finalizado = true;
                break;
            }

            var vecinos = obtener_vecinos(solucion);
            foreach (var vecino in vecinos)
            {
                var nuevaSolucion = new Solucion(coords: solucion.Coords.Concat(new[] { vecino }).ToList());

                if (!vistos.ContainsKey(string.Join("-", nuevaSolucion.Coords)))
                {
                    nuevaSolucion.Coste = solucion.Coste + calculo_coste(solucion, nuevaSolucion);
                    candidatos.anhadir(nuevaSolucion, calculo_de_prioridad(nuevaSolucion, calculoHeuristica));
                }
            }
        }

        if (!finalizado)
        {
            return null;
        }

        return (solucion, revisados);
    }
}



// ESTA ESTÁ BIEN
class AEstrella : AlgoritmoDeBusqueda
{
    public AEstrella(): base() {}

    // Método que calcula la prioridad usando la función externa
    public int calculo_de_prioridad(Solucion solucion)
    {
        return solucion.Coste + calculo_heuristica(solucion);
    }
}




