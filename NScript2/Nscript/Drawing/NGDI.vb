Imports System.Drawing
Imports System.Runtime.InteropServices
Imports File = System.IO.File
Namespace NScript.Drawing
    <ComVisible(True)> <ComClass()> Public Class NGDI
        Friend GDI As Graphics
        Private ReadOnly Locker As New Object
        Public ReadOnly Property BaseGraphics As Graphics
            Get
                SyncLock Locker
                    If GDI Is Nothing Then
                        Throw New NotSupportedException()
                    End If
                    Return GDI
                End SyncLock
            End Get
        End Property
        Public Sub Init(BaseGraphics As Graphics)
            If BaseGraphics Is Nothing Then

            Else
                If TypeOf BaseGraphics Is Graphics Then
                Else
                    Throw New NotImplementedException()
                End If
            End If
            GDI = BaseGraphics
        End Sub
        Public Sub DrawLine(Color As Int32, width As Single, x1 As Single, y1 As Single, x2 As Single, y2 As Single)
            SyncLock Locker
                Dim gdip As Graphics = BaseGraphics
                Dim pen As New Pen(System.Drawing.Color.FromArgb(Color), width)
                gdip.DrawLine(pen, x1, y1, x2, y2)
                pen.Dispose()
            End SyncLock
        End Sub
        Public Sub DrawImage(Path As String, x1 As Single, y1 As Single, x2 As Single, y2 As Single)
            If File.Exists(Path) Then
                Dim bitmap As New Bitmap(Path)
                Dim gdip As Graphics = BaseGraphics
                gdip.DrawImage(bitmap, x1, y1, x2, y2)
                bitmap.Dispose()
            End If
        End Sub
        Public Sub DrawRect(x1 As Int32, y1 As Int32, x2 As Int32, y2 As Int32, Optional P As NPen = NPen.NullPen)
            If P Is Nothing Then
                P = New NPen()
            End If
            Dim RlP As Pen = P.BasePen
            GDI.DrawRectangle(RlP, x1, y1, x2, y2)
        End Sub
        Public Sub FillRect(x1 As Int32, y1 As Int32, x2 As Int32, y2 As Int32, Optional color As Int32 = 0)
            Dim brush As New SolidBrush(System.Drawing.Color.FromArgb(color))
            GDI.FillRectangle(brush, x1, y1, x2, y2)
            brush.Dispose()
        End Sub
        Public Sub DrawElp(x1 As Int32, y1 As Int32, x2 As Int32, y2 As Int32, Optional P As NPen = NPen.NullPen)
            If P Is Nothing Then
                P = New NPen()
            End If
            Dim RlP As Pen = P.BasePen
            GDI.DrawEllipse(RlP, x1, y1, x2, y2)
        End Sub
        Public Sub FillElp(x1 As Int32, y1 As Int32, x2 As Int32, y2 As Int32, Optional color As Int32 = 0)
            Dim brush As New SolidBrush(System.Drawing.Color.FromArgb(color))
            GDI.FillEllipse(brush, x1, y1, x2, y2)
            brush.Dispose()
        End Sub
        Public Sub DrawStr(Str As String, x1 As Int32, y1 As Int32, Optional color As Int32 = 0)
            Dim brush As New SolidBrush(System.Drawing.Color.FromArgb(color))
            GDI.DrawString(Str, SystemFonts.DefaultFont, brush, New PointF(x1, y1))
            brush.Dispose()
        End Sub
    End Class
End Namespace
