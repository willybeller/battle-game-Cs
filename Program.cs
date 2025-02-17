using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace JeuCombat{
    // -----------------------------
    // Programme principal
    // -----------------------------
    class Program
    {
        static void Main(string[] args)
        {
            bool rejouer = true;
            while(rejouer){
                //mettre a true pour attendre la validation du joueur à chaque round
                bool appuyerPourJouer = false;

                Random rnd = new Random();
                Console.WriteLine("Bienvenue dans le simulateur de combat !");
                Console.WriteLine("Choisissez le mode de combat :");
                Console.WriteLine("1. Duel");
                Console.WriteLine("2. Battle Royale");
                
                char? choix = null;
                while(choix!='1' && choix!='2'){
                    choix = Console.ReadKey().KeyChar;
                }

                if (choix == '1')
                {
                    // MODE DUEL : Exemple – un Guerrier contre un Prêtre
                    Personnage p1 = new Guerrier("Hector");
                    Personnage p2 = new Robot("Simon");

                    Console.WriteLine("Début du duel entre Hector et Simon !");
                    int round = 1;
                    while (!p1.EstMort && !p2.EstMort)
                    {
                        Console.WriteLine($"\n----- Round {round} -----");
                        // Réinitialisation des attaques disponibles (ou application de la pénalité de douleur)
                        p1.DebutRound(rnd);
                        p2.DebutRound(rnd);

                        // Calcul des initiatives
                        int initiative1 = p1.TirerJetInitiative(rnd);
                        int initiative2 = p2.TirerJetInitiative(rnd);
                        Console.WriteLine($"{p1.Nom} a une initiative de {initiative1}");
                        Console.WriteLine($"{p2.Nom} a une initiative de {initiative2}");

                        // Le personnage avec la meilleure initiative attaque en premier
                        if (initiative1 >= initiative2)
                        {
                            while (p1.CurrentAttackNumber > 0 && !p2.EstMort)
                            {
                                // S'il s'agit d'un Kamikaze, utiliser l'attaque de groupe (même dans le duel, la cible inclut lui-même)
                                if (p1 is Kamikaze km)
                                    km.AttaquerGroupe(new List<Personnage> { p1, p2 }, rnd);
                                else
                                    p1.Attaquer(p2, rnd);
                            }
                            if (!p2.EstMort)
                            {
                                while (p2.CurrentAttackNumber > 0 && !p1.EstMort)
                                {
                                    if (p2 is Kamikaze km2)
                                        km2.AttaquerGroupe(new List<Personnage> { p1, p2 }, rnd);
                                    else
                                        p2.Attaquer(p1, rnd);
                                }
                            }
                        }
                        else
                        {
                            while (p2.CurrentAttackNumber > 0 && !p1.EstMort)
                            {
                                if (p2 is Kamikaze km2)
                                    km2.AttaquerGroupe(new List<Personnage> { p1, p2 }, rnd);
                                else
                                    p2.Attaquer(p1, rnd);
                            }
                            if (!p1.EstMort)
                            {
                                while (p1.CurrentAttackNumber > 0 && !p2.EstMort)
                                {
                                    if (p1 is Kamikaze km)
                                        km.AttaquerGroupe(new List<Personnage> { p1, p2 }, rnd);
                                    else
                                        p1.Attaquer(p2, rnd);
                                }
                            }
                        }

                        if (p1.EstMort || p2.EstMort)
                            break;

                        if(appuyerPourJouer){
                            Console.WriteLine("Appuyez sur une touche pour lancer le prochain round...");
                            Console.ReadKey();
                        }
                        round++;
                    }
                    Console.WriteLine("\nFin du combat !");
                    if (p1.EstMort && p2.EstMort)
                        Console.WriteLine("Les deux combattants sont morts !");
                    else if (p1.EstMort)
                        Console.WriteLine($"{p2.Nom} a gagné !");
                    else
                        Console.WriteLine($"{p1.Nom} a gagné !");
                }
                else if (choix == '2')
                {
                    // MODE BATTLE ROYALE : plusieurs personnages issus de différents types
                    List<Personnage> combattants = new List<Personnage>
                    {
                        new Guerrier("Guerrier Hector"),
                        new Pretre("Prêtre Simon"),
                        new Berseker("Berseker Bob"),
                        new Zombie("Zombie Zed"),
                        new Robot("Robot R2D2"),
                        new Liche("Liche Larry"),
                        new Goule("Goule Gina"),
                        new Vampire("Vampire Vlad"),
                        new Kamikaze("Kamikaze Karl")
                    };

                    Console.WriteLine("Début de la Battle Royale !");
                    int round = 1;
                    while (combattants.Count(c => !c.EstMort) > 1)
                    {
                        Console.WriteLine($"\n----- Round {round} -----");
                        // Chaque combattant vivant réinitialise son nombre d'attaques (ou reste en pénalité de douleur)
                        foreach (var c in combattants.Where(c => !c.EstMort))
                            c.DebutRound(rnd);

                        // Calcul des initiatives pour déterminer l'ordre de passage
                        var ordre = combattants.Where(c => !c.EstMort)
                            .Select(c => new { Combattant = c, Initiative = c.TirerJetInitiative(rnd) })
                            .OrderByDescending(x => x.Initiative)
                            .ToList();

                        foreach (var participant in ordre)
                        {
                            if (participant.Combattant.EstMort)
                                continue;

                            Console.WriteLine($"\nC'est au tour de {participant.Combattant.Nom} (initiative = {participant.Initiative}).");
                            while (participant.Combattant.CurrentAttackNumber > 0 && combattants.Count(c => !c.EstMort) > 1)
                            {
                                // Choisir une cible aléatoire (le Prêtre privilégie un mort-vivant)
                                List<Personnage> cibles = combattants.Where(c => c != participant.Combattant && !c.EstMort).ToList();
                                if (cibles.Count == 0)
                                    break;
                                Personnage cible;
                                if (participant.Combattant is Pretre)
                                {
                                    var mortsVivant = cibles.Where(c => c.EstMortVivant).ToList();
                                    cible = mortsVivant.Any() ? mortsVivant[rnd.Next(mortsVivant.Count)] : cibles[rnd.Next(cibles.Count)];
                                }
                                else
                                {
                                    cible = cibles[rnd.Next(cibles.Count)];
                                }

                                if (participant.Combattant is Kamikaze km)
                                    km.AttaquerGroupe(combattants, rnd);
                                else
                                    participant.Combattant.Attaquer(cible, rnd);
                            }
                        }

                        // Après chaque round, les charognards récupèrent entre 50 et 100 points de vie sur les cadavres
                        var morts = combattants.Where(c => c.EstMort).ToList();
                        foreach (var mort in morts)
                        {
                            foreach (var c in combattants.Where(x => !x.EstMort && x.EstCharognard))
                            {
                                int soin = rnd.Next(50, 101);
                                c.CurrentLife = Math.Min(c.MaximumLife, c.CurrentLife + soin);
                                Console.WriteLine($"{c.Nom} (charognard) récupère {soin} points de vie en mangeant le cadavre de {mort.Nom}. Vie actuelle = {c.CurrentLife}");
                            }
                        }

                        if(appuyerPourJouer){
                            Console.WriteLine("Appuyez sur une touche pour lancer le prochain round...");
                            Console.ReadKey();
                        }
                        round++;
                    }

                    Console.WriteLine("\nFin de la Battle Royale !");
                    var vainqueur = combattants.FirstOrDefault(c => !c.EstMort);
                    if (vainqueur != null)
                        Console.WriteLine($"{vainqueur.Nom} est le vainqueur de la Battle Royale !");
                    else
                        Console.WriteLine("Aucun vainqueur, tous les combattants sont morts !");
                }
                else
                {
                    Console.WriteLine("Mode non reconnu.");
                }

                Console.WriteLine("\nM. retourner au menu\n"+
                                "Autre. quitter...");
                char? rep = Console.ReadKey().KeyChar;
                if(!(rep=='m' || rep=='M')){
                    Console.WriteLine("Fermeture de l'application...");
                    rejouer = false;
                }
            }
        }
    }
}
