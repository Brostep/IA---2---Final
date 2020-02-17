using System;
using System.Collections.Generic;

public class PriorityQueue<Data>
{
    public bool IsEmpty { get { return data.Count <= 0; } }

    private List<Tuple<Data, float>> data;

    public PriorityQueue()
    {
        data = new List<Tuple<Data, float>>();
    }

    public void Enqueue(Data data, float priority)
    {
        Enqueue(new Tuple<Data, float>(data, priority));
    }

    private void Enqueue(Tuple<Data, float> pair)
    {
        data.Add(pair);

        var currIndex = data.Count - 1;
        if (currIndex == 0) return;

        var parentIndex = (currIndex - 1) / 2;

        while (data[currIndex].Item2 < data[parentIndex].Item2)
        {
            Swap(currIndex, parentIndex);

            currIndex = parentIndex;
            parentIndex = (currIndex - 1) / 2;
        }
    }

    public Data Dequeue()
    {
        return DequeuePair().Item1;
    }

    private Tuple<Data,float> DequeuePair()
    {
        var min = data[0];
        data.RemoveAt(0);

        if (data.Count == 0) return min;

        int curIndex = 0;
        int leftIndex = 1;
        int rightIndex = 2;
        int minorIndex;

        if (data.Count > rightIndex)
            minorIndex = data[leftIndex].Item2 < data[rightIndex].Item2 ? leftIndex : rightIndex;
        else if (data.Count > leftIndex)
            minorIndex = leftIndex;
        else return min;

        while(data[minorIndex].Item2 < data[curIndex].Item2)
        {
            Swap(minorIndex, curIndex);

            curIndex = minorIndex;
            leftIndex = curIndex * 2 + 1;
            rightIndex = curIndex * 2 + 2;

            if (data.Count > rightIndex)
                minorIndex = data[leftIndex].Item2 < data[rightIndex].Item2 ? leftIndex : rightIndex;
            else if (data.Count > leftIndex)
                minorIndex = leftIndex;
            else return min;
        }

        return min;
    }

    private void Swap(int i1, int i2)
    {
        var aux = data[i1];
        data[i1] = data[i2];
        data[i2] = aux;
    }
}
