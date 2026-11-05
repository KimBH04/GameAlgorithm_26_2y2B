using System.Collections.Generic;
using AlgoCourse.Lesson4;

namespace AlgoCourse.StudentWork
{
    public sealed class BfsRescuePathfinder : IRescuePathfinder
    {
        private readonly (int x, int y)[] directions = { (0, 1), (1, 0), (0, -1), (-1, 0) };
        
        public PathSearchResult FindPath(
            int width,
            int height,
            GridPosition start,
            GridPosition goal,
            IReadOnlyCollection<GridPosition> blockedCells)
        {
            // TODO 01: Queue, visited, cameFrom을 생성합니다.
            Queue<GridPosition> frontier = new();
            HashSet<GridPosition> visited = new();
            Dictionary<GridPosition, GridPosition> cameFrom = new();

            List<GridPosition> visitedOrder = new();
            HashSet<GridPosition> blocked = new(blockedCells);

            // TODO 02: 시작 칸을 Queue와 visited에 넣습니다.
            frontier.Enqueue(start);
            visited.Add(start);
            
            // TODO 03: Queue가 빌 때까지 상하좌우 이웃을 탐색합니다.
            while (frontier.Count > 0)
            {
                var from = frontier.Dequeue();
                visitedOrder.Add(from);

                if (from == goal)
                {
                    break;
                }

                // TODO 04: 범위, 장애물, 방문 여부를 검사합니다.
                foreach (var (right, up) in directions)
                {
                    int toX = from.X + right;
                    int toY = from.Y + up;
                    GridPosition to = new(toX, toY);
                    if ((uint)toX >= width || (uint)toY >= height || blocked.Contains(to) || visited.Contains(to))
                    {
                        continue;
                    }

                    frontier.Enqueue(to);
                    visited.Add(to);
                    cameFrom[to] = from;
                }
            }

            if (!visited.Contains(goal))
            {
                return new(visitedOrder, new());
            }

            // TODO 05: cameFrom을 따라 최단 경로를 복원합니다.
            List<GridPosition> path = new() { goal };
            for (var to = goal; to != start; to = path[^1])
            {
                path.Add(cameFrom[to]);
            }
            path.Reverse();

            return new PathSearchResult(visitedOrder, path);
        }
    }
}
