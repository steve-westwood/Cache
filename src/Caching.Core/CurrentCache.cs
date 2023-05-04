public class CurrentCache
{
    private CacheEntry Head;
    private CacheEntry Tail;

    public CurrentCache()
    {
        Head = new CacheEntry();
        Tail = new CacheEntry();
        Head.After = Tail;
        Tail.Before = Head;
    }

    public void AddToTop(CacheEntry node) {
        node.After = Head.After;
        if (Head.After?.Before != null) Head.After.Before = node;
        node.Before = Head;
        Head.After = node;
    }

    public void RemoveNode(CacheEntry node) {
        if (node.Before?.After != null ) node.Before.After = node.After;
        if (node.After?.Before != null ) node.After.Before = node.Before;
        node.After = null;
        node.Before = null;
    }

    public CacheEntry? RemoveCacheEntry() {
        var node = Tail?.Before;
        if (node != null ) RemoveNode(node);
        return node;
    }
}
