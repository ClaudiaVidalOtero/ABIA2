// Claudia Vidal Otero (claudia.votero@gudc.es)
// Aldana Smyna Medina Lostaunau (aldana.medina@udc.es)
// Grupo 2 (Jueves)
class Program
{
    static void EjecutarPlan(Estado estadoInicial, List<Accion> plan, Estado objetivo)
    {
        Console.WriteLine("\nPlan generado:");
        foreach (Accion accion in plan)
        {
            Console.WriteLine(accion.Nombre);
        }
        Console.WriteLine("\nEjecutando plan...\n");
        List<string> ordenInicial = estadoInicial.ObtenerOrdenDesdeEstado();
        Estado estadoActual = estadoInicial;
        estadoActual.ImprimirEstado(ordenInicial);
        
        foreach (Accion accion in plan)
        {
            Console.WriteLine($"Ejecutando: {accion.Nombre}\n");
            estadoActual = estadoActual.AplicarAccion(accion);
            List<string> ordenFinal = objetivo.ObtenerOrdenDesdeEstado();
            estadoActual.ImprimirEstado(ordenFinal);
        }
    }

    static void Main()
    {
        Estado estadoInicial = new Estado(new List<string>
        {
            "Encima(A,Mesa)", "Encima(B,A)", "Encima(C,B)", "Encima(E,Mesa)", "Encima(F,E)", "Encima(D,Mesa)", "Libre(C)", "Libre(F)", "Libre(D)"
        });

        Estado objetivo = new Estado(new List<string>
        {
            "Encima(E,C)", "Encima(F,B)", "Encima(D,A)", "Encima(C,Mesa)", "Encima(B,Mesa)", "Encima(A,Mesa)"
        });

        Planificador planificador = new Planificador();

        string[] bloques = { "A", "B", "C", "D", "E", "F" };
        
        foreach (string b in bloques)
        {
            foreach (string x in bloques.Append("Mesa"))
            {
                if (b != x)
                {
                    foreach (string y in bloques.Append("Mesa"))
                    {
                        if (b != y && x != y)
                        {
                            planificador.AgregarAccion(new Accion(
                                $"Mover({b},{x},{y})",
                                new List<string> { $"Encima({b},{x})", $"Libre({b})", $"Libre({y})" },
                                new List<string> { $"Encima({b},{y})", $"Libre({x})" },
                                new List<string> { $"Encima({b},{x})", $"Libre({y})" }
                            ));
                        }
                    }
                    
                    if (x != "Mesa")
                    {
                        planificador.AgregarAccion(new Accion(
                            $"MoverAMesa({b},{x})",
                            new List<string> { $"Encima({b},{x})", $"Libre({b})" },
                            new List<string> { $"Encima({b},Mesa)", $"Libre({x})" },
                            new List<string> { $"Encima({b},{x})" }
                        ));
                    }
                }
            }
        }

        List<Accion> plan = planificador.GenerarPlan(estadoInicial, objetivo);
        
        if (plan != null)
        {
            EjecutarPlan(estadoInicial, plan, objetivo);
        }
        else
        {
            Console.WriteLine("No se pudo generar un plan.");
        }
    }
}