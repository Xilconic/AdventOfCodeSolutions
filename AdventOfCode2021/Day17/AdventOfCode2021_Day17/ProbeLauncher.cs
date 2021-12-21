using System;
using System.Diagnostics;

namespace AdventOfCode2021_Day17
{
    internal class ProbeLauncher
    {
        public Trajectory FindHighestArcToHitTarget(TargetArea target)
        {
            // Assume target area is always fully below the launching point:
            Debug.Assert(target.Ymax < 0);
            
            // Note: Because of the physics simulated here, the probe will always get back at Y=0 with a negative initial dy velocity.
            //  Therefore we can calculate the maximum velocity that still hits the target as:
            int maxYVelocity = Math.Abs(target.Ymin) - 1;
            
            // Assume that because the puzzle is solvable, there exists some dX velocity that reaches terminal velocity
            // in maxYVelocity steps or sooner which still hits the target.

            return new Trajectory(CalculateDistanceAtTerminalVelocity(maxYVelocity));
        }

        /// <remarks>
        /// Because both X and Y decrement by 1 with each step, assuming positive launch velocities
        /// than these values will determine the distance where terminal velocity is reached.
        /// https://en.wikipedia.org/wiki/1_%2B_2_%2B_3_%2B_4_%2B_%E2%8B%AF
        /// </remarks>
        public int CalculateDistanceAtTerminalVelocity(int velocity)
        {
            Debug.Assert(velocity > 0);
            return (velocity * (velocity + 1)) / 2;
        }
    }
}