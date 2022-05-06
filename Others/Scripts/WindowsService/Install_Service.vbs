' =================================================
' Skrypt 'Install_Service.vbs' instaluje usluge
' 'SkayTek.CertificateGenerator.exe'.
' =================================================
' Wymagania.
' 1. W pliku 'Install_Service.vbs' nalezy podac
'    parametry w sekcji: Parameters (patrz ponizej).
' 2. Plik nalezy uruchomic z uprawnieniami administratora.
' =================================================
' Edycja skryptu.
' Do edycji skryptu 'Install_Service.vbs' mozna
' uzyc notatnika.
' =================================================
' Uruchomienie.
' Skrypt mozna uruchomic:
' 1. Klikajac w plik Install_Service.cmd.
' 2. Programem CScript.
' 3. Programem WScript - niezalecane.
' =================================================
' Uwagi.
' Dwukrotne klikniecie w plik 'Install_Service.vbs'
' moze spowodowac jego uruchomienie jesli jego
' rozszerzenie jest skojarzone z programem WScript.
' =================================================

Option Explicit

Const OWN_PROCESS = 16
Const DESKTOP_INTERACT = True
Const NORMAL_ERROR_CONTROL = 2
Const HKEY_LOCAL_MACHINE = &H80000002
Const HIDDEN_WINDOW = 12

Dim strComputer, strPath, strServiceName, strDisplayName, strStartMode, strStartName, strStartPassword, strDescription

' Parameters:
strComputer = "."
strPath = "D:\Projekty\SkayTek\SkayTek.CryptoBot\branches\1.0.0\SkayTek.CryptoBotService\bin\Debug\SkayTek.CryptoBotService.exe"
strServiceName = "CryptoBotService"
strDisplayName = "CryptoBotService"

strStartMode = "Automatic" ' Boot, System, Automatic, Manual, Disabled
strStartName = "LOCALSYSTEM" ' .\dariusz.ugarenko, LOCALSYSTEM, NT AUTHORITY\SYSTEM, NT AUTHORITY\LocalService
strStartPassword = Null
strDescription = "Us³uga Crypto Bot"

Call DeleteService(strComputer, strServiceName)
Call CreateService(strComputer, strPath, strServiceName, strDisplayName, strStartMode, strStartName, strStartPassword)
Call SetDescriptionService(strComputer, strServiceName, strDescription)
Call SetFailureParametersService(strComputer, strServiceName)
Call StartService(strComputer, strServiceName)

' Rejestruje usluge.
Function CreateService(strComputer, strPath, strServiceName, strDisplayName, strStartMode, strStartName, strStartPassword)
	Dim objWMIService, objService, errReturn
	
	If ServiceExists(strComputer, strServiceName) Then
		WScript.Echo "Usluga '" & strServiceName & "' jest juz zarejestrowana."
	Else
		Set objWMIService = GetObject("winmgmts:{impersonationLevel=impersonate}!\\" & strComputer & "\root\cimv2")
		Set objService = objWMIService.Get("Win32_BaseService")
		
		If IsNull(strStartPassword) Then
			errReturn = objService.Create(strServiceName, strDisplayName, strPath, _
				OWN_PROCESS, NORMAL_ERROR_CONTROL, strStartMode, DESKTOP_INTERACT, strStartName)
		Else
			errReturn = objService.Create(strServiceName, strDisplayName, strPath, _
				OWN_PROCESS, NORMAL_ERROR_CONTROL, strStartMode, DESKTOP_INTERACT, strStartName, strStartPassword)
		End If
		
		If errReturn = 0 Then
			WScript.Echo "Zarejestrowano usluge '" & strServiceName & "'"
		Else
			WScript.Echo "Rejestracja uslugi '" & strServiceName & "' niepowiodla sie." 
		End If
	End If
End Function

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

' Dodaje opis uslugi.
Function SetDescriptionService(strComputer, strServiceName, strDescription)
	Dim oReg, strKeyPath, strValueName, strValue, errReturn
	
	strKeyPath = "SYSTEM\CurrentControlSet\Services\" & strServiceName
	strValueName = "Description"
	
	Set oReg = GetObject("winmgmts:{impersonationLevel=impersonate}!\\" & strComputer & "\root\default:StdRegProv")
	errReturn = oReg.SetStringValue(HKEY_LOCAL_MACHINE, strKeyPath, strValueName, strDescription)
	
	If errReturn = 0 Then
	    WScript.Echo "Dodano opis dla uslugi '" & strServiceName & "'"
	Else
	    WScript.Echo "Nie mozna bylo dodac opisu dla uslugi '" & strServiceName & "' (blad: " & errReturn & ")"
	End If
End Function

' Ustawia akcje wykonywane przez usluge po wystapieniu bledu.
Function SetFailureParametersService(strComputer, strServiceName)
	Dim objWMIService, objStartup, objConfig, objProcess, intProcessID, errReturn
	
	Set objWMIService = GetObject("winmgmts:{impersonationLevel=impersonate}!\\" & strComputer & "\root\cimv2")
	Set objStartup = objWMIService.Get("Win32_ProcessStartup")
		
	Set objConfig = objStartup.SpawnInstance_
		objConfig.ShowWindow = HIDDEN_WINDOW

	Set objProcess = GetObject("winmgmts:root\cimv2:Win32_Process")
	errReturn = objProcess.Create("C:\Windows\system32\sc.exe failure " & strServiceName & _
		" reset= 0 actions= restart/5000/restart/10000/restart/15000", _
		"C:\Windows\system32\", objConfig, intProcessID)
	
	If errReturn = 0 Then
	    WScript.Echo "Opcje odzyskiwania dla uslugi '" & strServiceName & "' zostaly ustawione."
	Else
	    WScript.Echo "Nie mozna bylo ustawic opcji odzyskiwania dla uslugi '" & strServiceName & "' (blad: " & errReturn & ")"
	End If
End Function

' Uruchamia usluge.
Function StartService(strComputer, strServiceName)
	Dim objWMIService, objServices, objService, errReturn
	
	Set objWMIService = GetObject("winmgmts:{impersonationLevel=impersonate}!\\" & strComputer & "\root\cimv2")
	Set objServices = objWMIService.ExecQuery("Select * from Win32_Service Where State = 'Stopped' and Name = '" & strServiceName & "'")
	
	For Each objService in objServices
		WScript.Echo "Uruchamianie uslugi '" & objService.Name & "'..."
	    errReturn = objService.StartService()
	    
	    If errReturn = 0 Then
	    	WScript.Echo "Usluga '" & strServiceName & "' uruchomila sie prawidlowo."
	    Else
	    	WScript.Echo "Nie mozna bylo uruchomic uslugi '" & strServiceName & "' (blad: " & errReturn & ")"
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

' Zwraca informacje czy usluga o wskazanej nazwie istnieje.
Function ServiceExists(strComputer, strServiceName)
	Dim objWMIService, objServices, objService, errReturn
	
	Set objWMIService = GetObject("winmgmts:{impersonationLevel=impersonate}!\\" & strComputer & "\root\cimv2")
	Set objServices = objWMIService.ExecQuery("Select * from Win32_Service Where Name = '" & strServiceName & "'")
	
	If Not objServices Is Nothing And objServices.Count > 0 Then
		ServiceExists = True
	Else
		ServiceExists = False
	End if
End Function