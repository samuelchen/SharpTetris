using System;

namespace Net.SamuelChen.Tetris.Statistics
{
	/// <summary>
	/// The score counter.
	/// </summary>
	public class ScoreCounter
	{
		public ScoreCounter()	{
		}
		
		public void ScoreAdd(int nDestroiedLines){
			if (nDestroiedLines <= 0)
				return;
			m_nScore += (UInt32)(m_nLevel << (nDestroiedLines-1));
		}

		/// <summary>
		/// Current game level
		/// </summary>
        public int Level { get; set; }

		/// <summary>
		/// The score.
		/// </summary>
        public UInt32 Score { get; }
	}
}
