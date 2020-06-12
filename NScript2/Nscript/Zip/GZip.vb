Imports System.IO
Imports System.Runtime.InteropServices
Imports System.Text
Namespace NScript
    <ComClass()> <ComVisible(True)> Public Class GZip
        Public Sub New()

        End Sub
        Public Function Compress(data As Byte()) As Byte()
            If data Is Nothing Then

                Throw New ArgumentNullException(NameOf(data))
            End If
            Using MenStream As New MemoryStream()
                Dim stream As New Compression.GZipStream(MenStream, Compression.CompressionMode.Compress)
                stream.Write(data, 0, data.Length)
                stream.Dispose()
                Return MenStream.GetBuffer()
            End Using
        End Function
        Public Function DeCompress(data As Byte()) As Byte()
            If data Is Nothing Then
                Throw New ArgumentNullException(NameOf(data))
            End If
            Dim block(1024) As Byte
            Using MenStream As New MemoryStream()
                Using stream As New Compression.GZipStream(New MemoryStream(data), Compression.CompressionMode.Decompress)
                    Do
                        Dim wawa As Int32 = stream.Read(block, 0, 1024)
                        If (wawa <= 0) Then
                            Exit Do
                        Else
                            MenStream.Write(block, 0, wawa)
                        End If
                    Loop
                End Using
                Return MenStream.GetBuffer()
            End Using
        End Function
        Public Function DeCompressToStr(Data As Byte()) As String
            Return Encoding.Default.GetString(Me.DeCompress(Data))
        End Function
    End Class
End Namespace
