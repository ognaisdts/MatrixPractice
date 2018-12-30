using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RuleController {

    private IRule curRule = null;

    public void SetRule(IRule _rule)
    {
        curRule = _rule;
    }

    public void ProcessRule()
    {
        curRule.Run();
    }

}
