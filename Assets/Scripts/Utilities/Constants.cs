using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Utilities
{
    public static class Constants
    {
        public static readonly string TagArena = "Arena";
        public static readonly string TagSwitchDirectionImage = "SwitchDirectionImage";
        public static readonly string TagStartGameBtn = "StartGameBtn";
        public static readonly string TagBall = "Ball";
        public static readonly string TagWall = "Wall";
        public static readonly string TagGridPiece = "GridPiece";
    }

    public enum WallType
    {
        Left, Right, Bottom, Top
    }

    public enum WallState
    {
        Creating, Created
    }

}

