using System;

namespace CombatSimulation
{
    public class Zombie : Character
    {
        public Zombie(string name)
            : base(name, baseAttack: 100, defense: 0, initiative: 20, damages: 60, maximumLife: 1000, totalAttacks: 1,
                sensitiveToPain: false, attackDamageType: "normal", alignment: "cursed", isCharognard: true)
        {
            CharType = "Zombie";
        }

        public override int RollDefense()
        {
            return 0;
        }
    }
}