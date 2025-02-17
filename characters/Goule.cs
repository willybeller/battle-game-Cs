namespace JeuCombat{
    public class Goule : Personnage
    {
        public Goule(string nom) : base(nom)
        {
            Attack = 50;
            Defense = 80;
            Initiative = 120;
            Damages = 30;
            MaximumLife = 250;
            CurrentLife = 250;
            TotalAttackNumber = 5;
            CurrentAttackNumber = 5;
            EstMortVivant = true;
            EstCharognard = true;
            SensibleDouleur = true; // Contrairement aux autres morts-vivants, la Goule est sensible à la douleur
        }
    }
}