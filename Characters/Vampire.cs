using System;

namespace CombatSimulation
{
    public class Vampire : Character
    {
        public Vampire(string name)
            : base(name, baseAttack: 100, defense: 100, initiative: 120, damages: 50, maximumLife: 300, totalAttacks: 2,
                sensitiveToPain: false, attackDamageType: "normal", alignment: "cursed")
        {
            CharType = "Vampire";
        }
    }
}