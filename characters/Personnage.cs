namespace JeuCombat
{
    // Classe de base pour tous les personnages
    public abstract class Personnage
    {
        public string Nom { get; set; }
        public int Attack { get; set; }
        public int Defense { get; set; }
        public int Initiative { get; set; }
        public int Damages { get; set; }
        public int MaximumLife { get; set; }
        public int CurrentLife { get; set; }
        public int TotalAttackNumber { get; set; }
        public int CurrentAttackNumber { get; set; }
        // Nombre de rounds pendant lesquels le personnage ne peut pas attaquer (en raison de la douleur)
        public int RoundsSansAttaque { get; set; } = 0;
        // Indique si le personnage est sensible à la douleur (les vivants le sont, les morts-vivants non, sauf exception)
        public bool SensibleDouleur { get; set; } = true;
        // Indique si le personnage est un mort-vivant (insensible à la douleur par défaut)
        public bool EstMortVivant { get; set; } = false;
        // Indique si le personnage est charognard (il peut récupérer de la vie sur les cadavres)
        public bool EstCharognard { get; set; } = false;
        // Indique si le personnage est béni
        public bool EstBeni { get; set; } = false;
        // Indique si le personnage est maudit
        public bool EstMaudit { get; set; } = false;

        // Le personnage est considéré comme mort si sa vie est inférieure ou égale à 0
        public bool EstMort => CurrentLife <= 0;

        // Par défaut, les attaques infligent des dégâts de type Normal
        public virtual TypeDegats DegatsType => TypeDegats.Normal;

        public Personnage(string nom)
        {
            Nom = nom;
        }

        // Jet d'initiative : caractéristique + un nombre aléatoire entre 1 et 100
        public virtual int TirerJetInitiative(Random rnd)
        {
            return Initiative + rnd.Next(1, 101);
        }

        // Jet d'attaque : caractéristique + un nombre aléatoire entre 1 et 100
        public virtual int TirerJetAttaque(Random rnd)
        {
            return Attack + rnd.Next(1, 101);
        }

        // Jet de défense : caractéristique + un nombre aléatoire entre 1 et 100
        public virtual int TirerJetDefense(Random rnd)
        {
            return Defense + rnd.Next(1, 101);
        }

        // En début de round, on réinitialise le nombre d'attaques disponibles.
        // Si le personnage est affecté par la douleur (RoundsSansAttaque > 0), il ne peut pas attaquer.
        public virtual void DebutRound(Random rnd)
        {
            if (EstMort)
                return; // Si mort, on ne fait rien
            if (RoundsSansAttaque > 0)
            {
                Console.WriteLine($"{Nom} est affecté par la douleur et ne peut pas attaquer ce round (il lui reste {RoundsSansAttaque} round(s) de pénalité).");
                CurrentAttackNumber = 0;
                RoundsSansAttaque--;
            }
            else
            {
                CurrentAttackNumber = TotalAttackNumber;
            }
        }

        // Méthode d'attaque standard (contre-attaque comprise)
        public virtual void Attaquer(Personnage defenseur, Random rnd)
        {
            if (CurrentAttackNumber <= 0 || EstMort)
                return;

            int jetAttaque = TirerJetAttaque(rnd);
            int jetDefense = defenseur.TirerJetDefense(rnd);
            Console.WriteLine($"{Nom} attaque {defenseur.Nom} : jet d'attaque = {jetAttaque} vs jet de défense = {jetDefense}");
            int marge = jetAttaque - jetDefense;
            if (marge > 0)
            {
                int degats = (marge * Damages) / 100;
                Console.WriteLine($"Attaque réussie ! Marge = {marge} → dégâts de base = {degats}");
                defenseur.SubirDegats(degats, rnd, this, this.DegatsType);
            }
            else if (marge < 0)
            {
                Console.WriteLine($"{defenseur.Nom} a bien défendu (marge négative = {marge}).");
                // Contre-attaque (uniquement si le défenseur peut attaquer et s'il n'est pas un type qui ne peut contre-attaquer)
                if (defenseur.CurrentAttackNumber > 0 && defenseur.SensibleDouleur && !(defenseur is Zombie) && !(defenseur is Kamikaze))
                {
                    int bonus = -marge;
                    // Pour le Gardien, le bonus de contre-attaque est doublé
                    if (defenseur is Gardien)
                        bonus *= 2;
                    Console.WriteLine($"{defenseur.Nom} contre-attaque avec un bonus de {bonus} !");
                    int jetContre = defenseur.Attack + bonus + rnd.Next(1, 101);
                    int jetDefenseContre = TirerJetDefense(rnd);
                    int margeContre = jetContre - jetDefenseContre;
                    if (margeContre > 0)
                    {
                        int degatsContre = (margeContre * defenseur.Damages) / 100;
                        Console.WriteLine($"Contre-attaque réussie ! Marge = {margeContre} → dégâts = {degatsContre}");
                        this.SubirDegats(degatsContre, rnd, defenseur, defenseur.DegatsType);
                    }
                    else
                    {
                        Console.WriteLine("Contre-attaque ratée !");
                    }
                    defenseur.CurrentAttackNumber--; // la contre-attaque utilise une attaque disponible
                }
                else
                {
                    Console.WriteLine($"{defenseur.Nom} ne peut pas contre-attaquer.");
                }
            }
            else
            {
                Console.WriteLine("Attaque sans effet (égalité des jets).");
            }
            CurrentAttackNumber--;
        }

        // Gestion des dégâts reçus, intégrant :
        // - L'application d'un éventuel effet double si le personnage est béni/maudit face à des dégâts impies/sacrés.
        // - La gestion de la douleur.
        public virtual void SubirDegats(int degats, Random rnd, Personnage attaquant, TypeDegats typeAttaque)
        {
            int degatsFinal = degats;
            // Application des effets liés aux dégâts sacrés/impies
            if (typeAttaque == TypeDegats.Impie && this.EstBeni)
            {
                degatsFinal *= 2;
                Console.WriteLine($"{Nom} est béni et subit des dégâts impies doublés !");
            }
            else if (typeAttaque == TypeDegats.Sacre && this.EstMaudit)
            {
                degatsFinal *= 2;
                Console.WriteLine($"{Nom} est maudit et subit des dégâts sacrés doublés !");
            }

            CurrentLife -= degatsFinal;
            Console.WriteLine($"{Nom} perd {degatsFinal} points de vie. Vie restante = {CurrentLife}");

            // Gestion de la douleur (uniquement si le personnage est sensible et encore en vie)
            if (CurrentLife > 0 && SensibleDouleur)
            {
                // Si les dégâts subis sont supérieurs à la vie restante après coup...
                if (degatsFinal > CurrentLife)
                {
                    double chance = ((degatsFinal - CurrentLife) * 2.0) / (CurrentLife + degatsFinal);
                    double tirage = rnd.NextDouble();
                    if (tirage < chance)
                    {
                        int roundsPerdus = rnd.Next(1, 4); // 1, 2 ou 3 rounds
                        // Pour le Guerrier, la pénalité se limite au round en cours
                        if (this is Guerrier)
                        {
                            roundsPerdus = 1;
                            Console.WriteLine($"{Nom} (Guerrier) subit la douleur et perd ses attaques pour le round en cours.");
                        }
                        else
                        {
                            Console.WriteLine($"{Nom} subit la douleur et ne pourra pas attaquer pendant {roundsPerdus} round(s).");
                        }
                        RoundsSansAttaque = roundsPerdus;
                        CurrentAttackNumber = 0; // on annule les attaques restantes ce round
                    }
                }
            }

            if (CurrentLife <= 0)
                Console.WriteLine($"{Nom} est mort !");
                CurrentAttackNumber = 0; // Désactivation immédiate des attaques
        }
    }
}