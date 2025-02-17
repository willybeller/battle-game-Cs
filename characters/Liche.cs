namespace JeuCombat{
public class Liche : Personnage
    {
        public Liche(string nom) : base(nom)
        {
            Attack = 75;
            Defense = 125;
            Initiative = 80;
            Damages = 50;
            MaximumLife = 125;
            CurrentLife = 125;
            TotalAttackNumber = 3;
            CurrentAttackNumber = 3;
            EstMortVivant = true;
            EstMaudit = true; // La Liche inflige des dégâts impies
        }
        public override TypeDegats DegatsType => TypeDegats.Impie;
    }
}