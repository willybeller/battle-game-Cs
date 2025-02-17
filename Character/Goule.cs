using System;

namespace CombatSimulation
{
    public class Goule : Character
    {
        public Goule(string name)
            : base(name, baseAttack: 50, defense: 80, initiative: 120, damages: 30, maximumLife: 250, totalAttacks: 5,
                sensitiveToPain: true, attackDamageType: "normal", alignment: "cursed", isCharognard: true)
        {
            CharType = "Goule";
        }
    }
}