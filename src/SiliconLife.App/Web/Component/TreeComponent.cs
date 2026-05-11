// Copyright (c) 2026 Hoshino Kennji
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
//     http://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.

namespace SiliconLife.App.Web.Component;

/// <summary>
/// Tree view component
/// </summary>
public class TreeComponent : ComponentBase
{
    private readonly List<TreeNode> _nodes = new();

    /// <summary>
    /// Add a tree node
    /// </summary>
    public TreeComponent AddNode(string label, List<TreeNode>? children = null, bool expanded = false)
    {
        _nodes.Add(new TreeNode { Label = label, Children = children ?? new List<TreeNode>(), Expanded = expanded });
        return this;
    }

    public override H Render()
    {
        var tree = H.Ul();

        if (!string.IsNullOrEmpty(Id))
            tree.Id(Id);

        var classes = new List<string> { "tree" };
        if (!string.IsNullOrEmpty(Class))
            classes.Add(Class);
        tree.Class(string.Join(" ", classes));

        if (Style != null && Style.HasInlineStyles)
            tree.Style(Style);

        foreach (var kvp in Attributes)
        {
            tree.Attr(kvp.Key, kvp.Value);
        }

        foreach (var node in _nodes)
        {
            tree.Add(RenderNode(node));
        }

        return tree;
    }

    private H RenderNode(TreeNode node)
    {
        var li = H.Li().Class("tree-node");

        var hasChildren = node.Children.Count > 0;
        var nodeContent = H.Div().Class("tree-node-content");

        if (hasChildren)
        {
            var toggle = H.Span(node.Expanded ? "▼" : "▶").Class("tree-toggle");
            nodeContent.Add(toggle);
        }

        nodeContent.Add(H.Span(H.Escape(node.Label)).Class("tree-label"));
        li.Add(nodeContent);

        if (hasChildren)
        {
            var childUl = H.Ul().Class("tree-children" + (node.Expanded ? "" : " collapsed"));
            foreach (var child in node.Children)
            {
                childUl.Add(RenderNode(child));
            }
            li.Add(childUl);
        }

        return li;
    }
}

/// <summary>
/// Tree node data
/// </summary>
public class TreeNode
{
    public string Label { get; set; } = "";
    public List<TreeNode> Children { get; set; } = new();
    public bool Expanded { get; set; } = false;
}
