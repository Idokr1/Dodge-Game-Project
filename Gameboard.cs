using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dodge_Game_Project
{
    public class Gameboard
    {
        public Player p1;
        public Enemy[] enemies;
        public double _boardWidth;
        public double _boardHeight;
        public const int enemyPlayerDistance = 15;
        public int countCollisions = 0;
        Random r = new Random();

        public Gameboard(double boardWidth, double boardHeight)
        {
            _boardWidth = boardWidth;
            _boardHeight = boardHeight;

            p1 = new Player((int)_boardWidth / 2 - 15, (int)_boardHeight / 2 - 15);

            enemies = new Enemy[10];
            for (int i = 0; i < 10; i++)
            {
                enemies[i] = new Enemy(r.Next(110, (int)_boardWidth - 10), r.Next(80, (int)_boardHeight - 10));
            }
        }

        //checking if the player won
        public bool isWinner()
        {
            if (countCollisions == 9)
                return true;
            return false;
        }

        //checking if the player lost
        public bool gameOver()
        {
            for (int i = 0; i < enemies.Length; i++)
            {
                if (enemies[i] != null && Math.Abs(enemies[i]._locationY - p1._locationY) < enemyPlayerDistance &&
                    Math.Abs(enemies[i]._locationX - p1._locationX) < enemyPlayerDistance)
                    return true;
            }
            return false;
        }
    }
}