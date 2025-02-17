using System;

namespace CombatSimulation
{
    public class Kamikaze : Character
    {
        public Kamikaze(string name)
            : base(name, baseAttack: 50, defense: 125, initiative: 20, damages: 75, maximumLife: 500, totalAttacks: 6,
                sensitiveToPain: true, attackDamageType: "normal", alignment: "normal")
        {
            CharType = "Kamikaze";
        }
    }
}