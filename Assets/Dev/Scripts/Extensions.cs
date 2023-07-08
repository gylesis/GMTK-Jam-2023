using System;
using UnityEngine;

namespace Dev.Scripts
{
    public static class Extensions
    {
        public static Vector2 GetStraightDirection(this Vector2 rawDirection)
        {
            rawDirection.Normalize();

            Vector2 direction;

            if (rawDirection.y > 0.5f)
            {
                direction = Vector2.up;
            }
            else if (rawDirection.x > 0.5f)
            {
                direction = Vector2.right;
            }
            else if (rawDirection.x < -0.5f)
            {
                direction = Vector2.left;
            }
            else if (rawDirection.y < -0.5f)
            {
                direction = Vector2.down;
            }
            else
            {
                direction = Vector2.zero;
            }

            return direction;
        }


        public static Vector2 GetDirectionByType(this Vector2 vector2, SwipeDirection swipeDirection)
        {
            Vector2 direction = Vector2.zero;
                
            switch (swipeDirection)
            {
                case SwipeDirection.Up:
                    direction = Vector2.up;
                    break;
                case SwipeDirection.Down:
                    direction = Vector2.down;
                    break;
                case SwipeDirection.Left:
                    direction = Vector2.left;
                    break;
                case SwipeDirection.Right:
                    direction = Vector2.right;
                    break;
            }

            return direction;
        }
        
    }
}