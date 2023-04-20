using System;
using System.Collections.Generic;

public class NodeHandler {

    internal class Node {

        public int Id { get; }
        public string Name { get; set; }
        public List<Node> Children { get; }
        public Node Parent { get; }

        public Node(int id, string name)
        {
            this.Id = id;
            this.Name = name;
        }

        public void setChildrenList(List<Node> newChildren)
        {
            this.Children = newChildren;
            foreach(Node child in newChildren)
            {
                child.addParent(this);
            }
        }

        public void removeParent()
        {
            this.Parent = null;
        }


        public void addChild(Node child)
        {
            this.Children.Add(child);
            child.addParent(this);
        }

        public void addParent(Node newParent)
        {
            if(this.Parent != null)
            {
                this.Parent.removeChild(this,false);
            }
            this.Parent = newParent;
        }


        public void removeChild(Node child, bool deep = true)
        {
            if(!this.Children.Contains(child))
            {
                return;
            }

            this.Children.Remove(child);
            if(deep)
            {
                child.removeParent(this);
            }
        }

        public bool isRoot()
        {
            return this.Parent == null;
        }

        public override string ToString()
        {
            List<int> childIDs = new List<int>();
            foreach(Node child in this.Children)
            {
                childIDs.Add(child.Id);
            }

            return $"Id={this.Id};Name={this.Name};Parent={this.Parent.Id};Children={string.Join(",", childIDs)}";
        }

        public void removeNode()
        {
            foreach(Node child in this.Children)
            {
                child.removeParent();
            }
            if(this.Parent != null)
            {
                this.Parent.removeChild(this,false);
                this.Parent = null;
            }
            
        }

    }

    private Dictionary<int, Node> nodes;

    public NodeHandler()
    {
        nodes = new Dictionary<int, Node>();
    }

    public Node addNode(int id, string name, Node parent=null)
    {
        if(nodes.ContainsKey(id))
        {
            throw new ArguementException("Node with this ID already exists");
        }

        Node node = new Node(id, name);
        nodes.Add(id,node);
        if(parent!=null)
        {
            parent.addChild(node);
        }
        return node;
    }

    public void removeNode(int id)
    {
        if(!nodes.ContainsKey(id))
        {
            throw new ArguementException($"No node with this ID exists: {id}");
        }

        Node node = nodes[id];
        nodes.Remove(id);
        node.removeNode();
    }

    public Node updateName(int id, string newName)
    {
        if(!nodes.ContainsKey(id))
        {
            throw new ArguementException($"No node with this ID exists: {id}");
        }

        nodes[id].Name = newName;
    }

    public Node getName(int id)
    {
        if(!nodes.ContainsKey(id))
        {
            throw new ArguementException($"No node with this ID exists: {id}");
        }

        return nodes[id];
    }

    public void setParent(int parentID, int childID)
    {
        if(!nodes.ContainsKey(parentID) || !nodes.ContainsKey(childID))
        {
            throw new ArguementException($"One or both of the provided IDs cannot be found in the list of Nodes: {parentID}, {childID}");
        }

        Node parent = nodes[parentID];
        Node child = nodes[childID];
        parent.addChild(child);
    }

    public Node[] getTreeBranceStyle(int rootID)
    {
        if(!nodes.ContainsKey(rootID))
        {
            throw new ArguementException($"No node with this ID exists: {id}");
        }

        Node root = nodes[rootID];
        List<Node> nodesTree = new List<Node>();
        getSubTreeBranch(root, nodesTree);
        return nodesTree.ToArray();
    }

    public void getSubTreeBranch(Node node, List<Node> nodesTree)
    {
        nodesList.Add(node);
        foreach(Node child in Node.Children)
        {
            getSubTreeBranch(child, nodesTree);
        }
    }

    public Node[] getTreeTierStyle(int RootID)
    {
        if(!nodes.ContainsKey(rootID))
        {
            throw new ArguementException($"No node with this ID exists: {id}");
        }

        Node root = nodes[id];
        List<Node> nodesTree = new List<Node>();
        nodesTree.Add(root);
        if(root.Children.Any())
        {
            getSubTier(root.Children,nodesTree);
        }

        return nodesTree.ToArray();
        
    }

    public void getSubTier(List<Node> children, List<Node> nodeTree)
    {
        if(!children.Any())
        {
            return;
        }

        nodeTree.AddRange(children);
        List<Node> newChildren = List<Node>();
        foreach(Node child in Children)
        {
            if(child.Children.Any())
            {
                newChildren.AddRange(child.Children);
            }
        }

        getSubTier(newChildren, nodeTree);
    }

    public string getNodeAsString(int id)
    {
        if(!nodes.ContainsKey(id))
        {
            throw new ArguementException($"No node with this ID exists: {id}");
        }

        return nodes[id].ToString();
    }



}