using CommunityToolkit.Mvvm.ComponentModel;

namespace Examar.Utils;

public partial class ObservableKeyValuePair<TKey, TValue> : ObservableObject
{
    [ObservableProperty] 
    private TKey _key;

    [ObservableProperty] 
    private TValue _value;

    public ObservableKeyValuePair(TKey key, TValue value)
    {
        Key = key;
        Value = value;
    }
}
