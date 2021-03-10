using System.Collections.Generic;


public class BTSelector : BTNode
{
    List<BTNode> _nodes = new List<BTNode>();

    public BTSelector(List<BTNode> nodes)
    {
        this._nodes = nodes;
    }

    public override Result Execute()
    {
        foreach (var node in _nodes)
        {
            switch (node.Execute())
            {
                case Result.RUNNING:
                    _result = Result.RUNNING;
                    return _result;

                case Result.SUCCESS:
                    _result = Result.SUCCESS;
                    return _result;

                case Result.FAILURE:
                    break;
            }
        }
        _result = Result.FAILURE;
        return _result;
    }
}
