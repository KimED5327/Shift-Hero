using System.Collections.Generic;

public class BTSequence : BTNode
{
    List<BTNode> _nodes = new List<BTNode>();

    public BTSequence(List<BTNode> nodes)
    {
        this._nodes = nodes;
    }

    public override Result Execute()
    {
        bool isAnyNodeRunning = false;

        foreach(var node in _nodes)
        {
            switch (node.Execute())
            {
                case Result.RUNNING:
                    isAnyNodeRunning = true;
                    break;

                case Result.SUCCESS:
                    break;

                case Result.FAILURE:
                    _result = Result.FAILURE;
                    return _result;
            }
        }

        _result = isAnyNodeRunning ? Result.RUNNING : Result.SUCCESS;
        return _result;
    }
}
