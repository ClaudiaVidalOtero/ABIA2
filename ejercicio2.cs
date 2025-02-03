using System;
using System.Collections.Generic;

namespace Ejercicio2
{
    class Bolsa
    {
        private List<string> bolas;
        private Random random;

        public Bolsa()
        {
            bolas = new List<string> { "azul", "roja", "verde" };
            random = new Random();
        }

        public string RetirarBola()
        {
            if (bolas.Count == 0)
                return "No quedan bolas";

            int index = random.Next(bolas.Count);
            string bolaSeleccionada = bolas[index];
            bolas.RemoveAt(index);
            return bolaSeleccionada;
        }

        public List<string> ObtenerBolas()
        {
            return new List<string>(bolas);
        }
    }

    class Perro
    {
        public string ColorPelo { get; private set; }
        public double Altura { get; private set; }
        public double Peso { get; private set; }
        public Bolsa BolsaDeBolas { get; private set; }

        public Perro(string colorPelo, double altura, double peso, Bolsa bolsa)
        {
            ColorPelo = colorPelo;
            Altura = altura;
            Peso = peso;
            BolsaDeBolas = bolsa;
        }

        public string ComerBola()
        {
            return BolsaDeBolas.RetirarBola();
        }
    }

        class Program
    {
        public static void Ejecutar()
        {
            Console.Write("Ingrese el color del perro: ");
            string color = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(color)) color = "rojo";
            
            Console.Write("Ingrese la altura del perro: ");
            string alturaInput = Console.ReadLine();
            double altura = string.IsNullOrWhiteSpace(alturaInput) ? 20 : double.Parse(alturaInput);
            
            Console.Write("Ingrese el peso del perro: ");
            string pesoInput = Console.ReadLine();
            double peso = string.IsNullOrWhiteSpace(pesoInput) ? 1.8 : double.Parse(pesoInput);
            
            Perro perroOriginal = new Perro(color, altura, peso, new Bolsa());

            Console.Write("¿Cuántas veces quieres clonar el perro? ");
            if (int.TryParse(Console.ReadLine(), out int cantidadClones) && cantidadClones > 0)
            {
                Console.WriteLine($"Original - Color: {perroOriginal.ColorPelo} Altura: {perroOriginal.Altura} Peso: {perroOriginal.Peso} Bolsa: {string.Join(" ", perroOriginal.BolsaDeBolas.ObtenerBolas())}");

                for (int i = 1; i <= cantidadClones; i++)
                {
                    Perro clon = new Perro(perroOriginal.ColorPelo, perroOriginal.Altura, perroOriginal.Peso, new Bolsa());  // Cada clon tiene su propia bolsa
                    string bolaComida = clon.ComerBola();
                    Console.WriteLine($"Clon {i} - Color: {clon.ColorPelo} Altura: {clon.Altura} Peso: {clon.Peso} Bolsa: {string.Join(" ", clon.BolsaDeBolas.ObtenerBolas())} (Comió: {bolaComida})");
                }
            }
            else
            {
                Console.WriteLine("Entrada inválida. Debe ingresar un número entero positivo.");
            }
        }
    }

}

