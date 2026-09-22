using System.Collections.Generic;
using AlgoCourse.Lesson3;

namespace AlgoCourse.StudentWork
{
    public sealed class PandemicInfectionAlgorithm : ICityInfectionAlgorithm
    {
        private const int MaximumInfectionLevel = 3;

        private readonly Dictionary<int, int[]> graph = new();

        private readonly Dictionary<int, int> infectionLevels = new();

        private readonly Queue<int> infectionQueue = new();
    
        private readonly HashSet<int> outbreakCities = new();

        // TODO 01: 도시 그래프, 감염 단계, 감염 요청 Queue를 생성합니다.
        // TODO 02: 한 번의 연쇄 감염에서 Outbreak한 도시를 기록합니다.
        public int PendingCount => infectionQueue.Count;
        public int OutbreakCount { get; private set; }

        public void Initialize(IReadOnlyDictionary<int, int[]> cityGraph)
        {
            // TODO 03: 그래프를 복사하고 모든 도시의 감염 단계를 0으로 만듭니다.
            graph.Clear();
            infectionLevels.Clear();
            infectionQueue.Clear();
            outbreakCities.Clear();

            OutbreakCount = 0;

            foreach (var city in cityGraph)
            {
                graph.Add(city.Key, city.Value);
                infectionLevels.Add(city.Key, 0);
            }
        }

        public bool QueueInfection(int cityId)
        {
            // TODO 04: 존재하는 도시의 감염 요청을 Enqueue합니다.
            if (!graph.ContainsKey(cityId))
            {
                return false;
            }
            
            if (infectionQueue.Count == 0)
            {
                outbreakCities.Clear();
            }

            infectionQueue.Enqueue(cityId);

            return true;
        }

        public CityInfectionStep ProcessNext()
        {
            // TODO 05: Dequeue 후 감염 단계를 증가시키거나 Outbreak를 처리합니다.
            if (infectionLevels.Count == 0)
            {
                return new(-1, 0, false, false);
            }

            int cityId = infectionQueue.Dequeue();
            int currentLevel = infectionLevels[cityId];

            if (currentLevel < MaximumInfectionLevel)
            {
                int nextLevel = currentLevel + 1;
                infectionLevels[cityId] = nextLevel;
                return new(cityId, nextLevel, false, true);
            }

            if (!outbreakCities.Add(cityId))
            {
                return new(cityId, currentLevel, false, false);
            }

            OutbreakCount++;

            foreach (int neighborId in graph[cityId])
            {
                infectionQueue.Enqueue(neighborId);
            }

            return new CityInfectionStep(cityId, currentLevel, true, true);
        }

        public int GetInfectionLevel(int cityId)
        {
            // TODO 06: 도시의 현재 감염 단계를 반환합니다.
            return infectionLevels.TryGetValue(cityId, out var l) ? l : 0;
        }
    }
}
