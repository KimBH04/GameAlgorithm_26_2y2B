using System.Collections.Generic;
using UnityEngine;

public sealed class WarehouseData : MonoBehaviour, IWarehouseData
{
    private readonly Queue<ForkliftBox> incomingQueue = new();

    private readonly Queue<ForkliftBox> outgoingQueue = new();

    private readonly Dictionary<int, Stack<ForkliftBox>> stacks = new();

    public int IncomingCount => incomingQueue.Count;

    public int OutgoingCount => outgoingQueue.Count;

    public void Clear()
    {
        incomingQueue.Clear();
        outgoingQueue.Clear();
        stacks.Clear();
    }

    public void EnqueueIncoming(ForkliftBox box)
    {
        if (box == null)
        {
            return;
        }

        incomingQueue.Enqueue(box);
    }

    public ForkliftBox PeekIncoming()
    {
        if (incomingQueue.Count == 0)
        {
            return null;
        }

        return incomingQueue.Peek();
    }

    public ForkliftBox DequeueIncoming()
    {
        if (incomingQueue.Count == 0)
        {
            return null;
        }

        return incomingQueue.Dequeue();
    }

    public ForkliftBox[] GetIncomingItems()
    {
        return incomingQueue.ToArray();
    }

    public void EnqueueOutgoing(ForkliftBox box)
    {
        if (box == null)
        {
            return;
        }

        outgoingQueue.Enqueue(box);
    }

    public ForkliftBox PeekOutgoing()
    {
        if (outgoingQueue.Count == 0)
        {
            return null;
        }

        return outgoingQueue.Peek();
    }

    public ForkliftBox DequeueOutgoing()
    {
        if (outgoingQueue.Count == 0)
        {
            return null;
        }

        return outgoingQueue.Dequeue();
    }

    public ForkliftBox[] GetOutgoingItems()
    {
        return outgoingQueue.ToArray();
    }

    public int GetStackCount(int stackIndex)
    {
        return GetOrCreateStack(stackIndex).Count;
    }

    public void PushStack(int stackIndex, ForkliftBox box)
    {
        if (box == null)
        {
            return;
        }

        GetOrCreateStack(stackIndex).Push(box);
    }

    public ForkliftBox PeekStack(int stackIndex)
    {
        Stack<ForkliftBox> stack = GetOrCreateStack(stackIndex);

        if (stack.Count == 0)
        {
            return null;
        }

        return stack.Peek();
    }

    public ForkliftBox PopStack(int stackIndex)
    {
        Stack<ForkliftBox> stack = GetOrCreateStack(stackIndex);

        if (stack.Count == 0)
        {
            return null;
        }

        return stack.Pop();
    }

    private Stack<ForkliftBox> GetOrCreateStack(int stackIndex)
    {
        if (!stacks.TryGetValue(stackIndex, out var stack))
        {
            stack = new();
            stacks.Add(stackIndex, stack);
        }

        return stack;
    }
}
