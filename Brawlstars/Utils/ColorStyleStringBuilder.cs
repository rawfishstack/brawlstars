#nullable enable
using System.Text;

namespace brawlstars.Brawlstars.Utils;

public class ColorStyleStringBuilder {
    private StringBuilder _stringBuilder = new();

    private string? _currentColor;

    public string? CurrentColor {
        get => _currentColor;
        set {
            var oldColor = _currentColor;
            _currentColor = value;
            if (_currentColor == null) {
                if (oldColor != null) {
                    Close();
                }
            } else {
                if (oldColor == null) {
                    Open();
                } else {
                    if (_currentColor != oldColor) {
                        Close();
                        Open();
                    }
                }
            }
        }
    }

    private void Open() {
        _stringBuilder.Append($"[c/{_currentColor}:");
    }

    private void Close() {
        _stringBuilder.Append(']');
    }


    public ColorStyleStringBuilder Color(string? color) {
        CurrentColor = color;
        return this;
    }

    public ColorStyleStringBuilder Append(string str) {
        _stringBuilder.Append(str);
        return this;
    }

    public ColorStyleStringBuilder Clear() {
        _stringBuilder.Clear();
        return this;
    }

    public override string ToString() {
        return Color(null)._stringBuilder.ToString();
    }
}