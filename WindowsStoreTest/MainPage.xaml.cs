﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using Neo.IronLua;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// Die Elementvorlage "Leere Seite" ist unter http://go.microsoft.com/fwlink/?LinkId=234238 dokumentiert.

namespace WindowsStoreTest
{
	/// <summary>
	/// Eine leere Seite, die eigenständig verwendet werden kann oder auf die innerhalb eines Rahmens navigiert werden kann.
	/// </summary>
	public sealed partial class MainPage : Page
	{
		private Lua lua = new Lua();
		private LuaTable table = new LuaTable();

		public MainPage()
		{
			this.InitializeComponent();
		}

		private void Button_Click(object sender, RoutedEventArgs e)
		{
			string sLuaCode = txtLuaCode.Text;

			try
			{
				var c = lua.CompileChunk(sLuaCode, "test", null);
				var r = c.Run(table);

				var sb = new StringBuilder();
				for (int i = 0; i < r.Count; i++)
				{
					sb.AppendFormat("[{0}] = ", i);
					sb.Append(r[i]);
					sb.AppendLine();
				}
				sb.AppendLine("==> Successfull");
				txtResult.Text = sb.ToString();
			}
			catch(Exception ex)
			{
				txtResult.Text = String.Format("[{0}] {1}", ex.GetType().Name, ex.Message);
			}
		}

		private void Button_Click_1(object sender, RoutedEventArgs e)
		{
			using (Lua l = new Lua())
			{
				var t = new LuaTable();
				var c = l.CompileChunk(
					String.Join(Environment.NewLine,
						"local v1 = 2;",
						"local v2 = 4;",
						"function f()",
						"  return v1 + v2;",
						"end;",
						"return f();"), "test", null);
				var r = c.Run(t);
				txtResult.Text = String.Format("Test: v1=[{0}], v2=[{1}], r={2}", Lua.RtGetUpValue(t.GetMemberValue("f") as Delegate, 1), Lua.RtGetUpValue(t.GetMemberValue("f") as Delegate, 2), r.ToInt32());
			}
		}
	}
}