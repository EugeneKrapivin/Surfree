using System.Data;

using Terminal.Gui;

namespace Surfree.Host.Views.ResponseViews;

public class ResponseCookiesFrame : FrameView
{
    private DataTable _dt;

    public ResponseCookiesFrame(ResponseViewModel viewModel)
    {
        ViewModel = viewModel;
        
        Title = "Cookies";
        BorderStyle = LineStyle.None;
        Height = Dim.Fill();
        Width = Dim.Fill();
        
        _dt = CreateCookiesTable();
        var cookiesTableView = new TableView
        {
            Width = Dim.Fill(),
            Height = Dim.Fill(),
            Table = new DataTableSource(_dt),
            CellActivationKey = KeyCode.Enter,
        };
        
        cookiesTableView.Style.ShowHorizontalBottomline = true;
        cookiesTableView.Style.ShowHorizontalScrollIndicators = false;
        cookiesTableView.BorderStyle = LineStyle.None;
        Add(cookiesTableView);

        viewModel.PropertyChanged += (sender, e) =>
        {
            if (e.PropertyName == nameof(ResponseViewModel.Cookies))
            {
                FillTable(viewModel.Cookies);
            }
        };
    }

    public ResponseViewModel ViewModel { get; }

    private DataTable CreateCookiesTable()
    {
        var dt = new DataTable();
        dt.Columns.Add("Name");
        dt.Columns.Add("Value");

        return dt;
    }

    private DataTable FillTable(Dictionary<string, string> cookies)
    {
        if (_dt.Rows.Count != 0)
            _dt.Rows.Clear();

        if (cookies.Any() == true)
        {
            foreach (var (key, value) in cookies)
            {
                _dt.Rows.Add(key, value);
            }

            _dt.AcceptChanges();
        }

        return _dt;
    }
}
