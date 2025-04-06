// Claudia Vidal Otero (claudia.votero@udc.es)
// Aldana Smyna Medina Lostaunau (aldana.medina@udc.es)
// Grupo 2 (Jueves)

/// <summary>
/// Clase principal que ejecuta el sistema de planificación.
/// Incluye lógica para configurar escenarios, generar acciones y ejecutar planes.
/// </summary>
class Program
{
    /// <summary>
    /// Ejecuta el plan generado paso a paso y muestra el estado de bloques tras cada acción.
    /// Se utiliza para visualizar cómo cambia el entorno tras aplicar cada acción.
    /// </summary>
    static void EjecutarPlan(Estado estadoInicial, List<Accion> plan, Estado estadoObjetivo)
    {
        Console.WriteLine("\nPlan generado:");
        foreach (Accion accion in plan)
        {
            Console.WriteLine(accion.Nombre);
        }

        Console.WriteLine("\nEjecutando plan...\n");

        // Obtener el orden visual de los bloques al inicio
        List<string> ordenInicial = estadoInicial.ObtenerOrdenDesdeEstado();
        Estado estadoActual = estadoInicial;
        estadoActual.ImprimirEstado(ordenInicial);

        // Se ejecuta el plan paso a paso
        foreach (Accion accion in plan)
        {
            Console.WriteLine($"Ejecutando: {accion.Nombre}\n");
            estadoActual = estadoActual.AplicarAccion(accion);
            // Visualizar el nuevo estado tras aplicar la acción
            List<string> ordenFinal = estadoObjetivo.ObtenerOrdenDesdeEstado();
            estadoActual.ImprimirEstado(ordenFinal);
        }
    }

    /// <summary>
    /// Genera todas las acciones posibles que se pueden aplicar al conjunto de bloques dado.
    /// Incluye acciones de mover de bloque a bloque, de bloque a mesa y de mesa a bloque.
    /// </summary>
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
                            // Acción de mover un bloque (desde otro bloque o mesa) a un destino válido
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
                        // Acción específica para mover desde un bloque a la mesa
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

    /// <summary>
    /// Escenario 1: Tres bloques (A, B, C) desordenados sobre la mesa.
    /// Objetivo: apilarlos en el orden A sobre B sobre C.
    /// </summary>
    static void Escenario1()
    {
        Estado estadoInicial = new Estado(new List<string>
        {
            "Encima(A,Mesa)", "Encima(B,Mesa)", "Encima(C,Mesa)",
            "Libre(A)", "Libre(B)", "Libre(C)"
        });

        Estado estadoObjetivo = new Estado(new List<string>
        {
            "Encima(A,B)", "Encima(B,C)", "Encima(C,Mesa)"
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

    /// <summary>
    /// Escenario 2: Seis bloques organizados en torres.
    /// Objetivo: Reordenar las torres formando nuevas pilas en un orden específico.
    /// Ejemplo más complejo que demuestra la escalabilidad del modelo STRIPS.
    /// </summary>
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

    /// <summary>
    /// Punto de entrada principal del programa. Permite elegir entre dos escenarios disponibles.
    /// </summary>
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
