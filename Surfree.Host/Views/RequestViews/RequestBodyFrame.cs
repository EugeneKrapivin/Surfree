using Surfree.Host.ViewModels;

using System.Collections.ObjectModel;
using System.Data;

using Terminal.Gui;

namespace Surfree.Host.Views.RequestViews;

public sealed class RequestBodyFrame : FrameView
{
    private Label _contentTypeLabel;
    private ComboBox _contentTypeCombo;
    private TextView _jsonTextView;
    private TextView _xmlTextView;
    private TableView _formTableView;

    public RequestBodyFrame(RequestViewModel viewModel)
    {
        ViewModel = viewModel;
        
        InitComponent();

        _contentTypeCombo.SelectedItemChanged += (sender, e) =>
        {
            if (_contentTypeCombo.Text == "application/json")
            {
                _jsonTextView.Visible = true;
                _formTableView.Visible = false;
                _xmlTextView.Visible = false;
                _jsonTextView.TextChanged += (s, args) => viewModel.Body = _jsonTextView.Text;
                viewModel.ContentType = "application/json";

            }
            else if (_contentTypeCombo.Text == "application/x-www-form-urlencoded")
            {
                _jsonTextView.Visible = false;
                _formTableView.Visible = true;
                _xmlTextView.Visible = false;
                viewModel.ContentType = "application/x-www-form-urlencoded";
            }
            else
            {
                _xmlTextView.Visible = true;
                _jsonTextView.Visible = false;
                _formTableView.Visible = false;
                viewModel.ContentType = "application/xml";
            }
        };
    }

    private void InitComponent()
    {
        Title = "Body";
        BorderStyle = LineStyle.None;
        CanFocus = false;
        Height = Dim.Fill();
        Width = Dim.Fill(1);
        X = 1;

        _contentTypeLabel = new Label()
        {
            X = 1,
            Y = 0,
            Width = Dim.Auto(),
            Height = 1,
            Text = "Content-Type:"
        };

        _contentTypeCombo = new ComboBox()
        {
            X = Pos.Right(_contentTypeLabel) + 1,
            Y = 0,
            Width = 40,
            Height = 4,
            ReadOnly = true,
            CanFocus = false,
            HideDropdownListOnClick = true,
        };
        var source = new ObservableCollection<string>
        {
            "application/json",
            "application/x-www-form-urlencoded",
            "application/xml"
        };
        _contentTypeCombo.SetSource(source);

        _jsonTextView = new TextView
        {
            Visible = true,
            Text = """
            { 
            }
            """,
            Width = Dim.Fill(1),
            Height = Dim.Fill(1),
            X = 1,
            Y = Pos.Bottom(_contentTypeCombo),
        };

        _xmlTextView = new TextView
        {
            Visible = false,
            Text = """
            <root> 
            </root>
            """,
            Width = Dim.Fill(1),
            Height = Dim.Fill(1),
            X = 1,
            Y = Pos.Bottom(_contentTypeCombo),
        };

        _formTableView = new TableView
        {
            Visible = false,
            Width = Dim.Fill(1),
            Height = Dim.Fill(1),
            X = 1,
            Y = Pos.Bottom(_contentTypeCombo),
            Table = new DataTableSource(new DataTable() { Columns = { "Name", "Value" } }),
        };

        _formTableView.Style.ShowHorizontalBottomline = true;
        _formTableView.Style.ShowHorizontalScrollIndicators = false;
        _formTableView.BorderStyle = LineStyle.Rounded;

        Add(_formTableView, _jsonTextView, _xmlTextView, _contentTypeLabel, _contentTypeCombo);

    }

    public RequestViewModel ViewModel { get; }
}
