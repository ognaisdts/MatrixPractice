using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class IRule  {
    protected Board board = null;
    public abstract void Run();
}
