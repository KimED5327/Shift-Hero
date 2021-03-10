using System.Collections.Generic;

public class BTInverter : BTNode
{
    BTNode _node;

    public BTInverter(BTNode node)
    {
        this._node = node;
    }

    public override Result Execute()
    {

        switch (_node.Execute())
        {
            case Result.RUNNING:
                _result = Result.RUNNING;
                break;
            case Result.SUCCESS:
                _result = Result.FAILURE;
                break;
            case Result.FAILURE:
                _result = Result.SUCCESS;
                break;
        }
        
        return _result;
    }
}
