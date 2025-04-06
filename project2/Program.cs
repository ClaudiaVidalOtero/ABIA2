// Claudia Vidal Otero (claudia.votero@udc.es)
// Aldana Smyna Medina Lostaunau (aldana.medina@udc.es)
// Grupo 2 (Jueves)
class Program
{
    // Ejecuta el plan paso a paso, mostrando cada estado tras aplicar una acción
    static void EjecutarPlan(Estado estadoInicial, List<Accion> plan, Estado estadoObjetivo)
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

            List<string> ordenFinal = estadoObjetivo.ObtenerOrdenDesdeEstado();
            estadoActual.ImprimirEstado(ordenFinal);
        }
    }

    // Genera todas las acciones posibles según los bloques que hay
    static List<Accion> GenerarAcciones(string[] bloques)
    {
        Planificador planificador = new Planificador();

        foreach (string bloque in bloques)
        {
            foreach (string origen in bloques.Append("Mesa"))
            {
                if (bloque != origen)
                {
                    foreach (string destino in bloques.Append("Mesa"))
                    {
                        if (bloque != destino && origen != destino)
                        {
                            // Acción general: Mover bloque de un lugar a otro
                            Accion mover = new Accion(
                                $"Mover({bloque},{origen},{destino})",
                                new List<string> { $"Encima({bloque},{origen})", $"Libre({bloque})", $"Libre({destino})" },
                                new List<string> { $"Encima({bloque},{destino})", $"Libre({origen})" },
                                new List<string> { $"Encima({bloque},{origen})", $"Libre({destino})" }
                            );
                            planificador.AgregarAccion(mover);
                        }
                    }

                    if (origen != "Mesa")
                    {
                        // Acción especial: Mover un bloque desde otro bloque hacia la mesa
                        Accion moverAMesa = new Accion(
                            $"MoverAMesa({bloque},{origen})",
                            new List<string> { $"Encima({bloque},{origen})", $"Libre({bloque})" },
                            new List<string> { $"Encima({bloque},Mesa)", $"Libre({origen})" },
                            new List<string> { $"Encima({bloque},{origen})" }
                        );
                        planificador.AgregarAccion(moverAMesa);
                    }
                }
            }
        }

        return planificador.Acciones;
    }

    // Primer escenario simple de 3 bloques
    static void Escenario1()
    {
        Estado estadoInicial = new Estado(new List<string>
        {
            "Encima(A,Mesa)", "Encima(B,Mesa)", "Encima(C,Mesa)",
            "Libre(A)", "Libre(B)", "Libre(C)"
        });

        Estado estadoObjetivo = new Estado(new List<string>
        {
            "Encima(B,A)", "Encima(C,B)", "Encima(A,Mesa)"
        });

        string[] bloques = new string[] { "A", "B", "C" };
        List<Accion> acciones = GenerarAcciones(bloques);

        Planificador planificador = new Planificador();
        foreach (Accion accion in acciones)
        {
            planificador.AgregarAccion(accion);
        }

        List<Accion> plan = planificador.GenerarPlan(estadoInicial, estadoObjetivo);

        if (plan != null)
        {
            EjecutarPlan(estadoInicial, plan, estadoObjetivo);
        }
        else
        {
            Console.WriteLine("No se pudo generar un plan.");
        }
    }

    // Segundo escenario más complejo
    static void Escenario2()
    {
        Estado estadoInicial = new Estado(new List<string>
        {
            "Encima(A,Mesa)", "Encima(B,A)", "Encima(C,B)",
            "Encima(E,Mesa)", "Encima(F,E)", "Encima(D,Mesa)",
            "Libre(C)", "Libre(F)", "Libre(D)"
        });

        Estado estadoObjetivo = new Estado(new List<string>
        {
            "Encima(E,C)", "Encima(F,B)", "Encima(D,A)",
            "Encima(C,Mesa)", "Encima(B,Mesa)", "Encima(A,Mesa)"
        });

        string[] bloques = new string[] { "A", "B", "C", "D", "E", "F" };
        List<Accion> acciones = GenerarAcciones(bloques);

        Planificador planificador = new Planificador();
        foreach (Accion accion in acciones)
        {
            planificador.AgregarAccion(accion);
        }

        List<Accion> plan = planificador.GenerarPlan(estadoInicial, estadoObjetivo);

        if (plan != null)
        {
            EjecutarPlan(estadoInicial, plan, estadoObjetivo);
        }
        else
        {
            Console.WriteLine("No se pudo generar un plan.");
        }
    }

    // Menú para escoger escenario
    static void Main()
    {
        Console.WriteLine("Selecciona un escenario:");
        Console.WriteLine("1. Escenario (A, B, C)");
        Console.WriteLine("2. Escenario (A-F)");
        Console.Write("Opción: ");
        string opcion = Console.ReadLine();

        switch (opcion)
        {
            case "1":
                Escenario1();
                break;
            case "2":
                Escenario2();
                break;
            default:
                Console.WriteLine("Opción no válida");
                break;
        }
    }
}
