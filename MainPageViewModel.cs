using Microsoft.Data.Sqlite;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace MauiWithMSSqlite
{
    public class MainPageViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public string dbfile { get; set; } = FileSystem.Current.AppDataDirectory + nameof(MauiWithMSSqlite) + ".db";
        
        string _tableNameEntry = "Test";
        
        public string TableNameEntry
        {
            get
            {
                return _tableNameEntry;
            }
        
            set
            {
                _tableNameEntry = value;
                OnPropertyChanged();
            }
        }
        
        string _writeDataEntry = "Write Data...";
        
        public string WriteDataEntry
        {
            get
            {
                return _writeDataEntry;
            }

            set
            {
                _writeDataEntry = value;
                OnPropertyChanged();
            }
        }




        private string _LogEditor = string.Empty;
        public string LogEditor
        {
            get
            {
                return _LogEditor;
            }
            set
            {
                _LogEditor = value;
                OnPropertyChanged();
            }
        }


        public Command CreateDBCommand => new Command(() => 
            {
                SqliteExtensions.CreateDBFile(dbfile);
            });


        public Command CreateTableCommand => new Command(() =>
        {
            using (SqliteConnection db =
            new SqliteConnection($"Filename={dbfile}"))
            {
                db.CreateTable(TableNameEntry, new SqliteField() { name = FieldEntry, fieldType = FieldTyp.TEXT });
            }
        }
        );


        string _fieldEntry = "TestField";
        
        public string FieldEntry
        {
            get
            {
                return _fieldEntry;
            }
        
            set
            {
                _fieldEntry = value;
                OnPropertyChanged();
            }
        }

        public Command CreateFieldCommand => new Command(() =>
        {
            using (SqliteConnection db =
            new SqliteConnection($"Filename={dbfile}"))
            {
                db.NewColumn(TableNameEntry, FieldEntry, FieldTyp.TEXT );
            }
        });


        public Command WriteDataCommand => new Command(() =>
        {
            using (SqliteConnection db =
            new SqliteConnection($"Filename={dbfile}"))
            {
                db.Insert(TableNameEntry, FieldEntry, WriteDataEntry);
            }
        }
);


        public Command ReadDataCommand => new Command(() =>
        {
            LogEditor = "";
            using (SqliteConnection db = new SqliteConnection($"Filename={dbfile}"))
            {
                foreach (Column column in db.Select("SELECT * FROM " + TableNameEntry))
                {
                    LogEditor  += "------- Column:" + column.name + "\n";

                    foreach (object v in column.values)
                    {
                        LogEditor += v.ToString() + "\n";
                    }
                }
            }
        }
);


    }
}

