Public Class EntitiesManagerControl
    Property SelectedEntity As Object

    Property ObjectsSource As Object

    Shared ReadOnly SourceProperty As DependencyProperty =
        DependencyProperty.Register(NameOf(ObjectsSource),
                                    GetType(Object),
                                    GetType(EntitiesManagerControl),
                                    New PropertyMetadata(Nothing))
End Class
