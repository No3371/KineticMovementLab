using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IServer
{
    event System.Action<IResponse> OnResponseReceived;
    void Send (IPacket packet);
}

public class Server : IServer
{
    public event Action<IResponse> OnResponseReceived;

    public void Send(IPacket packet)
    {
        throw new NotImplementedException();
    }
}

public interface IPacket
{
    long Tick { get; set; }
}

public interface IResponse : IPacket
{
    long ServerTick { get; set; }
}