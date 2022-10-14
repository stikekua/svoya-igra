namespace SvoyaIgra.Game.Metadata
{
    public class Player
    {
        public string Name { get; set; }
        public int Score { get; set; }=0;
        public string ColorCode { get; set; }
        public bool isActive { get; set; } = true;
        public bool isSelected { get; set; } = false;

        public Player(string name, string colorCode)
        {
            Name = name;
            ColorCode = colorCode;
        }
    }
}
