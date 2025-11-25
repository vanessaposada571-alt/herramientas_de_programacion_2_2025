using Hospital.domain.model;
using Hospital.domain.ports;
using System;
using Microsoft.Data.SqlClient;

namespace Hospital.infraestructure.adapters.output
{
    public class SQLMedicalOrderPort : Order_medicine_port
    {
        private SqlConnection GetConn() => new SqlConnection(Config.SqlConnectionString);

        public Order_medicine FindByNumOrder(Order_medicine orderMedicine)
        {
            const string sql = @"
                SELECT om.NumOrder, om.IdMedicine, om.Dose, om.DurationTreat, om.Item
                FROM dbo.OrderMedicine om
                WHERE om.NumOrder = @numOrder";

            using var cn = GetConn();
            cn.Open();
            using var cmd = new SqlCommand(sql, cn);
            cmd.Parameters.AddWithValue("@numOrder", orderMedicine.NumOrder);
            using var rd = cmd.ExecuteReader(System.Data.CommandBehavior.SingleRow);
            if (!rd.Read()) return null;

            return new Order_medicine
            {
                NumOrder = rd["NumOrder"] != DBNull.Value ? Convert.ToInt64(rd["NumOrder"]) : 0,
                IdMedicine = rd["IdMedicine"] != DBNull.Value ? Convert.ToInt64(rd["IdMedicine"]) : 0,
                Dose = rd["Dose"]?.ToString() ?? "",
                DurationTreat = rd["DurationTreat"] != DBNull.Value ? Convert.ToInt32(rd["DurationTreat"]) : 0,
                Item = rd["Item"] != DBNull.Value ? Convert.ToInt64(rd["Item"]) : 0
            };
        }

        public void Save(Order_medicine orderMedicine)
        {
            const string sql = @"
                INSERT INTO dbo.OrderMedicine (OrderId, NumOrder, IdMedicine, Dose, DurationTreat, Item)
                VALUES (@orderId, @numOrder, @idMedicine, @dose, @durationTreat, @item);";

            using var cn = GetConn();
            cn.Open();
            using var cmd = new SqlCommand(sql, cn);
            cmd.Parameters.AddWithValue("@orderId", DBNull.Value);
            cmd.Parameters.AddWithValue("@numOrder", orderMedicine.NumOrder);
            cmd.Parameters.AddWithValue("@idMedicine", orderMedicine.IdMedicine);
            cmd.Parameters.AddWithValue("@dose", orderMedicine.Dose ?? "");
            cmd.Parameters.AddWithValue("@durationTreat", orderMedicine.DurationTreat);
            cmd.Parameters.AddWithValue("@item", orderMedicine.Item);
            cmd.ExecuteNonQuery();
        }

        public void Update(Order_medicine orderMedicine)
        {
            const string sql = @"
                UPDATE dbo.OrderMedicine
                SET IdMedicine = @idMedicine, Dose = @dose, DurationTreat = @durationTreat, Item = @item
                WHERE NumOrder = @numOrder;";

            using var cn = GetConn();
            cn.Open();
            using var cmd = new SqlCommand(sql, cn);
            cmd.Parameters.AddWithValue("@idMedicine", orderMedicine.IdMedicine);
            cmd.Parameters.AddWithValue("@dose", orderMedicine.Dose ?? "");
            cmd.Parameters.AddWithValue("@durationTreat", orderMedicine.DurationTreat);
            cmd.Parameters.AddWithValue("@item", orderMedicine.Item);
            cmd.Parameters.AddWithValue("@numOrder", orderMedicine.NumOrder);
            cmd.ExecuteNonQuery();
        }

        public void Delete(Order_medicine orderMedicine)
        {
            const string sql = @"DELETE FROM dbo.OrderMedicine WHERE NumOrder = @numOrder;";
            using var cn = GetConn();
            cn.Open();
            using var cmd = new SqlCommand(sql, cn);
            cmd.Parameters.AddWithValue("@numOrder", orderMedicine.NumOrder);
            cmd.ExecuteNonQuery();
        }

        public void Search(Order_medicine orderMedicine)
        {
            throw new NotImplementedException();
        }
    }
}