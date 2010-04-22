using System;
using System.Xml;
using System.Collections;

namespace Net.SamuelChen.Tetris.Setting
{
	/// <summary>
	/// CSetting ��ժҪ˵����
	/// </summary>
	public class CSetting : System.Xml.XmlDocument
	{
		public const int	adMaxPlayerNum = 4;			// ������������
		public const int	adDefaultBlockNum = 4;		// Ĭ�Ͽ�������
		
		protected System.Resources.ResourceManager m_theResMgr;	// ��Դ�������

		public CSetting()
		{
			try {
				m_theResMgr = new System.Resources.ResourceManager("Tetris.default", GetType().Assembly);
			}catch (Exception err) {
				throw new SettingException("Load resource error.", err);
			}
		}
		
		/// <summary>
		/// ���ļ��������ý��й���
		/// </summary>
		/// <param name="sFile">�ļ���</param>
		public CSetting(string sFile) : this(){
			try {
				base.Load(sFile);
			}catch {
				this.LoadXml(this.GetString("defaultSetting"));
			}
		}

		public string GetString(string sName) {
			return m_theResMgr.GetString(sName);
		}

		public object GetObject(string sName) {
			return m_theResMgr.GetObject(sName);
		}

		/// <summary>
		/// ȡ��������������
		/// </summary>
		/// <param name="theController"></param>
		public Hashtable GetKeyMap(int nCtrlNum) {
			if (nCtrlNum<0 || nCtrlNum>=adMaxPlayerNum)
				return null;

			XmlNodeList nodeList = this.GetElementsByTagName("keymap");
			if (null == nodeList) 
				return null;

            Hashtable keyMaps = new Hashtable();
			System.Collections.IEnumerator theEnumer = nodeList.GetEnumerator();
			XmlElement elmt;
			for (int i=0; i<nodeList.Count; i++) {
				theEnumer.MoveNext();
				elmt = (XmlElement)theEnumer.Current;
				if (elmt.GetAttribute("no") == nCtrlNum.ToString()) {
					keyMaps.Add( elmt.GetAttribute("left"), enumControllerKey.adKeyLeft);
					keyMaps.Add( elmt.GetAttribute("right"), enumControllerKey.adKeyRight);
					keyMaps.Add( elmt.GetAttribute("rotate"), enumControllerKey.adKeyRotate);
					keyMaps.Add( elmt.GetAttribute("down"), enumControllerKey.adKeyDown);
					keyMaps.Add( elmt.GetAttribute("direct"), enumControllerKey.adKeyDirectDown);
					return keyMaps;
				}
			}

			return null;
		}
		
		public int GetFirstNoneUsedCtrl(){
			XmlNodeList nodeList = this.GetElementsByTagName("keymap");
			if (null != nodeList) {
				System.Collections.IEnumerator theEnumer = nodeList.GetEnumerator();
				XmlElement elmt;
				for (int i=0; i<nodeList.Count; i++) {
					theEnumer.MoveNext();
					elmt = (XmlElement)theEnumer.Current;
					if ("" == elmt.GetAttribute("no")){
						elmt.SetAttribute("no", i.ToString());
						return i;
					}
				}
			}
			return -1;
		}

		public int BlockNum {
			get {
				return 4;
			}
		}
	}
}
