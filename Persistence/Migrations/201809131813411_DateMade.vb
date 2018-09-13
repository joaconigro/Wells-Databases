Imports System
Imports System.Data.Entity.Migrations
Imports Microsoft.VisualBasic

Namespace Migrations
    Public Partial Class DateMade
        Inherits DbMigration
    
        Public Overrides Sub Up()
            AlterColumn("dbo.ChemicalAnalysis", "SampleDate", Function(c) c.String())
            AlterColumn("dbo.Wells", "DateMade", Function(c) c.String())
            AlterColumn("dbo.Measurements", "SampleDate", Function(c) c.String())
            AlterColumn("dbo.Precipitations", "PrecipitationDate", Function(c) c.String())
        End Sub
        
        Public Overrides Sub Down()
            AlterColumn("dbo.Precipitations", "PrecipitationDate", Function(c) c.DateTime(nullable := False))
            AlterColumn("dbo.Measurements", "SampleDate", Function(c) c.DateTime(nullable := False))
            AlterColumn("dbo.Wells", "DateMade", Function(c) c.DateTime(nullable := False))
            AlterColumn("dbo.ChemicalAnalysis", "SampleDate", Function(c) c.DateTime(nullable := False))
        End Sub
    End Class
End Namespace
