using System.Data;

using Terminal.Gui;

namespace Surfree.Host.Views.ResponseViews;

public class ResponseInfoFrame : FrameView
{
    private DataTable _dt;

    public ResponseViewModel ViewModel { get; }

    public ResponseInfoFrame(ResponseViewModel viewModel)
    {
        ViewModel = viewModel;

        Title = "Info";
        BorderStyle = LineStyle.None;
        Height = Dim.Fill();
        Width = Dim.Fill();
        
        _dt = CreateInfoTable();
        
        var infoTableView = new TableView
        {
            Width = Dim.Fill(),
            Height = Dim.Fill(),
            Table = new DataTableSource(_dt),
            CellActivationKey = KeyCode.Enter,
        };
        infoTableView.Style.ShowHorizontalBottomline = true;
        infoTableView.Style.ShowHorizontalScrollIndicators = false;
        infoTableView.BorderStyle = LineStyle.None;
        Add(infoTableView);
        
        viewModel.PropertyChanged += (sender, e) =>
        {
            if (e.PropertyName == nameof(ResponseViewModel.Info))
            {
                FillTable(viewModel.Info);
            }
        };
    }
    
    private DataTable CreateInfoTable()
    {
        var dt = new DataTable();
        dt.Columns.Add("Name");
        dt.Columns.Add("Value");

        return dt;
    }

    private DataTable FillTable(Dictionary<string, string> info)
    {
        if (_dt.Rows.Count != 0)
            _dt.Rows.Clear();

        if (info.Any() == true)
        {
            foreach (var (key, value) in info)
            {
                _dt.Rows.Add(key, value);
            }

            _dt.AcceptChanges();
        }

        return _dt;
    }
}
