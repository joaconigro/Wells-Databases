Imports System
Imports System.Data.Entity.Migrations
Imports Microsoft.VisualBasic

Namespace Migrations
    Public Partial Class NewWellParameters
        Inherits DbMigration
    
        Public Overrides Sub Up()
            AddColumn("dbo.Wells", "Height", Function(c) c.Double(nullable := False))
            AddColumn("dbo.Wells", "Exists", Function(c) c.Boolean(nullable := False))
        End Sub
        
        Public Overrides Sub Down()
            DropColumn("dbo.Wells", "Exists")
            DropColumn("dbo.Wells", "Height")
        End Sub
    End Class
End Namespace
