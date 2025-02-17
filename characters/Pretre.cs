namespace JeuCombat{
    public class Pretre : Personnage
    {
        public Pretre(string nom) : base(nom)
        {
            Attack = 75;
            Defense = 125;
            Initiative = 50;
            Damages = 50;
            MaximumLife = 150;
            CurrentLife = 150;
            TotalAttackNumber = 1;
            CurrentAttackNumber = 1;
            EstBeni = true; // Le prêtre est béni
        }

        public override void DebutRound(Random rnd)
        {
            base.DebutRound(rnd);
            // Le prêtre se soigne de 10 % de sa vie maximum en début de round
            int soin = (int)(MaximumLife * 0.10);
            CurrentLife = Math.Min(MaximumLife, CurrentLife + soin);
            Console.WriteLine($"{Nom} (Prêtre) se soigne de {soin} points de vie en début de round. Vie actuelle = {CurrentLife}");
        }

        public override void Attaquer(Personnage defenseur, Random rnd)
        {
            if (CurrentAttackNumber <= 0 || EstMort)
                return;

            // Le prêtre privilégie les morts-vivants lorsqu'il attaque
            Console.WriteLine($"{Nom} (Prêtre) cherche à attaquer un mort-vivant en priorité.");
            int jetAttaque = Attack + rnd.Next(1, 101);
            int jetDefense = defenseur.TirerJetDefense(rnd);
            Console.WriteLine($"{Nom} attaque {defenseur.Nom} : jet d'attaque = {jetAttaque} vs jet de défense = {jetDefense}");
            int marge = jetAttaque - jetDefense;
            if (marge > 0)
            {
                int degats = (marge * Damages) / 100;
                Console.WriteLine($"Attaque réussie ! Marge = {marge} → dégâts = {degats}");
                defenseur.SubirDegats(degats, rnd, this, this.DegatsType);
            }
            else
            {
                Console.WriteLine("Attaque ratée !");
            }
            CurrentAttackNumber--;
        }
        public override TypeDegats DegatsType => TypeDegats.Sacre;
    }
}