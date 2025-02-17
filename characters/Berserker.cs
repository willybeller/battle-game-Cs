namespace JeuCombat{
    public class Berseker : Personnage
    {
        public Berseker(string nom) : base(nom)
        {
            Attack = 100;
            Defense = 100;
            Initiative = 80;
            Damages = 20;
            MaximumLife = 300;
            CurrentLife = 300;
            TotalAttackNumber = 1;
            CurrentAttackNumber = 1;
            SensibleDouleur = false; // Le Berseker n'est pas affecté par la douleur
        }

        public override void Attaquer(Personnage defenseur, Random rnd)
        {
            if (CurrentAttackNumber <= 0 || EstMort)
                return;

            // Le Berseker ajoute à ses dégâts les points de vie perdus
            int bonus = MaximumLife - CurrentLife;
            int effectiveDamages = Damages + bonus;
            int jetAttaque = Attack + rnd.Next(1, 101);
            int jetDefense = defenseur.TirerJetDefense(rnd);
            Console.WriteLine($"{Nom} (Berseker) attaque {defenseur.Nom} : jet d'attaque = {jetAttaque} vs jet de défense = {jetDefense} (bonus vie perdue = {bonus})");
            int marge = jetAttaque - jetDefense;
            if (marge > 0)
            {
                int degats = (marge * effectiveDamages) / 100;
                Console.WriteLine($"Attaque réussie ! Marge = {marge} → dégâts = {degats}");
                defenseur.SubirDegats(degats, rnd, this, this.DegatsType);
                // Si le Berseker est en dessous de 50 % de vie, son nombre d'attaques passe à 4
                if (CurrentLife < (MaximumLife / 2) && TotalAttackNumber < 4)
                {
                    TotalAttackNumber = 4;
                    CurrentAttackNumber = 4;
                    Console.WriteLine($"{Nom} est en dessous de 50 % de vie. Nombre d'attaques passe à 4.");
                }
            }
            else
            {
                Console.WriteLine("Attaque ratée !");
            }
            CurrentAttackNumber--;
        }
    }
}