' =================================================
' Skrypt 'Uninstall_Service.vbs' odinstalowywuje usluge
' 'SkayTek.CertificateGenerator.exe'.
' =================================================
' Wymagania.
' 1. W pliku 'Uninstall_Service.vbs' nalezy podac
'    parametry w sekcji: Parameters (patrz ponizej).
' 2. Plik nalezy uruchomic z uprawnieniami administratora.
' =================================================

Option Explicit

Dim strComputer, strServiceName

' Parameters:
strComputer = "."
strServiceName = "CryptoBotService"

Call DeleteService(strComputer, strServiceName)

' Usuwa usluge.
Function DeleteService(strComputer, strServiceName)
	Dim objWMIService, objServices, objService, errReturn
	
	Set objWMIService = GetObject("winmgmts:{impersonationLevel=impersonate}!\\" & strComputer & "\root\cimv2")
	Set objServices = objWMIService.ExecQuery("Select * from Win32_Service Where Name = '" & strServiceName & "'")
	
	For Each objService in objServices
		StopService strComputer, strServiceName
    	errReturn = objService.Delete()
    	
    	If errReturn = 0 Then
	    	WScript.Echo "Usluga '" & strServiceName & "' zostala usunieta."
	    Else
	    	WScript.Echo "Nie mozna bylo usunac uslugi '" & strServiceName & "' (blad: " & errReturn & ")"
	    End If
    Next
End Function

' Zatrzymuje usluge.
Function StopService(strComputer, strServiceName)
	Dim objWMIService, objServices, objService, errReturn
	
	Set objWMIService = GetObject("winmgmts:{impersonationLevel=impersonate}!\\" & strComputer & "\root\cimv2")
	Set objServices = objWMIService.ExecQuery("Select * from Win32_Service Where State = 'Running' and Name = '" & strServiceName & "'")
	
	For Each objService in objServices
		WScript.Echo "Zatrzymywanie uslugi '" & objService.Name & "'..."
	    errReturn = objService.StopService()
	    
	    If errReturn = 0 Then
	    	WScript.Echo "Usluga '" & strServiceName & "' zatrzymala sie prawidlowo."
	    Else
	    	WScript.Echo "Nie mozna bylo zatrzymac uslugi '" & strServiceName & "' (blad: " & errReturn & ")"
	    End If
	Next
End Function