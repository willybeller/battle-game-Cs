using System;

namespace CombatSimulation
{
    public class Liche : Character
    {
        public Liche(string name)
            : base(name, baseAttack: 75, defense: 125, initiative: 80, damages: 50, maximumLife: 125, totalAttacks: 3,
                sensitiveToPain: false, attackDamageType: "impious", alignment: "cursed")
        {
            CharType = "Liche";
        }
    }
}