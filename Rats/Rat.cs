namespace Rats
{
    public class Rat
    {
        public int Column { get; set; }
        public int Row { get; set; }
        public int Score { get; set; } = 0;
        public string Name { get; set; }
        public char Initial { get; }
        public bool IsOnMap { get; set; } = false;
        public Rat(string name)
        {
            Name = name;
            Initial = name[0];
        }
        public Rat() { }
    }
}
