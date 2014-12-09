using OpenTK;
using OpenTK.Graphics.OpenGL;
using OpenTK.Input;
using OpenTKUtil.Minimal;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LD31
{
	public class Menu
	{
		private const int itemSize = 35;
		public delegate void MenuItemSelected(int index);

		public event MenuItemSelected OnMenuItemSelected;
		public string[] Items;

		private int selectedItem;

		public Menu(params string[] menuItems)
		{
			Items = menuItems;
		}

		public void Update()
		{
			if (Input.NewKey(Key.Enter))
			{
				if (OnMenuItemSelected != null)
					OnMenuItemSelected(selectedItem);
			}
			if (Input.NewKey(Key.Down) || Input.NewKey(Key.S))
			{
				Down();
			}
			if (Input.NewKey(Key.Up) || Input.NewKey(Key.W))
			{
				Up();
			}
		}

		public void Render()
		{
			var windowSize = Game.Instance.ClientSize;
			float startY = (windowSize.Y - Items.Length * itemSize) / 2;

			for (int i = 0; i < Items.Length; i++)
			{
				bool selected = selectedItem == i;
				Fonts.RenderString(Items[i], new Vector2(100 + (selected ? 10 : 0), startY + i * itemSize), Vector2.Zero, "pdos25", selected ? Color.OrangeRed : Color.White);
			}
		}

		private void Down()
		{
			selectedItem++;
			selectedItem %= Items.Length;
			Sounds.Play("bop");
		}

		private void Up()
		{
			selectedItem--;
			if (selectedItem < 0)
				selectedItem = Items.Length - 1;
			Sounds.Play("bip");
		}
	}
}
