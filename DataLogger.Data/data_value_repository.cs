using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Data;
using Npgsql;
using DataLogger.Entities;

namespace DataLogger.Data
{
    public class data_value_repository : NpgsqlDBConnection
    {
        #region Public procedure

        /// <summary>
        /// add new
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public int add(ref data_value obj)
        {
            using (NpgsqlDBConnection db = new NpgsqlDBConnection())
            {
                try
                {
                    Int32 ID = -1;

                    if (db.open_connection())
                    {
                        string sql_command = "INSERT INTO data_values (var1, var1_status, var2, var2_status, " +
                                            " var3, var3_status, var4, var4_status, " +
                                            " var5, var5_status, var6, var6_status, var7, var7_status, " +
                                            " var8, var8_status, var9, var9_status, var10, var10_status, " +
                                            " var11, var11_status, var12, var12_status, var13, var13_status, " +
                                            " var14, var14_status, var15, var15_status, var16, var16_status, " +
                                            " var17, var17_status, var18, var18_status, " +
                                            " var19, var19_status, var20, var20_status, " +
                                            " var21, var21_status, var22, var22_status, var23, var23_status, " +
                                            " var24, var24_status, var25, var25_status, var26, var26_status, " +
                                            " var27, var27_status, var28, var28_status, var29, var29_status, " +
                                            " var30, var30_status, var31, var31_status, var32, var32_status, " +
                                            " var33, var33_status, var34, var34_status,  var35, var35_status," +
                                            " stored_date, stored_hour, stored_minute, MPS_status, " +
                                            " push, push_time, " +
                                            " created)" +
                                            " VALUES (:var1, :var1_status, :var2, :var2_status, " +
                                            " :var3,:var3_status, :var4, :var4_status, " +
                                            " :var5, :var5_status, :var6, :var6_status, :var7, :var7_status, " +
                                            " :var8, :var8_status, :var9, :var9_status, :var10, :var10_status, " +
                                            " :var11, :var11_status, :var12, :var12_status, :var13, :var13_status, " +
                                            " :var14, :var14_status, :var15, :var15_status, :var16, :var16_status, " +
                                            " :var17, :var17_status, :var18, :var18_status, " +
                                            " :var19, :var19_status, :var20, :var20_status, " +
                                            " :var21, :var21_status, :var22, :var22_status, :var23, :var23_status, " +
                                            " :var24, :var24_status, :var25, :var25_status, :var26, :var26_status, " +
                                            " :var27, :var27_status, :var28, :var28_status, :var29, :var29_status, " +
                                            " :var30, :var30_status, :var31, :var31_status, :var32, :var32_status, " +
                                            " :var33, :var33_status, :var34, :var34_status, :var35, :var35_status," +
                                            " :stored_date, :stored_hour, :stored_minute, :MPS_status, " +
                                            " :push, :push_time, " +
                                            " :created)";
                        sql_command += " RETURNING id;";

                        using (NpgsqlCommand cmd = db._conn.CreateCommand())
                        {
                            cmd.CommandText = sql_command;

                            cmd.Parameters.Add(":var1", NpgsqlTypes.NpgsqlDbType.Double).Value = obj.var1;
                            cmd.Parameters.Add(":var1_status", NpgsqlTypes.NpgsqlDbType.Integer).Value = obj.var1_status;
                            cmd.Parameters.Add(":var2", NpgsqlTypes.NpgsqlDbType.Double).Value = obj.var2;
                            cmd.Parameters.Add(":var2_status", NpgsqlTypes.NpgsqlDbType.Integer).Value = obj.var2_status;
                            cmd.Parameters.Add(":var3", NpgsqlTypes.NpgsqlDbType.Double).Value = obj.var3;
                            cmd.Parameters.Add(":var3_status", NpgsqlTypes.NpgsqlDbType.Integer).Value = obj.var3_status;
                            cmd.Parameters.Add(":var4", NpgsqlTypes.NpgsqlDbType.Double).Value = obj.var4;
                            cmd.Parameters.Add(":var4_status", NpgsqlTypes.NpgsqlDbType.Integer).Value = obj.var4_status;
                            cmd.Parameters.Add(":var5", NpgsqlTypes.NpgsqlDbType.Double).Value = obj.var5;
                            cmd.Parameters.Add(":var5_status", NpgsqlTypes.NpgsqlDbType.Integer).Value = obj.var5_status;
                            cmd.Parameters.Add(":var6", NpgsqlTypes.NpgsqlDbType.Double).Value = obj.var6;
                            cmd.Parameters.Add(":var6_status", NpgsqlTypes.NpgsqlDbType.Integer).Value = obj.var6_status;
                            cmd.Parameters.Add(":var7", NpgsqlTypes.NpgsqlDbType.Double).Value = obj.var7;
                            cmd.Parameters.Add(":var7_status", NpgsqlTypes.NpgsqlDbType.Integer).Value = obj.var7_status;

                            cmd.Parameters.Add(":var8", NpgsqlTypes.NpgsqlDbType.Double).Value = obj.var8;
                            cmd.Parameters.Add(":var8_status", NpgsqlTypes.NpgsqlDbType.Integer).Value = obj.var8_status;
                            cmd.Parameters.Add(":var9", NpgsqlTypes.NpgsqlDbType.Double).Value = obj.var9;
                            cmd.Parameters.Add(":var9_status", NpgsqlTypes.NpgsqlDbType.Integer).Value = obj.var9_status;
                            cmd.Parameters.Add(":var10", NpgsqlTypes.NpgsqlDbType.Double).Value = obj.var10;
                            cmd.Parameters.Add(":var10_status", NpgsqlTypes.NpgsqlDbType.Integer).Value = obj.var10_status;
                            cmd.Parameters.Add(":var11", NpgsqlTypes.NpgsqlDbType.Double).Value = obj.var11;
                            cmd.Parameters.Add(":var11_status", NpgsqlTypes.NpgsqlDbType.Integer).Value = obj.var11_status;
                            cmd.Parameters.Add(":var12", NpgsqlTypes.NpgsqlDbType.Double).Value = obj.var12;
                            cmd.Parameters.Add(":var12_status", NpgsqlTypes.NpgsqlDbType.Integer).Value = obj.var12_status;
                            cmd.Parameters.Add(":var13", NpgsqlTypes.NpgsqlDbType.Double).Value = obj.var13;
                            cmd.Parameters.Add(":var13_status", NpgsqlTypes.NpgsqlDbType.Integer).Value = obj.var13_status;
                            cmd.Parameters.Add(":var14", NpgsqlTypes.NpgsqlDbType.Double).Value = obj.var14;
                            cmd.Parameters.Add(":var14_status", NpgsqlTypes.NpgsqlDbType.Integer).Value = obj.var14_status;
                            cmd.Parameters.Add(":var15", NpgsqlTypes.NpgsqlDbType.Double).Value = obj.var15;
                            cmd.Parameters.Add(":var15_status", NpgsqlTypes.NpgsqlDbType.Integer).Value = obj.var15_status;
                            cmd.Parameters.Add(":var16", NpgsqlTypes.NpgsqlDbType.Double).Value = obj.var16;
                            cmd.Parameters.Add(":var16_status", NpgsqlTypes.NpgsqlDbType.Integer).Value = obj.var16_status;
                            cmd.Parameters.Add(":var17", NpgsqlTypes.NpgsqlDbType.Double).Value = obj.var17;
                            cmd.Parameters.Add(":var17_status", NpgsqlTypes.NpgsqlDbType.Integer).Value = obj.var17_status;
                            cmd.Parameters.Add(":var18", NpgsqlTypes.NpgsqlDbType.Double).Value = obj.var18;
                            cmd.Parameters.Add(":var18_status", NpgsqlTypes.NpgsqlDbType.Integer).Value = obj.var18_status;

                            cmd.Parameters.Add(":var19", NpgsqlTypes.NpgsqlDbType.Double).Value = obj.var19;
                            cmd.Parameters.Add(":var19_status", NpgsqlTypes.NpgsqlDbType.Integer).Value = obj.var19_status;
                            cmd.Parameters.Add(":var20", NpgsqlTypes.NpgsqlDbType.Double).Value = obj.var20;
                            cmd.Parameters.Add(":var20_status", NpgsqlTypes.NpgsqlDbType.Integer).Value = obj.var20_status;
                            cmd.Parameters.Add(":var21", NpgsqlTypes.NpgsqlDbType.Double).Value = obj.var21;
                            cmd.Parameters.Add(":var21_status", NpgsqlTypes.NpgsqlDbType.Integer).Value = obj.var21_status;
                            cmd.Parameters.Add(":var22", NpgsqlTypes.NpgsqlDbType.Double).Value = obj.var22;
                            cmd.Parameters.Add(":var22_status", NpgsqlTypes.NpgsqlDbType.Integer).Value = obj.var22_status;
                            cmd.Parameters.Add(":var23", NpgsqlTypes.NpgsqlDbType.Double).Value = obj.var23;
                            cmd.Parameters.Add(":var23_status", NpgsqlTypes.NpgsqlDbType.Integer).Value = obj.var23_status;
                            cmd.Parameters.Add(":var24", NpgsqlTypes.NpgsqlDbType.Double).Value = obj.var24;
                            cmd.Parameters.Add(":var24_status", NpgsqlTypes.NpgsqlDbType.Integer).Value = obj.var24_status;
                            cmd.Parameters.Add(":var25", NpgsqlTypes.NpgsqlDbType.Double).Value = obj.var25;
                            cmd.Parameters.Add(":var25_status", NpgsqlTypes.NpgsqlDbType.Integer).Value = obj.var25_status;

                            cmd.Parameters.Add(":var26", NpgsqlTypes.NpgsqlDbType.Double).Value = obj.var26;
                            cmd.Parameters.Add(":var26_status", NpgsqlTypes.NpgsqlDbType.Integer).Value = obj.var26_status;
                            cmd.Parameters.Add(":var27", NpgsqlTypes.NpgsqlDbType.Double).Value = obj.var27;
                            cmd.Parameters.Add(":var27_status", NpgsqlTypes.NpgsqlDbType.Integer).Value = obj.var27_status;
                            cmd.Parameters.Add(":var28", NpgsqlTypes.NpgsqlDbType.Double).Value = obj.var28;
                            cmd.Parameters.Add(":var28_status", NpgsqlTypes.NpgsqlDbType.Integer).Value = obj.var28_status;
                            cmd.Parameters.Add(":var29", NpgsqlTypes.NpgsqlDbType.Double).Value = obj.var29;
                            cmd.Parameters.Add(":var29_status", NpgsqlTypes.NpgsqlDbType.Integer).Value = obj.var29_status;
                            cmd.Parameters.Add(":var30", NpgsqlTypes.NpgsqlDbType.Double).Value = obj.var30;
                            cmd.Parameters.Add(":var30_status", NpgsqlTypes.NpgsqlDbType.Integer).Value = obj.var30_status;
                            cmd.Parameters.Add(":var31", NpgsqlTypes.NpgsqlDbType.Double).Value = obj.var31;
                            cmd.Parameters.Add(":var31_status", NpgsqlTypes.NpgsqlDbType.Integer).Value = obj.var31_status;
                            cmd.Parameters.Add(":var32", NpgsqlTypes.NpgsqlDbType.Double).Value = obj.var32;
                            cmd.Parameters.Add(":var32_status", NpgsqlTypes.NpgsqlDbType.Integer).Value = obj.var32_status;
                            cmd.Parameters.Add(":var33", NpgsqlTypes.NpgsqlDbType.Double).Value = obj.var33;
                            cmd.Parameters.Add(":var33_status", NpgsqlTypes.NpgsqlDbType.Integer).Value = obj.var33_status;
                            cmd.Parameters.Add(":var34", NpgsqlTypes.NpgsqlDbType.Double).Value = obj.var34;
                            cmd.Parameters.Add(":var34_status", NpgsqlTypes.NpgsqlDbType.Integer).Value = obj.var34_status;
                            cmd.Parameters.Add(":var35", NpgsqlTypes.NpgsqlDbType.Double).Value = obj.var35;
                            cmd.Parameters.Add(":var35_status", NpgsqlTypes.NpgsqlDbType.Integer).Value = obj.var35_status;

                            cmd.Parameters.Add(":stored_date", NpgsqlTypes.NpgsqlDbType.Date).Value = obj.stored_date;
                            cmd.Parameters.Add(":stored_hour", NpgsqlTypes.NpgsqlDbType.Integer).Value = obj.stored_hour;
                            cmd.Parameters.Add(":stored_minute", NpgsqlTypes.NpgsqlDbType.Integer).Value = obj.stored_minute;
                            cmd.Parameters.Add(":MPS_status", NpgsqlTypes.NpgsqlDbType.Integer).Value = obj.MPS_status;
                            cmd.Parameters.Add(":created", NpgsqlTypes.NpgsqlDbType.Timestamp).Value = obj.created;

                            cmd.Parameters.Add(":push", NpgsqlTypes.NpgsqlDbType.Integer).Value = obj.push;
                            cmd.Parameters.Add(":push_time", NpgsqlTypes.NpgsqlDbType.Timestamp).Value = obj.push_time;

                            //cmd.ExecuteNonQuery();
                            ID = (Int32)cmd.ExecuteScalar();
                            obj.id = ID;

                            db.close_connection();
                            return ID;
                        }
                    }
                    else
                    {
                        db.close_connection();
                        return -1;
                    }
                }
                catch (Exception e)
                {
                    if (db != null)
                    {
                        db.close_connection();
                    }
                    return -1;
                }
                finally
                {
                    db.close_connection();
                }
            }
        }

        public IEnumerable<data_value> get_all()
        {
            List<data_value> listUser = new List<data_value>();
            using (NpgsqlDBConnection db = new NpgsqlDBConnection())
            {
                try
                {
                    if (db.open_connection())
                    {
                        string sql_command = "SELECT * FROM data_values";
                        using (NpgsqlCommand cmd = db._conn.CreateCommand())
                        {
                            cmd.CommandText = sql_command;
                            NpgsqlDataReader reader = cmd.ExecuteReader();
                            while (reader.Read())
                            {
                                data_value obj = new data_value();
                                obj = (data_value)_get_info(reader);
                                listUser.Add(obj);
                            }
                            reader.Close();
                            db.close_connection();
                            return listUser;
                        }
                    }
                    else
                    {
                        db.close_connection();
                        return null;
                    }
                }
                catch
                {
                    if (db != null)
                    {
                        db.close_connection();
                    }
                    return null;
                }
                finally
                { db.close_connection(); }
            }
        }

        public data_value get_info_by_id(int id)
        {
            using (NpgsqlDBConnection db = new NpgsqlDBConnection())
            {
                try
                {

                    data_value obj = null;
                    if (db.open_connection())
                    {
                        string sql_command = "SELECT * FROM data_values WHERE id = " + id;
                        sql_command += " LIMIT 1";
                        using (NpgsqlCommand cmd = db._conn.CreateCommand())
                        {
                            cmd.CommandText = sql_command;

                            NpgsqlDataReader reader = cmd.ExecuteReader();

                            while (reader.Read())
                            {
                                obj = new data_value();
                                obj = (data_value)_get_info(reader);
                                break;
                            }
                            reader.Close();
                            db.close_connection();
                            return obj;
                        }
                    }
                    else
                    {
                        db.close_connection();
                        return null;
                    }
                }
                catch
                {
                    if (db != null)
                    {
                        db.close_connection();
                    }
                    return null;
                }
                finally
                { db.close_connection(); }
            }
        }

        #endregion Public procedure

        #region private procedure

        private data_value _get_info(NpgsqlDataReader dataReader)
        {
            data_value obj = new data_value();
            try
            {
                if (!DBNull.Value.Equals(dataReader["id"]))
                    obj.id = Convert.ToInt32(dataReader["id"].ToString().Trim());
                else
                    obj.id = 0;
                if (!DBNull.Value.Equals(dataReader["var1"]))
                    obj.var1 = Convert.ToDouble(dataReader["var1"].ToString().Trim());
                else
                    obj.var1 = 0;
                if (!DBNull.Value.Equals(dataReader["var1_status"]))
                    obj.var1_status = Convert.ToInt32(dataReader["var1_status"].ToString().Trim());
                else
                    obj.var1_status = 0;

                if (!DBNull.Value.Equals(dataReader["var2"]))
                    obj.var2 = Convert.ToDouble(dataReader["var2"].ToString().Trim());
                else
                    obj.var2 = 0;
                if (!DBNull.Value.Equals(dataReader["var2_status"]))
                    obj.var2_status = Convert.ToInt32(dataReader["var2_status"].ToString().Trim());
                else
                    obj.var2_status = 0;

                if (!DBNull.Value.Equals(dataReader["var3"]))
                    obj.var3 = Convert.ToDouble(dataReader["var3"].ToString().Trim());
                else
                    obj.var3 = 0;
                if (!DBNull.Value.Equals(dataReader["var3_status"]))
                    obj.var3_status = Convert.ToInt32(dataReader["var3_status"].ToString().Trim());
                else
                    obj.var3_status = 0;

                if (!DBNull.Value.Equals(dataReader["var4"]))
                    obj.var4 = Convert.ToDouble(dataReader["var4"].ToString().Trim());
                else
                    obj.var4 = 0;
                if (!DBNull.Value.Equals(dataReader["var4_status"]))
                    obj.var4_status = Convert.ToInt32(dataReader["var4_status"].ToString().Trim());
                else
                    obj.var4_status = 0;

                if (!DBNull.Value.Equals(dataReader["var5"]))
                    obj.var5 = Convert.ToDouble(dataReader["var5"].ToString().Trim());
                else
                    obj.var5 = 0;
                if (!DBNull.Value.Equals(dataReader["var5_status"]))
                    obj.var5_status = Convert.ToInt32(dataReader["var5_status"].ToString().Trim());
                else
                    obj.var5_status = 0;

                if (!DBNull.Value.Equals(dataReader["var6"]))
                    obj.var6 = Convert.ToDouble(dataReader["var6"].ToString().Trim());
                else
                    obj.var6 = 0;
                if (!DBNull.Value.Equals(dataReader["var6_status"]))
                    obj.var6_status = Convert.ToInt32(dataReader["var6_status"].ToString().Trim());
                else
                    obj.var6_status = 0;

                if (!DBNull.Value.Equals(dataReader["var7"]))
                    obj.var7 = Convert.ToDouble(dataReader["var7"].ToString().Trim());
                else
                    obj.var7 = 0;
                if (!DBNull.Value.Equals(dataReader["var7_status"]))
                    obj.var7_status = Convert.ToInt32(dataReader["var7_status"].ToString().Trim());
                else
                    obj.var7_status = 0;

                if (!DBNull.Value.Equals(dataReader["var8"]))
                    obj.var8 = Convert.ToDouble(dataReader["var8"].ToString().Trim());
                else
                    obj.var8 = 0;
                if (!DBNull.Value.Equals(dataReader["var8_status"]))
                    obj.var8_status = Convert.ToInt32(dataReader["var8_status"].ToString().Trim());
                else
                    obj.var8_status = 0;

                if (!DBNull.Value.Equals(dataReader["var9"]))
                    obj.var9 = Convert.ToDouble(dataReader["var9"].ToString().Trim());
                else
                    obj.var9 = 0;
                if (!DBNull.Value.Equals(dataReader["var9_status"]))
                    obj.var9_status = Convert.ToInt32(dataReader["var9_status"].ToString().Trim());
                else
                    obj.var9_status = 0;

                if (!DBNull.Value.Equals(dataReader["var10"]))
                    obj.var10 = Convert.ToDouble(dataReader["var10"].ToString().Trim());
                else
                    obj.var10 = 0;
                if (!DBNull.Value.Equals(dataReader["var10_status"]))
                    obj.var10_status = Convert.ToInt32(dataReader["var10_status"].ToString().Trim());
                else
                    obj.var10_status = 0;

                if (!DBNull.Value.Equals(dataReader["var11"]))
                    obj.var11 = Convert.ToDouble(dataReader["var11"].ToString().Trim());
                else
                    obj.var11 = 0;
                if (!DBNull.Value.Equals(dataReader["var11_status"]))
                    obj.var11_status = Convert.ToInt32(dataReader["var11_status"].ToString().Trim());
                else
                    obj.var11_status = 0;

                if (!DBNull.Value.Equals(dataReader["var12"]))
                    obj.var12 = Convert.ToDouble(dataReader["var12"].ToString().Trim());
                else
                    obj.var12 = 0;
                if (!DBNull.Value.Equals(dataReader["var12_status"]))
                    obj.var12_status = Convert.ToInt32(dataReader["var12_status"].ToString().Trim());
                else
                    obj.var12_status = 0;

                if (!DBNull.Value.Equals(dataReader["var13"]))
                    obj.var13 = Convert.ToDouble(dataReader["var13"].ToString().Trim());
                else
                    obj.var13 = 0;
                if (!DBNull.Value.Equals(dataReader["var13_status"]))
                    obj.var13_status = Convert.ToInt32(dataReader["var13_status"].ToString().Trim());
                else
                    obj.var13_status = 0;

                if (!DBNull.Value.Equals(dataReader["var14"]))
                    obj.var14 = Convert.ToDouble(dataReader["var14"].ToString().Trim());
                else
                    obj.var14 = 0;
                if (!DBNull.Value.Equals(dataReader["var14_status"]))
                    obj.var14_status = Convert.ToInt32(dataReader["var14_status"].ToString().Trim());
                else
                    obj.var14_status = 0;

                if (!DBNull.Value.Equals(dataReader["var15"]))
                    obj.var15 = Convert.ToDouble(dataReader["var15"].ToString().Trim());
                else
                    obj.var15 = 0;
                if (!DBNull.Value.Equals(dataReader["var15_status"]))
                    obj.var15_status = Convert.ToInt32(dataReader["var15_status"].ToString().Trim());
                else
                    obj.var15_status = 0;

                if (!DBNull.Value.Equals(dataReader["var16"]))
                    obj.var16 = Convert.ToDouble(dataReader["var16"].ToString().Trim());
                else
                    obj.var16 = 0;
                if (!DBNull.Value.Equals(dataReader["var16_status"]))
                    obj.var16_status = Convert.ToInt32(dataReader["var16_status"].ToString().Trim());
                else
                    obj.var16_status = 0;

                if (!DBNull.Value.Equals(dataReader["var17"]))
                    obj.var17 = Convert.ToDouble(dataReader["var17"].ToString().Trim());
                else
                    obj.var17 = 0;
                if (!DBNull.Value.Equals(dataReader["var17_status"]))
                    obj.var17_status = Convert.ToInt32(dataReader["var17_status"].ToString().Trim());
                else
                    obj.var17_status = 0;

                if (!DBNull.Value.Equals(dataReader["var18"]))
                    obj.var18 = Convert.ToDouble(dataReader["var18"].ToString().Trim());
                else
                    obj.var18 = 0;
                if (!DBNull.Value.Equals(dataReader["var18_status"]))
                    obj.var18_status = Convert.ToInt32(dataReader["var18_status"].ToString().Trim());
                else
                    obj.var18_status = 0;

                if (!DBNull.Value.Equals(dataReader["var19"]))
                    obj.var19 = Convert.ToDouble(dataReader["var19"].ToString().Trim());
                else
                    obj.var19 = 0;
                if (!DBNull.Value.Equals(dataReader["var19_status"]))
                    obj.var19_status = Convert.ToInt32(dataReader["var19_status"].ToString().Trim());
                else
                    obj.var19_status = 0;

                if (!DBNull.Value.Equals(dataReader["var20"]))
                    obj.var20 = Convert.ToDouble(dataReader["var20"].ToString().Trim());
                else
                    obj.var20 = 0;
                if (!DBNull.Value.Equals(dataReader["var20_status"]))
                    obj.var20_status = Convert.ToInt32(dataReader["var20_status"].ToString().Trim());
                else
                    obj.var20_status = 0;

                if (!DBNull.Value.Equals(dataReader["var21"]))
                    obj.var21 = Convert.ToDouble(dataReader["var21"].ToString().Trim());
                else
                    obj.var21 = 0;
                if (!DBNull.Value.Equals(dataReader["var21_status"]))
                    obj.var21_status = Convert.ToInt32(dataReader["var21_status"].ToString().Trim());
                else
                    obj.var21_status = 0;

                if (!DBNull.Value.Equals(dataReader["var22"]))
                    obj.var22 = Convert.ToDouble(dataReader["var22"].ToString().Trim());
                else
                    obj.var22 = 0;
                if (!DBNull.Value.Equals(dataReader["var22_status"]))
                    obj.var22_status = Convert.ToInt32(dataReader["var22_status"].ToString().Trim());
                else
                    obj.var22_status = 0;

                if (!DBNull.Value.Equals(dataReader["var23"]))
                    obj.var23 = Convert.ToDouble(dataReader["var23"].ToString().Trim());
                else
                    obj.var23 = 0;
                if (!DBNull.Value.Equals(dataReader["var23_status"]))
                    obj.var23_status = Convert.ToInt32(dataReader["var23_status"].ToString().Trim());
                else
                    obj.var23_status = 0;

                if (!DBNull.Value.Equals(dataReader["var24"]))
                    obj.var24 = Convert.ToDouble(dataReader["var24"].ToString().Trim());
                else
                    obj.var24 = 0;
                if (!DBNull.Value.Equals(dataReader["var24_status"]))
                    obj.var24_status = Convert.ToInt32(dataReader["var24_status"].ToString().Trim());
                else
                    obj.var24_status = 0;

                if (!DBNull.Value.Equals(dataReader["var25"]))
                    obj.var25 = Convert.ToDouble(dataReader["var25"].ToString().Trim());
                else
                    obj.var25 = 0;
                if (!DBNull.Value.Equals(dataReader["var25_status"]))
                    obj.var25_status = Convert.ToInt32(dataReader["var25_status"].ToString().Trim());
                else
                    obj.var25_status = 0;

                if (!DBNull.Value.Equals(dataReader["var26"]))
                    obj.var26 = Convert.ToDouble(dataReader["var26"].ToString().Trim());
                else
                    obj.var26 = 0;
                if (!DBNull.Value.Equals(dataReader["var26_status"]))
                    obj.var26_status = Convert.ToInt32(dataReader["var26_status"].ToString().Trim());
                else
                    obj.var26_status = 0;

                if (!DBNull.Value.Equals(dataReader["var27"]))
                    obj.var27 = Convert.ToDouble(dataReader["var27"].ToString().Trim());
                else
                    obj.var27 = 0;
                if (!DBNull.Value.Equals(dataReader["var27_status"]))
                    obj.var27_status = Convert.ToInt32(dataReader["var27_status"].ToString().Trim());
                else
                    obj.var27_status = 0;

                if (!DBNull.Value.Equals(dataReader["var28"]))
                    obj.var28 = Convert.ToDouble(dataReader["var28"].ToString().Trim());
                else
                    obj.var28 = 0;
                if (!DBNull.Value.Equals(dataReader["var28_status"]))
                    obj.var28_status = Convert.ToInt32(dataReader["var28_status"].ToString().Trim());
                else
                    obj.var28_status = 0;

                if (!DBNull.Value.Equals(dataReader["var29"]))
                    obj.var29 = Convert.ToDouble(dataReader["var29"].ToString().Trim());
                else
                    obj.var29 = 0;
                if (!DBNull.Value.Equals(dataReader["var29_status"]))
                    obj.var29_status = Convert.ToInt32(dataReader["var29_status"].ToString().Trim());
                else
                    obj.var29_status = 0;

                if (!DBNull.Value.Equals(dataReader["var30"]))
                    obj.var30 = Convert.ToDouble(dataReader["var30"].ToString().Trim());
                else
                    obj.var30 = 0;
                if (!DBNull.Value.Equals(dataReader["var30_status"]))
                    obj.var30_status = Convert.ToInt32(dataReader["var30_status"].ToString().Trim());
                else
                    obj.var30_status = 0;

                if (!DBNull.Value.Equals(dataReader["var31"]))
                    obj.var31 = Convert.ToDouble(dataReader["var31"].ToString().Trim());
                else
                    obj.var31 = 0;
                if (!DBNull.Value.Equals(dataReader["var31_status"]))
                    obj.var31_status = Convert.ToInt32(dataReader["var31_status"].ToString().Trim());
                else
                    obj.var31_status = 0;

                if (!DBNull.Value.Equals(dataReader["var32"]))
                    obj.var32 = Convert.ToDouble(dataReader["var32"].ToString().Trim());
                else
                    obj.var32 = 0;
                if (!DBNull.Value.Equals(dataReader["var32_status"]))
                    obj.var32_status = Convert.ToInt32(dataReader["var32_status"].ToString().Trim());
                else
                    obj.var32_status = 0;

                if (!DBNull.Value.Equals(dataReader["var33"]))
                    obj.var33 = Convert.ToDouble(dataReader["var33"].ToString().Trim());
                else
                    obj.var33 = 0;
                if (!DBNull.Value.Equals(dataReader["var33_status"]))
                    obj.var33_status = Convert.ToInt32(dataReader["var33_status"].ToString().Trim());
                else
                    obj.var33_status = 0;

                if (!DBNull.Value.Equals(dataReader["var34"]))
                    obj.var34 = Convert.ToDouble(dataReader["var34"].ToString().Trim());
                else
                    obj.var34 = 0;
                if (!DBNull.Value.Equals(dataReader["var34_status"]))
                    obj.var34_status = Convert.ToInt32(dataReader["var34_status"].ToString().Trim());
                else
                    obj.var34_status = 0;

                if (!DBNull.Value.Equals(dataReader["var35"]))
                    obj.var35 = Convert.ToDouble(dataReader["var35"].ToString().Trim());
                else
                    obj.var35 = 0;
                if (!DBNull.Value.Equals(dataReader["var35_status"]))
                    obj.var35_status = Convert.ToInt32(dataReader["var35_status"].ToString().Trim());
                else
                    obj.var35_status = 0;

                if (!DBNull.Value.Equals(dataReader["stored_date"]))
                    obj.stored_date = Convert.ToDateTime(dataReader["stored_date"].ToString().Trim());
                else
                    obj.stored_date = DateTime.Now;
                if (!DBNull.Value.Equals(dataReader["stored_hour"]))
                    obj.stored_hour = Convert.ToInt32(dataReader["stored_hour"].ToString().Trim());
                else
                    obj.stored_hour = 0;
                if (!DBNull.Value.Equals(dataReader["stored_minute"]))
                    obj.stored_minute = Convert.ToInt32(dataReader["stored_minute"].ToString().Trim());
                else
                    obj.stored_minute = 0;
                if (!DBNull.Value.Equals(dataReader["MPS_status"]))
                    obj.MPS_status = Convert.ToInt32(dataReader["MPS_status"].ToString().Trim());
                else
                    obj.MPS_status = 0;

                if (!DBNull.Value.Equals(dataReader["created"]))
                    obj.created = Convert.ToDateTime(dataReader["created"].ToString().Trim());
                else
                    obj.created = DateTime.Now;

                if (!DBNull.Value.Equals(dataReader["push"]))
                    obj.push = Convert.ToInt32(dataReader["push"].ToString().Trim());
                else
                    obj.push = -1;
                if (!DBNull.Value.Equals(dataReader["push_time"]))
                    obj.push_time = Convert.ToDateTime(dataReader["push_time"].ToString().Trim());
                else
                    obj.push_time = new DateTime();

            }
            catch (Exception ex)
            {
                throw ex;
            }
            return obj;
        }

        #endregion private procedure
    }
}