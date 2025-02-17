namespace JeuCombat{
    public class Kamikaze : Personnage
    {
        public Kamikaze(string nom) : base(nom)
        {
            Attack = 50;
            Defense = 125;
            Initiative = 20;
            Damages = 75;
            MaximumLife = 500;
            CurrentLife = 500;
            TotalAttackNumber = 6;
            CurrentAttackNumber = 6;
        }

        // Les attaques du Kamikaze ne sont pas contre-attaquables.
        // Sa particularité : à chaque attaque, chacun des personnages sur le champ (y compris lui) a 50 % de chance d'être ciblé,
        // et tous se défendent contre le même jet d'attaque.
        public void AttaquerGroupe(List<Personnage> tousLesCombattants, Random rnd)
        {
            if (CurrentAttackNumber <= 0 || EstMort)
                return;

            int jetAttaque = Attack + rnd.Next(1, 101);
            Console.WriteLine($"{Nom} (Kamikaze) lance une attaque de groupe avec un jet d'attaque de {jetAttaque}.");

            foreach (var cible in tousLesCombattants.Where(p => !p.EstMort))
            {
                // Chaque personnage a 50 % de chance d'être ciblé
                if (rnd.NextDouble() < 0.5)
                {
                    Console.WriteLine($"{cible.Nom} est ciblé par l'attaque du Kamikaze.");
                    int jetDefense = cible.TirerJetDefense(rnd);
                    int marge = jetAttaque - jetDefense;
                    if (marge > 0)
                    {
                        int degats = (marge * Damages) / 100;
                        Console.WriteLine($"Attaque sur {cible.Nom} réussie ! Marge = {marge} → dégâts = {degats}");
                        cible.SubirDegats(degats, rnd, this, this.DegatsType);
                    }
                    else
                    {
                        Console.WriteLine($"{cible.Nom} esquive l'attaque du Kamikaze.");
                    }
                }
                else
                {
                    Console.WriteLine($"{cible.Nom} n'est pas ciblé par l'attaque du Kamikaze.");
                }
            }
            CurrentAttackNumber--;
        }
    }
}