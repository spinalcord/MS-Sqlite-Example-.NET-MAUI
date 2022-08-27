using Microsoft.Data.Sqlite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MauiWithMSSqlite
{
    public enum FieldTyp
    {
        INTEGER,
        TEXT,
        BLOB,
        REAL,
        NUMERIC
    }

    public struct SqliteField
    {
        public string name { get; set; }
        public FieldTyp fieldType { get; set; }
    }


    public class Column
    {
        public string name { get; set; } = string.Empty;
        public List<object> values { get; set; } = new List<object>();
    }


    public static class SqliteExtensions
    {
        public static SqliteDataReader TryExecuteReader(this SqliteCommand sqliteCommand)
        {
            try
            {
                return sqliteCommand.ExecuteReader();
            }
            catch
            {
                return null;
            }
        }


        public static void CreateDBFile(string path)
        {

            using (SqliteConnection db =
            new SqliteConnection($"Filename={path}"))
            {
                db.Open();
                db.Close();
            }
        }


        public static List<Column> Select(this SqliteConnection db, string selectCommand)
        {
            db.Open();

            SqliteCommand cmd = new SqliteCommand();
            cmd.Connection = db;


            cmd.CommandText = selectCommand;


            List<Column> columns = new List<Column>();
            using (var reader = cmd.TryExecuteReader())
            {
                if (reader != null)
                {

                    for (int i = 0; i < reader.FieldCount /* Current number of columns.*/; i++)
                    {
                        Column col = new Column();
                        columns.Add(col);
                    }


                    while (reader.Read())
                    {
                        for (int i = 0; i < reader.FieldCount /* Current number of columns.*/; i++)
                        {
                            columns[i].values.Add(reader.GetValue(i));
                            columns[i].name = reader.GetName(i);
                        }
                    }


                }

            }
            db.Close();

            return columns;
        }

        public static void Insert(this SqliteConnection db, string Tabelname, string fieldName, params string[] values)
        {
            db.Open();

            SqliteCommand cmd = new SqliteCommand();
            cmd.Connection = db;
            string commandStr = $"INSERT INTO {Tabelname}({fieldName}) VALUES("; // Insert into a specific field
            // also posible: INSERT INTO {Tabelname} VALUES(1,2,3,..)";
             

            foreach (string value in values)
            {
                commandStr += "\"" + value + "\",";
            }

            commandStr = commandStr.Remove(commandStr.Length - 1) + ");";
            cmd.CommandText = commandStr;

            cmd.TryExecuteReader();

            db.Close();
        }

        public static void NewColumn(this SqliteConnection db, string tabelName, string newColumnName, FieldTyp fieldTyp)
        {
            db.Open();

            SqliteCommand cmd = new SqliteCommand();
            cmd.Connection = db;

            cmd.CommandText = @"ALTER TABLE "+ tabelName + " ADD COLUMN "+ newColumnName + " "+ fieldTyp.ToString() +";";

            cmd.TryExecuteReader();

            db.Close();
        }


        public static void CreateTable(this SqliteConnection db, string Tabelname, params SqliteField[] fields)
        {
            if (fields.Length == 0)
                throw new ArgumentException("A table need at least 1 field.");

            db.Open();

            SqliteCommand cmd = new SqliteCommand();
            cmd.Connection = db;
            string commandStr = $"CREATE TABLE \"{Tabelname}\" (";

            foreach (SqliteField field in fields)
            {
                commandStr += "\"" + field.name + "\" " + field.fieldType.ToString() + ",";
            }

            commandStr = commandStr.Remove(commandStr.Length - 1) + ");";
            cmd.CommandText = commandStr;

            cmd.TryExecuteReader();

            db.Close();
        }

    }
}
