namespace JeuCombat{
    public class Vampire : Personnage
    {
        public Vampire(string nom) : base(nom)
        {
            Attack = 100;
            Defense = 100;
            Initiative = 120;
            Damages = 50;
            MaximumLife = 300;
            CurrentLife = 300;
            TotalAttackNumber = 2;
            CurrentAttackNumber = 2;
            EstMortVivant = true;
        }

        public override void Attaquer(Personnage defenseur, Random rnd)
        {
            if (CurrentAttackNumber <= 0 || EstMort)
                return;

            int jetAttaque = Attack + rnd.Next(1, 101);
            int jetDefense = defenseur.TirerJetDefense(rnd);
            Console.WriteLine($"{Nom} (Vampire) attaque {defenseur.Nom} : jet d'attaque = {jetAttaque} vs jet de défense = {jetDefense}");
            int marge = jetAttaque - jetDefense;
            if (marge > 0)
            {
                int degats = (marge * Damages) / 100;
                Console.WriteLine($"Attaque réussie ! Marge = {marge} → dégâts = {degats}");
                defenseur.SubirDegats(degats, rnd, this, this.DegatsType);
                // Le Vampire se soigne de la moitié des dégâts infligés
                int soin = degats / 2;
                CurrentLife = Math.Min(MaximumLife, CurrentLife + soin);
                Console.WriteLine($"{Nom} se soigne de {soin} points de vie. Vie actuelle = {CurrentLife}");
            }
            else
            {
                Console.WriteLine("Attaque ratée !");
            }
            CurrentAttackNumber--;
        }
    }
}