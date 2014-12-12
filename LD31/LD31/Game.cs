using Cgen.Audio;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using OpenTK.Input;
using OpenTKUtil.Minimal;
using QuickFont;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Media;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace LD31
{
	public class Game : BaseGame
	{
		public GameScreen Game_Screen;
		public MenuScreen Menu_Screen;
		public GameOverSreen GameOver_Screen;
		public PauseScreen Pause_Screen;
		public HowToScreen HowTo_Screen;

		public Screen CurrentScreen;

		private GameState pausedState;

		public Game()
			: base(true)
		{
			Load += Game_Load;
			UpdateFrame += Game_UpdateFrame;
			RenderFrame += Game_RenderFrame;

			ClientSize = new Vector2(500, 500);
			WindowBorder = OpenTK.WindowBorder.Fixed;
			Title = "Don't you touch da bouncy";
			Icon = new Icon("Resources/donttouchdabouncy.ico");

			CenterWindow();
		}

		void Game_Load(object sender, EventArgs e)
		{
			EnableTransparency();

			SoundSystem.Instance().Init();

			Sounds.Load("bounce", "Resources/bounce.wav");
			Sounds.Load("bounce2", "Resources/bounce2.wav");
			Sounds.Load("bounce3", "Resources/bounce3.wav");
			Sounds.Load("bounce4", "Resources/bounce4.wav");

			Sounds.Load("bip", "Resources/bip.wav");
			Sounds.Load("bop", "Resources/bop.wav");
			Sounds.Load("die", "Resources/die.wav");
			Sounds.Load("play", "Resources/play.wav");

			Fonts.Load("pdos16", "Resources/Perfect DOS VGA 437.ttf", 16, FontStyle.Regular);
			Fonts.Load("pdos25", "Resources/Perfect DOS VGA 437.ttf", 25, FontStyle.Regular);

			Game_Screen = new GameScreen();
			Menu_Screen = new MenuScreen();
			GameOver_Screen = new GameOverSreen();
			Pause_Screen = new PauseScreen();
			HowTo_Screen = new HowToScreen();

			SetState(GameState.Menu);
		}

		void Game_UpdateFrame(object sender, FrameEventArgs e)
		{
			if (CurrentScreen != null)
				CurrentScreen.Update();
		}

		void Game_RenderFrame(object sender, FrameEventArgs e)
		{
			GL.Clear(ClearBufferMask.ColorBufferBit);
			GL.Disable(EnableCap.Texture2D);
			if (CurrentScreen != null)
				CurrentScreen.Render();
		}

		public void RenderCredits()
		{
			string text = "This is not the #LD31 version";
			Fonts.RenderString(text, Game.Instance.ClientSize - new Vector2(0, 25), Fonts.MesureString(text, "pdos16") + new Vector2(5), "pdos16");
			text = "Made by Philippe Pare for #LD31";
			Fonts.RenderString(text, Game.Instance.ClientSize, Fonts.MesureString(text, "pdos16") + new Vector2(5), "pdos16");
		}

		public static Game Instance;
		static void Main(string[] args)
		{
			try
			{
				Instance = new Game();
				Debug.WriteLine(DateTime.Now.ToString());
				Instance.Run(60);
			}
			catch (Exception ex)
			{
				File.WriteAllLines(DateTime.Now.ToString().Replace(':', '-') + ".log", new string[] { ex.Message, ex.StackTrace });
			}
		}

		public void SetState(GameState gameState, bool unpause = false)
		{
			switch (gameState)
			{
				case GameState.Menu:
					CurrentScreen = Menu_Screen;
					break;
				case GameState.PlayingSingle:
					if (unpause && !(Game_Screen is GameScreen))
						Game_Screen = new GameScreen();
					CurrentScreen = Game_Screen;
					break;
				case GameState.PlayingMulti:
					if (!(Game_Screen is MultiplayerGameScreen))
						Game_Screen = new MultiplayerGameScreen();
					CurrentScreen = Game_Screen;
					break;
				case GameState.Paused:
					CurrentScreen = Pause_Screen;
					break;
				case GameState.GameOver:
					CurrentScreen = GameOver_Screen;
					break;
				case GameState.HowTo:
					CurrentScreen = HowTo_Screen;
					break;
			}
		}

		public void ResetGame()
		{
			Game_Screen.Reset();
		}

		public void ResumeGame()
		{
			CurrentScreen = Game_Screen;
		}
	}
}
