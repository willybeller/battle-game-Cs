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
            char mode = 'n';
            while(!(mode=='1' || mode=='2')){
                mode = Console.ReadKey().KeyChar;
            }

            List<Character> characters = new List<Character>();

            if (mode == '1')
            {
                Console.WriteLine("\nDuel - Types disponibles :\n0.Guerrier\n1.Gardien\n2.Berseker\n3.Zombie\n4.Robot\n5.Liche\n6.Goule\n7.Vampire\n8.Kamikaze\n9.Prêtre");
                Console.Write("Nom du premier personnage : ");
                string name1 = Console.ReadLine();
                Console.Write("Type du premier personnage : ");
                char type1 = 'n';
                while(!(type1=='0' || type1=='1' || type1=='2' || type1=='3' || type1=='4' || type1=='5' || type1=='6' || type1=='7' || type1=='8' || type1=='9')){
                    type1 = Console.ReadKey().KeyChar;
                }
                Console.Write("\nNom du deuxième personnage : ");
                string name2 = Console.ReadLine();
                Console.Write("Type du deuxième personnage : ");
                char type2 = 'n';
                while(!(type2=='0' || type2=='1' || type2=='2' || type2=='3' || type2=='4' || type2=='5' || type2=='6' || type2=='7' || type2=='8' || type2=='9')){
                    type2 = Console.ReadKey().KeyChar;
                }

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
            else if (mode == '2')
            {
                Console.Write("\nNombre de personnages : ");
                int n;
                if (!int.TryParse(Console.ReadLine(), out n))
                {
                    Console.WriteLine("Nombre invalide.");
                    return;
                }
                Console.WriteLine("Battle Royale - Types disponibles :\n0.Guerrier\n1.Gardien\n2.Berseker\n3.Zombie\n4.Robot\n5.Liche\n6.Goule\n7.Vampire\n8.Kamikaze\n9.Prêtre");
                for (int i = 0; i < n; i++)
                {
                    Console.Write($"\nNom du personnage {i + 1} : ");
                    string name = Console.ReadLine();
                    Console.Write($"Type du personnage {i + 1} : ");
                    char t = 'n';
                    while(!(t=='0' || t=='1' || t=='2' || t=='3' || t=='4' || t=='5' || t=='6' || t=='7' || t=='8' || t=='9')){
                        t = Console.ReadKey().KeyChar;
                    }
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

        static Character CreateCharacter(char? charType, string name)
        {
            switch (charType)
            {
                case '0':
                    return new Guerrier(name);
                case '1':
                    return new Gardien(name);
                case '2':
                    return new Berseker(name);
                case '3':
                    return new Zombie(name);
                case '4':
                    return new Robot(name);
                case '5':
                    return new Liche(name);
                case '6':
                    return new Goule(name);
                case '7':
                    return new Vampire(name);
                case '8':
                    return new Kamikaze(name);
                case '9':
                    return new Pretre(name);
                default:
                    throw new Exception("Type de personnage inconnu");
            }
        }
    }
}
