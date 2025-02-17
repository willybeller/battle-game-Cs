using System;

namespace CombatSimulation
{
    public class Guerrier : Character
    {
        public Guerrier(string name)
            : base(name, baseAttack: 100, defense: 100, initiative: 50, damages: 100, maximumLife: 200, totalAttacks: 2,
                sensitiveToPain: true, attackDamageType: "normal", alignment: "normal")
        {
            CharType = "Guerrier";
        }

        public override void ApplyPain(int damage, int lifeBefore)
        {
            if (!SensitiveToPain)
                return;
            // Pour le Guerrier, la douleur est appliquée uniquement pour le round en cours.
            PainDuration = 1;
            Console.WriteLine($"{Name} (Guerrier) est affecté par la douleur pour 1 round.");
        }
    }
}