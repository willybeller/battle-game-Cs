using System;

namespace CombatSimulation
{
    public abstract class Character
    {
        public string Name { get; set; }
        public string CharType { get; protected set; }
        public int BaseAttack { get; protected set; }
        public int Attack { get; set; }
        public int Defense { get; protected set; }
        public int Initiative { get; protected set; }
        public int Damages { get; protected set; }
        public int MaximumLife { get; protected set; }
        public int CurrentLife { get; set; }
        public int BaseTotalAttacks { get; protected set; }
        public int TotalAttacks { get; set; }
        public int CurrentAttacks { get; set; }
        public bool SensitiveToPain { get; protected set; }
        // Valeurs possibles : "normal", "sacred", "impious"
        public string AttackDamageType { get; protected set; }
        // Valeurs possibles : "normal", "blessed", "cursed"
        public string Alignment { get; protected set; }
        public bool IsCharognard { get; protected set; }
        // Nombre de rounds durant lesquels le personnage ne peut pas attaquer
        public int PainDuration { get; set; }

        protected static Random rand = new Random();

        public Character(string name, int baseAttack, int defense, int initiative, int damages, int maximumLife, int totalAttacks,
                         bool sensitiveToPain, string attackDamageType, string alignment, bool isCharognard = false)
        {
            Name = name;
            BaseAttack = baseAttack;
            Attack = baseAttack;
            Defense = defense;
            Initiative = initiative;
            Damages = damages;
            MaximumLife = maximumLife;
            CurrentLife = maximumLife;
            BaseTotalAttacks = totalAttacks;
            TotalAttacks = totalAttacks;
            CurrentAttacks = totalAttacks;
            SensitiveToPain = sensitiveToPain;
            AttackDamageType = attackDamageType;
            Alignment = alignment;
            IsCharognard = isCharognard;
            PainDuration = 0;
        }

        public virtual int RollInitiative()
        {
            return Initiative + rand.Next(1, 101);
        }

        public virtual int RollAttack()
        {
            return Attack + rand.Next(1, 101);
        }

        public virtual int RollDefense()
        {
            return Defense + rand.Next(1, 101);
        }

        public virtual void ResetForRound()
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
        }

        public virtual void ApplyPain(int damage, int lifeBefore)
        {
            if (!SensitiveToPain)
                return;

            int remainingLife = CurrentLife;
            if (damage > remainingLife)
            {
                double chance = ((damage - remainingLife) * 2.0) / (damage + remainingLife);
                if (rand.NextDouble() < chance)
                {
                    int duration = rand.Next(1, 4);
                    PainDuration = duration;
                    Console.WriteLine($"{Name} est affecté par la douleur pour {duration} round(s).");
                }
            }
        }

        public virtual void UpdatePain()
        {
            if (PainDuration > 0)
                PainDuration--;
        }

        public virtual bool IsAlive()
        {
            return CurrentLife > 0;
        }

        public virtual void ReceiveHeal(int amount)
        {
            CurrentLife = Math.Min(MaximumLife, CurrentLife + amount);
            Console.WriteLine($"{Name} se soigne de {amount} points, vie = {CurrentLife}");
        }
    }
}
