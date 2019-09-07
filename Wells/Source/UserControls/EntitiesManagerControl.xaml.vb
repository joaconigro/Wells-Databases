Imports Wells.ViewBase

Public Class EntitiesManagerControl
    Property SelectedEntity As Object
        Get
            Return _SelectedEntity
        End Get
        Set
            _SelectedEntity = Value
        End Set
    End Property

    Property ObjectsSource As Object
        Get
            Return _ObjectsSource
        End Get
        Set
            _ObjectsSource = Value
        End Set
    End Property

    Shared ReadOnly ObjectsSourceProperty As DependencyProperty =
        DependencyProperty.Register(NameOf(ObjectsSource),
                                    GetType(Object),
                                    GetType(EntitiesManagerControl),
                                    New PropertyMetadata(Nothing))

    Property NewEntityCommand As ICommand
    Shared ReadOnly NewEntityCommandProperty As DependencyProperty =
        DependencyProperty.Register(NameOf(NewEntityCommand),
                                    GetType(ICommand),
                                    GetType(EntitiesManagerControl),
                                    New PropertyMetadata(New RelayCommand(Sub()

                                                                          End Sub)))


    Property EditEntityCommand As ICommand
    Shared ReadOnly EditEntityCommandProperty As DependencyProperty =
        DependencyProperty.Register(NameOf(EditEntityCommand),
                                    GetType(ICommand),
                                    GetType(EntitiesManagerControl),
                                    New PropertyMetadata(New RelayCommand(Sub()

                                                                          End Sub)))

    Property DeleteEntityCommand As ICommand
    Shared ReadOnly DeleteEntityCommandProperty As DependencyProperty =
        DependencyProperty.Register(NameOf(DeleteEntityCommand),
                                    GetType(ICommand),
                                    GetType(EntitiesManagerControl),
                                    New PropertyMetadata(New RelayCommand(Sub()

                                                                          End Sub)))

    Private _SelectedEntity As Object
    Private _ObjectsSource As Object
End Class
