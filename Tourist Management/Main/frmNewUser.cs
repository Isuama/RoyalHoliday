using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlClient;
using C1.Win.C1FlexGrid;
using System.IO;
using CRPT;
namespace Tourist_Management.Main
{
    public partial class frmNewUser : Form
    { 
        private const string msghd = "New User";
        public int Mode = 0; //TO GET TO KNOW WEATHER INSERTION OR UPDATION
        public int SystemCode = 0; // TO KEEP THE SYSTEM GENERATED CODE
        public string SqlQry = "SELECT ID,UserName,EmpID,AccCatList,IsCanChange,IsMustChange,NoOfDays,IsActive FROM mst_UserMaster Where Isnull([Status],0)<>7 AND DataBaseID=" + Convert.ToInt16(Tourist_Management.Classes.clsGlobal.Rtn_DatabaseID()) + " Order By UserName";
        int ChkSts;
        enum UG { gMID, gMD, gFID, gFD, gLR, gID, gIM, gNM, gAD, gED, gVI, gPR, gDE, gST };
        private int ImgIndex = -1;
        private int RowNumb = 1;
        float CountNode = 0;
        string NodeRange,ModeType;
        byte[] imageData = null;  //TO KEEP COMPANY LOGO IMAGE AS A BINARY DATA
        public frmNewUser(){InitializeComponent();}
        private void Intializer()
        {
            try
            {
                if (Convert.ToInt32(Classes.clsGlobal.UserID.ToString()) == 1001)
                    chkUnmask.Visible = true;
                else
                    chkUnmask.Visible = false;
                if (Mode == 0)
                {
                    txtName.Text = "";
                    txtDesc.Text = "";
                    txtPw.Text = "";
                    txtPwHint.Text = "";
                    txtConfmPw.Text = "";
                    fill_control();
                    chkActive.Checked = true;
                }
                else if (Mode == 1)
                {
                    fill_control();
                    Fill_Details();
                    ModeType = "Edit";
                    Grd_Initializer();
                    ChkSts = SystemCode;
                    Make_tree();
                    Merge_Columns();
                    treeMain.Nodes[0].Expand();
                    Manage_Selected();
                }
            }
            catch (Exception ex){db.MsgERR(ex);} 
        }
        private void Fill_Details()
        {
            DataTable DT;
            CRPT.CRPT Crpt;
            DataRow rw;
            try
            {
                Crpt = new CRPT.CRPT();
                string sql;
                sql = "SELECT ID,UserName,Password,Hint,[Desc],EmpID,AccCatList,"+
                      "IsNull(IsManager,0)AS IsManager,IsNull(IsDirector,0)AS IsDirector,"+
                      "IsCanChange,IsMustChange,IsActive,NoOfDays,UserGroupID"+
                      " FROM mst_UserMaster  Where ID=" + SystemCode + "";
                DT = Tourist_Management.Classes.clsGlobal.objComCon.Fill_Table(sql);
                rw = DT.Rows[0];
                txtName.Text   = rw["UserName"].ToString();
                txtPw.Text = Crpt.DECRYPT(rw["Password"].ToString(), Tourist_Management.Classes.clsGlobal.RevertME());
                if(rw["Hint"].ToString()!="")
                    txtPwHint.Text = rw["Hint"].ToString();
                if (rw["Desc"].ToString() != "")
                txtDesc.Text   = rw["Desc"].ToString();
                txtConfmPw.Text = Crpt.DECRYPT(rw["Password"].ToString(), Tourist_Management.Classes.clsGlobal.RevertME());
                if (rw["EmpID"].ToString() != "")
                    drpEmp.setSelectedValue(rw["EmpID"].ToString());
                chkIsManager.Checked = System.Convert.ToBoolean(rw["IsManager"].ToString());
                chkIsDirector.Checked = System.Convert.ToBoolean(rw["IsDirector"].ToString());
                chkCanChange.Checked = System.Convert.ToBoolean(rw["IsCanChange"].ToString());
                chkMustChng.Checked   = System.Convert.ToBoolean(rw["IsMustChange"].ToString());
                chkActive.Checked     = System.Convert.ToBoolean(rw["IsActive"].ToString());
                ddlDays.Value = Convert.ToInt32(rw["NoOfDays"].ToString());
                drpUserGroupID.setSelectedValue(rw["UserGroupID"].ToString());
                Load_Emp_Photo();
                DataTable DT1;
                DT1 = Tourist_Management.Classes.clsGlobal.objCon.Fill_Table("SELECT [ID],[GroupName],[UserMode] FROM mst_User_Groups Where ID =" + drpUserGroupID.SelectedValue.ToString() + "  ");
                if (DT1.Rows[0]["UserMode"].ToString() == "1")
                    chkIsAdmin.Checked = true;
                else
                    chkIsAdmin.Checked = false;
            }
            catch (Exception ex){db.MsgERR(ex);}
        }
        private Boolean Validate_Data()
        {
            try
            {
                DataTable DT;
                DT = Tourist_Management.Classes.clsGlobal.objComCon.Fill_Table("SELECT ID,UserName,Password,Status FROM mst_UserMaster where Isnull([Status],0)<>7 and ID =" + SystemCode + "");
                if (DT.Rows.Count != 0)
                {
                    if (MessageBox.Show("This Record Is Being Used...Do you Want To Update?", msghd, MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
                    {
                        return false;
                    }
                }
                if (txtName.Text.Trim() == "")
                {
                    MessageBox.Show("Name cannot be blank", msghd, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return false;
                }
                if (txtPw.Text.Trim() == "")
                {
                    MessageBox.Show("Password cannot be blank", msghd, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return false;
                }
                if (txtConfmPw.Text.Trim() == "")
                {
                    MessageBox.Show("Password Confermation cannot be blank", msghd, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return false;
                }
                if (txtPw.Text != txtConfmPw.Text)
                {
                    MessageBox.Show(" confirm password is incorrect", msghd, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    txtConfmPw.Text = "";
                    return false;
                }
                if (drpUserGroupID.SelectedValue.Trim() == "")//drpUserGroupID.SelectedList == null ||
                {
                    MessageBox.Show("User Group Name Cannot Be Null..!", msghd, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;
                }
                if (Tourist_Management.Classes.clsGlobal.objComCon.Fill_Table("Select UserName From mst_UserMaster Where UserName='" + txtName.Text.Trim() + "' and ID <> " + SystemCode + "").Rows.Count > 0)
                {
                    MessageBox.Show("Name already exists", msghd, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return false;
                }
                return true;
            }
            catch (Exception ex)
            {
                db.MsgERR(ex);
                return false;
            }
        }
        private Boolean Save_Pro()
        {
            Boolean rtnVal = false;
            try
            {
                if (Validate_Data() == false)
                {
                    rtnVal = false;
                    return rtnVal;
                }
                if (Save_Procedure() == true)//Save_Procedure()
                {
                    rtnVal = true;
                    return rtnVal;
                }
                return rtnVal;
            }
            catch (Exception ex)
            {
                db.MsgERR(ex);
                return false;
            }
        }
        private Boolean Save_Procedure()
        { 
            System.Data.SqlClient.SqlCommand objCom;
            System.Data.SqlClient.SqlTransaction objTrn;
            System.Data.SqlClient.SqlConnection objCon;
            try
            {
                objCom = new System.Data.SqlClient.SqlCommand();
                objCon = Tourist_Management.Classes.clsGlobal.objComCon.ReturnConnection;
                objCon.Open();
                objTrn = objCon.BeginTransaction();
                objCom.Connection = objCon;
                objCom.Transaction = objTrn;
                if ((Save_UserMaster(objCom) == true) && (Save_User_Permission() == true)) 
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
            catch (Exception ex)
            {
                db.MsgERR(ex);
                return false;
            }
        }
        private Boolean Save_UserMaster(SqlCommand sqlCom)
        {
            Boolean RtnVal = false;
            CRPT.CRPT Crpt;
            try
            {
                Crpt = new CRPT.CRPT();
                sqlCom.CommandType = CommandType.StoredProcedure;
                sqlCom.CommandText = "sp_mst_UserMaster";
                sqlCom.Parameters.Add("@ID", SqlDbType.Int).Value = SystemCode;
                sqlCom.Parameters["@ID"].Direction = ParameterDirection.InputOutput;
                sqlCom.Parameters.Add("@UserName", SqlDbType.VarChar, 50).Value = txtName.Text.Trim();
                sqlCom.Parameters.Add("@Password", SqlDbType.VarChar, 50).Value = Crpt.ENCRYPT(txtPw.Text.Trim(), Tourist_Management.Classes.clsGlobal.RevertME());
                sqlCom.Parameters.Add("@Hint", SqlDbType.VarChar, 100).Value = txtPwHint.Text.Trim();
                sqlCom.Parameters.Add("@Desc", SqlDbType.VarChar, 50).Value = txtDesc.Text.Trim();
                if(drpEmp.SelectedValue.ToString()!="" && drpEmp.SelectedValue !=null)
                    sqlCom.Parameters.Add("@EmpID", SqlDbType.Int).Value = drpEmp.SelectedValue.ToString();
                sqlCom.Parameters.Add("@IsCanChange", SqlDbType.Int).Value = chkCanChange.Checked == true ? "1" : "0";
                sqlCom.Parameters.Add("@IsMustChange", SqlDbType.Int).Value = chkMustChng.Checked == true ? "1" : "0";
                sqlCom.Parameters.Add("@NoOfDays", SqlDbType.Int).Value = ddlDays.Value;
                sqlCom.Parameters.Add("@UserGroupID", SqlDbType.Int).Value = drpUserGroupID.SelectedValue.ToString();
                sqlCom.Parameters.Add("@DataBaseID", SqlDbType.Int).Value = Convert.ToInt16(Tourist_Management.Classes.clsGlobal.Rtn_DatabaseID());
                sqlCom.Parameters.Add("@IsManager", SqlDbType.Int).Value = chkIsManager.Checked == true ? "1" : "0";
                if(chkIsManager.Checked)
                    sqlCom.Parameters.Add("@ManagerID", SqlDbType.Int).Value = drpMarketingDep.SelectedValue.ToString();
                sqlCom.Parameters.Add("@IsDirector", SqlDbType.Int).Value = chkIsDirector.Checked == true ? "1" : "0";
                sqlCom.Parameters.Add("@IsActive", SqlDbType.Int).Value = chkActive.Checked == true ? "1" : "0";
                sqlCom.Parameters.Add("@CreatedBy", SqlDbType.Int).Value = Convert.ToInt32(Classes.clsGlobal.UserID.ToString());
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
            catch (Exception ex)
            {
                db.MsgERR(ex);
                return false;
            }
        }
        private Boolean Save_User_Permission()
        {
                System.Data.SqlClient.SqlCommand sqlCom;
                System.Data.SqlClient.SqlTransaction objTrn;
                System.Data.SqlClient.SqlConnection objCon;
                sqlCom = new System.Data.SqlClient.SqlCommand();
                objCon = Tourist_Management.Classes.clsGlobal.objCon.ReturnConnection;
                objCon.Open();
                objTrn = objCon.BeginTransaction();
                sqlCom.Connection = objCon;
                sqlCom.Transaction = objTrn;
            int RowNumb;
            Boolean RtnVal = false;
            try
            {
                sqlCom.CommandType = CommandType.StoredProcedure;
                sqlCom.CommandText = "spSave_User_Permissions";
                RowNumb = 1;
                while (RowNumb < flx.Rows.Count)
                {
                    sqlCom.Parameters.Clear();
                    sqlCom.Parameters.Add("@ID", SqlDbType.Int).Value = SystemCode;
                    sqlCom.Parameters.Add("@Status", SqlDbType.Int).Value = (flx[RowNumb, (int)UG.gST].ToString()) == "PR" ? "0" : "1";
                    if (flx[RowNumb, (int)UG.gST].ToString() == "LF" && CheckSelected("LF", RowNumb) == true)
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
                if (RtnVal == true)
                    objTrn.Commit();
                else
                    objTrn.Rollback();
                objCon.Close();
                return RtnVal;
            }
            catch (Exception ex)
            {
                db.MsgERR(ex);
                return false;
            }
        }
        private void frmNewUser_Load(object sender, EventArgs e)
        {
            Intializer();
        }
        private void btnOk_Click(object sender, EventArgs e)
        {
            if (Save_Pro() == true)
            {
                MessageBox.Show("Record sucessfully Updated", msghd, MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.Close();
            }
            else
            {
                MessageBox.Show("Error occured", msghd, MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
        }
        private void btnCancel_Click(object sender, EventArgs e){this.Close();}
        public void loadEmp()
        {
            DataTable DT;
            try
            {
                DT = Tourist_Management.Classes.clsGlobal.objCon.Fill_Table("SELECT [ID],[FirstName] FROM mst_EmployeePersonal Where Isnull([Status],0)<>7");
                drpEmp.DataSource = DT;
                drpEmp.SelectedValue = DT.Rows[0][0].ToString();
            }
            catch (Exception ex){db.MsgERR(ex);}
        }
        public void Fill_User_drp()
        {
            DataTable DT;
            try
            {
                DT = Tourist_Management.Classes.clsGlobal.objCon.Fill_Table("SELECT [ID],[GroupName] FROM mst_User_Groups Where Isnull([Status],0)<>7");
                drpUserGroupID.DataSource = DT;
            }
            catch (Exception ex){db.MsgERR(ex);}
        }
        private void fill_control()
        {
            try
            {
                loadEmp();
                Fill_User_drp();
                DataTable DTB = Tourist_Management.Classes.clsGlobal.objCon.Fill_Table("SELECT ID,Name FROM mst_MarketingDep Where IsNull(IsActive,0)=1 ORDER BY Name");
                drpMarketingDep.DataSource = DTB;
            }
            catch (Exception ex){db.MsgERR(ex);}
        }
        private void drpUserGroupID_Click_Open(object sender, EventArgs e)
        {
            Form frm;
            frm = Classes.clsForms.rtnForm("frmUserGroups", 0, 0);
            frm.StartPosition = FormStartPosition.CenterScreen;
            frm.ShowDialog();
            return;
        }
        private void drpUserGroupID_Selected_TextChanged(object sender, EventArgs e)
        {
            if ((drpUserGroupID.SelectedValue.ToString().Trim() == "") || (drpUserGroupID.SelectedValue == null))
            {
                MessageBox.Show("You have to Select a User Group...!", msghd, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                ModeType = "";
                RowNumb = 1;
                Grd_Initializer();
                ChkSts = Convert.ToInt16(drpUserGroupID.SelectedValue.ToString());
                treeMain.Nodes.Clear();
                Make_tree();
                Merge_Columns();
                treeMain.Nodes[0].Expand();
                Manage_Selected();
                fillisAdmin();
            }
        }
        public void fillisAdmin()
        {
            DataTable DT;
            DT = Classes.clsGlobal.objCon.Fill_Table("SELECT [ID],[GroupName],[UserMode] FROM mst_User_Groups Where ID =" + ChkSts + "  ");
            if (DT.Rows[0]["UserMode"].ToString() == "1")
                chkIsAdmin.Checked = true;
            else
                chkIsAdmin.Checked = false;
        }
        private void Grd_Initializer()
        {
            try
            {
                flx.Cols.Count = 14;
                flx.Rows.Count = 1;
                flx.Cols[(int)UG.gMID].Width = 0;
                flx.Cols[(int)UG.gMD].Width = 0;
                flx.Cols[(int)UG.gFID].Width = 0;
                flx.Cols[(int)UG.gFD].Width = 0;
                flx.Cols[(int)UG.gLR].Width = 0;
                flx.Cols[(int)UG.gID].Width = 0;
                flx.Cols[(int)UG.gIM].Width = 20;
                flx.Cols[(int)UG.gNM].Width = 168;
                flx.Cols[(int)UG.gAD].Width = 60;
                flx.Cols[(int)UG.gED].Width = 60;
                flx.Cols[(int)UG.gDE].Width = 60;
                flx.Cols[(int)UG.gPR].Width = 60;
                flx.Cols[(int)UG.gVI].Width = 60;
                flx.Cols[(int)UG.gST].Width = 0;
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
                flx.AllowMerging = AllowMergingEnum.RestrictCols;
                flx.Cols[(int)UG.gAD].AllowMerging = true;
                flx.Cols[(int)UG.gED].AllowMerging = true;
                flx.Cols[(int)UG.gDE].AllowMerging = true;
                flx.Cols[(int)UG.gPR].AllowMerging = true;
                flx.Cols[(int)UG.gVI].AllowMerging = true;
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
                Color SubColour = Color.PowderBlue;
                int IsModule = 1;
                if (ModeType == "Edit")
                    ssql = " SELECT ModuleID, [Name], [Desc], ParentID, Img, IsActive, SortOrder,[View],[Add],[Edit],[Delete],[Print] FROM dbo.[Fun_ReturnUserModule](" + ChkSts + ",0,0) Where Isnull(IsActive,0)=1 and ISNULL(ParentID,0)=0  and ISNULL(IsCritical,0)=0  Order by ModuleID ";                 
                else
                   ssql = " SELECT ModuleID, [Name], [Desc], ParentID, Img, IsActive,SortOrder,[View],[Add],[Edit],[Delete],[Print] FROM dbo.Fun_ReturnUserGroupPermission(" + ChkSts + ") Where Isnull(IsActive,0)=1 and ISNULL(ParentID,0)=0 Order by ModuleID ";
                dt = Classes.clsGlobal.objCon.Fill_Table(ssql);
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
                    flx[RowNumb, (int)UG.gAD] = Convert.ToBoolean(dr["Add"]);
                    flx[RowNumb, (int)UG.gED] = Convert.ToBoolean(dr["Edit"]);
                    flx[RowNumb, (int)UG.gDE] = Convert.ToBoolean(dr["Delete"]);
                    flx[RowNumb, (int)UG.gPR] = Convert.ToBoolean(dr["Print"]);
                    flx[RowNumb, (int)UG.gVI] = Convert.ToBoolean(dr["View"]);
                    NodeRange = ((Space / 4) - 1).ToString(); //NodeRange = CountNode.ToString() + ",";
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
                if (dt.Rows.Count < 0)
                {
                }
            }
            catch (Exception ex){db.MsgERR(ex);}
        }
        private void Add_SubNodes(TreeNode trn, int modID, int Space, int IsModule)
        {
            string ssql;
            DataTable DT;
            Image img;
            Color SubCol = Color.DeepSkyBlue;
            string NoOfSpaces = new string(' ', Space); 
            try
            {
                if(ModeType=="Edit")
                    ssql = " SELECT ModuleID, [Name], [Desc], ParentID, Img, IsActive, SortOrder,[View],[Add],[Edit],[Delete],[Print] FROM dbo.Fun_ReturnUserModule(" + ChkSts + ",0,0) Where ISNULL(ParentID,0)=" + modID + " and Isnull(IsActive,0)=1 Order by ModuleID ";
                else
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
                        flx.SetCellImage(RowNumb - 1, (int)UG.gIM, img);
                    }
                    trn.Nodes.Add(TRN);
                    int Spaces = Space + 4;
                    NodeRange = ((Spaces / 4) - 1).ToString(); //NodeRange + CountNode.ToString() + ",";
                    flx[RowNumb - 1, (int)UG.gLR] = NodeRange;
                    Add_SubNodes(TRN, System.Convert.ToInt16(dr["ModuleID"].ToString()), Spaces, 0);
                }
                if (DT.Rows.Count <= 0 && IsModule != 1)
                {
                    C1.Win.C1FlexGrid.CellStyle rs2 = flx.Styles.Add("SubColStrLeaf");
                    rs2.BackColor = Color.White;
                    flx.Rows[RowNumb - 1].Style = flx.Styles["SubColStrLeaf"];
                    flx[RowNumb - 1, (int)UG.gST] = "LF";
                    flx[RowNumb - 1, (int)UG.gFD] = flx[RowNumb - 1, (int)UG.gMD].ToString();
                    flx[RowNumb - 1, (int)UG.gMD] = null;
                }
            }
            catch (Exception ex){db.MsgERR(ex);}
        }
        private void treeMain_AfterSelect(object sender, TreeViewEventArgs e)
        {
                int CountChild = 0, Flag = 0;
                string abc = e.Node.Text;
                if ((e.Node.LastNode == null))
                {
                    CountChild = 0;
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
                                if (e.Node.LastNode.Nodes.Count != 0)
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
                            CountChild = i - 1;
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
                    if (flx[x + 1, (int)UG.gVI] == null)
                        rng.Data = 0;
                    else if (Convert.ToBoolean(flx[x + 1, (int)UG.gVI]) == false)
                        rng.Data = 0;
                    else
                        rng.Data = 1;
                    flx.Rows[x + 1].TextAlign = TextAlignEnum.LeftCenter;
                    flx.Rows[x + 1].AllowEditing = true;
                }
            }
            flx.Update();
        }
        private void flx_AfterEdit(object sender, C1.Win.C1FlexGrid.RowColEventArgs e)
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
                        if (StatusVI == false && flx[i, (int)UG.gST].ToString() == "PR")
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
                                if (temp_i == (flx.Rows.Count))
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
        }
        private Boolean CheckSelected(string Status, int sRow)
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
        private void Load_Emp_Photo()
        {
            DataTable DT;
            try
            {
                DT = Tourist_Management.Classes.clsGlobal.objCon.Fill_Table("SELECT EmpPhoto FROM mst_EmployeePersonal  Where ID=" + drpEmp.SelectedValue.ToString() + "");
                if (DT.Rows[0]["EmpPhoto"].ToString()!="")
                {
                    byte[] EmpPhoto = (byte[])DT.Rows[0]["EmpPhoto"];
                    imageData = EmpPhoto;
                    MemoryStream ms = new MemoryStream(EmpPhoto);
                    pbEmpPhoto.Image = Image.FromStream(ms, false, false);
                    lblEmpPhoto.Visible = false;
                }
                else
                    lblEmpPhoto.Visible = true;
            }
            catch (Exception ex){db.MsgERR(ex);}
        }
        private void drpEmp_Selected_TextChanged(object sender, EventArgs e)
        {
            if ((drpEmp.SelectedValue.ToString().Trim() == "")||(drpEmp.SelectedValue==null))
            {
                MessageBox.Show("You have to Select an Employee...!", msghd, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
              Load_Emp_Photo();
            }
        }
        private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (tabControl1.SelectedIndex == 0)
            {
                this.ClientSize = new System.Drawing.Size(548, 432);
                this.chkActive.Location = new System.Drawing.Point(285, 405);
                this.btnCancel.Location = new System.Drawing.Point(447, 402);
                this.btnOk.Location = new System.Drawing.Point(366, 402);
                this.tabControl1.Size = new System.Drawing.Size(532, 385);
            }
            else 
            {
                this.ClientSize = new System.Drawing.Size(802, 432); //form
                this.chkActive.Location = new System.Drawing.Point(535, 405); //chk active 
                this.btnCancel.Location = new System.Drawing.Point(717, 402);//btnCancel
                this.btnOk.Location = new System.Drawing.Point(635, 402);//ok
                this.tabControl1.Size = new System.Drawing.Size(787, 385);//tabcontrl
            }
        }
        private void button1_Click(object sender, EventArgs e)
        {
            this.ClientSize = new System.Drawing.Size(200, 200);
        }
        private void button2_Click(object sender, EventArgs e)
        {
            this.ClientSize = new System.Drawing.Size(804, 465);
        }
        private void chkUnmask_CheckedChanged(object sender, EventArgs e)
        {
            if (chkUnmask.Checked)
            {
                txtPw.PasswordChar ='\0';
                txtConfmPw.PasswordChar = '\0';
            }
            else
            {
                txtPw.PasswordChar = '*';
                txtConfmPw.PasswordChar = '*';
            }
        }
        private void chkIsManager_CheckedChanged(object sender, EventArgs e)
        {
            if (chkIsManager.Checked)
                drpMarketingDep.Enabled = true;
            else
                drpMarketingDep.Enabled = false;
        }
    }
}
