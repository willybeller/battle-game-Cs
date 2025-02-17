using System;

namespace CombatSimulation
{
    public class Berseker : Character
    {
        public Berseker(string name)
            : base(name, baseAttack: 100, defense: 100, initiative: 80, damages: 20, maximumLife: 300, totalAttacks: 1,
                sensitiveToPain: false, attackDamageType: "normal", alignment: "normal")
        {
            CharType = "Berseker";
        }

        public override void ResetForRound()
        {
            if (PainDuration > 0)
            {
                CurrentAttacks = 0;
            }
            else
            {
                if (CurrentLife < MaximumLife / 2)
                    TotalAttacks = 4;
                else
                    TotalAttacks = BaseTotalAttacks;
                CurrentAttacks = TotalAttacks;
            }
        }
    }
}