// The original version of this file is part of <see href="https://github.com/aimacode/aima-java"/> which is released under
// MIT License
// Copyright (c) 2015 aima-java contributors

using System.Collections.Generic;
using System.Text;

namespace Italbytz.AI.Agent;

public abstract class ObjectWithDynamicAttributes
{
    protected Dictionary<object, object> Attributes { get; set; } = new();

    public override string ToString()
    {
        return DescribeType() + DescribeAttributes();
    }

    private string DescribeAttributes()
    {
        var sb = new StringBuilder();
        sb.Append("[");
        var first = true;
        foreach (var attribute in Attributes)
        {
            if (first)
                first = false;
            else
                sb.Append(", ");

            sb.Append($"{attribute.Key}={attribute.Value}");
        }

        sb.Append(']');
        return sb.ToString();
    }

    private string DescribeType()
    {
        return GetType().Name;
    }
}