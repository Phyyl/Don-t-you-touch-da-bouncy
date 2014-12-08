using OpenTK;
using OpenTK.Graphics.OpenGL;
using OpenTK.Input;
using OpenTKUtil.Minimal;
using QuickFont;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LD31
{
	public class Game : BaseGame
	{
		private const int initialShake = 20;
		private const float shakeLength = 10;

		private Sound[] bounceSounds;
		private Sound pauseSound;
		private Sound unpauseSound;
		private Sound dieSound;
		private Sound playSound;

		private QFont default_font_16;
		private QFont default_font_25;
		private int record;

		public Random Random = new Random();

		public PlayerRect PlayerRectangle;
		public MasterEnemyRect MasterEnemy;
		public List<EnemyRect> Rectangles = new List<EnemyRect>();
		public ParticlePool Particles = new ParticlePool();

		private int points = 0;
		public int Points
		{
			get { return points; }
			set
			{
				points = value;
				if (points > record)
					record = points;
			}
		}
		public GameState State = GameState.None;

		private int shakeAmount;

		public Game()
			: base(true)
		{
			Load += Game_Load;
			UpdateFrame += Game_UpdateFrame;
			RenderFrame += Game_RenderFrame;

			Width = 500;
			Height = 500;
			WindowBorder = OpenTK.WindowBorder.Fixed;
			Title = "Don't you touch da bouncy";
			Icon = new Icon("Resources/donttouchdabouncy.ico");

			CenterWindow();
		}

		void Game_Load(object sender, EventArgs e)
		{
			default_font_16 = new QFont("Resources/Perfect DOS VGA 437 Win.ttf", 16, FontStyle.Regular);
			default_font_25 = new QFont("Resources/Perfect DOS VGA 437 Win.ttf", 25, FontStyle.Regular);

			bounceSounds = new Sound[]
			{
				Sound.LoadFromWaveFile("Resources/bounce.wav"),
				Sound.LoadFromWaveFile("Resources/bounce2.wav"),
				Sound.LoadFromWaveFile("Resources/bounce3.wav"),
				Sound.LoadFromWaveFile("Resources/bounce4.wav"),
			};

			pauseSound = Sound.LoadFromWaveFile("Resources/pause.wav");
			unpauseSound = Sound.LoadFromWaveFile("Resources/unpause.wav");
			dieSound = Sound.LoadFromWaveFile("Resources/die.wav");
			playSound = Sound.LoadFromWaveFile("Resources/play.wav");
		}

		void Game_UpdateFrame(object sender, FrameEventArgs e)
		{
			if (Input.NewKey(Key.Delete))
			{
				record = 0;
				SaveRecordPoints();
			}

			switch (State)
			{
				case GameState.Playing:
					{
						PlayerRectangle.Update();
						MasterEnemy.Update();
						Particles.Update();
						foreach (var rect in Rectangles)
						{
							rect.Update();
							if (rect.Rectangle.IntersectsWith(PlayerRectangle.Rectangle))
							{
								EndGame();
								break;
							}
						}

						if (MasterEnemy.Rectangle.IntersectsWith(PlayerRectangle.Rectangle))
						{
							EndGame();
						}

						if (Input.NewKey(Key.Escape))
						{
							State = GameState.Paused;
							pauseSound.Play();
						}
					}
					break;
				case GameState.Paused:
					{
						if (Input.NewKey(Key.Escape))
						{
							State = GameState.Playing;
							unpauseSound.Play();
						}
					}
					break;
				case GameState.GameOver:
					{
						if (Input.NewKey(Key.Enter))
						{
							State = GameState.Playing;
							GenerateGameStart();
							dieSound.Play();
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

#if DEBUG
			RenderString(Particles.Particles.Count.ToString(), new Vector2(5, 25), Vector2.Zero, default_font_16);
#endif
			
			GL.Disable(EnableCap.Lighting);
			GL.Disable(EnableCap.Texture2D);


			switch (State)
			{
				case GameState.Playing:
					{
						GL.PushMatrix();
						{
							if (shakeAmount > 0)
							{
								Vector2 shakeVector = new Vector2((float)Random.NextDouble(), (float)Random.NextDouble()).Normalized() * ((float)shakeAmount / initialShake * shakeLength);
								GL.Translate(new Vector3(shakeVector));
								shakeAmount--;
							}
							Particles.Render();
							foreach (var rect in Rectangles)
							{
								rect.Render();
							}
							PlayerRectangle.Render();
							MasterEnemy.Render();
						}
						GL.PopMatrix();

						RenderPoints();
					}
					break;
				case GameState.Paused:
					{
						RenderPoints();
						RenderStringCentered("Paused (Escape to resume)", ClientCenter, default_font_25);
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
			RenderString(string.Format("{0} ({1} record)", Points, record), new Vector2(5), Vector2.Zero, default_font_16);
		}

		void RenderCredits()
		{
			string text = "Made by Philippe Pare for #LD31";
			RenderString(text, ClientSize, MesureString(text, default_font_16) + new Vector2(5), default_font_16);
		}

		void GenerateGameStart()
		{
			Rectangles.Clear();
			Particles.Clear();
			MasterEnemy = new MasterEnemyRect(
				Random.Next(20, Width - 40),
				Random.Next(20, Height - 40),
				20, 20);
			PlayerRectangle = new PlayerRect(15, 15);
			Points = 0;

			LoadRecordPoints();
			MasterEnemy.OnHitWall += MasterEnemy_OnHitWall;
			MasterEnemy.OnHitWall += Enemy_OnHitWall;

			playSound.Play();
		}

		void EndGame()
		{
			State = GameState.GameOver;
			SaveRecordPoints();
		}

		void MasterEnemy_OnHitWall(GameRect rect, Direction wall)
		{
			Shake();
			bounceSounds[Random.Next(bounceSounds.Length)].Play();
		}

		void Enemy_OnHitWall(GameRect rect, Direction wall)
		{
			for (int i = 0; i < 50; i++)
			{
				Particles.Add(new HitParticle(rect.Center, 3, 50, 10, wall));
			}
		}

		public void AddRandomEnemyAt(float x, float y)
		{
			var newEnemeny = new EnemyRect(x, y, 15, 15);
			newEnemeny.OnHitWall += Enemy_OnHitWall;
			Rectangles.Add(newEnemeny);
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
			RenderString(str, position, MesureString(str, font) / 2, font);
		}

		private void Shake()
		{
			shakeAmount = initialShake;
		}

		private void LoadRecordPoints()
		{
			try
			{
				var reader = new BinaryReader(File.OpenRead("data.dat"));
				int read = reader.ReadInt32();
				if (read > record)
					record = read;
				reader.Close();
			}
			catch { }
		}

		private void SaveRecordPoints()
		{
			try
			{
				var writer = new BinaryWriter(File.OpenWrite("data.dat"));
				writer.Write(record);
				writer.Close();
			}
			catch { }
		}

		public static Game Instance;
		static void Main(string[] args)
		{
			Instance = new Game();
			Instance.Run(60);
		}
	}
}
