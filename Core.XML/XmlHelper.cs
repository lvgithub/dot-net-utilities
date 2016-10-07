using System; 
using System.Data; 
using System.IO; 
using System.Xml;    

namespace Core.XML
{ 
    /// <summary> 
    /// XML 操作基类 
    /// </summary> 
    public class XMLHelper //: IDisposable 
    {

        #region XML文件操作
        /// <summary>
        /// 从XML文件转换为Object对象类型.
        /// </summary>
        /// <param name="path">XML文件路径</param>
        /// <param name="type">Object对象类型</param>
        /// <returns></returns>
        public static object LoadObjectFromXml(string path, Type type)
        {
            object obj = null;
            using (StreamReader reader = new StreamReader(path))
            {
                string content = reader.ReadToEnd();
                obj = XmlConvertor.XmlToObject(content, type);
            }
            return obj;
        }

        /// <summary>
        /// 保存对象到特定格式的XML文件
        /// </summary>
        /// <param name="path">XML文件路径.</param>
        /// <param name="obj">待保存的对象</param>
        public static void SaveObjectToXml(string path, object obj)
        {
            string xml = XmlConvertor.ObjectToXml(obj, true);
            using (StreamWriter writer = new StreamWriter(path))
            {
                writer.Write(xml);
            }
        }
        #endregion

         
        //以下为单一功能的静态类 
         
        #region 读取XML到DataSet 
        /************************************************** 
         * 函数名称:GetXml(string XmlPath) 
         * 功能说明:读取XML到DataSet 
         * 参    数:XmlPath:xml文档路径 
         * 使用示列: 
         *          using EC; //引用命名空间 
         *          string xmlPath = Server.MapPath("/EBDnsConfig/DnsConfig.xml"); //获取xml路径 
         *          DataSet ds = EC.XmlObject.GetXml(xmlPath); //读取xml到DataSet中 
         ************************************************/ 
        /// <summary> 
        /// 功能:读取XML到DataSet中 
        /// </summary> 
        /// <param name="XmlPath">xml路径</param> 
        /// <returns>DataSet</returns> 
        public static DataSet GetXml(string XmlPath) 
        { 
            DataSet ds = new DataSet(); 
            ds.ReadXml(@XmlPath); 
            return ds; 
        } 
        #endregion 

        #region 读取xml文档并返回一个节点 
        /************************************************** 
         * 函数名称:ReadXmlReturnNode(string XmlPath,string Node) 
         * 功能说明:读取xml文档并返回一个节点:适用于一级节点 
         * 参    数: XmlPath:xml文档路径;Node 返回的节点名称  
         * 适应用Xml:<?xml version="1.0" encoding="utf-8" ?> 
         *           <root> 
         *               <dns1>ns1.everdns.com</dns1> 
         *          </root> 
         * 使用示列: 
         *          using EC; //引用命名空间 
         *          string xmlPath = Server.MapPath("/EBDnsConfig/DnsConfig.xml"); //获取xml路径 
         *          Response.Write(XmlObject.ReadXmlReturnNode(xmlPath, "mailmanager")); 
         ************************************************/ 
        /// <summary> 
        /// 读取xml文档并返回一个节点:适用于一级节点 
        /// </summary> 
        /// <param name="XmlPath">xml路径</param> 
        /// <param name="NodeName">节点</param> 
        /// <returns></returns> 
        public static string ReadXmlReturnNode(string XmlPath,string Node) 
        { 
            XmlDocument docXml = new XmlDocument(); 
            docXml.Load(@XmlPath); 
            XmlNodeList xn = docXml.GetElementsByTagName(Node); 
            return xn.Item(0).InnerText.ToString();            
        } 
        #endregion 

        #region 查找数据,返回一个DataSet 
        /************************************************** 
         * 函数名称:GetXmlData(string xmlPath, string XmlPathNode) 
         * 功能说明:查找数据,返回当前节点的所有下级节点,填充到一个DataSet中 
         * 参    数:xmlPath:xml文档路径;XmlPathNode:当前节点的路径 
         * 使用示列: 
         *          using EC; //引用命名空间 
         *          string xmlPath = Server.MapPath("/EBDomainConfig/DomainConfig.xml"); //获取xml路径 
         *          DataSet ds = new DataSet(); 
         *          ds = XmlObject.GetXmlData(xmlPath, "root/items");//读取当前路径 
         *          this.GridView1.DataSource = ds; 
         *          this.GridView1.DataBind(); 
         *          ds.Clear(); 
         *          ds.Dispose(); 
         * Xml示例: 
         *         <?xml version="1.0" encoding="utf-8" ?> 
         *            <root> 
         *              <items name="xinnet"> 
         *                <url>http://www.paycenter.com.cn/cgi-bin/</url> 
         *                <port>80</port> 
         *              </items> 
         *            </root> 
         ************************************************/ 
        /// <summary> 
        /// 查找数据,返回当前节点的所有下级节点,填充到一个DataSet中 
        /// </summary> 
        /// <param name="xmlPath">xml文档路径</param> 
        /// <param name="XmlPathNode">节点的路径:根节点/父节点/当前节点</param> 
        /// <returns></returns> 
        public static DataSet GetXmlData(string xmlPath, string XmlPathNode) 
        { 
            XmlDocument objXmlDoc = new XmlDocument(); 
            objXmlDoc.Load(xmlPath); 
            DataSet ds = new DataSet(); 
            StringReader read = new StringReader(objXmlDoc.SelectSingleNode(XmlPathNode).OuterXml); 
            ds.ReadXml(read); 
            return ds;         
        } 


        #endregion 

        #region 更新Xml节点内容 
        /************************************************** 
         * 函数名称:XmlNodeReplace(string xmlPath,string Node,string Content) 
         * 功能说明:更新Xml节点内容 
         * 参    数:xmlPath:xml文档路径;Node:当前节点的路径;Content:内容 
         * 使用示列: 
         *          using EC; //引用命名空间 
         *          string xmlPath = Server.MapPath("/EBDomainConfig/DomainConfig.xml"); //获取xml路径 
         *          XmlObject.XmlNodeReplace(xmlPath, "root/test", "56789");  //更新节点内容 
         ************************************************/ 
        /// <summary> 
        /// 更新Xml节点内容 
        /// </summary> 
        /// <param name="xmlPath">xml路径</param> 
        /// <param name="Node">要更换内容的节点:节点路径 根节点/父节点/当前节点</param> 
        /// <param name="Content">新的内容</param> 
        public static void XmlNodeReplace(string xmlPath,string Node,string Content) 
        { 
            XmlDocument objXmlDoc = new XmlDocument(); 
            objXmlDoc.Load(xmlPath); 
            objXmlDoc.SelectSingleNode(Node).InnerText = Content; 
            objXmlDoc.Save(xmlPath); 
             
        } 

        #endregion 

        #region 删除XML节点和此节点下的子节点 
        /************************************************** 
         * 函数名称:XmlNodeDelete(string xmlPath,string Node) 
         * 功能说明:删除XML节点和此节点下的子节点 
         * 参    数:xmlPath:xml文档路径;Node:当前节点的路径; 
         * 使用示列: 
         *          using EC; //引用命名空间 
         *          string xmlPath = Server.MapPath("/EBDomainConfig/DomainConfig.xml"); //获取xml路径 
         *          XmlObject.XmlNodeDelete(xmlPath, "root/test");  //删除当前节点 
         ************************************************/ 
        /// <summary> 
        /// 删除XML节点和此节点下的子节点 
        /// </summary> 
        /// <param name="xmlPath">xml文档路径</param> 
        /// <param name="Node">节点路径</param> 
        public static void XmlNodeDelete(string xmlPath,string Node) 
        { 
            XmlDocument objXmlDoc = new XmlDocument(); 
            objXmlDoc.Load(xmlPath); 
            string mainNode = Node.Substring(0, Node.LastIndexOf("/")); 
            objXmlDoc.SelectSingleNode(mainNode).RemoveChild(objXmlDoc.SelectSingleNode(Node)); 
            objXmlDoc.Save(xmlPath); 
        } 

        #endregion 

        #region 插入一个节点和此节点的字节点 
        /************************************************** 
         * 函数名称:XmlInsertNode(string xmlPath, string MailNode, string ChildNode, string Element,string Content) 
         * 功能说明:插入一个节点和此节点的字节点 
         * 参    数:xmlPath:xml文档路径;MailNode:当前节点的路径;ChildNode:新插入的节点;Element:插入节点的子节点;Content:子节点的内容 
         * 使用示列: 
         *          using EC; //引用命名空间 
         *          string xmlPath = Server.MapPath("/EBDomainConfig/DomainConfig.xml"); //获取xml路径 
         *          XmlObject.XmlInsertNode(xmlPath, "root/test","test1","test2","测试内容");  //插入一个节点和此节点的字节点 
         * 生成的XML格式为 
         *          <test> 
         *               <test1> 
         *                    <test2>测试内容</test2> 
         *                </test1> 
         *            </test> 
         ************************************************/ 
        /// <summary> 
        /// 插入一个节点和此节点的字节点 
        /// </summary> 
        /// <param name="xmlPath">xml路径</param> 
        /// <param name="MailNode">当前节点路径</param> 
        /// <param name="ChildNode">新插入节点</param> 
        /// <param name="Element">插入节点的子节点</param> 
        /// <param name="Content">子节点的内容</param> 
        public static void XmlInsertNode(string xmlPath, string MailNode, string ChildNode, string Element,string Content) 
        { 
            XmlDocument objXmlDoc = new XmlDocument(); 
            objXmlDoc.Load(xmlPath); 
            XmlNode objRootNode = objXmlDoc.SelectSingleNode(MailNode); 
            XmlElement objChildNode = objXmlDoc.CreateElement(ChildNode); 
            objRootNode.AppendChild(objChildNode); 
            XmlElement objElement = objXmlDoc.CreateElement(Element); 
            objElement.InnerText = Content; 
            objChildNode.AppendChild(objElement); 
            objXmlDoc.Save(xmlPath); 
        } 

        #endregion 

        #region 插入一节点,带一属性 
        /************************************************** 
         * 函数名称:XmlInsertElement(string xmlPath, string MainNode, string Element, string Attrib, string AttribContent, string Content) 
         * 功能说明:插入一节点,带一属性 
         * 参    数:xmlPath:xml文档路径;MailNode:当前节点的路径;Element:新插入的节点;Attrib:属性名称;AttribContent:属性值;Content:节点的内容 
         * 使用示列: 
         *          using EC; //引用命名空间 
         *          string xmlPath = Server.MapPath("/EBDomainConfig/DomainConfig.xml"); //获取xml路径 
         *         XmlObject.XmlInsertElement(xmlPath, "root/test", "test1", "Attrib", "属性值", "节点内容");  //插入一节点,带一属性 
         * 生成的XML格式为 
         *          <test> 
         *              <test1 Attrib="属性值">节点内容</test1> 
         *          </test> 
         ************************************************/ 
        /// <summary> 
        /// 插入一节点,带一属性 
        /// </summary> 
        /// <param name="xmlPath">Xml文档路径</param> 
        /// <param name="MainNode">当前节点路径</param> 
        /// <param name="Element">新节点</param> 
        /// <param name="Attrib">属性名称</param> 
        /// <param name="AttribContent">属性值</param> 
        /// <param name="Content">新节点值</param> 
        public static void XmlInsertElement(string xmlPath, string MainNode, string Element, string Attrib, string AttribContent, string Content) 
        { 
            XmlDocument objXmlDoc = new XmlDocument(); 
            objXmlDoc.Load(xmlPath); 
            XmlNode objNode = objXmlDoc.SelectSingleNode(MainNode); 
            XmlElement objElement = objXmlDoc.CreateElement(Element); 
            objElement.SetAttribute(Attrib, AttribContent); 
            objElement.InnerText = Content; 
            objNode.AppendChild(objElement); 
            objXmlDoc.Save(xmlPath); 
        } 

        #endregion 

        #region 插入一节点不带属性 
        /************************************************** 
         * 函数名称:XmlInsertElement(string xmlPath, string MainNode, string Element, string Content) 
         * 功能说明:插入一节点不带属性 
         * 参    数:xmlPath:xml文档路径;MailNode:当前节点的路径;Element:新插入的节点;Content:节点的内容 
         * 使用示列: 
         *          using EC; //引用命名空间 
         *          string xmlPath = Server.MapPath("/EBDomainConfig/DomainConfig.xml"); //获取xml路径 
         *          XmlObject.XmlInsertElement(xmlPath, "root/test", "text1", "节点内容");  //插入一节点不带属性 
         * 生成的XML格式为 
         *          <test> 
         *                  <text1>节点内容</text1> 
         *          </test> 
         ************************************************/ 
        public static void XmlInsertElement(string xmlPath, string MainNode, string Element, string Content) 
        { 
            XmlDocument objXmlDoc = new XmlDocument(); 
            objXmlDoc.Load(xmlPath); 
            XmlNode objNode = objXmlDoc.SelectSingleNode(MainNode); 
            XmlElement objElement = objXmlDoc.CreateElement(Element); 
            objElement.InnerText = Content; 
            objNode.AppendChild(objElement); 
            objXmlDoc.Save(xmlPath); 
        } 

        #endregion 
        

        //必须创建对象才能使用的类 

        private bool _alreadyDispose = false; 
        private string xmlPath;  
        private XmlDocument xmlDoc=new XmlDocument();    

        private XmlNode xmlNode; 
        private XmlElement xmlElem;     

        #region 构造与释构 
        public XMLHelper() 
        {             

        }
        ~XMLHelper() 
        { 
            Dispose(); 
        } 
        protected virtual void Dispose(bool isDisposing) 
        { 
            if (_alreadyDispose) return; 
            if (isDisposing) 
            { 
               // 
            } 
            _alreadyDispose = true; 
        } 
        #endregion 

        #region IDisposable 成员 

        public void Dispose() 
        { 
            Dispose(true); 
            GC.SuppressFinalize(this); 
        } 

        #endregion 

        #region 创建xml文档 
        /************************************************** 
         * 对象名称:XmlObject 
         * 功能说明:创建xml文档         
         * 使用示列: 
         *          using EC; //引用命名空间 
         *          string xmlPath = Server.MapPath("test.xml"); 
         *          XmlObject obj = new XmlObject(); 
         *          创建根节点 
         *          obj.CreateXmlRoot("root"); 
         *          // 创建空节点 
         *          //obj.CreatXmlNode("root", "Node"); 
         *          //创建一个带值的节点 
         *          //obj.CreatXmlNode("root", "Node", "带值的节点"); 
         *          //创建一个仅带属性的节点 
         *          //obj.CreatXmlNode("root", "Node", "Attribe", "属性值"); 
         *          //创建一个仅带两个属性值的节点 
         *          //obj.CreatXmlNode("root", "Node", "Attribe", "属性值", "Attribe2", "属性值2"); 
         *          //创建一个带属性值的节点值的节点 
         *          // obj.CreatXmlNode("root", "Node", "Attribe", "属性值","节点值"); 
         *          //在当前节点插入带两个属性值的节点 
         *          obj.CreatXmlNode("root", "Node", "Attribe", "属性值", "Attribe2", "属性值2","节点值"); 
         *          obj.XmlSave(xmlPath); 
         *          obj.Dispose();         
         ************************************************/ 


        #region 创建一个只有声明和根节点的XML文档 
        /// <summary> 
        /// 创建一个只有声明和根节点的XML文档 
        /// </summary> 
        /// <param name="root"></param> 
        public void CreateXmlRoot(string root) 
        { 
          //加入XML的声明段落 
            xmlNode = xmlDoc.CreateNode(XmlNodeType.XmlDeclaration, "", ""); 
            xmlDoc.AppendChild(xmlNode); 
            //加入一个根元素 
            xmlElem = xmlDoc.CreateElement("", root, ""); 
            xmlDoc.AppendChild(xmlElem); 

        } 
        #endregion 

        #region 在当前节点下插入一个空节点节点 
        /// <summary> 
        /// 在当前节点下插入一个空节点节点 
        /// </summary> 
        /// <param name="mainNode">当前节点路径</param> 
        /// <param name="node">节点名称</param> 
        public void CreatXmlNode(string mainNode,string node) 
        { 
            XmlNode MainNode = xmlDoc.SelectSingleNode(mainNode); 
            XmlElement objElem = xmlDoc.CreateElement(node);             
            MainNode.AppendChild(objElem); 
        } 
        #endregion 

        #region 在当前节点插入一个仅带值的节点 
        /// <summary> 
        ///  在当前节点插入一个仅带值的节点 
        /// </summary> 
        /// <param name="mainNode">当前节点</param> 
        /// <param name="node">新节点</param> 
        /// <param name="content">新节点值</param> 
        public void CreatXmlNode(string mainNode, string node, string content) 
        { 
            XmlNode MainNode = xmlDoc.SelectSingleNode(mainNode); 
            XmlElement objElem = xmlDoc.CreateElement(node); 
            objElem.InnerText = content; 
            MainNode.AppendChild(objElem); 
        } 
        #endregion 

        #region 在当前节点的插入一个仅带属性值的节点 
        /// <summary> 
        /// 在当前节点的插入一个仅带属性值的节点 
        /// </summary> 
        /// <param name="MainNode">当前节点或路径</param> 
        /// <param name="Node">新节点</param> 
        /// <param name="Attrib">新节点属性名称</param> 
        /// <param name="AttribValue">新节点属性值</param> 
        public void CreatXmlNode(string MainNode, string Node, string Attrib, string AttribValue) 
        { 
            XmlNode mainNode = xmlDoc.SelectSingleNode(MainNode); 
            XmlElement objElem = xmlDoc.CreateElement(Node); 
            objElem.SetAttribute(Attrib, AttribValue); 
            mainNode.AppendChild(objElem); 
        }        
        #endregion 

        #region 创建一个带属性值的节点值的节点 
        /// <summary> 
        /// 创建一个带属性值的节点值的节点 
        /// </summary> 
        /// <param name="MainNode">当前节点或路径</param> 
        /// <param name="Node">节点名称</param> 
        /// <param name="Attrib">属性名称</param> 
        /// <param name="AttribValue">属性值</param> 
        /// <param name="Content">节点传情</param> 
        public void CreatXmlNode(string MainNode, string Node, string Attrib, string AttribValue,string Content) 
        { 
            XmlNode mainNode = xmlDoc.SelectSingleNode(MainNode); 
            XmlElement objElem = xmlDoc.CreateElement(Node); 
            objElem.SetAttribute(Attrib, AttribValue); 
            objElem.InnerText = Content; 
            mainNode.AppendChild(objElem); 
        }        
        #endregion       

        #region 在当前节点的插入一个仅带2个属性值的节点 
        /// <summary> 
        ///  在当前节点的插入一个仅带2个属性值的节点 
        /// </summary> 
        /// <param name="MainNode">当前节点或路径</param> 
        /// <param name="Node">节点名称</param> 
        /// <param name="Attrib">属性名称一</param> 
        /// <param name="AttribValue">属性值一</param> 
        /// <param name="Attrib2">属性名称二</param> 
        /// <param name="AttribValue2">属性值二</param> 
        public void CreatXmlNode(string MainNode, string Node, string Attrib, string AttribValue,string Attrib2,string AttribValue2) 
        { 
            XmlNode mainNode = xmlDoc.SelectSingleNode(MainNode); 
            XmlElement objElem = xmlDoc.CreateElement(Node); 
            objElem.SetAttribute(Attrib, AttribValue); 
            objElem.SetAttribute(Attrib2, AttribValue2); 
            mainNode.AppendChild(objElem); 
        } 
        #endregion 

        #region 在当前节点插入带两个属性的节点 
        /// <summary> 
        ///  在当前节点插入带两个属性的节点 
        /// </summary> 
        /// <param name="MainNode">当前节点或路径</param> 
        /// <param name="Node">节点名称</param> 
        /// <param name="Attrib">属性名称一</param> 
        /// <param name="AttribValue">属性值一</param> 
        /// <param name="Attrib2">属性名称二</param> 
        /// <param name="AttribValue2">属性值二</param> 
        /// <param name="Content">节点值</param> 
        public void CreatXmlNode(string MainNode, string Node, string Attrib, string AttribValue, string Attrib2, string AttribValue2,string Content) 
        { 
            XmlNode mainNode = xmlDoc.SelectSingleNode(MainNode); 
            XmlElement objElem = xmlDoc.CreateElement(Node); 
            objElem.SetAttribute(Attrib, AttribValue); 
            objElem.SetAttribute(Attrib2, AttribValue2); 
            objElem.InnerText = Content; 
            mainNode.AppendChild(objElem); 
        } 
        #endregion 

        #region 保存Xml 
        /// <summary> 
        /// 保存Xml 
        /// </summary> 
        /// <param name="path">保存的当前路径</param> 
        public void XmlSave(string path) 
        { 
            xmlDoc.Save(path); 
        } 

        #endregion 

        #endregion 

        #region 根据父节点属性值读取子节点值 
        /************************************************** 
         * 函数名称:GetSubElementByAttribute(string XmlPath, string FatherElenetName, string AttributeName, int AttributeIndex, int ArrayLength) 
         * 功能说明:根据父节点属性值读取子节点值 
         * 参    数: XmlPath:xml路径;FatherElenetName:父节点名;AttributeName:属性值;AttributeIndex:属性索引;ArrayLength:要返回的节点数组长度 
         * 适应用Xml: 
         * <root> 
      *   <page name="/index.aspx"> 
   *      <title>域名注册、虚拟主机、企业邮局、服务器托管、网站空间租用|---第一商务</title> 
   *      <keywords>虚拟主机，域名注册，服务器托管，杭州，服务器租用，</keywords> 
   *      <description>描述内容 </description> 
      *    </page> 
         * </root> 
         *           ArrayList al = new ArrayList(); 
         *           al = EC.XmlObject.GetSubElementByAttribute(XmlPath, "page", "/index.aspx", 0, 3); 
         *           for (int i = 0; i < al.Count; i++) 
         *           { 
         *               Response.Write(al[i].ToString()); 
         *               Response.Write("<br>"); 
         *           } 
         ************************************************/ 
        /// <summary> 
        /// 根据父节点属性读取字节点值 
        /// </summary> 
        /// <param name="XmlPath">xml路径</param> 
        /// <param name="FatherElenetName">父节点名</param> 
        /// <param name="AttributeName">属性值</param> 
        /// <param name="AttributeIndex">属性索引</param> 
        /// <param name="ArrayLength">要返回的节点数组长度</param> 
        /// <returns></returns> 
        public static System.Collections.ArrayList GetSubElementByAttribute(string XmlPath, string FatherElenetName, string AttributeName, int AttributeIndex, int ArrayLength) 
        { 
            System.Collections.ArrayList al = new System.Collections.ArrayList(); 
            XmlDocument docXml = new XmlDocument(); 
            docXml.Load(@XmlPath); 
            XmlNodeList xn = docXml.DocumentElement.ChildNodes; 
            //遍历第一层节点 
            foreach (XmlElement element in xn) 
            { 
                //判断父节点是否为指定节点 
                if (element.Name == FatherElenetName) 
                { 
                    //判断父节点属性的索引是否大于指定索引 
                    if (element.Attributes.Count < AttributeIndex) 
                        return null; 
                    //判断父节点的属性值是否等于指定属性 
                    if (element.Attributes[AttributeIndex].Value == AttributeName) 
                    { 
                        XmlNodeList xx = element.ChildNodes; 
                        if (xx.Count > 0) 
                        { 
                            for (int i = 0; i < ArrayLength & i < xx.Count; i++) 
                            { 
                                al.Add(xx[i].InnerText); 
                            } 
                        } 
                    } 

                } 
            } 
            return al; 
        } 

        #endregion 

        #region 根据节点属性读取子节点值(较省资源模式) 
        /************************************************** 
         * 函数名称:GetSubElementByAttribute(string XmlPath, string FatherElement, string AttributeName, string AttributeValue, int ArrayLength) 
         * 功能说明:根据父节点属性值读取子节点值 
         * 参    数: XmlPath:xml路径;FatherElenetName:父节点名;AttributeName:属性名;AttributeValue:属性值;ArrayLength:要返回的节点数组长度 
         * 适应用Xml: 
         * <root> 
      *   <page name="/index.aspx"> 
   *      <title>域名注册、虚拟主机、企业邮局、服务器托管、网站空间租用|---第一商务</title> 
   *      <keywords>虚拟主机，域名注册，服务器托管，杭州，服务器租用，</keywords> 
   *      <description>描述内容 </description> 
      *    </page> 
         * </root> 
         *           ArrayList al = new ArrayList(); 
         *           al = EC.XmlObject.GetSubElementByAttribute(XmlPath, "page", "@name", "/index.aspx", 3); 
         *           for (int i = 0; i < al.Count; i++) 
         *           { 
         *               Response.Write(al[i].ToString()); 
         *               Response.Write("<br>"); 
         *           } 
         ************************************************/ 
        /// <summary> 
        /// 根据节点属性读取子节点值(较省资源模式) 
        /// </summary> 
        /// <param name="XmlPath">xml路径</param> 
        /// <param name="FatherElement">父节点值</param> 
        /// <param name="AttributeName">属性名称</param> 
        /// <param name="AttributeValue">属性值</param> 
        /// <param name="ArrayLength">返回的数组长度</param> 
        /// <returns></returns> 
        public static  System.Collections.ArrayList GetSubElementByAttribute(string XmlPath, string FatherElement, string AttributeName, string AttributeValue, int ArrayLength) 
        { 
            System.Collections.ArrayList al = new System.Collections.ArrayList(); 
            XmlDocument docXml = new XmlDocument(); 
            docXml.Load(@XmlPath); 
            XmlNodeList xn; 
            xn = docXml.DocumentElement.SelectNodes("//" + FatherElement + "[" + @AttributeName + "='" + AttributeValue + "']"); 
            XmlNodeList xx = xn.Item(0).ChildNodes; 
            for (int i = 0; i < ArrayLength & i < xx.Count; i++) 
            { 

                al.Add(xx.Item(i).InnerText); 
            } 
            return al; 

        } 
        #endregion 
    } 
} 
