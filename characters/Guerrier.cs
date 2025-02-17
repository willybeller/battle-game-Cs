namespace JeuCombat{
public class Guerrier : Personnage
    {
        public Guerrier(string nom) : base(nom)
        {
            Attack = 100;
            Defense = 100;
            Initiative = 50;
            Damages = 100;
            MaximumLife = 200;
            CurrentLife = 200;
            TotalAttackNumber = 2;
            CurrentAttackNumber = 2;
        }
        // Le comportement en cas de douleur est géré dans la classe de base.
    }
}