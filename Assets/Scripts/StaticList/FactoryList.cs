using System.Collections.Generic;

public static class FactoryList
{
    ///// Metal ///// 
    public static Factory metalFactory = new Factory("Mining Exploitation", new Resource(Resource.TypeResource.Metal, 1));
    ///// Gaz ///// 
    public static Factory gazFactory = new Factory("Gaz Extractor", new Resource(Resource.TypeResource.Gaz, 1));
    ///// Water ///// 
    public static Factory waterFactory = new Factory("Water Extractor", new Resource(Resource.TypeResource.Water, 1));

    public static List<Factory> GetFactories()
    {
        return new List<Factory>() { metalFactory, gazFactory, waterFactory };
    }

    public static List<Factory> GetDefaultFactories()
    {
        return new List<Factory>() { metalFactory, gazFactory, waterFactory };
    }
}
