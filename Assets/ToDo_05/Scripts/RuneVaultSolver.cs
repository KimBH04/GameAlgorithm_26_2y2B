using System.Collections.Generic;
using AlgoCourse.Lesson5;

namespace AlgoCourse.StudentWork
{
    public sealed class RuneVaultSolver : IRuneVaultSolver
    {
        private RunePuzzle puzzle;
        private readonly List<int> selectedRuneIDs = new();
        private readonly HashSet<int> usedRuneIDs = new();
        private readonly List<RuneSearchStep> steps = new();
        private int currentSum;

        public RuneSolveResult Solve(RunePuzzle targetPuzzle)
        {
            // TODO 01: 선택 상태를 초기화합니다.
            puzzle = targetPuzzle;
            selectedRuneIDs.Clear();
            usedRuneIDs.Clear();
            steps.Clear();
            currentSum = 0;

            // TODO 02: Search(0)을 호출합니다.
            bool solved = Search(0);

            if (!solved)
            {
                steps.Add(
                    new RuneSearchStep(
                        RuneSearchAction.Failed,
                        -1,
                        0,
                        currentSum,
                        "가능한 룬 조합을 찾지 못했습니다."
                    )
                );
            }

            // TODO 03: 탐색 단계와 정답을 반환합니다.
            return new RuneSolveResult(
                new List<RuneSearchStep>(steps),
                solved ? new List<int>(selectedRuneIDs) : new List<int>());
        }

        // TODO 04: 재귀 종료 조건을 작성합니다.
        // TODO 05: 후보를 선택하고 다음 깊이를 호출합니다.
        // TODO 06: 실패하면 선택을 취소합니다.
        private bool Search(int depth)
        {
            if (depth == puzzle.SlotCount)
            {
                if (currentSum == puzzle.TargetSum)
                {
                    steps.Add(
                        new RuneSearchStep(
                            RuneSearchAction.Solved,
                            -1,
                            depth,
                            currentSum,
                            "모든 조건을 만족했습니다. 금고가 열립니다!"
                        )
                    );
                    return true;
                }

                return false;
            }

            foreach (RuneData rune in puzzle.Runes)
            {
                if (!CanChoose(rune, depth, out string rejectReason))
                {
                    steps.Add(
                        new RuneSearchStep(
                            RuneSearchAction.Reject,
                            rune.Id,
                            depth,
                            currentSum,
                            rejectReason
                        )
                    );
                }

                Choose(rune, depth);

                if (Search(depth + 1))
                {
                    return true;
                }

                Cancel(rune, depth);
            }

            return false;
        }

        private RuneData FindRune(int runeID)
        {
            foreach (RuneData rune in puzzle.Runes)
            {
                if (rune.Id == runeID)
                {
                    return rune;
                }
            }

            return null;
        }

        private bool CanChoose(RuneData rune, int depth, out string reason)
        {
            if (usedRuneIDs.Contains(rune.Id))
            {
                reason = $"{rune.DisplayName} : 이미 사용한 룬 입니다.";
                return false;
            }

            if (currentSum + rune.Value > puzzle.TargetSum)
            {
                reason = $"{rune.DisplayName} : 합계가 {puzzle.TargetSum}을 초과합니다.";
                return false;
            }

            bool isLastSlot = depth == puzzle.SlotCount - 1;
            if (puzzle.RequireLightLast && isLastSlot && rune.Element != RuneElement.Light)
            {
                reason = $"빛 룬은 마지막 슬롯을 위해 남겨야 합니다.";
                return false;
            }

            if (puzzle.ForbidFireWaterAdjacency && selectedRuneIDs.Count > 0)
            {
                RuneData previous = FindRune(selectedRuneIDs[^1]);
                bool forbiddenPair =
                    previous.Element == RuneElement.Fire && rune.Element == RuneElement.Water ||
                    previous.Element == RuneElement.Water && rune.Element == RuneElement.Fire;

                if (forbiddenPair)
                {
                    reason = "불 룬과 물 룬은 서로 붙을 수 없습니다.";
                    return false;
                }
            }

            reason = string.Empty;
            return true;
        }

        private void Choose(RuneData rune, int depth)
        {
            selectedRuneIDs.Add(rune.Id);
            usedRuneIDs.Add(rune.Id);
            currentSum += rune.Value;
            steps.Add(
                new RuneSearchStep(
                    RuneSearchAction.Choose,
                    rune.Id,
                    depth,
                    currentSum,
                    $"슬롯 {depth + 1}에 {rune.DisplayName} 룬을 선택했습니다."
                )
            );
        }

        private void Cancel(RuneData rune, int depth)
        {
            selectedRuneIDs.RemoveAt(selectedRuneIDs.Count - 1);
            usedRuneIDs.Remove(rune.Id);
            steps.Add(
                new RuneSearchStep(
                    RuneSearchAction.Backtrack,
                    rune.Id,
                    depth,
                    currentSum,
                    $"{rune.DisplayName} 선택으로해결할 수 없어서 되돌아갑니다."
                )
            );
        }
    }
}
