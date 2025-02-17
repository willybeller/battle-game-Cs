using System;
using System.Collections.Generic;
using System.Linq;

namespace CombatSimulation
{
    public class CombatSimulator
    {
        private static Random rand = new Random();

        public static void PerformAttack(Character attacker, Character defender)
        {
            if (attacker.CurrentAttacks <= 0)
                return;

            int attackRoll = attacker.RollAttack();
            int defenseRoll = defender.RollDefense();
            Console.WriteLine($"{attacker.Name} ({attacker.CharType}) attaque {defender.Name} ({defender.CharType}) (attaque = {attackRoll} vs défense = {defenseRoll})");

            if (attackRoll > defenseRoll)
            {
                double baseDamage = (attackRoll - defenseRoll) * attacker.Damages / 100.0;
                // Pour le Berseker, ajouter les points de vie perdus en bonus
                if (attacker is Berseker berseker)
                {
                    int bonus = berseker.MaximumLife - berseker.CurrentLife;
                    baseDamage += bonus;
                }

                // Multiplicateurs selon le type de dégâts et l'alignement
                if (attacker.AttackDamageType == "impious" && defender.Alignment == "blessed")
                    baseDamage *= 2;
                if (attacker.AttackDamageType == "sacred" && defender.Alignment == "cursed")
                    baseDamage *= 2;

                int damage = (int)baseDamage;
                defender.CurrentLife -= damage;
                Console.WriteLine($"Attaque réussie ! {defender.Name} subit {damage} dégâts, vie restante = {Math.Max(defender.CurrentLife, 0)}");

                // Le Vampire se soigne de la moitié des dégâts infligés
                if (attacker is Vampire)
                {
                    int heal = damage / 2;
                    attacker.ReceiveHeal(heal);
                }
                defender.ApplyPain(damage, defender.CurrentLife + damage);
            }
            else
            {
                Console.WriteLine($"Attaque échouée. {defender.Name} peut contre-attaquer.");
                if (defender.CurrentAttacks > 0 && !(defender is Zombie) && !(defender is Kamikaze))
                {
                    int counterBonus = defenseRoll - attackRoll;
                    if (defender is Gardien)
                        counterBonus *= 2;
                    int counterAttackRoll = defender.RollAttack() + counterBonus;
                    int attackerDefenseRoll = attacker.RollDefense();
                    Console.WriteLine($"{defender.Name} contre-attaque {attacker.Name} (contre = {counterAttackRoll} vs défense = {attackerDefenseRoll})");
                    if (counterAttackRoll > attackerDefenseRoll)
                    {
                        double baseDamage = (counterAttackRoll - attackerDefenseRoll) * defender.Damages / 100.0;
                        if (defender.AttackDamageType == "impious" && attacker.Alignment == "blessed")
                            baseDamage *= 2;
                        if (defender.AttackDamageType == "sacred" && attacker.Alignment == "cursed")
                            baseDamage *= 2;

                        int damage = (int)baseDamage;
                        attacker.CurrentLife -= damage;
                        Console.WriteLine($"Contre-attaque réussie ! {attacker.Name} subit {damage} dégâts, vie restante = {Math.Max(attacker.CurrentLife, 0)}");
                        attacker.ApplyPain(damage, attacker.CurrentLife + damage);
                    }
                    else
                    {
                        Console.WriteLine("Contre-attaque échouée.");
                    }
                    defender.CurrentAttacks--;
                }
            }
            attacker.CurrentAttacks--;
        }

        public static void PerformKamikazeAttack(Kamikaze attacker, List<Character> characters)
        {
            if (attacker.CurrentAttacks <= 0)
                return;

            int attackRoll = attacker.RollAttack();
            Console.WriteLine($"{attacker.Name} (Kamikaze) attaque avec un jet d'attaque = {attackRoll}");

            foreach (var target in characters)
            {
                if (!target.IsAlive())
                    continue;
                if (rand.NextDouble() < 0.5)
                {
                    int defenseRoll = target.RollDefense();
                    Console.WriteLine($"{target.Name} défend contre l'attaque kamikaze (défense = {defenseRoll})");
                    if (attackRoll > defenseRoll)
                    {
                        int damage = (int)((attackRoll - defenseRoll) * attacker.Damages / 100.0);
                        target.CurrentLife -= damage;
                        Console.WriteLine($"{target.Name} subit {damage} dégâts de l'attaque kamikaze, vie restante = {Math.Max(target.CurrentLife, 0)}");
                        target.ApplyPain(damage, target.CurrentLife + damage);
                    }
                }
            }
            attacker.CurrentAttacks--;
        }

        public static void SimulateRound(List<Character> characters)
        {
            Console.WriteLine("\n=== Nouveau round ===");
            foreach (var c in characters)
            {
                if (c.IsAlive())
                    c.ResetForRound();
            }

            var initiatives = new List<(int initiative, Character character)>();
            foreach (var c in characters)
            {
                if (c.IsAlive())
                {
                    int init = c.RollInitiative();
                    initiatives.Add((init, c));
                    Console.WriteLine($"{c.Name} ({c.CharType}) a une initiative de {init}");
                }
            }
            var ordered = initiatives.OrderByDescending(x => x.initiative).ToList();

            foreach (var item in ordered)
            {
                var c = item.character;
                if (!c.IsAlive())
                    continue;
                if (c.CurrentAttacks <= 0)
                {
                    Console.WriteLine($"{c.Name} ne peut pas attaquer ce round.");
                    continue;
                }
                while (c.CurrentAttacks > 0 && c.IsAlive())
                {
                    if (c is Kamikaze kamikaze)
                    {
                        PerformKamikazeAttack(kamikaze, characters);
                    }
                    else
                    {
                        var possibles = characters.Where(x => x.IsAlive() && x != c).ToList();
                        // Pour le Prêtre, privilégier les cibles morts-vivantes
                        if (c is Pretre)
                        {
                            var undead = possibles.Where(x => x.CharType == "Zombie" || x.CharType == "Liche" || x.CharType == "Vampire" || x.CharType == "Goule").ToList();
                            if (undead.Any())
                                possibles = undead;
                        }
                        if (!possibles.Any())
                            break;
                        int index = rand.Next(possibles.Count);
                        var target = possibles[index];
                        PerformAttack(c, target);
                        if (target.CurrentLife <= 0)
                        {
                            Console.WriteLine($"{target.Name} est mort !");
                            foreach (var t in characters)
                            {
                                if (t.IsAlive() && t.IsCharognard && t != target)
                                {
                                    int healAmount = rand.Next(50, 101);
                                    t.ReceiveHeal(healAmount);
                                }
                            }
                        }
                    }
                }
            }

            foreach (var c in characters)
            {
                c.UpdatePain();
            }
        }

        public static void SimulateCombat(List<Character> characters)
        {
            int roundNumber = 1;
            while (characters.Count(x => x.IsAlive()) > 1)
            {
                Console.WriteLine($"\n--- Round {roundNumber} ---");
                SimulateRound(characters);
                roundNumber++;
                Console.WriteLine("Appuyez sur une touche pour le round suivant...");
                Console.ReadKey();
            }
            var winners = characters.Where(x => x.IsAlive()).ToList();
            if (winners.Any())
            {
                Console.WriteLine($"\nLe combat est terminé. Le vainqueur est {winners[0].Name} ({winners[0].CharType}) avec {winners[0].CurrentLife} points de vie restants.");
            }
            else
            {
                Console.WriteLine("Tous les combattants sont morts.");
            }
        }
    }
}
