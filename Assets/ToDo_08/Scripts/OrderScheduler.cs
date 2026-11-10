using System.Collections.Generic;
using AlgoLesson8;

namespace AlgoLesson8.Student
{
    /// <summary>
    /// 긴급도, 남은 시간, 보상 순서로 다음 주문을 선택합니다.
    /// </summary>
    public sealed class OrderScheduler : IOrderScheduler
    {
        public int SelectNextOrder(
            IReadOnlyList<WorkOrder> orders,
            float currentTime)
        {
            if (orders == null || orders.Count == 0)
                return -1;

            // TODO 08: bestOrder를 첫 주문으로 초기화하세요.
            WorkOrder best = orders[0];

            for (int i = 0; i < orders.Count; i++)
            {
                WorkOrder candidate = orders[i];

                // TODO 09: Priority가 높은 주문을 우선하세요.
                // TODO 10: Priority가 같으면 Deadline이 짧은 주문을 우선하세요.
                // TODO 11: 그래도 같으면 Reward가 높은 주문을 우선하세요.
                // TODO 12: 마지막 동률은 Id가 작은 주문을 선택하세요.
                bool b1 = candidate.Priority == best.Priority;
                bool b2 = candidate.Deadline == best.Deadline;
                bool b3 = candidate.Reward == best.Reward;

                bool better =
                    candidate.Priority > best.Priority ||
                    (b1 && candidate.Deadline < best.Deadline) ||
                    (b1 && b2 && candidate.Reward > best.Reward) ||
                    (b1 && b2 && b3 && candidate.Id < best.Id);

                if (better)
                {
                    best = candidate;
                }
            }

            return best.Id;
        }
    }
}
