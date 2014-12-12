﻿using OpenTK;
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
		private const int initialShake = 10;
		private const int shakePer100 = 3;
		private const float shakeLength = 7;
		private static readonly string[] bounceSounds = new string[] { "bounce", "bounce2", "bounce3", "bounce4" };
		public const string SaveFile = "single.dat";

		public PlayerRect PlayerRectangle;
		public MasterEnemyRect MasterEnemy;
		public List<EnemyRect> Rectangles = new List<EnemyRect>();
		public ParticlePool Particles = new ParticlePool();

		private int shakeAmount;

		private int record;
		private int points;
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

		protected string saveFile = SaveFile;

		public override void Update()
		{
			PlayerRectangle.Update();
			MasterEnemy.Update();
			Particles.Update();
			foreach (var rect in Rectangles)
			{
				rect.Update();
				if (CheckPlayerCollisions(rect))
				{
					EndGame();
#if !DEBUG
					break;
#endif
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
					Vector2 shakeVector = new Vector2(Rand.NextFloat() - 0.5f, Rand.NextFloat() - 0.5f).Normalized() * ((float)shakeAmount / CalcInitialShake()) *  shakeLength;
					GL.Translate(new Vector3(shakeVector));
					shakeAmount--;
				}
				Particles.Render();
				RenderRects();
			}
			GL.PopMatrix();

			RenderPoints();
		}

		protected virtual void RenderRects()
		{
			foreach (var rect in Rectangles)
			{
				rect.Render();
			}
			PlayerRectangle.Render();
			MasterEnemy.Render();
		}

		public void RenderPoints()
		{
			Fonts.RenderString(string.Format("{0} ({1} record)", Points, record), new Vector2(5), Vector2.Zero, "pdos16");
		}

		private void Shake()
		{
			shakeAmount = CalcInitialShake();
		}

		private int CalcInitialShake()
		{
			return initialShake + shakePer100 * (Points / 100);
		}

		public virtual void Reset()
		{
			Rectangles.Clear();
			Particles.Clear();
			MasterEnemy = new MasterEnemyRect(
				Rand.Next(20, Game.Instance.Width - 40),
				Rand.Next(20, Game.Instance.Height - 40),
				20, 20);
			PlayerRectangle = new PlayerRect(PlayerRect.PlayerInputMode.Arrows|PlayerRect.PlayerInputMode.WASD);
			Points = 1000;

			LoadPointsToFile();
			MasterEnemy.OnHitWall += MasterEnemy_OnHitWall;
			MasterEnemy.OnHitWall += Enemy_OnHitWall;

			Sounds.Play("play");
		}

		protected virtual bool CheckPlayerCollisions(EnemyRect rect)
		{
			return rect.Rectangle.IntersectsWith(PlayerRectangle.Rectangle);
		}

		protected void EndGame()
		{
#if !DEBUG
			Game.Instance.SetState(GameState.GameOver);
			Sounds.Play("die");
			SavePointsToFile();
#endif
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

		private void LoadPointsToFile()
		{
			try
			{
				var reader = new BinaryReader(File.OpenRead(saveFile));
				int read = reader.ReadInt32();
				if (read > record)
					record = read;
				reader.Close();
			}
			catch { }
		}

		private void SavePointsToFile()
		{
			try
			{
				var writer = new BinaryWriter(File.OpenWrite(saveFile));
				writer.Write(record);
				writer.Close();
			}
			catch { }
		}
	}
}
