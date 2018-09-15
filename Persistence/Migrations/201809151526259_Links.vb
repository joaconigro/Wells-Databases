Imports System
Imports System.Data.Entity.Migrations
Imports Microsoft.VisualBasic

Namespace Migrations
    Public Partial Class Links
        Inherits DbMigration
    
        Public Overrides Sub Up()
            CreateTable(
                "dbo.ExternalLinks",
                Function(c) New With
                    {
                        .Id = c.String(nullable := False, maxLength := 128),
                        .Link = c.String(),
                        .Well_Id = c.String(maxLength := 128)
                    }) _
                .PrimaryKey(Function(t) t.Id) _
                .ForeignKey("dbo.Wells", Function(t) t.Well_Id) _
                .Index(Function(t) t.Well_Id)
            
        End Sub
        
        Public Overrides Sub Down()
            DropForeignKey("dbo.ExternalLinks", "Well_Id", "dbo.Wells")
            DropIndex("dbo.ExternalLinks", New String() { "Well_Id" })
            DropTable("dbo.ExternalLinks")
        End Sub
    End Class
End Namespace
