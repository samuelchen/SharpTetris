using System;

namespace Net.SamuelChen.Tetris.Statistics
{
	/// <summary>
	/// CInfomationPanel 的摘要说明。
	/// </summary>
	public class InfomationPanel : System.Windows.Forms.PictureBox
	{
		protected Rectangle[]	m_Block;
		protected CPlayPanel	m_Panel;
		protected bool			m_blGraphUsing;

		public InfomationPanel(CPlayPanel panel) {
			m_Panel = panel;

			Height = 50;
			BackColor = Color.Chocolate;
			BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		}
		
		protected override void Dispose(bool disposing) {
			base.Dispose (disposing);
		}

		public CBlock Block {
			set {
				//m_Block.Diamonds = (CDiamond[])value.Diamonds.Clone();
				CDiamond[]	diamonds = (CDiamond[])value.Diamonds;
				if (diamonds.Equals(null))
					return;

				m_Block = new Rectangle[diamonds.Length];
				int baseX = diamonds[0].X;

				for (int i=0; i<diamonds.Length; i++) {
					m_Block[i].X = (diamonds[i].X - baseX)*10 + 10;
					m_Block[i].Y = diamonds[i].Y * 10 + 5;
					m_Block[i].Width = 10;
					m_Block[i].Height = 10;
				}
				this.Invalidate();
			}
		}

		public void Show(int x, int y) {
			this.Location = new Point(x, y);
			this.Visible = true;
			base.Show();
		}

		protected override void OnPaint(System.Windows.Forms.PaintEventArgs e) {
			base.OnPaint (e);
			e.Graphics.TextRenderingHint = System.Drawing.Text.TextRenderingHint.AntiAlias;
			DrawBlock(e.Graphics);
			DrawName(e.Graphics);
			DrawLevel(e.Graphics);
			DrawScore(e.Graphics);
		}

		protected void DrawBlock(Graphics graph) {
			if (null == m_Block) 
				return;

			graph.DrawRectangles(Pens.Black, m_Block);
		}

		protected void DrawScore(Graphics graph) {
			graph.DrawString(m_Panel.Counter.Score.ToString(), 
				new Font("Tahoma", 18), Brushes.Gray, 71, 11);
			graph.DrawString(m_Panel.Counter.Score.ToString(), 
				new Font("Tahoma", 18), Brushes.Yellow, 70, 10);
		}

		protected void DrawLevel(Graphics graph) {
			graph.DrawString("Level:" + m_Panel.Counter.Level.ToString(),
				new Font("Tahoma", 7, FontStyle.Bold),
				Brushes.WhiteSmoke, 220, 2);
		}

		protected void DrawName(Graphics graph) {
			graph.DrawString(m_Panel.Player.Name,
				new Font("隶书", 18), Brushes.Cyan, 180, 10);
		}

	}
}
