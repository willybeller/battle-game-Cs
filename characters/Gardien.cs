namespace JeuCombat{
    public class Gardien : Personnage
    {
        public Gardien(string nom) : base(nom)
        {
            Attack = 50;
            Defense = 150;
            Initiative = 50;
            Damages = 50;
            MaximumLife = 150;
            CurrentLife = 150;
            TotalAttackNumber = 3;
            CurrentAttackNumber = 3;
        }
        // Le Gardien inflige des dégâts sacrés
        public override TypeDegats DegatsType => TypeDegats.Sacre;
    }
}