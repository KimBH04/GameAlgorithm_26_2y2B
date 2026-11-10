using System.Collections.Generic;
using AlgoLesson8;

namespace AlgoLesson8.Student
{
    /// <summary>
    /// 활성 기계 중 분당 처리량이 가장 낮은 병목 기계를 찾습니다.
    /// 
    /// 생산 라인에서 병목 기계를 찾는 분석기
    /// 
    /// 생산 라인 전체의 처리 속도는 가장 느린 기계에 의해 결정
    /// 목록을 한 번 훑으면서 현재까지의 최소값을 갱신하는 선형 탐색(Linear Scan)을 한다.
    /// 
    /// [복잡도]
    /// - 시간 : O(n)
    /// - 공간 : O(1)
    /// </summary>
    public sealed class BottleneckAnalyzer : IBottleneckAnalyzer
    {
        public string FindBottleneck(IReadOnlyList<MachineSnapshot> machines)
        {
            if (machines == null || machines.Count == 0)
                return string.Empty;

            MachineSnapshot best = null;

            foreach (MachineSnapshot machine in machines)
            {
                // TODO 13: 비활성 기계는 검사에서 제외하세요.
                if (!machine.Enabled)
                {
                    continue;
                }

                // TODO 14: ItemsPerMinute가 가장 낮은 기계를 찾으세요.
                // TODO 15: 처리량이 같으면 Id가 작은 기계를 선택하세요.
                if (best == null ||
                    machine.ItemsPerMinute < best.ItemsPerMinute ||
                    (machine.ItemsPerMinute == best.ItemsPerMinute && string.CompareOrdinal(machine.Id, best.Id) < 0))
                {
                    best = machine;
                }
            }

            // TODO 16: 선택된 기계의 DisplayName을 반환하세요.
            return best == null ? string.Empty : best.DisplayName;
        }
    }
}
