using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Terminal.Gui;

namespace Surfree.Host.Views;

internal class CollectionsView : FrameView
{
    public CollectionsView()
    {
        Title = "Collections";
        Width = 35; // Set a fixed width for the collections view
        Height = Dim.Fill();

        var tree = new TreeView()
        {
            Width = 30, // Set a fixed width for the tree view
            Height = Dim.Fill()
        };

        Add(tree);



    }
}

class Collection
{
    public string Name { get; set; }
    public List<Endpoint> Endpoints { get; set; }
}

class Endpoint
{
    public string Path { get; set; }

    public List<Method> Methods { get; set; }
}

class Method
{
    public HttpMethod HttpMethod { get; set; }
}

