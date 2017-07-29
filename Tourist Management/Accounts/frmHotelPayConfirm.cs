using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
namespace Tourist_Management.Accounts
{
    public partial class frmHotelPayConfirm : Form
    {
        private const string msghd = "Payment Confirm";
        enum PD { gTID, gHID, gVID, gCNM, gHNM, gAMT, gCON };
        public frmHotelPayConfirm(){InitializeComponent();}
        private void btnCancel_Click(object sender, EventArgs e){this.Close();}
        private void frmHotelPayConfirm_Load(object sender, EventArgs e)
        {
            Intializer();
        }
        private void Intializer()
        {
            try
            {
                Grd_Initializer();
                dtpPaidDate.Value = Classes.clsGlobal.CurDate();                
            }
            catch (Exception ex){db.MsgERR(ex);}
        }
         private void Grd_Initializer()
        {
            try
            {                                
                grdPayConf.Cols.Count = 7;
                grdPayConf.Rows.Count = 5000;
                grdPayConf.Cols[(int)PD.gTID].Width = 0;
                grdPayConf.Cols[(int)PD.gHID].Width = 0;
                grdPayConf.Cols[(int)PD.gVID].Width = 100;
                grdPayConf.Cols[(int)PD.gCNM].Width = 300;
                grdPayConf.Cols[(int)PD.gHNM].Width = 218;
                grdPayConf.Cols[(int)PD.gAMT].Width = 120;
                grdPayConf.Cols[(int)PD.gCON].Width = 80;
                grdPayConf.Cols[(int)PD.gTID].Caption = "Tour ID";
                grdPayConf.Cols[(int)PD.gHID].Caption = "Hotel ID";
                grdPayConf.Cols[(int)PD.gVID].Caption = "Voucher ID";
                grdPayConf.Cols[(int)PD.gCNM].Caption = "Client Name";
                grdPayConf.Cols[(int)PD.gHNM].Caption = "Hotel Name";
                grdPayConf.Cols[(int)PD.gAMT].Caption = "Amount";
                grdPayConf.Cols[(int)PD.gCON].Caption = "Confirm";
                grdPayConf.Cols[(int)PD.gAMT].Format = "##.##";
                grdPayConf.Cols[(int)PD.gCON].DataType = Type.GetType("System.Boolean");
            }
            catch (Exception ex){db.MsgERR(ex);}
        }
        private void Filter_Values()
        {
            try
            {
                DataTable DT;
                string ssql;
                int RowNumb = 0,  RoomCount = 1 ;
                string transid = "", temptransid = "";
                string vid = "" ;
                string format = "yyyy-MM-dd";
                DateTime paydate = dtpPaidDate.Value;
                string DateFrom = paydate.ToString(format).Substring(0, 10);
                DateTime Today = dtpPaidDate.Value;
                string today = Today.ToString(format).Substring(0, 10);
                ssql = "SELECT TransID,DateIn,DateOut,VoucherID,AmendNo,Guest," +
                "ISNULL(BillNoHotel,'')AS BillNoHotel,ISNULL(BillNoRoom,'')AS BillNoRoom,ISNULL(ChkNo,'')AS ChkNo," +
                "HandleBy,HotelID,HotelName,RoomTypeID,RoomBasisID,OccupancyID,RoomTypeName,RoomBasisName,Occupancy," +
                "IsNull(FOCAdult,0)AS FOCAdult,IsNull(FOCChild,0)AS FOCChild," +
                "IsNull(CurID,2)AS CurID,CurCode," +
                "GuideCurID,GuideCurCode," +
                "IsNull(ModifiedCost,0)AS ModifiedCost,GuideCost," +
                "IsNull(ExtraBed,0)AS Ebed,IsNull(EbedCost,0)AS EbedCost,Advance," +
                "IsNull(Commission,0)AS Commission,IsNull(ConRate,0)AS ConRate,IsNull(GuideConRate,0)AS GuideConRate," +
                "IsNull(RoomCount,0)AS RoomCount,GuideRooms,IsNull(FOCRooms,0)AS FOCRooms,IsNull(Nights,1)AS Nights,MealFor," +
                "IsNull(AdultMealCost,0)AS AdultMealCost,IsNull(ChildMealCost,0)AS ChildMealCost,IsNull(GuideMealCost,0)AS GuideMealCost," +
                "IsNull(NoOfAdult,0) AS NoOfAdult,IsNull(NoOfChild,0) AS NoOfChild,NoOfGuide,IsPaid,PaidDate,PaidBy," +
                "ConfirmPaid,PaidConfirmBy,PartiallyPaid,OtherAmt,Remarks," +
                "InitiatedCost,InitiatedEbedCost" +
                " FROM vw_acc_HotelMonthlyPayments WHERE PaidDate='" + DateFrom.Trim() + "'"+
                " ORDER BY Dateout";
                DT = Tourist_Management.Classes.clsGlobal.objCon.Fill_Table(ssql);
                if (DT.Rows.Count != 0 && DT.Rows[RowNumb]["TransID"].ToString() != "")
                {
                    transid = DT.Rows[RowNumb]["TransID"].ToString().Trim();
                    temptransid = DT.Rows[RowNumb]["TransID"].ToString().Trim();
                    vid = DT.Rows[RowNumb]["VoucherID"].ToString().Trim();
                }
                else
                {
                    grdPayConf.Rows.Count = RoomCount;                    
                    return;
                }
            }
            catch (Exception ex){db.MsgERR(ex);}
        }
    }
}
