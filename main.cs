using System;

class MainProgram
{
    static void Main()
    {
        Console.WriteLine("Seleccione el ejercicio a ejecutar (1 o 2): ");
        string opcion = Console.ReadLine();

        if (opcion == "1")
            Ejercicio1.Program.Ejecutar();
        else if (opcion == "2")
            Ejercicio2.Program.Ejecutar();
        else
            Console.WriteLine("Opción no válida.");
    }
}
