using CommunityToolkit.Mvvm.ComponentModel;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Surfree.Host;

public partial class ThemeConfig : ObservableObject
{
    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(Theme))]
    public partial string ThemeName { get; set; } = "aurora";

    public Theme Theme => BuiltInThemes.Themes[ThemeName];
}

public class TextAreaTheme
{
    // The style to apply to the gutter
    public string? Gutter { get; set; }

    // The style to apply to the cursor
    public string? Cursor { get; set; }

    // The style to apply to the line the cursor is on
    public string? CursorLine { get; set; }

    // The style to apply to the gutter of the line the cursor is on
    public string? CursorLineGutter { get; set; }

    // The style to apply to bracket matching
    public string? MatchedBracket { get; set; }

    // The style to apply to the selected text
    public string? Selection { get; set; }
}

public class VariableStyles : Dictionary<string, string>
{
    public string? Resolved { get; set; }
    public string? Unresolved { get; set; }

    public VariableStyles FillWithDefaults(Theme theme)
    {
        return new VariableStyles
        {
            Resolved = this.Resolved ?? theme.Success,
            Unresolved = this.Unresolved ?? theme.Error
        };
    }
}

public class UrlStyles
{
    // The style to apply to the base of the URL
    public string? Base { get; set; }

    // The style to apply to the URL protocol
    public string? Protocol { get; set; }

    // The style to apply to URL separators e.g. `/`.
    public string? Separator { get; set; } = "dim";

    public UrlStyles FillWithDefaults(Theme theme)
    {
        return new UrlStyles
        {
            Base = this.Base ?? theme.Secondary,
            Protocol = this.Protocol ?? theme.Accent,
            Separator = this.Separator ?? "dim"
        };
    }
}

// The style to apply to HTTP methods in the sidebar
public class MethodStyles
{
    public string Get { get; set; } = "#0ea5e9";
    public string Post { get; set; } = "#22c55e";
    public string Put { get; set; } = "#f59e0b";
    public string Delete { get; set; } = "#ef4444";
    public string Patch { get; set; } = "#14b8a6";
    public string Options { get; set; } = "#8b5cf6";
    public string Head { get; set; } = "#d946ef";
}

public class Theme
{
    public required string Name { get; set; }
    public required string Primary { get; set; }
    public string? Secondary { get; set; }
    public string? Background { get; set; }
    public string? Surface { get; set; }
    public string? Panel { get; set; }
    public string? Warning { get; set; }
    public string? Error { get; set; }
    public string? Success { get; set; }
    public string? Accent { get; set; }
    public bool Dark { get; set; } = true;

    public TextAreaTheme TextArea { get; set; } = new TextAreaTheme();
    public UrlStyles? Url { get; set; } = new UrlStyles();
    public VariableStyles? Variable { get; set; } = new VariableStyles();
    public MethodStyles? Method { get; set; } = new MethodStyles();
}

// copied from Posting.sh
public static class BuiltInThemes
{
    public static readonly Dictionary<string, Theme> Themes = new()
    {

        ["galaxy"] = new Theme
        {
            Name = "galaxy",
            Primary = "#C45AFF",
            Secondary = "#a684e8",
            Warning = "#FFD700",
            Error = "#FF4500",
            Success = "#00FA9A",
            Accent = "#FF69B4",
            Background = "#0F0F1F",
            Surface = "#1E1E3F",
            Panel = "#2D2B55",
            Dark = true,
            Variable = new VariableStyles
            {
                ["input-cursor-background"] = "#C45AFF",
                ["footer-background"] = "transparent"
            }
        },

        ["nebula"] = new Theme
        {
            Name = "nebula",
            Primary = "#4A9CFF",
            Secondary = "#66D9EF",
            Warning = "#FFB454",
            Error = "#FF5555",
            Success = "#50FA7B",
            Accent = "#FF79C6",
            Background = "#0D2137",
            Surface = "#193549",
            Panel = "#1F4662",
            Dark = true,
            Variable = new VariableStyles
            {
                ["input-selection-background"] = "#4A9CFF 35%",
            }
        },

        ["sunset"] = new Theme
        {
            Name = "sunset",
            Primary = "#FF7E5F",
            Secondary = "#FEB47B",
            Warning = "#FFD93D",
            Error = "#FF5757",
            Success = "#98D8AA",
            Accent = "#B983FF",
            Background = "#2B2139",
            Surface = "#362C47",
            Panel = "#413555",
            Dark = true,
            Variable = new VariableStyles
            {
                ["input-cursor-background"] = "#FF7E5F",
                ["input-selection-background"] = "#FF7E5 35%",
                ["footer-background"] = "transparent",
                ["button-color-foreground"] = "#2B2139",
                ["method-get"] = "#FF7E5F",
            },
        },

        ["aurora"] = new Theme
        {
            Name = "aurora",
            Primary = "#45FFB3",
            Secondary = "#A1FCDF",
            Accent = "#DF7BFF",
            Warning = "#FFE156",
            Error = "#FF6B6B",
            Success = "#64FFDA",
            Background = "#0A1A2F",
            Surface = "#142942",
            Panel = "#1E3655",
            Dark = true,
            Variable = new VariableStyles
            {
                ["input-cursor-background"] = "#45FFB3",
                ["input-selection-background"] = "#45FFB3 35%",
                ["footer-background"] = "transparent",
                ["button-color-foreground"] = "#0A1A2F",
                ["method-post"] = "#DF7BFF"
            },
        },
        
        ["nautilus"] = new Theme
        {
            Name = "nautilus",
            Primary = "#0077BE",
            Secondary = "#20B2AA",
            Warning = "#FFD700",
            Error = "#FF6347",
            Success = "#32CD32",
            Accent = "#FF8C00",
            Background = "#001F3F",
            Surface = "#003366",
            Panel = "#005A8C",
            Dark = true,
        },
        
        ["cobalt"] = new Theme
        {
            Name = "cobalt",
            Primary = "#334D5C",
            Secondary = "#66B2FF",
            Warning = "#FFAA22",
            Error = "#E63946",
            Success = "#4CAF50",
            Accent = "#D94E64",
            Surface = "#27343B",
            Panel = "#2D3E46",
            Background = "#1F262A",
            Dark = true,
            Variable = new VariableStyles
            {
                ["input-selection-background"] = "#4A9CFF 35%",
            }
        },
        
        ["twilight"] = new Theme
        {
            Name = "twilight",
            Primary = "#367588",
            Secondary = "#5F9EA0",
            Warning = "#FFD700",
            Error = "#FF6347",
            Success = "#00FA9A",
            Accent = "#FF7F50",
            Background = "#191970",
            Surface = "#3B3B6D",
            Panel = "#4C516D",
            Dark = true,
        },
        
        ["hacker"] = new Theme
        {
            Name = "hacker",
            Primary = "#00FF00",
            Secondary = "#3A9F3A",
            Warning = "#00FF66",
            Error = "#FF0000",
            Success = "#00DD00",
            Accent = "#00FF33",
            Background = "#000000",
            Surface = "#0A0A0A",
            Panel = "#111111",
            Dark = true,
            Variable = new VariableStyles
            {
                ["method-get"] = "#00FF00",
                ["method-post"] = "#00DD00",
                ["method-put"] = "#00BB00",
                ["method-delete"] = "#FF0000",
                ["method-patch"] = "#00FF33",
                ["method-options"] = "#3A9F3A",
                ["method-head"] = "#00FF66",
            }
        },
        
        ["manuscript"] = new Theme
        {
            Name = "manuscript",
            Primary = "#2C4251",
            Secondary = "#6B4423",
            Accent = "#8B4513",
            Warning = "#B4846C",
            Error = "#A94442",
            Success = "#2D5A27",
            Background = "#F5F1E9",
            Surface = "#EBE6D9",
            Panel = "#E0DAC8",
            Dark = false,
            Variable = new VariableStyles
            {
                ["input-cursor-background"] = "#2C4251",
                ["input-selection-background"] = "#2C4251 25%",
                ["footer-background"] = "#2C4251",
                ["footer-key-foreground"] = "#F5F1E9",
                ["footer-description-foreground"] = "#F5F1E9",
                ["button-color-foreground"] = "#F5F1E9",
                ["method-get"] = "#2C4251",
                ["method-post"] = "#2D5A27",
                ["method-put"] = "#6B4423",
                ["method-delete"] = "#A94442",
                ["method-patch"] = "#8B4513",
                ["method-options"] = "#4A4A4A",
                ["method-head"] = "#5C5C5C",
            }
        }
    };
}