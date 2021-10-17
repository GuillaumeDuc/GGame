using System.Collections.Generic;

public static class FactoryList
{
    ///// Metal ///// 
    public static Factory metalFactory = new Factory("Mining Factory", new Resource(Resource.TypeResource.Metal, 10));
    ///// Gaz ///// 
    public static Factory gazFactory = new Factory("Gaz Extractor", new Resource(Resource.TypeResource.Metal, 10));
    ///// Water ///// 
    public static Factory waterFactory = new Factory("Water Extractor", new Resource(Resource.TypeResource.Metal, 10));

    public static List<Factory> GetFactories()
    {
        return new List<Factory>() { metalFactory, gazFactory, waterFactory };
    }

    public static List<Factory> GetDefaultFactories(long size = 100)
    {
        return new List<Factory>() {
            new Factory("Mining Exploitation", new Resource(Resource.TypeResource.Metal, size)),
            new Factory("Gaz Extractor", new Resource(Resource.TypeResource.Gaz, (int)size / 2)),
            new Factory("Water Extractor", new Resource(Resource.TypeResource.Water, (int)size / 3)),
        };
    }
}
