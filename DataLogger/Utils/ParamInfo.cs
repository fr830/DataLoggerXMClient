using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLogger.Utils
{
    public class ParamInfo
    {
        public string NameDB { get; set; }
        public string  NameDisplay { get; set; }
        public bool HasStatus { get; set; }
        public string StatusNameDB { get; set; }
        public string StatusNameDisplay { get; set; }
        public string StatusNameVisible { get; set; }
        public bool Selected { get; set; }
        public Color GraphColor { get; set; }
    }

    public static class DataLoggerParam
    {
        public static List<ParamInfo> PARAMETER_LIST = new List<ParamInfo>()
        {
            // new ParamInfo(){ NameDB = "var1",NameDisplay = "var1",HasStatus = true,StatusNameDB = "var1_status",StatusNameDisplay = "var1 Status", StatusNameVisible = "var1_Status_Val" , Selected = false, GraphColor = Color.OrangeRed}
            //,new ParamInfo(){ NameDB = "var2",NameDisplay = "var2",HasStatus = true,StatusNameDB = "var2_status",StatusNameDisplay = "var2 Status", StatusNameVisible = "var2_Status_Val" , Selected = false, GraphColor = Color.Blue }
            //,new ParamInfo(){ NameDB = "var3",NameDisplay = "var3",HasStatus = true,StatusNameDB = "var3_status",StatusNameDisplay = "var3 Status", StatusNameVisible = "var3_Status_Val" , Selected = false, GraphColor = Color.BlueViolet }
            //,new ParamInfo(){ NameDB = "var4",NameDisplay = "var4",HasStatus = true,StatusNameDB = "var4_status",StatusNameDisplay = "var4 Status", StatusNameVisible = "var4_Status_Val" , Selected = false, GraphColor = Color.Brown }
            //,new ParamInfo(){ NameDB = "var5",NameDisplay = "var5",HasStatus = true,StatusNameDB = "var5_status",StatusNameDisplay = "var5 Status", StatusNameVisible = "var5_Status_Val" , Selected = false, GraphColor = Color.Chocolate }
            //,new ParamInfo(){ NameDB = "var6",NameDisplay = "var6",HasStatus = true,StatusNameDB = "var6_status",StatusNameDisplay = "var6 Status", StatusNameVisible = "var6_Status_Val" , Selected = false, GraphColor = Color.Coral }
            //,new ParamInfo(){ NameDB = "var7",NameDisplay = "var7",HasStatus = true,StatusNameDB = "var7_status",StatusNameDisplay = "var7 Status", StatusNameVisible = "var7_Status_Val" , Selected = false, GraphColor = Color.CornflowerBlue }
            //,new ParamInfo(){ NameDB = "var8",NameDisplay = "var8",HasStatus = true,StatusNameDB = "var8_status",StatusNameDisplay = "var8 Status", StatusNameVisible = "var8_Status_Val" , Selected = false, GraphColor = Color.Crimson }
            //,new ParamInfo(){ NameDB = "var9",NameDisplay = "var9",HasStatus = true,StatusNameDB = "var9_status",StatusNameDisplay = "var9 Status", StatusNameVisible = "var9_Status_Val" , Selected = false, GraphColor = Color.DarkBlue }

            //,new ParamInfo(){ NameDB = "var10",NameDisplay = "var2",HasStatus = true,StatusNameDB = "var2_status",StatusNameDisplay = "var2 Status", StatusNameVisible = "var2_Status_Val" , Selected = false, GraphColor = Color.AliceBlue}
            //,new ParamInfo(){ NameDB = "var11",NameDisplay = "var3",HasStatus = true,StatusNameDB = "var3_status",StatusNameDisplay = "var3 Status", StatusNameVisible = "var3_Status_Val" , Selected = false, GraphColor = Color.AntiqueWhite }
            //,new ParamInfo(){ NameDB = "var12",NameDisplay = "var4",HasStatus = true,StatusNameDB = "var4_status",StatusNameDisplay = "var4 Status", StatusNameVisible = "var4_Status_Val" , Selected = false, GraphColor = Color.Aqua }
            //,new ParamInfo(){ NameDB = "var13",NameDisplay = "var5",HasStatus = true,StatusNameDB = "var5_status",StatusNameDisplay = "var5 Status", StatusNameVisible = "var5_Status_Val" , Selected = false, GraphColor = Color.Aquamarine }
            //,new ParamInfo(){ NameDB = "var14",NameDisplay = "var6",HasStatus = true,StatusNameDB = "var6_status",StatusNameDisplay = "var6 Status", StatusNameVisible = "var6_Status_Val" , Selected = false, GraphColor = Color.Azure}
            //,new ParamInfo(){ NameDB = "var15",NameDisplay = "var7",HasStatus = true,StatusNameDB = "var7_status",StatusNameDisplay = "var7 Status", StatusNameVisible = "var7_Status_Val" , Selected = false, GraphColor = Color.Beige }
            //,new ParamInfo(){ NameDB = "var16",NameDisplay = "var8",HasStatus = true,StatusNameDB = "var8_status",StatusNameDisplay = "var8 Status", StatusNameVisible = "var8_Status_Val" , Selected = false, GraphColor = Color.Bisque }
            //,new ParamInfo(){ NameDB = "var17",NameDisplay = "var9",HasStatus = true,StatusNameDB = "var9_status",StatusNameDisplay = "var9 Status", StatusNameVisible = "var9_Status_Val" , Selected = false, GraphColor = Color.Black }

            //,new ParamInfo(){ NameDB = "var18",NameDisplay = "var2",HasStatus = true,StatusNameDB = "var2_status",StatusNameDisplay = "var2 Status", StatusNameVisible = "var2_Status_Val" , Selected = false, GraphColor = Color.BlanchedAlmond }
            //,new ParamInfo(){ NameDB = "var19",NameDisplay = "var3",HasStatus = true,StatusNameDB = "var3_status",StatusNameDisplay = "var3 Status", StatusNameVisible = "var3_Status_Val" , Selected = false, GraphColor = Color.BurlyWood }
            //,new ParamInfo(){ NameDB = "var20",NameDisplay = "var4",HasStatus = true,StatusNameDB = "var4_status",StatusNameDisplay = "var4 Status", StatusNameVisible = "var4_Status_Val" , Selected = false, GraphColor = Color.CornflowerBlue }
            //,new ParamInfo(){ NameDB = "var21",NameDisplay = "var5",HasStatus = true,StatusNameDB = "var5_status",StatusNameDisplay = "var5 Status", StatusNameVisible = "var5_Status_Val" , Selected = false, GraphColor = Color.CadetBlue }
            //,new ParamInfo(){ NameDB = "var22",NameDisplay = "var6",HasStatus = true,StatusNameDB = "var6_status",StatusNameDisplay = "var6 Status", StatusNameVisible = "var6_Status_Val" , Selected = false, GraphColor = Color.Cornsilk }
            //,new ParamInfo(){ NameDB = "var23",NameDisplay = "var7",HasStatus = true,StatusNameDB = "var7_status",StatusNameDisplay = "var7 Status", StatusNameVisible = "var7_Status_Val" , Selected = false, GraphColor = Color.Cyan }
            //,new ParamInfo(){ NameDB = "var24",NameDisplay = "var8",HasStatus = true,StatusNameDB = "var8_status",StatusNameDisplay = "var8 Status", StatusNameVisible = "var8_Status_Val" , Selected = false, GraphColor = Color.DarkGoldenrod }
            //,new ParamInfo(){ NameDB = "var25",NameDisplay = "var9",HasStatus = true,StatusNameDB = "var9_status",StatusNameDisplay = "var9 Status", StatusNameVisible = "var9_Status_Val" , Selected = false, GraphColor = Color.DarkGray }

            //,new ParamInfo(){ NameDB = "var26",NameDisplay = "var2",HasStatus = true,StatusNameDB = "var2_status",StatusNameDisplay = "var2 Status", StatusNameVisible = "var2_Status_Val" , Selected = false, GraphColor = Color.DarkGreen }
            //,new ParamInfo(){ NameDB = "var27",NameDisplay = "var3",HasStatus = true,StatusNameDB = "var3_status",StatusNameDisplay = "var3 Status", StatusNameVisible = "var3_Status_Val" , Selected = false, GraphColor = Color.DarkKhaki }
            //,new ParamInfo(){ NameDB = "var28",NameDisplay = "var4",HasStatus = true,StatusNameDB = "var4_status",StatusNameDisplay = "var4 Status", StatusNameVisible = "var4_Status_Val" , Selected = false, GraphColor = Color.DarkMagenta }
            //,new ParamInfo(){ NameDB = "var29",NameDisplay = "var5",HasStatus = true,StatusNameDB = "var5_status",StatusNameDisplay = "var5 Status", StatusNameVisible = "var5_Status_Val" , Selected = false, GraphColor = Color.DarkOliveGreen }
            //,new ParamInfo(){ NameDB = "var30",NameDisplay = "var6",HasStatus = true,StatusNameDB = "var6_status",StatusNameDisplay = "var6 Status", StatusNameVisible = "var6_Status_Val" , Selected = false, GraphColor = Color.DarkOrange }
            //,new ParamInfo(){ NameDB = "var31",NameDisplay = "var7",HasStatus = true,StatusNameDB = "var7_status",StatusNameDisplay = "var7 Status", StatusNameVisible = "var7_Status_Val" , Selected = false, GraphColor = Color.DarkOrchid }
            //,new ParamInfo(){ NameDB = "var32",NameDisplay = "var8",HasStatus = true,StatusNameDB = "var8_status",StatusNameDisplay = "var8 Status", StatusNameVisible = "var8_Status_Val" , Selected = false, GraphColor = Color.DarkRed }
            //,new ParamInfo(){ NameDB = "var33",NameDisplay = "var9",HasStatus = true,StatusNameDB = "var9_status",StatusNameDisplay = "var9 Status", StatusNameVisible = "var9_Status_Val" , Selected = false, GraphColor = Color.DarkSeaGreen }
            //,new ParamInfo(){ NameDB = "var34",NameDisplay = "var8",HasStatus = true,StatusNameDB = "var8_status",StatusNameDisplay = "var8 Status", StatusNameVisible = "var8_Status_Val" , Selected = false, GraphColor = Color.DarkSalmon }
            //,new ParamInfo(){ NameDB = "var35",NameDisplay = "var9",HasStatus = true,StatusNameDB = "var9_status",StatusNameDisplay = "var9 Status", StatusNameVisible = "var9_Status_Val" , Selected = false, GraphColor = Color.DarkTurquoise }
        };

    }
}
