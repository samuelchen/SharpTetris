using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Data;

namespace Tetris
{
	/// <summary>
	/// frmMain ������
	/// </summary>
	public class frmMain : System.Windows.Forms.Form
	{
		private CGame	theGame;

		private System.Windows.Forms.MenuItem mnGame;
		private System.Windows.Forms.MenuItem mnGame_New;
		private System.Windows.Forms.MainMenu menuMain;		
		/// <summary>
		/// ����������������
		/// </summary>
		private System.ComponentModel.Container components = null;


		public frmMain()
		{
			//
			// Windows ���������֧���������
			//
			InitializeComponent();

			//
			// TODO: �� InitializeComponent ���ú�����κι��캯������
			//
		}

		/// <summary>
		/// ������������ʹ�õ���Դ��
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
				if (components != null) 
				{
					components.Dispose();
				}
			}
			base.Dispose( disposing );
		}

		#region Windows ������������ɵĴ���
		/// <summary>
		/// �����֧������ķ��� - ��Ҫʹ�ô���༭���޸�
		/// �˷��������ݡ�
		/// </summary>
		private void InitializeComponent()
		{
			System.Resources.ResourceManager resources = new System.Resources.ResourceManager(typeof(frmMain));
			this.menuMain = new System.Windows.Forms.MainMenu();
			this.mnGame = new System.Windows.Forms.MenuItem();
			this.mnGame_New = new System.Windows.Forms.MenuItem();
			// 
			// menuMain
			// 
			this.menuMain.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {this.mnGame});
			this.menuMain.RightToLeft = ((System.Windows.Forms.RightToLeft)(resources.GetObject("menuMain.RightToLeft")));
			// 
			// mnGame
			// 
			this.mnGame.Enabled = ((bool)(resources.GetObject("mnGame.Enabled")));
			this.mnGame.Index = 0;
			this.mnGame.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
																				   this.mnGame_New});
			this.mnGame.Shortcut = ((System.Windows.Forms.Shortcut)(resources.GetObject("mnGame.Shortcut")));
			this.mnGame.ShowShortcut = ((bool)(resources.GetObject("mnGame.ShowShortcut")));
			this.mnGame.Text = resources.GetString("mnGame.Text");
			this.mnGame.Visible = ((bool)(resources.GetObject("mnGame.Visible")));
			// 
			// mnGame_New
			// 
			this.mnGame_New.Enabled = ((bool)(resources.GetObject("mnGame_New.Enabled")));
			this.mnGame_New.Index = 0;
			this.mnGame_New.Shortcut = ((System.Windows.Forms.Shortcut)(resources.GetObject("mnGame_New.Shortcut")));
			this.mnGame_New.ShowShortcut = ((bool)(resources.GetObject("mnGame_New.ShowShortcut")));
			this.mnGame_New.Text = resources.GetString("mnGame_New.Text");
			this.mnGame_New.Visible = ((bool)(resources.GetObject("mnGame_New.Visible")));
			this.mnGame_New.Click += new System.EventHandler(this.mnGame_New_Click);
			// 
			// frmMain
			// 
			this.AccessibleDescription = resources.GetString("$this.AccessibleDescription");
			this.AccessibleName = resources.GetString("$this.AccessibleName");
			this.AutoScaleBaseSize = ((System.Drawing.Size)(resources.GetObject("$this.AutoScaleBaseSize")));
			this.AutoScroll = ((bool)(resources.GetObject("$this.AutoScroll")));
			this.AutoScrollMargin = ((System.Drawing.Size)(resources.GetObject("$this.AutoScrollMargin")));
			this.AutoScrollMinSize = ((System.Drawing.Size)(resources.GetObject("$this.AutoScrollMinSize")));
			this.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("$this.BackgroundImage")));
			this.ClientSize = ((System.Drawing.Size)(resources.GetObject("$this.ClientSize")));
			this.Enabled = ((bool)(resources.GetObject("$this.Enabled")));
			this.Font = ((System.Drawing.Font)(resources.GetObject("$this.Font")));
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.ImeMode = ((System.Windows.Forms.ImeMode)(resources.GetObject("$this.ImeMode")));
			this.Location = ((System.Drawing.Point)(resources.GetObject("$this.Location")));
			this.MaximumSize = ((System.Drawing.Size)(resources.GetObject("$this.MaximumSize")));
			this.Menu = this.menuMain;
			this.MinimumSize = ((System.Drawing.Size)(resources.GetObject("$this.MinimumSize")));
			this.Name = "frmMain";
			this.RightToLeft = ((System.Windows.Forms.RightToLeft)(resources.GetObject("$this.RightToLeft")));
			this.StartPosition = ((System.Windows.Forms.FormStartPosition)(resources.GetObject("$this.StartPosition")));
			this.Text = resources.GetString("$this.Text");

		}
		#endregion

		/// <summary>
		/// Ӧ�ó��������ڵ㡣
		/// </summary>
//		[STAThread]
//		static void Main() 
//		{
//			//Application.Run(new frmMain());
//			//System.Threading.Thread.CurrentThread.CurrentUICulture = new System.Globalization.CultureInfo("en-US", false); 
//			Application.Run(new frmTest());
//		}

		private void mnGame_New_Click(object sender, System.EventArgs e) {
			//ѡ������
			theGame = new CGame(this);
			//�½���Ϸ
			theGame.New();
		}
	}
}
