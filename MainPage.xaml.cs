
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.Runtime.CompilerServices;

namespace MauiWithMSSqlite;

public partial class MainPage : ContentPage
{

    public MainPage()
    {
        InitializeComponent();
    }


    public int MyProperty { get; set; }

}

