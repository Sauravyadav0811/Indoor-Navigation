using System;
using System.Collections.Generic;
using UnityEngine;

public class UnityMainThreadDispatcher : MonoBehaviour
{
    private static readonly Queue<Action> executionQueue = new Queue<Action>();
    private static UnityMainThreadDispatcher instance;

    public static UnityMainThreadDispatcher Instance()
    {
        if (instance == null)
        {
            var obj = new GameObject("UnityMainThreadDispatcher");
            instance = obj.AddComponent<UnityMainThreadDispatcher>();
            DontDestroyOnLoad(obj);
        }
        return instance;
    }

    void Update()
    {
        lock (executionQueue)
        {
            while (executionQueue.Count > 0)
            {
                executionQueue.Dequeue()?.Invoke();
            }
        }
    }

    public void Enqueue(Action action)
    {
        if (action == null) return;

        lock (executionQueue)
        {
            executionQueue.Enqueue(action);
        }
    }
}
