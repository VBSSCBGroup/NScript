Imports System.Drawing
Imports System.Runtime.InteropServices
Namespace NScript.Drawing
    <ComClass()> <ComVisible(True)> Public Class CPT
        Private _Value As New Point
        Public Sub New()

        End Sub
        Public Sub New(value As Point)
            Me._Value = value
        End Sub
        Public Property X As Int32
            Get
                Return _Value.X
            End Get
            Set(value As Int32)
                _Value.X = value
            End Set
        End Property
        Public Property Y As Int32
            Get
                Return _Value.Y
            End Get
            Set(value As Int32)
                _Value.Y = value
            End Set
        End Property
        Public Shared Widening Operator CType(point As CPT) As Point
            Return If(point Is Nothing, New Point(), point._Value)
        End Operator
        Public Shared Widening Operator CType(point As Point) As CPT
            Return New CPT() With {._Value = point}
        End Operator

        Public Function ToPoint() As Point
            Return Me._Value
        End Function

        Public Function ToCPT() As CPT
            Return Me
        End Function
    End Class
End Namespace
