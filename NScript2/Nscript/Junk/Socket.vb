Imports System.Runtime.InteropServices
Imports System.Text
Imports System.Net

Namespace NScript.Net
    <ComVisible(True), ComClass> Public NotInheritable Class Socket
        Implements IDisposable
        Private socket As System.Net.Sockets.Socket
        Private Sub New()

        End Sub
        Public Sub Send(data As String)
            socket.Send(Encoding.Unicode.GetBytes(data))
        End Sub
        Public Sub Revice(data As Byte())
            socket.Receive(data)
        End Sub
        Public Sub Init(ip As String, port As Short)
            socket = New System.Net.Sockets.Socket(System.Net.Sockets.SocketType.Dgram, System.Net.Sockets.ProtocolType.Tcp)
            socket.Bind(New IPEndPoint(IPAddress.Parse(ip), port))
        End Sub
        Public Sub ReviceStr(ByRef data As String)
            Dim data2(65535) As Byte
            socket.Receive(data2)
            data = Encoding.Unicode.GetString(data2)
            data2 = Nothing
        End Sub

        Public Sub Dispose() Implements IDisposable.Dispose
            DirectCast(socket, IDisposable).Dispose()
            GC.SuppressFinalize(Me)
        End Sub

        Public Function Accept() As Socket
            Dim socket2 As New Socket With {
                .socket = socket.Accept()
            }
            Return socket2
        End Function

    End Class
End Namespace
