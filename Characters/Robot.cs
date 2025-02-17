using System;

namespace CombatSimulation
{
    public class Robot : Character
    {
        public Robot(string name)
            : base(name, baseAttack: 10, defense: 100, initiative: 50, damages: 50, maximumLife: 200, totalAttacks: 1,
                sensitiveToPain: true, attackDamageType: "normal", alignment: "normal")
        {
            CharType = "Robot";
        }

        public override int RollAttack()
        {
            return Attack + 50;
        }

        public override int RollDefense()
        {
            return Defense + 50;
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
            // Augmenter l'attaque de 50 % au début du round.
            Attack = (int)(BaseAttack * 1.5);
        }
    }
}