using OpenTK;
using OpenTK.Graphics.OpenGL;
using OpenTK.Input;
using OpenTKUtil.Minimal;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LD31
{
	public class GameScreen : Screen
	{
		private const int initialShake = 20;
		private const float shakeLength = 10;
		private static readonly string[] bounceSounds = new string[] { "bounce", "bounce2", "bounce3", "bounce4" };

		public PlayerRect PlayerRectangle;
		public MasterEnemyRect MasterEnemy;
		public List<EnemyRect> Rectangles = new List<EnemyRect>();
		public ParticlePool Particles = new ParticlePool();

		private int shakeAmount;

		private int record;
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


		public override void Update()
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
				Game.Instance.SetState(GameState.Paused);
				Sounds.Play("bip");
			}
		}

		public override void Render()
		{
			GL.Disable(EnableCap.Texture2D);
			GL.PushMatrix();
			{
				if (shakeAmount > 0)
				{
					Vector2 shakeVector = new Vector2((float)Rand.NextDouble(), (float)Rand.NextDouble()).Normalized() * ((float)shakeAmount / initialShake * shakeLength);
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

		public void RenderPoints()
		{
			Fonts.RenderString(string.Format("{0} ({1} record)", Points, record), new Vector2(5), Vector2.Zero, "pdos16");
		}

		public void RenderCredits()
		{
			string text = "Made by Philippe Pare for #LD31";
			Fonts.RenderString(text, Game.Instance.ClientSize, Fonts.MesureString(text, "pdos16") + new Vector2(5), "pdos16");
		}

		private void Shake()
		{
			shakeAmount = initialShake;
		}

		public void Reset()
		{
			Rectangles.Clear();
			Particles.Clear();
			MasterEnemy = new MasterEnemyRect(
				Rand.Next(20, Game.Instance.Width - 40),
				Rand.Next(20, Game.Instance.Height - 40),
				20, 20);
			PlayerRectangle = new PlayerRect(15, 15);
			Points = 0;

			LoadRecordPoints();
			MasterEnemy.OnHitWall += MasterEnemy_OnHitWall;
			MasterEnemy.OnHitWall += Enemy_OnHitWall;

			Sounds.Play("play");
		}

		void EndGame()
		{
			Game.Instance.SetState(GameState.GameOver);
			Sounds.Play("die");
			SaveRecordPoints();
		}

		public void AddRandomEnemyAt(float x, float y)
		{
			var newEnemeny = new EnemyRect(x, y, 15, 15);
			newEnemeny.OnHitWall += Enemy_OnHitWall;
			Rectangles.Add(newEnemeny);
		}

		void MasterEnemy_OnHitWall(GameRect rect, Direction wall)
		{
			Shake();
			Sounds.Play(bounceSounds[Rand.Next(bounceSounds.Length)]);
		}

		void Enemy_OnHitWall(GameRect rect, Direction wall)
		{
			Particles.GenerateExplosion(rect.Center, 50, 3, 30, 10, wall);
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
	}
}
