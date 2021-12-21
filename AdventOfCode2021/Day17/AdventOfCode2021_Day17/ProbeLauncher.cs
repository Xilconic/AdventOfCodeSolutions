using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

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
        private int CalculateDistanceAtTerminalVelocity(int velocity)
        {
            Debug.Assert(velocity > 0);
            return (velocity * (velocity + 1)) / 2;
        }

        public IReadOnlyCollection<InitialVelocity> GetAllInitialVelocitiesThatHitTarget(TargetArea target)
        {
            var initialXVelocityPerStepsToHit = new Dictionary<int, List<int>>();
            var initialXVelocityAchievingTerminalVelocityPerStepsToReach = new Dictionary<int, List<int>>();
            for (int initialVelocityX = 1; initialVelocityX <= target.Xmax; initialVelocityX++)
            {
                IReadOnlyCollection<Tuple<TargetHit, int>> hits = CheckIfHitsTargetX(initialVelocityX, target);
                foreach ((TargetHit targetHit, var stepNumber) in hits)
                {
                    switch (targetHit)
                    {
                        case TargetHit.HitsDuringTraversal:
                            InsertOrAppend(stepNumber, initialVelocityX, initialXVelocityPerStepsToHit);
                            break;
                        case TargetHit.HitsWithTerminalVelocity:
                            InsertOrAppend(stepNumber, initialVelocityX, initialXVelocityAchievingTerminalVelocityPerStepsToReach);
                            break;
                    }
                }
            }
            
            var initialYVelocityPerStepsToHit = new Dictionary<int, List<int>>();
            for (int initialVelocityY = target.Ymin; initialVelocityY <= (Math.Abs(target.Ymin)+1); initialVelocityY++)
            {
                IReadOnlyCollection<Tuple<bool, int>> hits = CheckIfHitsTargetY(initialVelocityY, target);
                foreach (var (hitsTarget, stepNumber) in hits)
                {
                    if (hitsTarget)
                    {
                        InsertOrAppend(stepNumber, initialVelocityY, initialYVelocityPerStepsToHit);
                    }
                }
            }

            var results = new HashSet<InitialVelocity>();
            foreach ((int stepToHit, List<int> initialXVelocities) in initialXVelocityPerStepsToHit)
            {
                if (initialYVelocityPerStepsToHit.TryGetValue(stepToHit, out List<int> initialYVelocities))
                {
                    AddAllPermutationsToResult(initialXVelocities, initialYVelocities, results);
                }
            }
            
            foreach ((int stepAtWhichTerminalVelocityIsReached, List<int> initialVelocitiesX) in initialXVelocityAchievingTerminalVelocityPerStepsToReach)
            {
                var compatibleVelocitiesY = initialYVelocityPerStepsToHit
                    .Where(kvp => kvp.Key >= stepAtWhichTerminalVelocityIsReached)
                    .SelectMany(kvp => kvp.Value)
                    .ToArray();
                AddAllPermutationsToResult(initialVelocitiesX, compatibleVelocitiesY, results);
            }

            return results;
        }

        private static IReadOnlyCollection<Tuple<TargetHit, int>> CheckIfHitsTargetX(
            int initialVelocityX,
            TargetArea target)
        {
            Debug.Assert(initialVelocityX > 0);
            
            var result = new List<Tuple<TargetHit, int>>();
            
            var x = 0;
            var numberOfSteps = 0;
            
            while (initialVelocityX > 1)
            {
                x += initialVelocityX;
                
                numberOfSteps += 1;

                if (target.Xmin <= x && x <= target.Xmax)
                {
                    result.Add(Tuple.Create(TargetHit.HitsDuringTraversal, numberOfSteps));
                }

                if (x > target.Xmax)
                {
                    return result;
                }

                initialVelocityX -= 1;
            }

            x += 1;
            numberOfSteps += 1;
            if (target.Xmin <= x && x <= target.Xmax)
            {
                result.Add(Tuple.Create(TargetHit.HitsWithTerminalVelocity, numberOfSteps));
            }

            return result;
        }


        private static IReadOnlyCollection<Tuple<bool, int>> CheckIfHitsTargetY(
            int initialVelocityY,
            TargetArea target)
        {
            var result = new List<Tuple<bool, int>>();
            
            var y = 0;
            var numberOfSteps = 0;
            
            while (true)
            {
                y += initialVelocityY;
                
                numberOfSteps += 1;

                if (target.Ymin <= y && y <= target.Ymax)
                {
                    result.Add(Tuple.Create(true, numberOfSteps));
                }

                if (y < target.Ymin)
                {
                    return result;
                }

                initialVelocityY -= 1;
            }
        }

        private static void InsertOrAppend(
            int stepCount,
            int initialVelocityX,
            Dictionary<int, List<int>> initialXVelocityPerStepsToHit)
        {
            if (initialXVelocityPerStepsToHit.TryGetValue(stepCount, out List<int> initialVelocitiesX))
            {
                initialVelocitiesX.Add(initialVelocityX);
            }
            else
            {
                initialXVelocityPerStepsToHit[stepCount] = new List<int> { initialVelocityX };
            }
        }

        private static void AddAllPermutationsToResult(
            IReadOnlyCollection<int> initialXVelocities,
            IReadOnlyCollection<int> initialYVelocities,
            HashSet<InitialVelocity> results)
        {
            foreach (int dx in initialXVelocities)
            {
                foreach (int dy in initialYVelocities)
                {
                    results.Add(new InitialVelocity(dx, dy));
                }
            }
        }
        
        private enum TargetHit
        {
            HitsDuringTraversal,
            HitsWithTerminalVelocity
        }
    }
}