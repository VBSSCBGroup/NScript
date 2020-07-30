Imports System.IO
Imports System.Runtime.InteropServices
Namespace NScript.Drawing
    <ComClass> <ComVisible(True)> Public Class CBitmap
        Private _value As System.Drawing.Bitmap
        Public ReadOnly Property Bitmap As System.Drawing.Bitmap
            Get
                Return _value
            End Get
        End Property
        Public Sub LoadByFile(path As String)
            _value = New System.Drawing.Bitmap(path)
        End Sub
        Public Sub LoadByBytes(<MarshalAs(UnmanagedType.SafeArray)> Bytes As Byte())
            _value = New System.Drawing.Bitmap(New MemoryStream(Bytes))
        End Sub
        Public Sub SetPixel(x As Int32, y As Int32, color As Int32)
            _value.SetPixel(x, y, System.Drawing.Color.FromArgb(color))
        End Sub
        Public Function GetPixel(x As Int32, y As Int32) As Int32
            Return _value.GetPixel(x, y).ToArgb()
        End Function
    End Class
End Namespace

