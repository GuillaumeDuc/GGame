using System.Collections.Generic;

public static class UnitList
{
    ///// Ships ///// 
    static Ship quadrireme = new Ship("Quadrireme", new ResourceCollection() {
        new Resource(Resource.TypeResource.Metal, 1000),
        new Resource(Resource.TypeResource.Gaz, 500),
    });
    static Ship quinquereme = new Ship("Quinquereme", new ResourceCollection() {
        new Resource(Resource.TypeResource.Metal, 750),
        new Resource(Resource.TypeResource.Gaz, 400)
    });
    ///// Defense ///// 
    static Defense canon = new Defense("Canon", new Resource(Resource.TypeResource.Metal, 500));
    ///// Troop ///// 
    static Troop infantry = new Troop("Infantry", new ResourceCollection() {
        new Resource(Resource.TypeResource.Metal, 200),
        new Resource(Resource.TypeResource.Water, 100)
    });

    public static List<Ship> getShips()
    {
        return new List<Ship>() { quadrireme, quinquereme };
    }

    public static List<Defense> getDefenses()
    {
        return new List<Defense>() { canon };
    }

    public static List<Troop> getTroops()
    {
        return new List<Troop>() { infantry };
    }
}
