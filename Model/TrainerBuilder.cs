namespace PokemonTrainerAPI.Model
{
    public class TrainerBuilder
    {
        private readonly Trainer _trainer;

        public TrainerBuilder()
        {
            _trainer = new Trainer();
        }

        public TrainerBuilder WithName(string name)
        {
            _trainer.Name = name;
            return this;
        }

        public TrainerBuilder WithSurname(string surname)
        {
            _trainer.Surname = surname;
            return this;
        }

        public TrainerBuilder WithAge(int age)
        {
            _trainer.Age = age;
            return this;
        }

        public TrainerBuilder WithGender(string gender)
        {
            _trainer.Gender = gender;
            return this;
        }

        public Trainer Build()
        {
            return _trainer;
        }
    }

}
