set NS=wscript.createobject("NScript.NetScript")
NS.CheckVer 1.6,True,5
set NC=wscript.createobject("NScript.NSConsole")
NC.InitConsole()
set qrcode=wscript.createobject("NScript.QrCode.QrCodeEncoding")
NC.StdOut.WriteLine("")
NC.StdOut.Write(qrcode.GetQrCodeByStr(NC.Ask("��������Ҫ���ɶ�ά���ַ�����"),2))
NC.Ask(" ")