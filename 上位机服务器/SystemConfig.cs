using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.IO;

namespace ��λ��������
{
    public class SystemConfig
    {
        #region"������������"
        /// <summary>
        /// �õ�������Ŀ¼
        /// </summary>
        /// <returns></returns>
        private static string GetWorkDirectory()
        {
            try
            {
                return Path.GetDirectoryName(typeof(SystemConfig).Assembly.Location);
            }
            catch
            {
                return System.Windows.Forms.Application.StartupPath;
            }
        }
        /// <summary>
        /// �ж��ַ����Ƿ�Ϊ�մ�
        /// </summary>
        /// <param name="szString">Ŀ���ַ���</param>
        /// <returns>true:Ϊ�մ�;false:�ǿմ�</returns>
        private static bool IsEmptyString(string szString)
        {
            if (szString == null)
                return true;
            if (szString.Trim() == string.Empty)
                return true;
            return false;
        }
        /// <summary>
        /// ����һ���ƶ����ڵ�����XML�ļ�
        /// </summary>
        /// <param name="szFileName">XML�ļ�</param>
        /// <param name="szRootName">���ڵ���</param>
        /// <returns>bool</returns>
        private static bool CreateXmlFile(string szFileName, string szRootName)
        {
            if (szFileName == null || szFileName.Trim() == "")
                return false;
            if (szRootName == null || szRootName.Trim() == "")
                return false;

            XmlDocument clsXmlDoc = new XmlDocument();
            clsXmlDoc.AppendChild(clsXmlDoc.CreateXmlDeclaration("1.0", "GBK", null));
            clsXmlDoc.AppendChild(clsXmlDoc.CreateNode(XmlNodeType.Element, szRootName, ""));
            try
            {
                clsXmlDoc.Save(szFileName);
                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// ��XML�ļ���ȡ��Ӧ��XML�ĵ�����
        /// </summary>
        /// <param name="szXmlFile">XML�ļ�</param>
        /// <returns>XML�ĵ�����</returns>
        private static XmlDocument GetXmlDocument(string szXmlFile)
        {
            if (IsEmptyString(szXmlFile))
                return null;
            if (!File.Exists(szXmlFile))
                return null;
            XmlDocument clsXmlDoc = new XmlDocument();
            try
            {
                clsXmlDoc.Load(szXmlFile);
            }
            catch
            {
                return null;
            }
            return clsXmlDoc;
        }

        /// <summary>
        /// ��XML�ĵ����󱣴�ΪXML�ļ�
        /// </summary>
        /// <param name="clsXmlDoc">XML�ĵ�����</param>
        /// <param name="szXmlFile">XML�ļ�</param>
        /// <returns>bool:������</returns>
        private static bool SaveXmlDocument(XmlDocument clsXmlDoc, string szXmlFile)
        {
            if (clsXmlDoc == null)
                return false;
            if (IsEmptyString(szXmlFile))
                return false;
            try
            {
                if (File.Exists(szXmlFile))
                    File.Delete(szXmlFile);
            }
            catch
            {
                return false;
            }
            try
            {
                clsXmlDoc.Save(szXmlFile);
            }
            catch
            {
                return false;
            }
            return true;
        }

        /// <summary>
        /// ��ȡXPathָ��ĵ�һXML�ڵ�
        /// </summary>
        /// <param name="clsRootNode">XPath���ڵĸ��ڵ�</param>
        /// <param name="szXPath">XPath���ʽ</param>
        /// <returns>XmlNode</returns>
        private static XmlNode SelectXmlNode(XmlNode clsRootNode, string szXPath)
        {
            if (clsRootNode == null || IsEmptyString(szXPath))
                return null;
            try
            {
                return clsRootNode.SelectSingleNode(szXPath);
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// ��ȡXPathָ���XML�ڵ㼯
        /// </summary>
        /// <param name="clsRootNode">XPath���ڵĸ��ڵ�</param>
        /// <param name="szXPath">XPath���ʽ</param>
        /// <returns>XmlNodeList</returns>
        private static XmlNodeList SelectXmlNodes(XmlNode clsRootNode, string szXPath)
        {
            if (clsRootNode == null || IsEmptyString(szXPath))
                return null;
            try
            {
                return clsRootNode.SelectNodes(szXPath);
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// ����һ��XmlNode����ӵ��ĵ�
        /// </summary>
        /// <param name="clsParentNode">���ڵ�</param>
        /// <param name="szNodeName">�������</param>
        /// <returns>XmlNode</returns>
        private static XmlNode CreateXmlNode(XmlNode clsParentNode, string szNodeName)
        {
            try
            {
                XmlDocument clsXmlDoc = null;
                if (clsParentNode.GetType() != typeof(XmlDocument))
                    clsXmlDoc = clsParentNode.OwnerDocument;
                else
                    clsXmlDoc = clsParentNode as XmlDocument;
                XmlNode clsXmlNode = clsXmlDoc.CreateNode(XmlNodeType.Element, szNodeName, string.Empty);
                if (clsParentNode.GetType() == typeof(XmlDocument))
                {
                    clsXmlDoc.LastChild.AppendChild(clsXmlNode);
                }
                else
                {
                    clsParentNode.AppendChild(clsXmlNode);
                }
                return clsXmlNode;
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// ����ָ���ڵ���ָ�����Ե�ֵ
        /// </summary>
        /// <param name="parentNode">XML�ڵ�</param>
        /// <param name="szAttrName">������</param>
        /// <param name="szAttrValue">����ֵ</param>
        /// <returns>bool</returns>
        private static bool SetXmlAttr(XmlNode clsXmlNode, string szAttrName, string szAttrValue)
        {
            if (clsXmlNode == null)
                return false;
            if (IsEmptyString(szAttrName))
                return false;
            if (IsEmptyString(szAttrValue))
                szAttrValue = string.Empty;
            XmlAttribute clsAttrNode = clsXmlNode.Attributes.GetNamedItem(szAttrName) as XmlAttribute;
            if (clsAttrNode == null)
            {
                XmlDocument clsXmlDoc = clsXmlNode.OwnerDocument;
                if (clsXmlDoc == null)
                    return false;
                clsAttrNode = clsXmlDoc.CreateAttribute(szAttrName);
                clsXmlNode.Attributes.Append(clsAttrNode);
            }
            clsAttrNode.Value = szAttrValue;
            return true;
        }
        #endregion

        #region"�����ļ��Ķ�ȡ��д��"
        private static string CONFIG_FILE = "SystemConfig.xml";
        /// <summary>
        ///  ��ȡָ���������ļ���ָ��Key��ֵ
        /// </summary>
        /// <param name="szKeyName">��ȡ��Key����</param>
        /// <param name="szDefaultValue">ָ����Key������ʱ,���ص�ֵ</param>
        /// <returns>Keyֵ</returns>
        public static int GetConfigData(string szKeyName, int nDefaultValue)
        {
            string szValue = GetConfigData(szKeyName, nDefaultValue.ToString());
            try
            {
                return int.Parse(szValue);
            }
            catch
            {
                return nDefaultValue;
            }
        }

        /// <summary>
        ///  ��ȡָ���������ļ���ָ��Key��ֵ
        /// </summary>
        /// <param name="szKeyName">��ȡ��Key����</param>
        /// <param name="szDefaultValue">ָ����Key������ʱ,���ص�ֵ</param>
        /// <returns>Keyֵ</returns>
        public static float GetConfigData(string szKeyName, float fDefaultValue)
        {
            string szValue = GetConfigData(szKeyName, fDefaultValue.ToString());
            try
            {
                return float.Parse(szValue);
            }
            catch
            {
                return fDefaultValue;
            }
        }

        /// <summary>
        ///  ��ȡָ���������ļ���ָ��Key��ֵ
        /// </summary>
        /// <param name="szKeyName">��ȡ��Key����</param>
        /// <param name="szDefaultValue">ָ����Key������ʱ,���ص�ֵ</param>
        /// <returns>Keyֵ</returns>
        public static bool GetConfigData(string szKeyName, bool bDefaultValue)
        {
            string szValue = GetConfigData(szKeyName, bDefaultValue.ToString());
            try
            {
                return bool.Parse(szValue);
            }
            catch
            {
                return bDefaultValue;
            }
        }

        /// <summary>
        ///  ��ȡָ���������ļ���ָ��Key��ֵ
        /// </summary>
        /// <param name="szKeyName">��ȡ��Key����</param>
        /// <param name="szDefaultValue">ָ����Key������ʱ,���ص�ֵ</param>
        /// <returns>Keyֵ</returns>
        public static string GetConfigData(string szKeyName, string szDefaultValue)
        {
            string szConfigFile = string.Format("{0}\\{1}", GetWorkDirectory(), CONFIG_FILE);
            if (!File.Exists(szConfigFile))
            {
                return szDefaultValue;
            }

            XmlDocument clsXmlDoc = GetXmlDocument(szConfigFile);
            if (clsXmlDoc == null)
                return szDefaultValue;

            string szXPath = string.Format(".//key[@name='{0}']", szKeyName);
            XmlNode clsXmlNode = SelectXmlNode(clsXmlDoc, szXPath);
            if (clsXmlNode == null)
            {
                return szDefaultValue;
            }

            XmlNode clsValueAttr = clsXmlNode.Attributes.GetNamedItem("value");
            if (clsValueAttr == null)
                return szDefaultValue;
            return clsValueAttr.Value;
        }

        /// <summary>
        ///  ����ָ��Key��ֵ��ָ���������ļ���
        /// </summary>
        /// <param name="szKeyName">Ҫ���޸�ֵ��Key����</param>
        /// <param name="szValue">���޸ĵ�ֵ</param>
        public static bool WriteConfigData(string szKeyName, string szValue)
        {
            string szConfigFile = string.Format("{0}\\{1}", GetWorkDirectory(), CONFIG_FILE);
            if (!File.Exists(szConfigFile))
            {
                if (!CreateXmlFile(szConfigFile, "SystemConfig"))
                    return false;
            }
            XmlDocument clsXmlDoc = GetXmlDocument(szConfigFile);

            string szXPath = string.Format(".//key[@name='{0}']", szKeyName);
            XmlNode clsXmlNode = SelectXmlNode(clsXmlDoc, szXPath);
            if (clsXmlNode == null)
            {
                clsXmlNode = CreateXmlNode(clsXmlDoc, "key");
            }
            if (!SetXmlAttr(clsXmlNode, "name", szKeyName))
                return false;
            if (!SetXmlAttr(clsXmlNode, "value", szValue))
                return false;
            //
            return SaveXmlDocument(clsXmlDoc, szConfigFile);
        }
        #endregion
    }
}
