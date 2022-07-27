using System;
using System.Collections.Generic;

[Serializable]
public class GoogleFitData
{
    public InsertedDataPoint[] insertedDataPoint;
    public DeletedDataPoint[] deletedDataPoint;
    public string nextPageToken;
    public string dataSourceId;
}

[Serializable]
public class DeletedDataPoint
{
    public long startTimeNanos;
    public long endTimeNanos;
    public string dataTypeName;
    public string originDataSourceId;
    public Value[] value;
    public string modifiedTimeMillis;
}

[Serializable]
public class InsertedDataPoint
{
    public long startTimeNanos;
    public long endTimeNanos;
    public string dataTypeName;
    public string originDataSourceId;
    public Value[] value;
    public string modifiedTimeMillis;
}

[Serializable]
public class Value
{
    public int intVal;
    public object[] mapVal;
}

