using Caching.Core;

namespace Caching.Tests;

public class CachingTests
{
    public Cache cache => Cache.Instance;
    [Fact]
    public void GivenACacheItemCanBeCachedAndRetrivedFromCache()
    {
        cache.Flush();
        var cacheItem = "dave";
        cache.Set("cacheItem",cacheItem);
        var itemFromCache = cache.Get<string>("cacheItem");
        Assert.Equal(itemFromCache, cacheItem);
    }
    [Fact]
    public void GivenOneHundredAndOneItemsCachedEvictItemOne()
    {
        cache.Flush();
        for(var i = 1; i < 102; i++) 
        {
            var item = i.ToString();
            cache.Set(item,item);
        }
        var One = cache.Get<string>("1");
        var Two = cache.Get<string>("2");
        var OneHundred = cache.Get<string>("100");
        var OneHundredAndOne = cache.Get<string>("101");
        var OneHundredAndTwo = cache.Get<string>("102");
        Assert.Null(One);
        Assert.Equal("2", Two);
        Assert.Equal("100", OneHundred);
        Assert.Equal("101", OneHundredAndOne);
        Assert.Null(OneHundredAndTwo);
    }
    [Fact]
    public void GivenTheSameCacheKeyTwiceTheCacheOverwritesTheCacheEntry()
    {
        cache.Flush();
        var cacheKey = "testKey";
        cache.Set(cacheKey,1);
        cache.Set(cacheKey,2);
        var cacheValue = cache.Get<int>(cacheKey);
        Assert.Equal(2,cacheValue);
    }
    [Fact]
    public void GivenAComplexTypeCacheReturnsComplexType()
    {
        cache.Flush();
        var complexType = new ComplexType() 
            {
                StringType = "testString", 
                ObjectType = new ChildComplexType() { NumberType = 101 }, 
                ArrayType = new ChildComplexType[] { 
                    new ChildComplexType() {NumberType = 1}, 
                    new ChildComplexType() {NumberType = 2},
                    new ChildComplexType() {NumberType = 3} 
                }
            };
        cache.Set("cacheKey", complexType);
        var actual = cache.Get<ComplexType>("cacheKey");
        Assert.Equivalent(complexType, actual);
    }

    private class ComplexType 
    {
        public string? StringType { get; set; }
        public ChildComplexType? ObjectType { get; set; }
        public ChildComplexType[]? ArrayType { get; set; }
        public string? NullType { get; set; }
    }
    private class ChildComplexType
    {
        public int? NumberType { get; set; } 
    }
}