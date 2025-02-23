using Surfree.Host.ViewModels;

using System.Data;

using Terminal.Gui;

namespace Surfree.Host.Views.RequestViews;

public sealed class RequestQueryFrame : FrameView
{
    private DataTable _dt;

    public RequestQueryFrame(RequestViewModel viewModel, bool allowEdit = false)
    {
        Title = "Query";
        BorderStyle = LineStyle.None;
        Height = Dim.Fill();
        Width = Dim.Fill(1);
        X = 1;

        _dt = CreateQueryTable();
        var queryTableView = new TableView
        {
            X = 0,
            Y = 0,
            Width = Dim.Fill(),
            Height = Dim.Fill(2),
            Table = new DataTableSource(_dt),
            CellActivationKey = KeyCode.Enter,
        };
        queryTableView.Style.ShowHorizontalBottomline = true;
        queryTableView.Style.ShowHorizontalScrollIndicators = false;
        queryTableView.BorderStyle = LineStyle.None;

        Add(queryTableView);

        if (allowEdit)
        {
            var addQueryButton = new Button()
            {
                X = 0,
                Y = Pos.Bottom(queryTableView) + 1,
                Text = "Add Query"
            };

            addQueryButton.Accept += (sender, e) =>
            {
                var addDialog = new AddQueryDialog(_dt);
                Application.Run(addDialog);
            };

            Add(addQueryButton);

            queryTableView.CellActivated += (sender, e) =>
            {
                var editDialog = new EditQueryDialog(_dt, e.Row);
                Application.Run(editDialog);
            };
        }

        ViewModel = viewModel;
    }

    public RequestViewModel ViewModel { get; }

    private DataTable CreateQueryTable()
    {
        var tbl = new DataTable();
        var nameColumn = new DataColumn
        {
            DataType = typeof(string),
            ColumnName = "Name",
            Unique = true,
            Caption = "Name",
        };
        tbl.Columns.Add(nameColumn);
        tbl.Columns.Add(new DataColumn
        {
            DataType = typeof(string),
            ColumnName = "Value",
            Unique = false,
            Caption = "Value",
        });
        tbl.PrimaryKey = new[] { nameColumn };
        return tbl;
    }
}
