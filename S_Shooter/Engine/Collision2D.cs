using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace S_Shooter.Engine
{
    public class Collision2D
    {
        public bool Collides(Vector2 onePos, Vector2 twoPos, Vector2 oneScale, Vector2 twoScale)
        {
            // collision x-axis?
            bool collisionX = onePos.X + oneScale.X >= twoPos.X && twoPos.X + twoScale.X >= onePos.X;
            // collision y-axis?
            bool collisionY = onePos.Y + oneScale.Y >= twoPos.Y && twoPos.Y + twoScale.Y >= onePos.Y;
            // collision only if on both axes
            return collisionX && collisionY;
        }
    }
}
