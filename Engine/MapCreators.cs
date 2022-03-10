
using RogueSharp;
using RogueSharp.MapCreation;

namespace Engine;

public interface IMapCreator
{
    public int Width { get; set; }
    public int Height { get; set; }
    public IMap CreateMap();
}



public class CaveMapCreator : IMapCreator
{
    public int Width { get; set; }
    public int Height { get; set; }
    public int FillProbability { get; set; }
    public int TotalIterations { get; set; }
    public int CutoffOfBigAreaFill { get; set; }

    public CaveMapCreator(int width, int height, int fillProbability, int totalIterations, int cutoffOfBigAreaFill)
    {
        Width = width;
        Height = height;
        FillProbability = fillProbability;
        TotalIterations = totalIterations;
        CutoffOfBigAreaFill = cutoffOfBigAreaFill;
    }

    public IMap CreateMap()
    {
        var mapCreationStrategy = new CaveMapCreationStrategy<Map>(Width, Height, FillProbability, TotalIterations, CutoffOfBigAreaFill);
        return Map.Create(mapCreationStrategy);
    }
}

public class RoomsMapCreator : IMapCreator
{
    public int Width { get; set; }
    public int Height { get; set; }
    public int MaxRooms { get; set; }
    public int MaxRoomSize { get; set; }
    public int MinRoomSize { get; set; }

    public RoomsMapCreator(int width, int height, int maxRooms, int maxRoomSize, int minRoomSize)
    {
        Width = width;
        Height = height;
        MaxRooms = maxRooms;
        MaxRoomSize = maxRoomSize;
        MinRoomSize = minRoomSize;
    }
    public IMap CreateMap()
    {
        var mapCreationStrategy = new RandomRoomsMapCreationStrategy<Map>(Width, Height, MaxRooms, MaxRoomSize, MinRoomSize);
        return Map.Create(mapCreationStrategy);
    }
}

public class RectangleMapCreator : IMapCreator
{
    public int Width { get; set; }
    public int Height { get; set; }

    public RectangleMapCreator(int width, int height)
    {
        Width = width;
        Height = height;
    }

    public IMap CreateMap()
    {
        var mapCreationStrategy = new BorderOnlyMapCreationStrategy<Map>(Width, Height);
        return Map.Create(mapCreationStrategy);
    }
}