using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using C1.Win.C1FlexGrid;
namespace Tourist_Management.Main
{
    public partial class frmUserGroups : Form
    {
        private const string msghd = "Calculation Groups";
        public int Mode = 0; //TO GET TO KNOW WEATHER INSERTION OR UPDATION
        public int SystemCode = 0; // TO KEEP THE SYSTEM GENERATED CODE
        enum UG { gMID, gMD, gFID, gFD, gLR, gID, gIM, gNM, gAD, gED, gVI, gPR, gDE, gST };
        private int ImgIndex = -1;
        private int RowNumb = 1;
        float CountNode = 0;
        public string SqlQry = "SELECT ID,GroupName,GroupDesc,Convert(bit,UserMode) IsAdmin ,IsActive FROM dbo.mst_User_Groups Where Isnull(Status,0)<>7 Order By ID";
        string NodeRange;
        int ChkSts;
        public frmUserGroups(){InitializeComponent();}
        private void Intializer()
        {
            try
            {
                if (Mode == 0)
                {
                    ChkSts = 0;
                    Grd_Initializer();
                    Make_tree();
                }
                else
                {
                    ChkSts = SystemCode;
                    Grd_Initializer();
                    Make_tree();
                    Fill_Details();
                }
                Merge_Columns();
                treeMain.Nodes[0].Expand();
                Manage_Selected();
            }
            catch (Exception ex){db.MsgERR(ex);}
        }
        private void Grd_Initializer()
        {
            try
            {
                flx.Cols.Count = 14;
                flx.Rows.Count = 1;
                flx.Cols[(int)UG.gMID].Width = 0;
                flx.Cols[(int)UG.gMD].Width  = 0;
                flx.Cols[(int)UG.gFID].Width = 0;
                flx.Cols[(int)UG.gFD].Width  = 0;
                flx.Cols[(int)UG.gLR].Width  = 0;
                flx.Cols[(int)UG.gID].Width  = 0;
                flx.Cols[(int)UG.gIM].Width  = 20;
                flx.Cols[(int)UG.gNM].Width  = 168;
                flx.Cols[(int)UG.gAD].Width  = 60;
                flx.Cols[(int)UG.gED].Width  = 60;
                flx.Cols[(int)UG.gDE].Width  = 60;
                flx.Cols[(int)UG.gPR].Width  = 60;
                flx.Cols[(int)UG.gVI].Width  = 60;
                flx.Cols[(int)UG.gST].Width  = 60;
                flx.Cols[(int)UG.gMID].Caption = "MID";
                flx.Cols[(int)UG.gMD].Caption = "ModuleID";
                flx.Cols[(int)UG.gFID].Caption = "FID";
                flx.Cols[(int)UG.gFD].Caption = "FormID";
                flx.Cols[(int)UG.gLR].Caption = "LevelID";
                flx.Cols[(int)UG.gID].Caption = "NodeID";
                flx.Cols[(int)UG.gIM].Caption = "";
                flx.Cols[(int)UG.gNM].Caption = "Identity";
                flx.Cols[(int)UG.gAD].Caption = "Add";
                flx.Cols[(int)UG.gED].Caption = "Edit";
                flx.Cols[(int)UG.gDE].Caption = "Delete";
                flx.Cols[(int)UG.gPR].Caption = "Print";
                flx.Cols[(int)UG.gVI].Caption = "View";
                flx.Cols[(int)UG.gST].Caption = "Status";//WHETHER LEAF NODE OR PARENT NODE
                flx.Cols[(int)UG.gAD].DataType = Type.GetType("System.Boolean");
                flx.Cols[(int)UG.gED].DataType = Type.GetType("System.Boolean");
                flx.Cols[(int)UG.gDE].DataType = Type.GetType("System.Boolean");
                flx.Cols[(int)UG.gPR].DataType = Type.GetType("System.Boolean");
                flx.Cols[(int)UG.gVI].DataType = Type.GetType("System.Boolean");
                flx.Cols[(int)UG.gNM].AllowEditing = false;
                flx.Cols[(int)UG.gIM].ImageAlign = ImageAlignEnum.Stretch;
                flx.AllowMerging = AllowMergingEnum.RestrictCols ;
                 flx.Cols[(int)UG.gAD].AllowMerging = true;
                 flx.Cols[(int)UG.gED].AllowMerging = true;
                 flx.Cols[(int)UG.gDE].AllowMerging = true;
                 flx.Cols[(int)UG.gPR].AllowMerging = true;
                 flx.Cols[(int)UG.gVI].AllowMerging = true;
            }
            catch (Exception ex){db.MsgERR(ex);}
        }
        private void Fill_Details()
        {
            string ssql;
            DataTable DT=new DataTable();
            try
            {
                ssql = "SELECT GroupName,GroupDesc,IsActive,UserMode FROM mst_User_Groups WHERE ID=" + SystemCode + "";
                DT = Tourist_Management.Classes.clsGlobal.objCon.Fill_Table(ssql);
                if (DT.Rows.Count > 0)
                {
                    txtGroupName.Text = DT.Rows[0]["GroupName"].ToString();
                    txtGroupDesc.Text = DT.Rows[0]["GroupDesc"].ToString();
                    if (Convert.ToBoolean(DT.Rows[0]["IsActive"].ToString()))
                        chkActive.Checked = true;
                    else
                        chkActive.Checked = false;
                    if (DT.Rows[0]["UserMode"].ToString() == "1")
                        chkIsAdmin.Checked = true;
                    else
                        chkIsAdmin.Checked = false;
                }
            }
            catch (Exception ex){db.MsgERR(ex);}
        }
        private Boolean Validate_Data()
        {
            bool RtnVal = true;
                if (Tourist_Management.Classes.clsGlobal.objCon.Fill_Table("SELECT GroupName FROM mst_User_Groups WHERE GroupName='" + txtGroupName.Text.Trim() + "' and ID <> " + SystemCode + "").Rows.Count > 0)
                {
                    MessageBox.Show("Group Name Is Already Exist.", msghd, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return false;
                }
                if (txtGroupName.Text.Trim() == "")
                {
                    UserButton();
                    MessageBox.Show("Group Name Cannot Be Blank", msghd, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    txtGroupName.Select();
                    return false;
                }
                if (txtGroupDesc.Text.Trim() == "")
                {
                    UserButton();
                    MessageBox.Show("Group Description Cannot Be Blank", msghd, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    txtGroupDesc.Select();
                    return false;
                }
                return RtnVal;
        }
         private Boolean Save_Pro()
        {
            Boolean rtnVal = false;
                if (Validate_Data() == false)
                {
                    rtnVal = false;
                    return rtnVal;
                }
                if (Save_Procedure() == true)
                {
                    rtnVal = true;
                    return rtnVal;
                }
                return rtnVal;
        }
         private Boolean Save_Procedure()
         {
             System.Data.SqlClient.SqlCommand objCom;
             System.Data.SqlClient.SqlTransaction objTrn;
             System.Data.SqlClient.SqlConnection objCon;
                 objCom = new System.Data.SqlClient.SqlCommand();
                 objCon = Tourist_Management.Classes.clsGlobal.objCon.ReturnConnection;
                 objCon.Open();
                 objTrn = objCon.BeginTransaction();
                 objCom.Connection = objCon;
                 objCom.Transaction = objTrn;
                 if (Save_User_Groups(objCom) == true && User_Groups_Forms_Permission(objCom) == true)
                 {
                     objTrn.Commit();
                     MessageBox.Show("Transaction Sucessfully Completed", msghd, MessageBoxButtons.OK, MessageBoxIcon.Information);
                     objCon.Close();
                     return true;
                 }
                 else
                 {
                     objTrn.Rollback();
                     MessageBox.Show("Error Occured,Rollbacked", msghd, MessageBoxButtons.OK, MessageBoxIcon.Error);
                 }
                 objCon.Close();
                 return false;
         }
         private Boolean Save_User_Groups(System.Data.SqlClient.SqlCommand sqlCom)
        {
            Boolean RtnVal = false;
                sqlCom.CommandType = CommandType.StoredProcedure;
                sqlCom.CommandText = "spSave_User_Groups";
                sqlCom.Parameters.Add("@ID", SqlDbType.Int).Value = SystemCode;
                sqlCom.Parameters["@ID"].Direction = ParameterDirection.InputOutput;
                sqlCom.Parameters.Add("@GroupName", SqlDbType.VarChar, 50).Value = txtGroupName.Text.Trim();
                sqlCom.Parameters.Add("@GroupDesc", SqlDbType.VarChar, 100).Value = txtGroupDesc.Text.Trim();
                sqlCom.Parameters.Add("@IsActive", SqlDbType.Int).Value = chkActive.Checked == true ? "1" : "0";
                sqlCom.Parameters.Add("@UserMode", SqlDbType.Int).Value = chkIsAdmin.Checked == true ? "1" : "0";
                sqlCom.Parameters.Add("@CreatedBy", SqlDbType.Int).Value = 0;
                sqlCom.Parameters.Add("@InsMode", SqlDbType.Int).Value = Mode;
                sqlCom.Parameters.Add("@RtnValue", SqlDbType.Int).Value = 0;
                sqlCom.Parameters["@RtnValue"].Direction = ParameterDirection.InputOutput;
                sqlCom.ExecuteNonQuery();
                if ((int)sqlCom.Parameters["@RtnValue"].Value == 1)
                {
                    SystemCode = (int)sqlCom.Parameters["@ID"].Value;
                    RtnVal = true;
                }
                return RtnVal;
        }
         private Boolean User_Groups_Forms_Permission(System.Data.SqlClient.SqlCommand sqlCom)
         {
             int RowNumb;
             Boolean RtnVal = false;
                 sqlCom.CommandType = CommandType.StoredProcedure;
                 sqlCom.CommandText = "spSave_UserGroups_Permissions";
                 RowNumb = 1;
                 while (RowNumb < flx.Rows.Count)
                 {
                     sqlCom.Parameters.Clear();
                     sqlCom.Parameters.Add("@ID", SqlDbType.Int).Value = SystemCode;
                     sqlCom.Parameters.Add("@Status", SqlDbType.Int).Value = (flx[RowNumb, (int)UG.gST].ToString()) == "PR" ? "0" : "1";
                     if (flx[RowNumb, (int)UG.gST].ToString() == "LF" && CheckSelected("LF",RowNumb)==true)
                     {
                         sqlCom.Parameters.Add("@TypeID", SqlDbType.Int).Value = Int32.Parse(flx[RowNumb, (int)UG.gFD].ToString());
                         sqlCom.Parameters.Add("@Add", SqlDbType.Int).Value = Convert.ToBoolean(flx[RowNumb, (int)UG.gAD]) == true ? "1" : "0";
                         sqlCom.Parameters.Add("@Edit", SqlDbType.Int).Value = Convert.ToBoolean(flx[RowNumb, (int)UG.gED]) == true ? "1" : "0";
                         sqlCom.Parameters.Add("@Delete", SqlDbType.Int).Value = Convert.ToBoolean(flx[RowNumb, (int)UG.gDE]) == true ? "1" : "0";
                         sqlCom.Parameters.Add("@Print", SqlDbType.Int).Value = Convert.ToBoolean(flx[RowNumb, (int)UG.gPR]) == true ? "1" : "0";
                         sqlCom.Parameters.Add("@View", SqlDbType.Int).Value = Convert.ToBoolean(flx[RowNumb, (int)UG.gVI]) == true ? "1" : "0";
                         sqlCom.Parameters.Add("@RtnValue", SqlDbType.Int).Value = 0;
                         sqlCom.Parameters["@RtnValue"].Direction = ParameterDirection.InputOutput;
                         sqlCom.ExecuteNonQuery();
                         if ((int)sqlCom.Parameters["@RtnValue"].Value == 1)
                         {
                             RtnVal = true;
                         }
                     }
                     else if (CheckSelected("PR", RowNumb) == true)
                     {
                         sqlCom.Parameters.Add("@TypeID", SqlDbType.Int).Value = Int32.Parse(flx[RowNumb, (int)UG.gMD].ToString());//(flx[RowNumb, (int)UG.gFD].ToString()) == "PR" ? 
                         sqlCom.Parameters.Add("@View", SqlDbType.Int).Value = Convert.ToBoolean(flx[RowNumb, (int)UG.gVI]) == true ? "1" : "0";
                         sqlCom.Parameters.Add("@RtnValue", SqlDbType.Int).Value = 0;
                         sqlCom.Parameters["@RtnValue"].Direction = ParameterDirection.InputOutput;
                         sqlCom.ExecuteNonQuery();
                         if ((int)sqlCom.Parameters["@RtnValue"].Value == 1)
                         {
                             RtnVal = true;
                         }
                     }
                     RowNumb++;
                 }
                 return RtnVal;
         }
        private Boolean CheckSelected(string Status,int sRow)
        {
            try
            {
                if (Status == "PR")
                    return (Convert.ToBoolean(flx[sRow, (int)UG.gVI]));
                else
                    for (int i = (int)UG.gAD; i <= (int)UG.gDE; i++)
                    {
                        if (Convert.ToBoolean(flx[sRow, i]) == true)
                        {
                            return true;
                        }
                    }
                return false;
            }
            catch (Exception ex)
            {
                db.MsgERR(ex);
                return false;
            }
        }
        private void frmUserGroups_Load(object sender, EventArgs e)
        {
            try
            {
                Intializer();
            }
            catch (Exception ex){db.MsgERR(ex);}
        }
        private void Make_tree()
        {
            try
            {
                DataTable dt;
                string ssql;
                TreeNode trNode = new TreeNode("........");
                Image img;
                int Space = 4;
                Color SubColour=Color.PowderBlue;
                int IsModule = 1;
                ssql = " SELECT ModuleID, [Name], [Desc], ParentID, Img, IsActive,SortOrder,[View],[Add],[Edit],[Delete],[Print] FROM dbo.Fun_ReturnUserGroupPermission(" + ChkSts + ") Where Isnull(IsActive,0)=1 and ISNULL(ParentID,0)=0 Order by ModuleID ";
                dt = Tourist_Management.Classes.clsGlobal.objCon.Fill_Table(ssql);
                treeMain.Nodes.Add(trNode);
                treeMain.ImageList = imgTree;
                foreach (DataRow dr in dt.Rows)
                {
                    NodeRange = "";
                    TreeNode trn = new TreeNode(dr["Desc"].ToString());
                    trn.Name = dr["Name"].ToString();
                    img = Tourist_Management.Classes.clsGlobal.byteArrayToImage((byte[])dr["Img"]);
                    imgTree.Images.Add(img);
                    ImgIndex += 1;
                    trn.ImageIndex = ImgIndex;
                    trn.SelectedImageIndex = ImgIndex;
                    trNode.Nodes.Add(trn);
                    flx.Rows.Add();
                    flx.SetCellImage(RowNumb, (int)UG.gIM, img);
                    flx[RowNumb, (int)UG.gID] = CountNode;
                    flx[RowNumb, (int)UG.gMD] = dr["ModuleID"].ToString();
                    flx[RowNumb, (int)UG.gAD]=Convert.ToBoolean(dr["Add"]);
                    flx[RowNumb, (int)UG.gED] = Convert.ToBoolean(dr["Edit"]);
                    flx[RowNumb, (int)UG.gDE] = Convert.ToBoolean(dr["Delete"]);
                    flx[RowNumb, (int)UG.gPR] = Convert.ToBoolean(dr["Print"]);
                    flx[RowNumb, (int)UG.gVI] = Convert.ToBoolean(dr["View"]);
                    NodeRange = ((Space / 4)-1).ToString(); //NodeRange = CountNode.ToString() + ",";
                    flx[RowNumb, (int)UG.gLR] = NodeRange;
                    CountNode++;
                    flx[RowNumb, (int)UG.gNM] = dr["Desc"].ToString();
                    flx[RowNumb, (int)UG.gST] = "PR";
                    RowNumb++;
                    C1.Win.C1FlexGrid.CellStyle rs2 = flx.Styles.Add("MainNodeCol");
                    rs2.BackColor = Color.PowderBlue;
                    flx.Rows[RowNumb - 1].Style = flx.Styles["MainNodeCol"];
                    Add_SubNodes(trn, System.Convert.ToInt16(dr["ModuleID"].ToString()), Space, IsModule);
                }
            }
            catch (Exception ex){db.MsgERR(ex);}
        }
        private void Add_SubNodes(TreeNode trn, int modID, int Space, int IsModule)
        {
            string ssql;
            DataTable DT;
            Image img;
            Color SubCol=Color.DeepSkyBlue;
            string NoOfSpaces = new string(' ',Space); 
            try
            {
                ssql = " SELECT ModuleID, [Name], [Desc], ParentID, Img, IsActive, SortOrder,[View],[Add],[Edit],[Delete],[Print] FROM dbo.Fun_ReturnUserGroupPermission(" + ChkSts + ") Where ISNULL(ParentID,0)=" + modID + " and Isnull(IsActive,0)=1 Order by ModuleID ";
                DT = Tourist_Management.Classes.clsGlobal.objCon.Fill_Table(ssql);
                foreach (DataRow dr in DT.Rows)
                {
                    flx.Rows.Add();                    
                    flx[RowNumb, (int)UG.gID] = CountNode;
                    CountNode++;
                    flx[RowNumb, (int)UG.gNM] = NoOfSpaces + dr["Desc"].ToString();
                    flx[RowNumb, (int)UG.gMD] = dr["ModuleID"].ToString();
                    flx[RowNumb, (int)UG.gST] = "PR";
                    flx[RowNumb, (int)UG.gAD] = Convert.ToBoolean(dr["Add"]);
                    flx[RowNumb, (int)UG.gED] = Convert.ToBoolean(dr["Edit"]);
                    flx[RowNumb, (int)UG.gDE] = Convert.ToBoolean(dr["Delete"]);
                    flx[RowNumb, (int)UG.gPR] = Convert.ToBoolean(dr["Print"]);
                    flx[RowNumb, (int)UG.gVI] = Convert.ToBoolean(dr["View"]);
                    RowNumb++;
                    C1.Win.C1FlexGrid.CellStyle rs2 = flx.Styles.Add("SubColStr");
                    rs2.BackColor = Color.PowderBlue;
                    flx.Rows[RowNumb - 1].Style = flx.Styles["SubColStr"];
                    TreeNode TRN = new TreeNode(dr["Desc"].ToString());
                    TRN.Name = dr["Name"].ToString();
                    if ((dr[4].ToString() != null) && (dr[4].ToString() != ""))
                    {
                        img = Tourist_Management.Classes.clsGlobal.byteArrayToImage((byte[])dr["Img"]);
                        imgTree.Images.Add(img);
                        ImgIndex += 1;
                        TRN.ImageIndex = ImgIndex;
                        TRN.SelectedImageIndex = ImgIndex;
                        flx.SetCellImage(RowNumb-1, (int)UG.gIM, img);
                    }
                    trn.Nodes.Add(TRN);
                    int Spaces = Space + 4;
                    NodeRange = ((Spaces / 4)-1).ToString(); //NodeRange + CountNode.ToString() + ",";
                    flx[RowNumb-1, (int)UG.gLR] = NodeRange;
                    Add_SubNodes(TRN, System.Convert.ToInt16(dr["ModuleID"].ToString()), Spaces,0);
                }
                if (DT.Rows.Count <= 0 && IsModule!=1)
                {
                    C1.Win.C1FlexGrid.CellStyle rs2 = flx.Styles.Add("SubColStrLeaf");
                    rs2.BackColor = Color.White;
                    flx.Rows[RowNumb - 1].Style = flx.Styles["SubColStrLeaf"];
                    flx[RowNumb-1, (int)UG.gST] = "LF";
                    flx[RowNumb - 1, (int)UG.gFD] = flx[RowNumb - 1, (int)UG.gMD].ToString();
                    flx[RowNumb - 1, (int)UG.gMD] = null;
                }
            }
            catch (Exception ex){db.MsgERR(ex);}
        }
        private void treeMain_AfterSelect(object sender, TreeViewEventArgs e)
        {
                int CountChild=0,Flag=0;
                string abc = e.Node.Text;
                if((e.Node.LastNode==null))
                {
                    CountChild=0;
                }
                else
                {
                    string a = e.Node.LastNode.Text;
                    for (int i = 0; i < flx.Rows.Count; i++)
                    {
                        if (a == flx[i, (int)UG.gNM].ToString().Trim())
                        {
                            if ((e.Node.LastNode == null))
                            {
                                CountChild = Convert.ToInt16(flx[i, (int)UG.gID].ToString());
                            }
                            else
                            {
                                CountChild = Convert.ToInt16(flx[i, (int)UG.gID].ToString()) + e.Node.LastNode.Nodes.Count;
                                if(e.Node.LastNode.Nodes.Count!=0)
                                    break;
                            }
                        }
                    }
                }
                for (int i = 1; i < flx.Rows.Count; i++)
                {
                    if (abc == flx[i, (int)UG.gNM].ToString().Trim())
                    {
                        int y;
                        if (CountChild == 0)
                            CountChild = i-1;
                        if (Flag == 0)
                        {
                            for (y = Convert.ToInt16(flx[i, (int)UG.gID].ToString()); y <= CountChild; y++)
                            {
                                flx.Rows[y + 1].Visible = true;
                            }
                            i = y;
                            Flag = 1;
                        }
                    }
                    else
                        flx.Rows[i].Visible = false;
                }
        }
        private void Merge_Columns()
        {
            CellRange rng;
            for (int x = 0; x < flx.Rows.Count - 1; x++)
            {
                if (flx[x + 1, (int)UG.gST].ToString() == "PR")
                {
                    flx.Rows[x + 1].AllowMerging = true;
                    rng = flx.GetCellRange(x + 1, (int)UG.gAD, x + 1, (int)UG.gDE);
                    if (flx[x + 1, (int)UG.gVI]==null)
                        rng.Data = 0;
                    else if(Convert.ToBoolean(flx[x + 1, (int)UG.gVI])==false)
                        rng.Data = 0;
                    else
                        rng.Data = 1;
                    flx.Rows[x + 1].TextAlign = TextAlignEnum.LeftCenter;
                    flx.Rows[x + 1].AllowEditing = true;
                }
            }
            flx.Update();
        }
        private void flx_AfterEdit(object sender, RowColEventArgs e)
        {
                int i = 1;
                bool StatusVI = false, StatusAD = false, StatusED = false, StatusDE = false, StatusPR = false;
                int CurrentLevelID = Convert.ToInt16(flx[flx.Row, (int)UG.gLR].ToString());
                int CurrentNodeID = Convert.ToInt16(flx[flx.Row, (int)UG.gID].ToString());
                if (flx.Col < 8)
                    return;
                if (flx.Col == (int)UG.gAD)
                    StatusAD = Convert.ToBoolean(flx[flx.Row, (int)UG.gAD].ToString());
                else if (flx.Col == (int)UG.gED)
                    StatusED = Convert.ToBoolean(flx[flx.Row, (int)UG.gED].ToString());
                else if (flx.Col == (int)UG.gDE)
                    StatusDE = Convert.ToBoolean(flx[flx.Row, (int)UG.gDE].ToString());
                else if (flx.Col == (int)UG.gPR)
                    StatusPR = Convert.ToBoolean(flx[flx.Row, (int)UG.gPR].ToString());
                else if (flx.Col == (int)UG.gVI)
                    StatusVI = Convert.ToBoolean(flx[flx.Row, (int)UG.gVI].ToString());
                i = flx.Row;
                do
                {
                    if ((flx.Rows[i].Visible == false))
                        break;
                    if (Convert.ToInt16(flx[i, (int)UG.gLR]) < CurrentLevelID)
                        break;
                    if (flx.Col == (int)UG.gAD) flx[i, (int)UG.gAD] = StatusAD;
                    else if (flx.Col == (int)UG.gED) flx[i, (int)UG.gED] = StatusED;
                    else if (flx.Col == (int)UG.gED) flx[i, (int)UG.gED] = StatusED;
                    else if (flx.Col == (int)UG.gDE) flx[i, (int)UG.gDE] = StatusDE;
                    else if (flx.Col == (int)UG.gPR) flx[i, (int)UG.gPR] = StatusPR;
                    else if (flx.Col == (int)UG.gVI)
                    {
                        flx[i, (int)UG.gVI] = StatusVI;
                        if (StatusVI==false && flx[i, (int)UG.gST].ToString() == "PR")
                        {
                            int temp_i = i + 1;
                            while (flx[temp_i, (int)UG.gST].ToString() != "PR")
                            {
                                flx[temp_i, (int)UG.gAD] = false;
                                flx[temp_i, (int)UG.gED] = false;
                                flx[temp_i, (int)UG.gDE] = false;
                                flx[temp_i, (int)UG.gPR] = false;
                                flx[temp_i, (int)UG.gVI] = false;
                                temp_i++;
                                if(temp_i==(flx.Rows.Count))
                                    break;
                            }
                        }
                    }
                    i++;
                    if (i >= flx.Rows.Count)
                        break;
                } while (CurrentLevelID != Convert.ToInt16(flx[i, (int)UG.gLR].ToString()));
                i = flx.Row;
                CurrentLevelID = Convert.ToInt16(flx[flx.Row, (int)UG.gLR].ToString());
                int TempLevelID = 0;
                while (i >= 1)
                {
                    if (i > 1)
                    {
                        if ((CurrentLevelID - 1) == Convert.ToInt16(flx[i - 1, (int)UG.gLR].ToString()))//TempLevelID != CurrentLevelID &&
                        {   
                            if ((flx.Col == (int)UG.gAD) || (flx.Col == (int)UG.gED) || (flx.Col == (int)UG.gDE) || (flx.Col == (int)UG.gPR) || (flx.Col == (int)UG.gVI))
                            {
                                flx[i - 1, (int)UG.gVI] = true;
                            }
                            CurrentLevelID--;
                            TempLevelID = CurrentLevelID;
                        }
                    }
                    else
                    {
                        if ((CurrentLevelID - 2) == Convert.ToInt16(flx[i, (int)UG.gLR].ToString()))
                        {
                            if ((flx.Col == (int)UG.gAD) || (flx.Col == (int)UG.gED) || (flx.Col == (int)UG.gDE) || (flx.Col == (int)UG.gPR) || (flx.Col == (int)UG.gVI))
                            {
                                flx[i, (int)UG.gVI] = true;
                            }
                        }
                    }
                    i--;
                }
                Merge_Columns();
        }
        private void Manage_Selected()
        {
            treeMain.Dock = DockStyle.Fill;
            treeMain.Visible = true;
            grpUser.Visible = false;
        }
        private void UserButton()
        {
            treeMain.Visible = false;
            grpUser.Visible = true;
            btnUserDetails.Text = "";
            btnTreeView.Text = "";
        }
        private void TreeViewButton()
        {
            treeMain.Visible = true;
            grpUser.Visible = false;
            btnTreeView.Text = "";
            btnUserDetails.Text = "";
        }
        private void btnTreeView_Click(object sender, EventArgs e)
        {
            TreeViewButton();
        }
        private void btnUserDetails_Click(object sender, EventArgs e)
        {
            UserButton();
        }
        private void lblUserDetails_Click(object sender, EventArgs e)
        {
            UserButton();
        }
        private void lblTreeView_Click(object sender, EventArgs e)
        {
            TreeViewButton();
        }
        private void btnOk_Click(object sender, EventArgs e)
        {
            if (Save_Pro() == true){this.Close();}
            else
            {
                return;
            }
        }
        private void btnCancel_Click(object sender, EventArgs e){this.Close();}
    }
}
