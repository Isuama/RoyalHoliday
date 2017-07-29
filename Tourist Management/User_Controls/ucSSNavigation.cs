using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
namespace Tourist_Management.User_Controls
{
    public partial class ucSSNavigation : UserControl
    {
        int CatID, NOA, NOC;
        private const string msghd = "Sightseeing Expenses";
        int IsSaarc = 0;
        double TCostA=0.00,TCostC=0.00;
        int InsMode = 0;
        enum SE { gSSI, gSCI, gSCN, gSPC, gSPN, gNOA, gNOC, gSAC, gSCC, gNAC, gNCC, gTOT, gSEL };
        public int Mode
        {
            get
            {
                return InsMode;
            }
            set
            {
                InsMode = value;
            }
        }
        public decimal SysCode
        {
            get
            {
                return SysCode;
            }
            set
            {
                SysCode = value;
            }
        }
        public int SightCatID
        {
            get
            {
                return CatID;
            }
            set
            {
                CatID = value;
            }
        }
        public int Adult
        {
            get
            {
                return NOA;
            }
            set
            {
                NOA = value;
            }
        }
        public int Child
        {
            get
            {
                return NOC;
            }
            set
            {
                NOC = value;
            }
        }
        public ucSSNavigation(){InitializeComponent();}
        public int Saarc
        {
            get
            {
                return IsSaarc;
            }
            set
            {
                IsSaarc = value;
            }
        }
        private void btnSEGenerate_Click(object sender, EventArgs e)
        {
            if (Validate_Sightseeing_Expenses() == false)
                return;
            Generate_Sightseeing_Expenses();
        }
        private Boolean Validate_Sightseeing_Expenses()
        {
                int RowNumb = 1;
                while (grdSE[RowNumb, grdSE.Cols[(int)SE.gSSI].Index] != null)
                {
                    if (grdSE[RowNumb, grdSE.Cols[(int)SE.gNOA].Index] != null)
                    {
                        if (Tourist_Management.Classes.clsGlobal.IsNumeric(grdSE[RowNumb, (int)SE.gNOA].ToString()) == false)
                        {
                            MessageBox.Show("Please Enter Valid Values For No Of Adult", msghd, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                            return false;
                        }
                    }
                    else
                        grdSE[RowNumb, (int)SE.gNOA] = 0;
                    if (grdSE[RowNumb, grdSE.Cols[(int)SE.gNOC].Index] != null)
                    {
                        if (Tourist_Management.Classes.clsGlobal.IsNumeric(grdSE[RowNumb, (int)SE.gNOC].ToString()) == false)
                        {
                            MessageBox.Show("Please Enter Valid Values For No Of Child", msghd, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                            return false;
                        }
                    }
                    else
                        grdSE[RowNumb, (int)SE.gNOC] = 0;
                    if (IsSaarc == 1)
                    {
                        if (grdSE[RowNumb, grdSE.Cols[(int)SE.gSAC].Index] != null)
                        {
                            if (Tourist_Management.Classes.clsGlobal.IsNumeric(grdSE[RowNumb, (int)SE.gSAC].ToString()) == false)
                            {
                                MessageBox.Show("Please Enter Valid Values For Saarc Adult Ticket Cost.", msghd, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                                return false;
                            }
                        }
                        else
                            grdSE[RowNumb, (int)SE.gSAC] = 0;
                        if (grdSE[RowNumb, grdSE.Cols[(int)SE.gSCC].Index] != null)
                        {
                            if (Tourist_Management.Classes.clsGlobal.IsNumeric(grdSE[RowNumb, (int)SE.gSCC].ToString()) == false)
                            {
                                MessageBox.Show("Please Enter Valid Values For Saarc Child Ticket Cost.", msghd, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                                return false;
                            }
                        }
                        else
                            grdSE[RowNumb, (int)SE.gSCC] = 0;
                    }
                    else
                    {
                        if (grdSE[RowNumb, grdSE.Cols[(int)SE.gNAC].Index] != null)
                        {
                            if (Tourist_Management.Classes.clsGlobal.IsNumeric(grdSE[RowNumb, (int)SE.gNAC].ToString()) == false)
                            {
                                MessageBox.Show("Please Enter Valid Values For Non Saarc Adult Ticket Cost.", msghd, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                                return false;
                            }
                        }
                        else
                            grdSE[RowNumb, (int)SE.gNAC] = 0;
                        if (grdSE[RowNumb, grdSE.Cols[(int)SE.gNCC].Index] != null)
                        {
                            if (Tourist_Management.Classes.clsGlobal.IsNumeric(grdSE[RowNumb, (int)SE.gNCC].ToString()) == false)
                            {
                                MessageBox.Show("Please Enter Valid Values For Non Saarc Child Ticket Cost.", msghd, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                                return false;
                            }
                        }
                        else
                            grdSE[RowNumb, (int)SE.gNCC] = 0;
                    }
                    RowNumb++;
                }
                return true;
        }
        private void Generate_Sightseeing_Expenses()
        {
            try{
                int RowNumb = 1, Count = 1;
                int AdNo, ChNo;
                double AdCost, ChCost,TotAdCost=0.00,TotChCost=0.00;
                double TotCost, GrandTot=0.00;
                if (grdSE[RowNumb, grdSE.Cols[(int)SE.gSSI].Index] == null)
                {
                    MessageBox.Show("No Records Found To Be Processed", msghd, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return;
                }
                int MaxVal = 0;
                while (grdSE[RowNumb, grdSE.Cols[(int)SE.gSSI].Index] != null)
                {
                    if (Convert.ToBoolean(grdSE[RowNumb, (int)SE.gSEL]) == true)
                    {
                        MaxVal++;
                    }
                    RowNumb++;
                }
                pbSE.Maximum = MaxVal;
                RowNumb = 1;
                while (grdSE[RowNumb, grdSE.Cols[(int)SE.gSSI].Index] != null)
                {
                    if (Convert.ToBoolean(grdSE[RowNumb, (int)SE.gSEL]) == false)
                    {
                        grdSE[RowNumb, (int)SE.gTOT] = 0.00;
                        RowNumb++;
                        continue;
                    }
                    AdNo = Convert.ToInt32(grdSE[RowNumb, (int)SE.gNOA].ToString());
                    ChNo = Convert.ToInt32(grdSE[RowNumb, (int)SE.gNOC].ToString());
                    if (IsSaarc == 1)
                    {
                        AdCost = Convert.ToDouble(grdSE[RowNumb, (int)SE.gSAC].ToString());
                        ChCost = Convert.ToDouble(grdSE[RowNumb, (int)SE.gSCC].ToString());
                    }
                    else
                    {
                        AdCost = Convert.ToDouble(grdSE[RowNumb, (int)SE.gNAC].ToString());
                        ChCost = Convert.ToDouble(grdSE[RowNumb, (int)SE.gNCC].ToString());
                    }
                    TotCost = (AdNo * AdCost) + (ChNo * ChCost);
                    grdSE[RowNumb, (int)SE.gTOT] = TotCost.ToString();
                    TotAdCost += AdCost;
                    TotChCost += ChCost;
                    GrandTot += TotCost;
                    pbSE.Value = Count;
                    Count++;
                    RowNumb++;
                }
                grdSE[grdSE.Rows.Count-1, (int)SE.gSCN] = "TOTAL COST";
                grdSE[grdSE.Rows.Count - 1, (int)SE.gTOT] = GrandTot.ToString();
                grdSE[grdSE.Rows.Count - 1, (int)SE.gSAC] = TotAdCost.ToString();
                grdSE[grdSE.Rows.Count - 1, (int)SE.gSCC] = TotChCost.ToString();
                grdSE[grdSE.Rows.Count - 1, (int)SE.gNAC] = TotAdCost.ToString();
                grdSE[grdSE.Rows.Count - 1, (int)SE.gNCC] = TotChCost.ToString();
                C1.Win.C1FlexGrid.CellStyle rs2 = grdSE.Styles.Add("TotalColor");
                rs2.BackColor = Color.PowderBlue;
                grdSE.Rows[grdSE.Rows.Count - 1].Style = grdSE.Styles["TotalColor"];
            }
            catch (Exception ex){db.MsgERR(ex);}
        }
        private void Intializer()
        {
            try
            {
                Grd_Initializer();
                Fill_Grid();
            }
            catch (Exception ex){db.MsgERR(ex);}
        }
        private void Grd_Initializer()
        {
            try
            {
                grdSE.Cols.Count = 13;
                grdSE.Rows.Count = 20;
                grdSE.Cols[(int)SE.gSSI].Width = 0;
                grdSE.Cols[(int)SE.gSCI].Width = 0;
                grdSE.Cols[(int)SE.gSCN].Width = 0;
                grdSE.Cols[(int)SE.gSPC].Width = 0;
                grdSE.Cols[(int)SE.gSPN].Width = 233;
                grdSE.Cols[(int)SE.gNOA].Width = 80;
                grdSE.Cols[(int)SE.gNOC].Width = 80;
                if (IsSaarc == 1)
                {
                    grdSE.Cols[(int)SE.gSAC].Width = 120;
                    grdSE.Cols[(int)SE.gSCC].Width = 120;
                    grdSE.Cols[(int)SE.gNAC].Width = 0;
                    grdSE.Cols[(int)SE.gNCC].Width = 0;
                }
                else
                {
                    grdSE.Cols[(int)SE.gSAC].Width = 0;
                    grdSE.Cols[(int)SE.gSCC].Width = 0;
                    grdSE.Cols[(int)SE.gNAC].Width = 120;
                    grdSE.Cols[(int)SE.gNCC].Width = 120;
                }
                grdSE.Cols[(int)SE.gTOT].Width = 101;
                grdSE.Cols[(int)SE.gSEL].Width = 70;
                grdSE.Cols[(int)SE.gSSI].Caption = "Sightseeing ID";
                grdSE.Cols[(int)SE.gSCI].Caption = "Category ID";
                grdSE.Cols[(int)SE.gSCN].Caption = "Category Name";
                grdSE.Cols[(int)SE.gSPC].Caption = "Place Code";
                grdSE.Cols[(int)SE.gSPN].Caption = "Place Name";
                grdSE.Cols[(int)SE.gNOA].Caption = "No Of Adult";
                grdSE.Cols[(int)SE.gNOC].Caption = "No Of Child";
                grdSE.Cols[(int)SE.gSAC].Caption = "SAARC Adult Cost";
                grdSE.Cols[(int)SE.gSCC].Caption = "SAARC Child Cost";
                grdSE.Cols[(int)SE.gNAC].Caption = "Normal Adult Cost";
                grdSE.Cols[(int)SE.gNCC].Caption = "Normal Child Cost";
                grdSE.Cols[(int)SE.gTOT].Caption = "Total Cost";
                grdSE.Cols[(int)SE.gSEL].Caption = "Choose";
                grdSE.Cols[(int)SE.gSEL].DataType = Type.GetType(" System.Boolean");
                grdSE.Cols[(int)SE.gNOA].Format = "##";
                grdSE.Cols[(int)SE.gNOC].Format = "##";
                grdSE.Cols[(int)SE.gSAC].Format = "##.##";
                grdSE.Cols[(int)SE.gSCC].Format = "##.##";
                grdSE.Cols[(int)SE.gNAC].Format = "##.##";
                grdSE.Cols[(int)SE.gNCC].Format = "##.##";
                grdSE.Cols[(int)SE.gTOT].Format = "##.##";
            }
            catch (Exception ex){db.MsgERR(ex);}
        }
        private void ucSSNavigation_Load(object sender, EventArgs e)
        {
            Intializer();
        }
        private void Fill_Grid()
        {
            try
            {
                DataTable DT;
                string ssql;
                    ssql = "SELECT ID,CatCode,CategoryName,Code,Name,SAdult,SChild,NAdult,NChild " +
                           "FROM vw_SightSeeing " +
                           "WHERE CatCode=" + CatID + " AND Isnull([Status],0)<>7 AND IsNull(IsActive,0)=1";
                DT = Tourist_Management.Classes.clsGlobal.objComCon.Fill_Table(ssql);
                if (DT.Rows.Count > 0)
                {
                    grdSE.Rows.Count = DT.Rows.Count + 5; ;
                    int RowNumb = 0;
                    while (DT.Rows.Count > RowNumb)
                    {
                        if (DT.Rows[RowNumb]["ID"].ToString() != "")
                            grdSE[RowNumb + 1, (int)SE.gSSI] = DT.Rows[RowNumb]["ID"].ToString();
                        if (DT.Rows[RowNumb]["CatCode"].ToString() != "")
                            grdSE[RowNumb + 1, (int)SE.gSCI] = DT.Rows[RowNumb]["CatCode"].ToString();
                        if (DT.Rows[RowNumb]["CategoryName"].ToString() != "")
                            grdSE[RowNumb + 1, (int)SE.gSCN] = DT.Rows[RowNumb]["CategoryName"].ToString();
                        if (DT.Rows[RowNumb]["Code"].ToString() != "")
                            grdSE[RowNumb + 1, (int)SE.gSPC] = DT.Rows[RowNumb]["Code"].ToString();
                        if (DT.Rows[RowNumb]["Name"].ToString() != "")
                            grdSE[RowNumb + 1, (int)SE.gSPN] = DT.Rows[RowNumb]["Name"].ToString();
                        grdSE[RowNumb + 1, (int)SE.gNOA] = NOA;
                        grdSE[RowNumb + 1, (int)SE.gNOC] = NOC;
                        if (grdSE[RowNumb + 1, (int)SE.gSAC] == null)
                            grdSE[RowNumb + 1, (int)SE.gSAC] = DT.Rows[RowNumb]["SAdult"].ToString();
                        if (grdSE[RowNumb + 1, (int)SE.gSCC] == null) //if (DT.Rows[RowNumb]["SChild"].ToString() != "")
                            grdSE[RowNumb + 1, (int)SE.gSCC] = DT.Rows[RowNumb]["SChild"].ToString();
                        if (grdSE[RowNumb + 1, (int)SE.gNAC] == null) //if (DT.Rows[RowNumb]["NAdult"].ToString() != "")
                            grdSE[RowNumb + 1, (int)SE.gNAC] = DT.Rows[RowNumb]["NAdult"].ToString();
                        if (grdSE[RowNumb + 1, (int)SE.gNCC] == null) //if (DT.Rows[RowNumb]["NChild"].ToString() != "")
                            grdSE[RowNumb + 1, (int)SE.gNCC] = DT.Rows[RowNumb]["NChild"].ToString();
                        if (IsSaarc == 1)
                        {
                            if (grdSE[RowNumb + 1, grdSE.Cols[(int)SE.gSAC].Index] != null)
                                TCostA = Convert.ToDouble(grdSE[RowNumb + 1, (int)SE.gSAC]);
                            if (grdSE[RowNumb + 1, grdSE.Cols[(int)SE.gNCC].Index] != null)
                                TCostC = Convert.ToDouble(grdSE[RowNumb + 1, (int)SE.gNCC]);
                        }
                        else
                        {
                            if (grdSE[RowNumb + 1, grdSE.Cols[(int)SE.gNAC].Index] != null)
                                TCostA = Convert.ToDouble(grdSE[RowNumb + 1, (int)SE.gNAC]);
                            if (grdSE[RowNumb + 1, grdSE.Cols[(int)SE.gSCC].Index] != null)
                                TCostC = Convert.ToDouble(grdSE[RowNumb + 1, (int)SE.gSCC]);
                        }
                        grdSE[RowNumb + 1, (int)SE.gNOA] = NOA;
                        grdSE[RowNumb + 1, (int)SE.gNOC] = NOC;
                        TCostA = TCostA * (Convert.ToInt32(grdSE[RowNumb + 1, (int)SE.gNOA]));
                        TCostC = TCostC * (Convert.ToInt32(grdSE[RowNumb + 1, (int)SE.gNOC]));
                        grdSE[RowNumb + 1, (int)SE.gTOT] = (TCostA + TCostC).ToString();
                        RowNumb++;
                    }
                }
            }
            catch (Exception ex){db.MsgERR(ex);}
        }
        private void chkSelectAll_CheckedChanged(object sender, EventArgs e)
        {
            SelectAll_Change();
        }
        private void SelectAll_Change()
        {
            try
            {
                int RowNumb = 1;
                int Status = 1;
                chkSelectAll.Text = "Uncheck All";
                if (chkSelectAll.Checked == false)
                {
                    Status = 0;
                    chkSelectAll.Text = "Check All";
                }
                while (grdSE[RowNumb, grdSE.Cols[(int)SE.gSSI].Index] != null)
                {
                    grdSE[RowNumb, (int)SE.gSEL] =Convert.ToBoolean(Status);
                    RowNumb++;
                }
            }
            catch (Exception ex){db.MsgERR(ex);}
        }
    }
}
