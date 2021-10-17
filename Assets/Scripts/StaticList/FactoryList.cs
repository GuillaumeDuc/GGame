using System.Collections.Generic;

public static class FactoryList
{
    ///// Metal ///// 
    static Factory metalFactory = new Factory("Mining Factory", new Resource(Resource.TypeResource.Metal, 10));
    ///// Gaz ///// 
    static Factory gazFactory = new Factory("Gaz Extractor", new Resource(Resource.TypeResource.Metal, 10));
    ///// Water ///// 
    static Factory waterFactory = new Factory("Water Extractor", new Resource(Resource.TypeResource.Metal, 10));


    public static List<Factory> getFactories()
    {
        return new List<Factory>() { metalFactory };
    }
}
