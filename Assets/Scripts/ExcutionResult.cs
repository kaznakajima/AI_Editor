using UnityEngine;
using System;
using System.Collections;

namespace BehaviourTrees
{
    public class ExecutionResult
    {
        public enum RESULT_TYPE {
            BOOLEAN,
            INTEGER
        }

        public RESULT_TYPE type;
        public bool booleanResult;
        public int integerResult;

        public ExecutionResult(Boolean _result)
        {
            type = RESULT_TYPE.BOOLEAN;
            booleanResult = _result;
        }

        public ExecutionResult(int _result)
        {
            type = RESULT_TYPE.INTEGER;
            integerResult = _result;
        }
    }
}

