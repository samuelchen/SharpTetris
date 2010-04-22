using System;
using System.Xml;
using System.Collections;

namespace Net.SamuelChen.Tetris.Setting
{
	/// <summary>
	/// CSetting 的摘要说明。
	/// </summary>
	public class CSetting : System.Xml.XmlDocument
	{
		public const int	adMaxPlayerNum = 4;			// 最大控制器个数
		public const int	adDefaultBlockNum = 4;		// 默认块数常量
		
		protected System.Resources.ResourceManager m_theResMgr;	// 资源管理变量

		public CSetting()
		{
			try {
				m_theResMgr = new System.Resources.ResourceManager("Tetris.default", GetType().Assembly);
			}catch (Exception err) {
				throw new SettingException("Load resource error.", err);
			}
		}
		
		/// <summary>
		/// 从文件载入设置进行构造
		/// </summary>
		/// <param name="sFile">文件名</param>
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
		/// 取控制器按键定义
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
