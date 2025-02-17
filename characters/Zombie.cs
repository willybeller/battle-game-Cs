namespace JeuCombat{
    public class Zombie : Personnage
    {
        public Zombie(string nom) : base(nom)
        {
            Attack = 100;
            Defense = 0;
            Initiative = 20;
            Damages = 60;
            MaximumLife = 1000;
            CurrentLife = 1000;
            TotalAttackNumber = 1;
            CurrentAttackNumber = 1;
            EstMortVivant = true;
            EstCharognard = true;
            SensibleDouleur = false; // Les morts-vivants (sauf exception) sont insensibles à la douleur
        }

        // Pour le Zombie, le jet de défense est toujours 0
        public override int TirerJetDefense(Random rnd)
        {
            return 0;
        }
    }
}