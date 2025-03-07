﻿using Mediator;

using Surfree.Host.ViewModels;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Terminal.Gui;

namespace Surfree.Host.Views.RequestViews;

public class RequestFrame : FrameView
{
    public RequestFrame(RequestUrlFrame urlView, RequestViewModel viewModel, ThemeConfig themeConfig)
    {
        Title = "Request";
        BorderStyle = LineStyle.Rounded;

        Height = Dim.Fill();
        Width = Dim.Fill();
        Enabled = true;
        CanFocus = true;

        var theme = themeConfig.Theme;

        ColorScheme = new ColorScheme
        {
            Normal = new Terminal.Gui.Attribute(Color.Parse(theme.Secondary), Color.Parse(theme.Background)),
            Focus = new Terminal.Gui.Attribute(Color.Parse(theme.Primary), Color.Parse(theme.Background))
        };

        themeConfig.PropertyChanged += (sender, args) =>
        {
            if (args.PropertyName == nameof(ThemeConfig.Theme))
            {
                theme = themeConfig.Theme;
                ColorScheme = new ColorScheme
                {
                    Normal = new Terminal.Gui.Attribute(Color.Parse(theme.Secondary), Color.Parse(theme.Background)),
                    Focus = new Terminal.Gui.Attribute(Color.Parse(theme.Primary), Color.Parse(theme.Background))
                };
            }
        };

        urlView.X = 1;
        urlView.Y = 0;
        urlView.Width = Dim.Fill(1);
        urlView.BorderStyle = LineStyle.None;

        Add(urlView);
        
        var tabView = new TabView()
        {
            X = 1,
            Y = Pos.Bottom(urlView),
            Width = Dim.Fill(1),
            Height = Dim.Fill(),
            BorderStyle = LineStyle.None
        };
        Add(tabView);
        var headersTab = new Tab()
        {
            DisplayText = "Headers",
            X = 0,
            Y = 0,
            Height = Dim.Fill(),
            Width = Dim.Fill(),
            View = new RequestHeadersFrame(viewModel, themeConfig)
        };

        var bodyTab = new Tab()
        {
            DisplayText = "Body",
            X = 1,
            Y = 0,
            Height = Dim.Fill(),
            Width = Dim.Fill(1),
            View = new RequestBodyFrame(viewModel, themeConfig)
        };
        var query = new Tab()
        {
            DisplayText = "Query",
            X = 1,
            Y = 0,
            Height = Dim.Fill(),
            Width = Dim.Fill(1),
            View = new RequestQueryFrame(viewModel, true)
        };
        var auth = new Tab() { DisplayText = "Auth" };

        tabView.AddTab(headersTab, true);
        tabView.AddTab(bodyTab, false);
        tabView.AddTab(query, false);
        tabView.AddTab(auth, false);
    }
}