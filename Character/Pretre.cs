using System;

namespace CombatSimulation
{
    public class Pretre : Character
    {
        public Pretre(string name)
            : base(name, baseAttack: 75, defense: 125, initiative: 50, damages: 50, maximumLife: 150, totalAttacks: 1,
                sensitiveToPain: true, attackDamageType: "sacred", alignment: "blessed")
        {
            CharType = "Prêtre";
        }

        public override void ResetForRound()
        {
            if (PainDuration > 0)
            {
                CurrentAttacks = 0;
            }
            else
            {
                TotalAttacks = BaseTotalAttacks;
                CurrentAttacks = TotalAttacks;
            }
            int heal = (int)(0.1 * MaximumLife);
            CurrentLife = Math.Min(MaximumLife, CurrentLife + heal);
            Console.WriteLine($"{Name} (Prêtre) se soigne de {heal} points, vie = {CurrentLife}");
        }
    }
}