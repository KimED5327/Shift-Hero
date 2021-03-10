
using System.Collections.Generic;

public enum Result { RUNNING, SUCCESS, FAILURE };

public class BTNode
{
    public Result _result;

    public virtual Result Execute()
    {
        return Result.FAILURE;
    }
}
