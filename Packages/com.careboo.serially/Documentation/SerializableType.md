SerializableType
================

![SerializableTypeDrawer](images/SerializableTypeDrawer.png)

`SerializableType` can be used to Serialize a `System.Type` reference. The UI is very similar to the [ShowSerializeReference](ShowSerializeReference.md) attribute.

Using a `TypeFilter` attribute, you can filter the shown types by a base type, or a custom filter delegate.

```cs
using CareBoo.Serially;

public interface IPet {}
public class Cat : IPet {}
public class Dog : IPet {}

public class MyBehavior : MonoBehaviour
{
    [TypeFilter(derivedFrom: typeof(IPet))] // only show types derived from IPet
    public SerializableType SomePetType;

    public bool MyCustomFilter(Type type)
    {
        var acceptedTypes = new[] { typeof(int), SomePetType.Type };
        return acceptedTypes.Contains(type);
    }

    [TypeFilter(nameof(MyCustomFilter))] // must be a Func<Type, bool>
    public SerializableType SelectedSomePetTypeOrInt;
}
```

The delegate given to `TypeFilter` must be a `Func<Type, bool>` to work.

![TypePickerWindow](images/TypePickerWindow.png)

Similar to the [ShowSerializeReference](ShowSerializeReference.md) attribute, clicking the button to the side opens up a type list window.
