using System;

public interface IDataPersistenceRequester : IDataPersistence
{
    event Action SaveRequested;
}
