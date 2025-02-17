using System;
using System.Collections.Generic;

namespace CombatSimulation
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Choisissez le mode de combat :");
            Console.WriteLine("1. Duel (2 personnages)");
            Console.WriteLine("2. Battle Royale (groupe de personnages)");
            Console.Write("Entrez 1 ou 2 : ");
            string mode = Console.ReadLine();

            List<Character> characters = new List<Character>();

            if (mode == "1")
            {
                Console.WriteLine("Duel – Types disponibles : Guerrier, Gardien, Berseker, Zombie, Robot, Liche, Goule, Vampire, Kamikaze, Prêtre");
                Console.Write("Type du premier personnage : ");
                string type1 = Console.ReadLine();
                Console.Write("Nom du premier personnage : ");
                string name1 = Console.ReadLine();
                Console.Write("Type du deuxième personnage : ");
                string type2 = Console.ReadLine();
                Console.Write("Nom du deuxième personnage : ");
                string name2 = Console.ReadLine();

                try
                {
                    characters.Add(CreateCharacter(type1, name1));
                    characters.Add(CreateCharacter(type2, name2));
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    return;
                }
            }
            else if (mode == "2")
            {
                Console.Write("Nombre de personnages : ");
                int n;
                if (!int.TryParse(Console.ReadLine(), out n))
                {
                    Console.WriteLine("Nombre invalide.");
                    return;
                }
                Console.WriteLine("Battle Royale – Types disponibles : Guerrier, Gardien, Berseker, Zombie, Robot, Liche, Goule, Vampire, Kamikaze, Prêtre");
                for (int i = 0; i < n; i++)
                {
                    Console.Write($"Type du personnage {i + 1} : ");
                    string t = Console.ReadLine();
                    Console.Write($"Nom du personnage {i + 1} : ");
                    string name = Console.ReadLine();
                    try
                    {
                        characters.Add(CreateCharacter(t, name));
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                        i--; // Re-essayer pour ce personnage
                    }
                }
            }
            else
            {
                Console.WriteLine("Mode invalide.");
                return;
            }

            CombatSimulator.SimulateCombat(characters);
        }

        static Character CreateCharacter(string charType, string name)
        {
            switch (charType)
            {
                case "Guerrier":
                    return new Guerrier(name);
                case "Gardien":
                    return new Gardien(name);
                case "Berseker":
                    return new Berseker(name);
                case "Zombie":
                    return new Zombie(name);
                case "Robot":
                    return new Robot(name);
                case "Liche":
                    return new Liche(name);
                case "Goule":
                    return new Goule(name);
                case "Vampire":
                    return new Vampire(name);
                case "Kamikaze":
                    return new Kamikaze(name);
                case "Prêtre":
                    return new Pretre(name);
                default:
                    throw new Exception("Type de personnage inconnu");
            }
        }
    }
}
