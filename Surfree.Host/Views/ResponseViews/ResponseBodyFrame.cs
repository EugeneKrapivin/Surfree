using System.Data;

using Terminal.Gui;

namespace Surfree.Host.Views.ResponseViews;

public sealed class ResponseBodyFrame : FrameView
{
    private Label _contentTypeLabel;
    private View _contentView;

    public ResponseViewModel ViewModel { get; }

    public ResponseBodyFrame(ResponseViewModel viewModel)
    {
        Title = "Body";
        BorderStyle = LineStyle.None;
        CanFocus = false;
        Height = Dim.Fill();
        Width = Dim.Fill(1);
        X = 1;

        var headingLabel = new Label()
        {
            X = 1,
            Y = 0,
            Width = Dim.Auto(),
            Height = 1,
            Text = $"Content-Type: "
        };

        _contentTypeLabel = new Label()
        {
            X = Pos.Right(headingLabel) + 1,
            Y = 0,
            Width = 40,
            Height = 1,
            ColorScheme = new ColorScheme() { Normal = new Terminal.Gui.Attribute(ColorName.White, ColorName.Blue) }
        };

        _contentView = GetContentView();
        _contentView.Width = Dim.Fill(1);
        _contentView.Height = Dim.Fill(1);
        _contentView.X = 1;
        _contentView.Y = Pos.Bottom(_contentTypeLabel);

        Add(headingLabel, _contentView, _contentTypeLabel);
        ViewModel = viewModel;

        viewModel.PropertyChanged += (sender, args) =>
        {
            if (args.PropertyName == nameof(ResponseViewModel.Body))
            {
                _contentTypeLabel.Text = viewModel.Headers.TryGetValue("content-type", out var headerValue) ? headerValue.HeaderValue : string.Empty;
                SetViewModel(viewModel);
            }
        };
    }

    public void SetViewModel(ResponseViewModel model)
    {
        Remove(_contentView);
        
        var newView = GetContentView(model);
        newView.Width = Dim.Fill(1);
        newView.Height = Dim.Fill(1);
        newView.X = 1;
        newView.Y = Pos.Bottom(_contentTypeLabel);
        this.Add(newView);
        _contentView = newView;
        

    }

    private View GetContentView(ResponseViewModel? model = null)
    {
        View Default() => new TextView { ReadOnly = true, ColorScheme = new ColorScheme(new Terminal.Gui.Attribute(ColorName.Black, ColorName.DarkGray)) };
        if (model is null || model.Headers.TryGetValue("content-type", out var type))
            return Default();

        return (type?.HeaderValue) switch
        {
            "application/json" => new TextView { Text = model.Body, ReadOnly = true },
            "application/xml" => new TextView { Text = model.Body, ReadOnly = true },
            "application/x-www-form-urlencoded" => new TableView
            {
                Table = new DataTableSource(new DataTable() { Columns = { "Name", "Value" } }),
                Style = new TableStyle()
                {
                    ShowHorizontalBottomline = true,
                    ShowHorizontalScrollIndicators = false,
                },
                BorderStyle = LineStyle.None
            },
            _ => new TextView { Text = model.Body, ReadOnly = true }
        };
    }
}
