using System;
using System.Collections.Generic;

public class PriorityQueue<Data> {
    public bool IsEmpty { get { return _data.Count <= 0; } }

    private List<Tuple<Data, float>> _data;

    public PriorityQueue() {
        _data = new List<Tuple<Data, float>>();
    }

    public void Enqueue(Data data, float priority) {
        Enqueue(new Tuple<Data, float>(data, priority));
    }

    private void Enqueue(Tuple<Data, float> pair) {
        _data.Add(pair);

        var currIndex = _data.Count - 1;

        if (currIndex == 0)
            return;

        var parentIndex = (currIndex - 1) / 2;

        while (_data[currIndex].Item2 < _data[parentIndex].Item2) {
            Swap(currIndex, parentIndex);
            currIndex = parentIndex;
            parentIndex = (currIndex - 1) / 2;
        }
    }

    public Data Dequeue() {
        return DequeuePair().Item1;
    }

    private Tuple<Data,float> DequeuePair() {
        var min = _data[0];
        _data.RemoveAt(0);

        if (_data.Count == 0)
            return min;

        int curIndex = 0;
        int leftIndex = 1;
        int rightIndex = 2;
        int minorIndex;

        if (_data.Count > rightIndex)
            minorIndex = _data[leftIndex].Item2 < _data[rightIndex].Item2 ? leftIndex : rightIndex;
        else if (_data.Count > leftIndex)
            minorIndex = leftIndex;
        else
            return min;

        while(_data[minorIndex].Item2 < _data[curIndex].Item2) {
            Swap(minorIndex, curIndex);
            curIndex = minorIndex;
            leftIndex = curIndex * 2 + 1;
            rightIndex = curIndex * 2 + 2;

            if (_data.Count > rightIndex)
                minorIndex = _data[leftIndex].Item2 < _data[rightIndex].Item2 ? leftIndex : rightIndex;
            else if (_data.Count > leftIndex)
                minorIndex = leftIndex;
            else
                return min;
        }

        return min;
    }

    private void Swap(int i1, int i2) {
        var aux = _data[i1];
        _data[i1] = _data[i2];
        _data[i2] = aux;
    }
}
