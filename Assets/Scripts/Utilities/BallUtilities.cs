using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Ball
{
    public static class BallUtilities
    {
        public static bool IsBallLeftOfTheLane(int coordX, List<BallManager> ballManagers)
        {
            return ballManagers.Any(b => b.CurrentCoordX < coordX);
        }

        public static bool IsBallRightOfTheLane(int coordX, List<BallManager> ballManagers)
        {
            return ballManagers.Any(b => b.CurrentCoordX > coordX);
        }

        public static bool IsBallUpFromTheLane(int coordY, List<BallManager> ballManagers)
        {
            return ballManagers.Any(b => b.CurrentCoordY > coordY);
        }

        public static bool IsBallDownFromTheLane(int coordY, List<BallManager> ballManagers)
        {
            return ballManagers.Any(b => b.CurrentCoordY < coordY);
        }

    }
}


