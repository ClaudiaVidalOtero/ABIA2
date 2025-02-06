// Claudia Vidal Otero claudia.votero@udc.es
// Aldana Medina Lostaunau aldana.medina@udc.es
using System;
using System.Collections.Generic;

namespace Ejercicio2 
{
    
    class Bolsa
    {
        // Lista que almacena las bolas de colores
        private List<string> bolas; 

        // Objeto para generar números aleatorios
        private Random random; 

        // Constructor de la clase Bolsa, inicializa la lista con tres bolas y el objeto Random
        public Bolsa()
        {
            bolas = new List<string> { "azul", "roja", "verde" };
            random = new Random();
        }

        // Método para retirar una bola aleatoriamente de la bolsa
        public string RetirarBola()
        {
            int index = random.Next(bolas.Count);       // Elige un índice aleatorio
            string bolaSeleccionada = bolas[index];     // Guarda la bola en dicho índice
            bolas.RemoveAt(index);                      // Elimina la bola de la lista
            return bolaSeleccionada;                    // Devuelve el color de la bola retirada
        }

        // Método que devuelve una copia de la lista de bolas actuales en la bolsa
        public List<string> ObtenerBolas()
        {
            return new List<string>(bolas);     // Se devuelve una nueva lista 
        }
    }

    
    class Perro
    {
        public string ColorPelo { get; private set; }       // Propiedad para el color del pelo 
        public double Altura { get; private set; }          // Propiedad para la altura
        public double Peso { get; private set; }            // Propiedad para el peso
        public Bolsa BolsaDeBolas { get; private set; }     // Propiedad para la bolsa de bolas del perro

        // Constructor de la clase Perro, inicializa sus atributos
        public Perro(string colorPelo, double altura, double peso, Bolsa bolsa)
        {
            ColorPelo = colorPelo;
            Altura = altura;
            Peso = peso;
            BolsaDeBolas = bolsa;
        }

        // Método que permite al perro comer una bola de su bolsa
        public string ComerBola()
        {
            return BolsaDeBolas.RetirarBola(); 
        }
    }

    // Clase principal donde se ejecuta el programa
    class Program
    {
        // Método principal de ejecución
        public static void Ejecutar()
        {
            // Se solicita al usuario ingresar el color del perro
            Console.Write("Ingrese el color del perro: ");
            string color = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(color)) color = "rojo"; // Si no ingresa nada, el color por defecto es rojo
            

            // Si el usuario no ingresa datos para el perro, 
            //el perro será por defecto rojo, de altura 20 y de peso 1.8.
            // Se solicita la altura del perro
            Console.Write("Ingrese la altura del perro: ");
            string alturaInput = Console.ReadLine();
            double altura = string.IsNullOrWhiteSpace(alturaInput) ? 20 : double.Parse(alturaInput); // Si no ingresa nada, altura por defecto es 20
            
            // Se solicita el peso del perro
            Console.Write("Ingrese el peso del perro: ");
            string pesoInput = Console.ReadLine();
            double peso = string.IsNullOrWhiteSpace(pesoInput) ? 1.8 : double.Parse(pesoInput); // Si no ingresa nada, peso por defecto es 1.8
            
            // Se crea el objeto perro con los datos ingresados y una nueva bolsa de bolas
            Perro perroOriginal = new Perro(color, altura, peso, new Bolsa());


            // Se pregunta cuántos clones se desean generar
            Console.Write("¿Cuántas veces quieres clonar el perro? ");
            if (int.TryParse(Console.ReadLine(), out int cantidadClones) && cantidadClones > 0) // Se valida que el usuario ingrese un número válido
            {
                // Se muestra la información del perro original
                Console.WriteLine($"Original - Color: {perroOriginal.ColorPelo} Altura: {perroOriginal.Altura} Peso: {perroOriginal.Peso} Bolsa: {string.Join(" ", perroOriginal.BolsaDeBolas.ObtenerBolas())}");

                // Se crean los clones y cada uno come una bola
                for (int i = 1; i <= cantidadClones; i++)
                {
                    Perro clon = new Perro(perroOriginal.ColorPelo, perroOriginal.Altura, perroOriginal.Peso, new Bolsa());  // Cada clon tiene su propia bolsa
                    string bolaComida = clon.ComerBola(); // Se extrae una bola de la bolsa del clon
                    // Se muestra la información del clon, incluyendo la bola que comió
                    Console.WriteLine($"Clon {i} - Color: {clon.ColorPelo} Altura: {clon.Altura} Peso: {clon.Peso} Bolsa: {string.Join(" ", clon.BolsaDeBolas.ObtenerBolas())} (Comió: {bolaComida})");
                }
            }
            else
            {
                // Mensaje de error si el usuario no ingresa un número válido
                Console.WriteLine("Entrada inválida. Debe ingresar un número entero positivo.");
            }
        }
    }
}
