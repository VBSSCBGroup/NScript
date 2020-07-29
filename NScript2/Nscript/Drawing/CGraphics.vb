Imports System.Drawing
Imports System.Runtime.InteropServices
Imports File = System.IO.File
Namespace NScript.Drawing
    <ComVisible(True)> <ComClass()> Public Class CGraphics
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
            GDI.SmoothingMode = Drawing2D.SmoothingMode.AntiAlias
            GDI.CompositingQuality = Drawing2D.CompositingQuality.HighQuality
        End Sub
        Public Sub Clear(Color As Int32)
            GDI.Clear(System.Drawing.Color.FromArgb(Color))
        End Sub
        Public Sub DrawLine(Color As Int32, width As Int32, x1 As Int32, y1 As Int32, x2 As Int32, y2 As Int32)
            SyncLock Locker
                Dim gdip As Graphics = BaseGraphics
                Dim pen As New Pen(System.Drawing.Color.FromArgb(Color), width)
                gdip.DrawLine(pen, x1, y1, x2, y2)
                pen.Dispose()
            End SyncLock
        End Sub
        Public Sub DrawImage(Path As String, rct As Rect)
            If File.Exists(Path) Then
                Dim bitmap As New Bitmap(Path)
                Dim gdip As Graphics = BaseGraphics
                gdip.DrawImage(bitmap, rct.Value)
                bitmap.Dispose()
            End If
        End Sub
        Public Sub DrawRect(rct As Rect, Optional P As CPen = CPen.NullPen)
            If P Is Nothing Then
                P = New CPen()
            End If
            Dim RlP As Pen = P.BasePen
            GDI.DrawRectangle(RlP, rct.Value)
        End Sub
        Public Sub FillRect(rct As Rect, Optional color As Int32 = 0)
            Dim brush As New SolidBrush(System.Drawing.Color.FromArgb(color))
            GDI.FillRectangle(brush, rct.Value)
            brush.Dispose()
        End Sub
        Public Sub DrawElp(rct As Rect, Optional P As CPen = CPen.NullPen)
            If P Is Nothing Then
                P = New CPen()
            End If
            Dim RlP As Pen = P.BasePen
            GDI.DrawEllipse(RlP, rct.Value)
        End Sub
        Public Sub FillElp(rct As Rect, Optional color As Int32 = 0)
            Dim brush As New SolidBrush(System.Drawing.Color.FromArgb(color))
            GDI.FillEllipse(brush, rct.Value)
            brush.Dispose()
        End Sub
        Public Function NewRect(x, y, w, h) As Rect
            Return New Rect() With {.Value = New Rectangle(x, y, w, h)}
        End Function
        Public Sub DrawStr(Str As String, pt As CPoint, Optional color As Int32 = 0)
            Dim brush As New SolidBrush(System.Drawing.Color.FromArgb(color))
            GDI.DrawString(Str, SystemFonts.DefaultFont, brush, New PointF(pt.X, pt.Y))
            brush.Dispose()
        End Sub
    End Class
End Namespace
