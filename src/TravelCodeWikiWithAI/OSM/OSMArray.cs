namespace TravelCodeWikiWithAI.Data.OSM;
using System.Collections;


public class OSMArray<T> : IEnumerator<T>, IEnumerable<T>
    where T : OSMBaseData
{
    public OSMArray(PbfFileDataSource dataSource)
    {
        _dataSource = dataSource;
        dataObjs = new List<object>(_dataSource.GetDataBlockObject<T>());
    }

    private PbfFileDataSource _dataSource;

    private List<object> dataObjs;

    public bool MoveNext()
    {
        tempIndex++;
        if (tempIndex < tempData.Count)
        {
            return true;
        }
        else
        {
            index++;
            tempIndex = 0;
            if (index < dataObjs.Count)
            {
                OSMData d = _dataSource.GetDataFormBlockObject(dataObjs[index]);
                tempData = new List<T>();
                if (typeof(T) == typeof(OSMRelations))
                {
                    foreach (OSMRelations r in d.Relations)
                    {
                        tempData.Add(r as T);
                    }
                }
                else if (typeof(T) == typeof(OSMWay))
                {
                    foreach (OSMWay w in d.Ways)
                    {
                        tempData.Add(w as T);
                    }
                }
                else if (typeof(T) == typeof(OSMNode))
                {
                    foreach (OSMNode n in d.Nodes)
                    {
                        tempData.Add(n as T);
                    }
                }

                return tempData.Count != 0;
            }
            else
            {
                return false;
            }
        }
    }

    public void Reset()
    {
        throw new NotImplementedException();
    }

    object? IEnumerator.Current => Current;

    public T Current
    {
        get
        {
            if (tempIndex == -1)
            {
                return default(T);
            }

            if (tempIndex >= tempData.Count)
            {
                return default(T);
            }

            return tempData[tempIndex];
        }
    }

    public void Dispose()
    {
        tempData = new List<T>();
        index = -1;
        tempIndex = -1;
    }

    private int index = -1;

    private int tempIndex = -1;

    private List<T> tempData = new List<T>();

    public IEnumerator<T> GetEnumerator()
    {
        return this;
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }

    public OSMArray<T> GetCopy()
    {
        return new OSMArray<T>(_dataSource);
    }

    public HashSet<long> GetIds()
    {
        OSMArray<T> other = GetCopy();
        HashSet<long> ids = new HashSet<long>(other.Select<OSMBaseData, long>((OSMBaseData t) => t.Id));
        return ids;
    }

    public void ProBlock(Action<OSMData> func)
    {
        List<bool> b = new List<bool>();
        foreach (object o in dataObjs)
        {
            OSMData d = _dataSource.GetDataFormBlockObject(o);
            ThreadPool.QueueUserWorkItem(delegate
            {
                func(d);
                lock (b)
                {
                    b.Add(true);
                }
            });
        }

        Func<bool> a = delegate
        {
            lock (b)
            {
                return b.Count != dataObjs.Count;
            }
        };
        while (a())
        {
            Thread.Sleep(1000);
        }
    }

    public int Count
    {
        get
        {
            int result = 0;
            foreach (object o in dataObjs)
            {
                result += _dataSource.GetDataBlockSize<T>(o);
            }

            return result;
        }
    }

    public int GetIndex()
    {
        return index;
    }
}