namespace PrograMail_26_27
{
    public class ChronoModel
    {
        public int SecondesTotales { get; set; }

        public void Incrementer()
        {
            SecondesTotales++;
        }

        public void Reinitialiser()
        {
            SecondesTotales = 0;
        }
    }
}