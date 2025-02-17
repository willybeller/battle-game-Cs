using System;

namespace CombatSimulation
{
    public class Gardien : Character
    {
        public Gardien(string name)
            : base(name, baseAttack: 50, defense: 150, initiative: 50, damages: 50, maximumLife: 150, totalAttacks: 3,
                sensitiveToPain: true, attackDamageType: "sacred", alignment: "normal")
        {
            CharType = "Gardien";
        }
    }
}