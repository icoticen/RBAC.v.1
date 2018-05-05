using K.Y.DLL;
using K.Y.DLL.PlugIn;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Table;

namespace VEHICLEDETECTING.Models.Static
{
    public class LayUI
    {
        public interface IData_List
        {
            List<Property> Properties { get; set; }
            Dictionary<string, object> Model { get; set; }
            List<Node> Nodes { get; set; }
            List<Sort> Sorts { get; set; }
            List<Button> Buttons { get; set; }
        }
        public interface IData_Tree
        {
            List<Property> Properties { get; set; }
            Dictionary<string, object> Model { get; set; }
            List<Node> Nodes { get; set; }
            List<Sort> Sorts { get; set; }
            List<Button> Buttons { get; set; }
            zTree.Data zTreeData { get; set; }
        }
        public interface IData_Analysis
        {
            List<Property> Properties { get; set; }
            Dictionary<string, object> Model { get; set; }
            List<Node> Nodes { get; set; }
            List<Sort> Sorts { get; set; }
            List<Button> Buttons { get; set; }
        }
        public interface IData_AjaxList
        {
            List<Property> Properties { get; set; }
            List<Row> TableData { get; set; }
        }
        public interface IData_Save
        {
            List<Property> Properties { get; set; }
            Dictionary<string, object> Model { get; set; }
            List<Node> Nodes { get; set; }
        }
        public interface IData_View
        {
            List<Property> Properties { get; set; }
            Dictionary<string, object> Model { get; set; }

        }

        public class Data : IData_List, IData_Tree, IData_Analysis, IData_AjaxList, IData_Save, IData_View
        {

            private String TableName { get; set; } = "";

            public List<Property> Properties { get; set; } = new List<Property>();
            public Dictionary<string, object> Model { get; set; } = new Dictionary<string, object>();
            public List<Node> Nodes { get; set; } = new List<Node>();

            public List<Sort> Sorts { get; set; } = new List<Sort>();
            public List<Button> Buttons { get; set; } = new List<Button>();

            public List<Row> TableData { get; set; } = new List<Row>();

            public zTree.Data zTreeData { get; set; } = new zTree.Data();
            public EChart.Data EChartData { get; set; } = new EChart.Data();

            public Data()
            {
            }
            public Data(string tableName, 
                bool btnTableInsert = false, 
                bool btnTableView = false, bool btnTableUpdate = false, bool btnTableDelete = false,
                bool btnTableInsertSibling = false, bool btnTableInsertChild = false,
                bool btnExcelExport = false,
                bool btnTreeInsertSibling = false, bool btnTreeInsertChild = false, 
                bool btnTreeView = false, bool btnTreeUpdate = false, bool btnTreeDelete = false)
            {
                this.TableName = tableName;
                this.AddButton(
                    btnTableInsert                                   : btnTableInsert,
                    btnTableView                                    : btnTableView,
                    btnTableUpdate                                : btnTableUpdate,
                    btnTableDelete                                                     : btnTableDelete,
                    btnTableInsertSibling                                 : btnTableInsertSibling,
                    btnTableInsertChild                                         : btnTableInsertChild,
                    btnExcelExport                                                : btnExcelExport,
                    btnTreeInsertSibling                                   : btnTreeInsertSibling,
                    btnTreeInsertChild                                              : btnTreeInsertChild,
                    btnTreeView                                           : btnTreeView,
                    btnTreeUpdate                              : btnTreeUpdate,
                    btnTreeDelete                                             : btnTreeDelete
                    );
            }


            #region PROPERTY
            Data AddProperties(IEnumerable<Property> Range)
            {
                if (Range == null) return this;
                Properties.AddRange(Range);
                return this;
            }
            Data AddProperty(Property Property)
            {
                if (Property == null) return this;
                Properties.Add(Property);
                return this;
            }
            public Data AddProperty(string Code, E_Property_Type Type = E_Property_Type.@text, string Name = null)
            {
                if (string.IsNullOrWhiteSpace(Code)) return this;
                Properties.Add(new Property { Code = Code, Name = Name ?? Code, Type = Type.ToString() });
                return this;
            }

            #endregion

            #region MODEL
            public void AddModel(string Key, List<string> Value)
            {
                Value = Value ?? new List<string>();
                Model.Add(Key, Value);
            }
            public void AddModel(string Key, string Value)
            {
                Model.Add(Key, Value);
            }
            public void AddModel(string Key, int? Value)
            {
                Model.Add(Key, Value);
            }
            public void AddModel(string Key, bool? Value)
            {
                Model.Add(Key, Value);
            }
            public void AddModel(string Key, double? Value)
            {
                Model.Add(Key, Value);
            }
            public void AddModel(string Key, DateTime? Value, String Format = "yyyy-MM-dd HH:mm:ss")
            {
                Model.Add(Key, Value.Ex_ToString(Format));
            }

            #endregion

            #region NODE
            public void AddNode(Node Node)
            {
                if (Node == null) return;
                Nodes.Add(Node);
            }
            public Data AddNode(string Key, List<System.Web.Mvc.SelectListItem> Elements)
            {
                if (Elements == null) return this;
                Nodes.Add(new Node { Key = Key, Elements = Elements });
                return this;
            }

            #endregion

            #region BUTTON

            private Button Btn_Table_Insert { get { return new Button("新增", Link: this.TableName + "_Insert"); } }
            private Button Btn_Table_InsertSibling { get { return new Button("新增同级", Site: E_Button_Site.item, Link: this.TableName + "_Insert", Param: new Dictionary<string, string> { { "ParentID", "{{=row.Cells.ParentID}}" } }); } }
            private Button Btn_Table_InsertChild { get { return new Button("新增子级", Site: E_Button_Site.item, Link: this.TableName + "_Insert", Param: new Dictionary<string, string> { { "ParentID", "{{=row.Cells.ID}}" } }); } }
            private Button Btn_Table_View { get { return new Button("查看", Site: E_Button_Site.item, Link: TableName + "_View", Param: new Dictionary<string, string> { { "ID", "{{=row.Cells.ID}}" } }); } }
            private Button Btn_Table_Update { get { return new Button("编辑", Site: E_Button_Site.item, Link: TableName + "_Update", Param: new Dictionary<string, string> { { "ID", "{{=row.Cells.ID}}" } }); } }
            private Button Btn_Table_Delete { get { return new Button("删除", Site: E_Button_Site.item, Link: TableName + "_Delete", Param: new Dictionary<string, string> { { "ID", "{{=row.Cells.ID}}" } }, Confirm: true, ConfirmText: "确认删除？？？？"); } }

            private Button Btn_Tree_InsertSibling { get { return new Button("新增同级", Link: "Tree_AddSibling()", ActionType: LayUI.E_Button_Action.function, Confirm: true); } }
            private Button Btn_Tree_InsertChild { get { return new Button("新增子级", Link: "Tree_AddChild()", ActionType: LayUI.E_Button_Action.function, Confirm: true); } }
            private Button Btn_Tree_View { get { return new Button("查看节点", Link: "Tree_View()", ActionType: LayUI.E_Button_Action.function, Confirm: true); } }
            private Button Btn_Tree_Update { get { return new Button("编辑节点", Link: "Tree_Update()", ActionType: LayUI.E_Button_Action.function, Confirm: true); } }
            private Button Btn_Tree_Delete { get { return new Button("删除节点", Link: "Tree_Delete()", ActionType: LayUI.E_Button_Action.function, Confirm: true); } }

            private Button Btn_Excel_Export { get { return new Button("导出", Link: "Excel_Export()", ActionType: E_Button_Action.function); } }


            //public Data AddButton(Button Button)
            //{
            //    if (Button == null) return this;
            //    Buttons.Add(Button);
            //    return this;
            //}
            public Data AddButtons(params Button[] buttons)
            {
                if (buttons != null)
                    Buttons.AddRange(buttons);
                return this;
            }
            public Data AddButton(bool btnTableInsert = false, 
                bool btnTableView = false, bool btnTableUpdate = false, bool btnTableDelete = false,
                bool btnTableInsertSibling = false, bool btnTableInsertChild = false,
                bool btnExcelExport = false, 
                bool btnTreeInsertSibling = false, bool btnTreeInsertChild = false, 
                bool btnTreeView = false, bool btnTreeUpdate = false, bool btnTreeDelete = false)
            {
                Buttons = new List<Button>();
                if (btnTableInsert) AddButtons(Btn_Table_Insert);
                if (btnTableView) AddButtons(Btn_Table_View);
                if (btnTableUpdate) AddButtons(Btn_Table_Update);
                if (btnTableDelete) AddButtons(Btn_Table_Delete);

                if (btnTableInsertChild) AddButtons(Btn_Table_InsertChild);
                if (btnTableInsertSibling) AddButtons(Btn_Table_InsertSibling);

                if (btnExcelExport) AddButtons(Btn_Excel_Export);

                if (btnTreeInsertSibling) AddButtons(Btn_Tree_InsertSibling);
                if (btnTreeInsertChild) AddButtons(Btn_Tree_InsertChild);
                if (btnTreeView) AddButtons(Btn_Tree_View);
                if (btnTreeUpdate) AddButtons(Btn_Tree_Update);
                if (btnTreeDelete) AddButtons(Btn_Tree_Delete);
                return this;
            }
            public Data AddButton(string Name, E_Button_Site Site = E_Button_Site.model, String Link = "", Dictionary<String, string> Param = null, bool Confirm = false, string ConfirmText = "确认执行操作？", E_Button_Action ActionType = E_Button_Action.layer)
            {
                Buttons.Add(new Button { Site = Site.ToString(), Link = Link, Name = Name, Param = Param, Confirm = Confirm, ConfirmText = ConfirmText, ActionType = ActionType.ToString() });
                return this;
            }

            #endregion

            #region SORT
            public void AddSort(Sort Sort)
            {
                if (Sort == null) return;
                Sorts.Add(Sort);
            }
            public void AddSort(string Code, bool? IsAsc = true)
            {
                Sorts.Add(new Sort { Code = Code, IsAsc = IsAsc ?? true });
            }
            #endregion



            public object GetModel(string Key, object Default = null)
            {
                return Model.Keys.Contains(Key) ? Model[Key] : Default;
            }

            public List<System.Web.Mvc.SelectListItem> GetNode(string Key, List<System.Web.Mvc.SelectListItem> Default = null)
            {
                var N = Nodes.FirstOrDefault(p => p.Key == Key);
                return N == null
                    ? (Default == null
                        ? new List<System.Web.Mvc.SelectListItem>()
                        : Default
                      )
                    : N.Elements;
            }
            public Boolean ExistNode(string Key = "")
            {
                if (String.IsNullOrWhiteSpace(Key))
                    return Nodes != null || Nodes.Count > 0;
                return Nodes.Any(p => p.Key == Key);
            }
            public Boolean ExistButton(string Name = "")
            {
                if (String.IsNullOrWhiteSpace(Name))
                    return Buttons != null || Buttons.Count > 0;
                return Buttons.Any(p => p.Name == Name);
            }
            public void Translate(Dictionary<String, String> Dic = null)
            {
                Properties.ForEach(p => p.Translate(Dic));
            }
        }

        public class Property
        {
            public string Code { get; set; }
            public string Name { get; set; }
            public string Type { get; set; }
            public Property Translate(Dictionary<string, string> Dic = null)
            {
                Name = LayUI.Translate(Name, Dic);
                return this;
            }
        }
        public class Node
        {
            public string Key { get; set; }
            public List<System.Web.Mvc.SelectListItem> Elements { get; set; }
        }
        public class Sort
        {
            public string Code { get; set; }
            public bool IsAsc { get; set; }
        }
        public class Button
        {
            public string Name { get; set; }
            public String Site { get; set; }
            public string ActionType { get; set; }
            public string Link { get; set; }
            public Dictionary<string, string> Param { get; set; }
            public bool Confirm { get; set; }
            public string ConfirmText { get; set; }

            public string ClickScript
            {
                get
                {
                    if (ActionType == "layer")
                        return Confirm
                            ? $"layer.confirm('{ConfirmText }', {{ btn: ['确定', '取消'] }}, function () {{ LayUI_Layer_OpeniFrame(layer, '{ Link  }?{ (Param == null ? "" : String.Join("&", Param.Select(p => p.Key + "=" + (p.Value ?? "")))) }'); }}, function () {{ }})"
                            : $"LayUI_Layer_OpeniFrame(layer,'{ Link}?{ (Param == null ? "" : String.Join("&", Param.Select(p => $"{p.Key}={(p.Value ?? "")}"))) }','{Name}')";
                    return Link;
                }
            }

            public Button()
            {
                Site = "item";
                ActionType = "layer";
                Confirm = false;
            }
            public Button(string Name, E_Button_Site Site = E_Button_Site.model, String Link = "", Dictionary<String, string> Param = null, bool Confirm = false, string ConfirmText = "确认执行操作？", E_Button_Action ActionType = E_Button_Action.layer)
            {
                this.Site = Site.ToString();
                this.Link = Link;
                this.Name = Name;
                this.Param = Param;
                this.Confirm = Confirm;
                this.ConfirmText = ConfirmText;
                this.ActionType = ActionType.ToString();
            }
            public String ToHtml(String _Link = "")
            {
                if (ActionType == "layer")
                    return Confirm
                        ? $"layer.confirm('{ConfirmText }', {{ btn: ['确定', '取消'] }}, function () {{ LayUI_Layer_OpeniFrame(layer, '{(string.IsNullOrWhiteSpace(_Link) ? Link : _Link) }?{ (Param == null ? "" : String.Join("&", Param.Select(p => p.Key + "=" + (p.Value ?? "")))) }'); }}, function () {{ }})"
                        : $"LayUI_Layer_OpeniFrame(layer,'{(string.IsNullOrWhiteSpace(_Link) ? Link : _Link) }?{ (Param == null ? "" : String.Join("&", Param.Select(p => $"{p.Key}={(p.Value ?? "")}"))) }','{Name}')";
                return Link;
            }
        }

        public class Row
        {
            public Dictionary<String, object> Cells { get; set; }
            public List<Button> Buttons { get; set; }
        }
        public static String Translate(String Name, Dictionary<string, string> Dic = null)
        {
            return Dic == null
                ? Name
                : Dic.Keys.Contains(Name)
                    ? Dic[Name]
                    : Name;
        }

        public enum E_Property_Type
        {
            text,
            textarea,
            html,
            date,
            datetime,
            @enum,
            list,
            listac,
            @bool,
            video,
            image,
            imagelist,

            ztreeextra,

            file,
            download,

            hidden,
        }
        public enum E_Button_Site
        {
            model = 1,
            item = 2,
        }
        public enum E_Button_Action
        {
            layer = 1,
            function = 2,
        }
        public enum E_Button_Default
        {
            layer = 1,
            function = 2,
        }
    }
}