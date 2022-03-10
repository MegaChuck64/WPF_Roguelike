using Engine;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;


namespace WPF_Roguelike;

public class MainGame : BaseGame<MainWindow>
{
    public Player player;
    public ToolMop tileMap;
    public Enomy enemy;

    PathToTarget path;
    public float Scale = .75f;

    public MainGame(MainWindow window) : base(window) { }

    protected override void Start()
    {
        var mapCreator = new RoomsMapCreator(22, 18, 10, 7, 3);
        tileMap = new ToolMop(mapCreator, "floor", "wall", "door", 32, Scale, this)
        {
            FloorTintInView = new Color(128, 128, 128),
            WallTintInView = Color.White,

            FloorTintOutOfView = new Color(4, 4, 4),
            WallTintOutOfView = new Color(4, 4, 4),
        };

        GameObjects.Add(tileMap);

        path = new PathToTarget(tileMap.CellMap, "floor", this)
        {
            Tint = Color.DarkGoldenrod,
            Scale = Scale
        };
        GameObjects.Add(path);


        enemy = new Enomy(tileMap, "bee", this)
        {
            Scale = Scale,
            Health = 100,
        };
        RogueSharp.Cell enemyStart = tileMap.GetRandomEmptyCell();
        enemy.X = enemyStart.X;
        enemy.Y = enemyStart.Y;
        GameObjects.Add(enemy);

        player = new Player(null, "tileAtlas", Scale, this)
        {
            Health = 100,
        };

        RogueSharp.Cell startingCell = tileMap.GetRandomEmptyCell();
        player.X = startingCell.X;
        player.Y = startingCell.Y;

        GameObjects.Add(player);


    }

    protected override void Update(float dt)
    { 
    }

    protected override void Draw(SpriteBatch sb)
    {
        ParentWindow.FPSLabel.Content = "FPS: " + FPS;
        ParentWindow.HealthLabel.Content = "Health: " + player.Health;
    }

}

