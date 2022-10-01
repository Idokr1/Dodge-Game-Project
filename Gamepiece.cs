namespace Dodge_Game_Project
{
    public class Gamepiece
    {
        public int _locationX, _locationY, _pieceWidth, _pieceHeight;
        public const int playerWidth = 30, playerHeight = 30;
        public const int enemyWidth = 20, enemyHeight = 20;

        public Gamepiece(int locationX, int locationY, int pieceWidth, int pieceHeight)
        {
            _locationX = locationX;
            _locationY = locationY;
            _pieceWidth = pieceWidth;
            _pieceHeight = pieceHeight;
        }
    }
    public class Player : Gamepiece
    {
        public Player(int locationX, int locationY) : base(locationX, locationY, playerWidth, playerHeight) { }
    }
    public class Enemy : Gamepiece
    {
        public Enemy(int locationX, int locationY) : base(locationX, locationY, enemyWidth, enemyHeight) { }

    }
}