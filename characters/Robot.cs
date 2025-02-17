namespace JeuCombat{
    public class Robot : Personnage
    {
        public Robot(string nom) : base(nom)
        {
            Attack = 10;
            Defense = 100;
            Initiative = 50;
            Damages = 50;
            MaximumLife = 200;
            CurrentLife = 200;
            TotalAttackNumber = 1;
            CurrentAttackNumber = 1;
        }

        // Pour le Robot, les jets se font en ajoutant toujours 50 (sans aléatoire)
        public override int TirerJetInitiative(Random rnd)
        {
            return Initiative + 50;
        }
        public override int TirerJetAttaque(Random rnd)
        {
            return Attack + 50;
        }
        public override int TirerJetDefense(Random rnd)
        {
            return Defense + 50;
        }
        public override void DebutRound(Random rnd)
        {
            base.DebutRound(rnd);
            // Au début de chaque round, le Robot augmente son attaque de 50 %
            Attack = (int)(Attack * 1.5);
            Console.WriteLine($"{Nom} (Robot) augmente son attaque de 50 % et passe à {Attack}.");
        }
    }
}