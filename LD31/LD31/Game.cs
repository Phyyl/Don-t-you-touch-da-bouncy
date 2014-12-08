using OpenTK;
using OpenTK.Graphics.OpenGL;
using OpenTK.Input;
using OpenTKUtil.Minimal;
using QuickFont;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LD31
{
	public class Game : BaseGame
	{
		public Random Random = new Random();
		public PlayerRect PlayerRectangle;

		List<GameRect> rectangles = new List<GameRect>();
		List<GameRect> tempRectangles = new List<GameRect>();

		QFont default_font_16;
		QFont default_font_25;

		public int Points = 0;
		public GameState State = GameState.None;

		public Game()
			: base(true)
		{
			Load += Game_Load;
			UpdateFrame += Game_UpdateFrame;
			RenderFrame += Game_RenderFrame;

			Width = 500;
			Height = 500;
			CenterWindow();
			WindowBorder = OpenTK.WindowBorder.Fixed;
		}

		void Game_Load(object sender, EventArgs e)
		{
			default_font_16 = new QFont("Resources/Perfect DOS VGA 437 Win.ttf", 16, FontStyle.Regular);
			default_font_25 = new QFont("Resources/Perfect DOS VGA 437 Win.ttf", 25, FontStyle.Regular);

			GenerateGameStart();

			Title = "Don't you touch da bouncy";
			Icon = new Icon("Resources/donttouchdabouncy.ico");
		}

		void Game_UpdateFrame(object sender, FrameEventArgs e)
		{
			switch (State)
			{
				case GameState.Playing:
					{
						PlayerRectangle.Update();
						foreach (var rect in rectangles)
						{
							rect.Update();
							if (rect.Rectangle.IntersectsWith(PlayerRectangle.Rectangle))
							{
								State = GameState.GameOver;
							}
						}

						foreach (var rect in tempRectangles)
						{
							rectangles.Add(rect);
						}

						tempRectangles.Clear();

						if (Input.NewKey(Key.Escape))
						{
							State = GameState.Paused;
						}
					}
					break;
				case GameState.Paused:
					{
						if (Input.NewKey(Key.Escape))
						{
							State = GameState.Playing;
						}
					}
					break;
				case GameState.GameOver:
					{
						if (Input.NewKey(Key.Enter))
						{
							State = GameState.Playing;
							GenerateGameStart();
						}
					}
					break;
				case GameState.None:
					{
						if (Input.NewKey(Key.Enter))
						{
							GenerateGameStart();
							State = GameState.Playing;
						}
					}
					break;
			}
		}

		void Game_RenderFrame(object sender, FrameEventArgs e)
		{
			GL.Clear(ClearBufferMask.ColorBufferBit);
			GL.Disable(EnableCap.Lighting);
			GL.Disable(EnableCap.Texture2D);

			switch (State)
			{
				case GameState.Playing:
					{
						foreach (var rect in rectangles)
						{
							rect.Render();
						}
						PlayerRectangle.Render();
						RenderPoints();
					}
					break;
				case GameState.Paused:
					{
						RenderPoints();
						RenderStringCentered("Paused (Escape to resume)", ClientCenter,default_font_25);
					}
					break;
				case GameState.GameOver:
					{
						RenderPoints();
						RenderStringCentered("Game over! (Enter to play)", ClientCenter, default_font_25);
						RenderCredits();
					}
					break;
				case GameState.None:
					{
						RenderStringCentered("Press Enter to play!", ClientCenter, default_font_25);
						RenderCredits();
					}
					break;
			}
		}

		void RenderPoints()
		{
			RenderString(Points.ToString(), new Vector2(5), Vector2.Zero, default_font_16);
		}

		void RenderCredits()
		{
			string text = "Made by Philippe Pare for #LD31";
			RenderString(text, ClientSize, MesureString(text, default_font_16) + new Vector2(5), default_font_16);
		}

		void GenerateGameStart()
		{
			rectangles.Clear();
			tempRectangles.Clear();

			rectangles.Add(new MasterEnemyRect(
				Random.Next(100, Width - 200),
				Random.Next(100, Height - 200),
				20, 20));

			PlayerRectangle = new PlayerRect(15, 15);

			Points = 0;
		}

		public void AddRandomEnemyAt(float x, float y)
		{
			tempRectangles.Add(new EnemyRect(
				x, y,
				15, 15));
		}

		public Vector2 MesureString(string str, QFont font)
		{
			var size = font.Measure(str);
			return new Vector2(size.Width, size.Height);
		}

		public void RenderString(string str, Vector2 position, Vector2 origin, QFont font)
		{
			QFont.Begin();
			font.Print(str, position - origin);
			QFont.End();
		}

		public void RenderStringCentered(string str, Vector2 position, QFont font)
		{
			RenderString(str, position, MesureString(str,font) / 2,font);
		}

		public static Game Instance;
		static void Main(string[] args)
		{
			Instance = new Game();
			Instance.Run(60);
		}

		public void AddRect(GameRect gameRect)
		{
			tempRectangles.Add(gameRect);
		}
	}
}
