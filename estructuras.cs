using System;
using System.Collections.Generic;
using System.Linq;
using Ejecutable;
using Algoritmos;

//REMOVED = '<removed-task>'  // placeholder for a removed task

namespace Estructuras
{

    public class Solucion
    {
        public List<(int, int)> Coords { get; private set; }
        public int Coste { get; set; }

        public Solucion(List<(int, int)> coords, int coste)
        {
            Coords = coords;
            Coste = coste;
        }

        public bool __eq__(Solucion other)
        {
            if (other == null) return false;
            return string.Join("-", Coords) == string.Join("-", other.Coords);
        }

        public int __lt__(Solucion other)
        {
            if (other == null) return 1;
            return Coste < other.Coste ? -1 : (Coste > other.Coste ? 1 : 0);
        }

        public string __str__()
        {
            return string.Join("-", Coords);
        }
    }

    public class ListaCandidatos
    {
        public virtual void anhadir(Solucion solucion, int prioridad = 0) { }
        public virtual void borrar(Solucion solucion) { }
        public virtual Solucion obtener_siguiente() { throw new NotImplementedException(); }
        public virtual int __len__ => 0;
    }


    // HECHA EN C# NECESITA ARREGLOS
    public class ColaDePrioridad : ListaCandidatos
    {
        private readonly List<(int prioridad, Solucion solucion)> cp;
        private readonly Dictionary<string, (int prioridad, Solucion solucion)> buscador;
        private const int REMOVED = -1;                             ////////// en teoría tiene que se REMOVED = '<removed-task>' no -1

        public ColaDePrioridad()
        {
            cp = new List<(int prioridad, Solucion solucion)>();
            buscador = new Dictionary<string, (int prioridad, Solucion solucion)>();
        }
        public override void anhadir(Solucion solucion, int prioridad = 0)
        {
            string str_solucion = solucion.__str__();
            if (buscador.ContainsKey(str_solucion))
            {
                (int prioridad, Solucion solucion) solucionBuscador = buscador[str_solucion];
                if (solucionBuscador.prioridad <= prioridad)
                    return;
                borrar(solucion);
            }
            (int prioridad, Solucion solucion) entrada = (prioridad, solucion);
            buscador[str_solucion] = entrada;
            cp.Add(entrada);
            cp.Sort((a, b) => a.prioridad.CompareTo(b.prioridad));              //// HEAPP?????????????????????????? REVISAR
        }
        

        public override void borrar(Solucion solucion)
        {
            string str_solucion = solucion.__str__();
            
            if (buscador.ContainsKey(str_solucion))
            {
                var entrada = buscador[str_solucion];
                buscador.Remove(str_solucion);  

                entrada.Item2.Coste = REMOVED;                          // MIRAR COMO HACER CON LO DE -1 O PLACEHOLDER 
            }
        }


        public override Solucion obtener_siguiente()
        {
            while (cp.Count > 0)
            {
                (int prioridad, Solucion solucion) = cp[0];             /// IGUAL ES MEJOR USAR DEQUEUE, PARA FUNCIONALIDAD MAS SIMILAR A PYTHON
                cp.RemoveAt(0);
                if (solucion.Coste != REMOVED)
                {
                    buscador.Remove(solucion.__str__());
                    return solucion;
                }
            }
            throw new InvalidOperationException("No hay siguiente en una cola de prioridad vacía");
        }
        public override int __len__ => buscador.Count;
    }

}