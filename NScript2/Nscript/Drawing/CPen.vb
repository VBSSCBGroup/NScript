Imports System.Drawing
Imports System.Runtime.InteropServices
Namespace NScript.Drawing
    <ComVisible(True)> <ComClass> Public Class CPen
        Public Const NullPen = Nothing
        Public Property BasePen As Pen
        Public Sub New()
            BasePen = Pens.Black
        End Sub
        Public Property Color As Int32
            Get
                Return ColorTranslator.ToWin32(BasePen.Color)
            End Get
            Set(value As Int32)
                BasePen.Color = System.Drawing.Color.FromArgb(value)
            End Set
        End Property
        Public Property Width As Single
            Get
                Return BasePen.Width
            End Get
            Set(value As Single)
                BasePen.Width = value
            End Set
        End Property

    End Class
End Namespace
